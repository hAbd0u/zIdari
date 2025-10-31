using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing;
using PdfiumPdfDocument = PdfiumViewer.PdfDocument;
using zIdari.Model;

namespace zIdari.Forms
{
    public partial class ScanForm : Form
    {
        private readonly int _folderYear;
        private readonly int _folderNumYear;
        private readonly List<ScannedPage> _pages = new List<ScannedPage>();
        private int _currentPageIndex = -1;
        private double _currentZoom = 1.0;
        private Rectangle? _cropSelection = null;
        private bool _isCropping = false;
        private bool _isScanning = false;
        private string _selectedScannerId = null;
        private List<ScannerInfo> _availableScanners = new List<ScannerInfo>();
        private bool _isPanning = false;
        private Point _panStartPoint = Point.Empty;
        private Point _panStartScrollPosition = Point.Empty;

        private const string NAPS2_CONSOLE_PATH_64 = @"C:\Program Files\NAPS2\NAPS2.Console.exe";
        private const string NAPS2_CONSOLE_PATH_32 = @"C:\Program Files (x86)\NAPS2\NAPS2.Console.exe";
        private string _naps2ConsolePath = null;
        private string _tempScanFolder;

        public string SavePath { get; private set; }
        public List<ScannedPage> Pages => _pages;

        public class ScannerInfo
        {
            public string Id { get; set; }
            public string Name { get; set; }
            
            public override string ToString()
            {
                return Name ?? Id ?? "Unknown Scanner";
            }
        }
        
        private string FindNAPS2Path()
        {
            if (File.Exists(NAPS2_CONSOLE_PATH_64))
                return NAPS2_CONSOLE_PATH_64;
            if (File.Exists(NAPS2_CONSOLE_PATH_32))
                return NAPS2_CONSOLE_PATH_32;
            return null;
        }

        public ScanForm(int folderYear, int folderNumYear)
        {
            InitializeComponent();
            _folderYear = folderYear;
            _folderNumYear = folderNumYear;
            
            // Create temp folder for scanning
            _tempScanFolder = Path.Combine(Path.GetTempPath(), "ScanForm_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_tempScanFolder);
            
            InitializeForm();
        }

        private void InitializeForm()
        {
            // Find NAPS2 Console installation
            _naps2ConsolePath = FindNAPS2Path();
            
            // Initialize combo boxes
            InitializeDpiCombo();
            InitializeColorCombo();
            InitializeFormatCombo();
            InitializeZoomCombo();

            // Set default save location
            var defaultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Documents", "Scanned");
            if (!Directory.Exists(defaultPath))
                Directory.CreateDirectory(defaultPath);
            saveLocationTextBox.Text = defaultPath;

            // Generate filename
            UpdateFilename();

            // Load scanners
            LoadScanners();

            // Wire up events
            WireUpEvents();

            // Update UI state
            UpdateUI();
        }

        private void LoadScanners()
        {
            try
            {
                statusLabel.Text = "Loading scanners...";
                
                if (string.IsNullOrEmpty(_naps2ConsolePath) || !File.Exists(_naps2ConsolePath))
                {
                    // Fallback to WIA if NAPS2 not found
                    LoadScannersUsingWIA();
                    return;
                }

                _availableScanners.Clear();
                ScannerNameCombo.Items.Clear();

                // Use NAPS2 Console to list devices with WIA driver
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = _naps2ConsolePath,
                        Arguments = "--listdevices --driver wia",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        StandardOutputEncoding = Encoding.UTF8
                    }
                };

                process.Start();
                
                // Wait for process with timeout
                bool finished = process.WaitForExit(5000); // 5 second timeout
                if (!finished)
                {
                    process.Kill();
                    ScannerNameCombo.Items.Add("NAPS2 command timed out");
                    statusLabel.Text = "Scanner detection timed out";
                    return;
                }
                
                var output = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();
                
                // Debug: Show output if no scanners found
                System.Diagnostics.Debug.WriteLine($"NAPS2 Output: {output}");
                System.Diagnostics.Debug.WriteLine($"NAPS2 Error: {error}");
                System.Diagnostics.Debug.WriteLine($"Exit Code: {process.ExitCode}");

                if (!string.IsNullOrWhiteSpace(output))
                {
                    var lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    
                    foreach (var line in lines)
                    {
                        var trimmedLine = line.Trim();
                        
                        // Skip empty lines, help messages, and error messages
                        if (string.IsNullOrWhiteSpace(trimmedLine) || 
                            trimmedLine.StartsWith("Usage:") ||
                            trimmedLine.StartsWith("The --") ||
                            trimmedLine.StartsWith("Possible values:") ||
                            trimmedLine.StartsWith("NAPS2") ||
                            trimmedLine.Contains("www.naps2.com") ||
                            trimmedLine.Contains("https://") ||
                            trimmedLine.Contains("naps2.com") ||
                            trimmedLine.Length < 3)
                            continue;
                        
                        // NAPS2.Console outputs just the device name (e.g., "Canon MF3010")
                        // Use the device name as both ID and Name
                        var scanner = new ScannerInfo
                        {
                            Id = trimmedLine, // Use name as ID for WIA driver
                            Name = trimmedLine
                        };
                        _availableScanners.Add(scanner);
                        ScannerNameCombo.Items.Add(scanner);
                    }
                }

                if (ScannerNameCombo.Items.Count == 0)
                {
                    // Try alternative: Use WIA to detect scanners directly
                    LoadScannersUsingWIA();
                    
                    if (ScannerNameCombo.Items.Count == 0)
                    {
                        ScannerNameCombo.Items.Add("No scanners found");
                        statusLabel.Text = "No scanners available";
                    }
                }
                else
                {
                    // NAPS2 devices found, update status
                    if (ScannerNameCombo.Items.Count > 0)
                    {
                        ScannerNameCombo.SelectedIndex = 0;
                        _selectedScannerId = ((ScannerInfo)ScannerNameCombo.SelectedItem)?.Id;
                        statusLabel.Text = $"✓ {_availableScanners.Count} scanner(s) found (NAPS2)";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading scanners: {ex.Message}\n\nMake sure NAPS2 is installed from https://www.naps2.com/", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = "Error loading scanners";
                ScannerNameCombo.Items.Add("Error loading scanners");
                
                // Fallback to WIA
                try
                {
                    LoadScannersUsingWIA();
                }
                catch { }
            }
        }
        
        private void LoadScannersUsingWIA()
        {
            try
            {
                // Use dynamic typing to avoid compile-time dependency on WIA
                Type deviceManagerType = Type.GetTypeFromProgID("WIA.DeviceManager");
                if (deviceManagerType == null)
                {
                    System.Diagnostics.Debug.WriteLine("WIA DeviceManager not available");
                    return;
                }

                dynamic deviceManager = Activator.CreateInstance(deviceManagerType);
                int deviceCount = deviceManager.DeviceInfos.Count;

                for (int i = 1; i <= deviceCount; i++)
                {
                    dynamic deviceInfo = deviceManager.DeviceInfos[i];
                    int deviceType = (int)deviceInfo.Type;
                    
                    // WiaDeviceType.ScannerDeviceType = 1
                    // WiaDeviceType.CameraDeviceType = 2
                    if (deviceType == 1 || deviceType == 2)
                    {
                        string deviceName = "";
                        try
                        {
                            dynamic nameProp = deviceInfo.Properties["Name"];
                            deviceName = nameProp.get_Value().ToString();
                        }
                        catch
                        {
                            deviceName = $"Scanner {i}";
                        }

                        var scanner = new ScannerInfo
                        {
                            Id = i.ToString(),
                            Name = deviceName
                        };
                        _availableScanners.Add(scanner);
                        ScannerNameCombo.Items.Add(scanner);
                    }
                }
                
                if (ScannerNameCombo.Items.Count > 0)
                {
                    ScannerNameCombo.SelectedIndex = 0;
                    _selectedScannerId = ((ScannerInfo)ScannerNameCombo.SelectedItem)?.Id;
                    statusLabel.Text = $"✓ {_availableScanners.Count} scanner(s) found (WIA)";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"WIA Error: {ex.Message}");
                // WIA not available, ignore
            }
        }

        private void InitializeDpiCombo()
        {
            DpiCombo.Items.AddRange(new object[] { "150", "200", "300", "600", "1200" });
            DpiCombo.SelectedItem = "300";
        }

        private void InitializeColorCombo()
        {
            colorCombo.Items.AddRange(new object[] { "Colored scan", "Gray", "Black&White" });
            colorCombo.SelectedItem = "Colored scan";
        }

        private void InitializeFormatCombo()
        {
            formatCombo.Items.AddRange(new object[] { "PDF (Multi-page)", "JPEG", "PNG", "TIFF" });
            formatCombo.SelectedItem = "PDF (Multi-page)";
        }

        private void InitializeZoomCombo()
        {
            zoomCombo.Items.AddRange(new object[] { "25%", "50%", "75%", "100%", "150%", "200%" });
            zoomCombo.SelectedItem = "100%";
        }

        private void WireUpEvents()
        {
            // Navigation
            firstBtn.Click += FirstBtn_Click;
            prevBtn.Click += PrevBtn_Click;
            nextBtn.Click += NextBtn_Click;
            lastBtn.Click += LastBtn_Click;
            pageNumericUpDown.ValueChanged += PageNumericUpDown_ValueChanged;

            // Zoom
            zoomCombo.SelectedIndexChanged += ZoomCombo_SelectedIndexChanged;
            zoomInBtn.Click += ZoomInBtn_Click;
            zoomOutBtn.Click += ZoomOutBtn_Click;
            fitBtn.Click += FitBtn_Click;
            actualSizeBtn.Click += ActualSizeBtn_Click;
            previewPictureBox.MouseWheel += PreviewPictureBox_MouseWheel;

            // Rotation
            rotateLeftBtn.Click += RotateLeftBtn_Click;
            rotateRightBtn.Click += RotateRightBtn_Click;
            flip180Btn.Click += Flip180Btn_Click;
            resetRotationBtn.Click += ResetRotationBtn_Click;

            // Crop
            cropBtn.Click += CropBtn_Click;
            applyCropBtn.Click += ApplyCropBtn_Click;
            clearCropBtn.Click += ClearCropBtn_Click;
            previewPictureBox.MouseDown += PreviewPictureBox_MouseDown;
            previewPictureBox.MouseMove += PreviewPictureBox_MouseMove;
            previewPictureBox.MouseUp += PreviewPictureBox_MouseUp;
            
            // Pan support (for zoomed images)
            previewPictureBox.MouseDown += PreviewPictureBox_PanMouseDown;
            previewPictureBox.MouseMove += PreviewPictureBox_PanMouseMove;
            previewPictureBox.MouseUp += PreviewPictureBox_PanMouseUp;

            // Actions
            scanBtn.Click += ScanBtn_Click;
            importBtn.Click += ImportBtn_Click;
            saveAllBtn.Click += SaveAllBtn_Click;
            saveCurrentBtn.Click += SaveCurrentBtn_Click;
            cancelBtn.Click += CancelBtn_Click;
            retryScanBtn.Click += RetryScanBtn_Click;
            browseBtn.Click += BrowseBtn_Click;

            // Settings changes
            ScannerNameCombo.SelectedIndexChanged += ScannerNameCombo_SelectedIndexChanged;
            brightnessTrackBar.ValueChanged += BrightnessTrackBar_ValueChanged;
            contrastTrackBar.ValueChanged += ContrastTrackBar_ValueChanged;
            autoOptimizeCheckBox.CheckedChanged += AutoOptimizeCheckBox_CheckedChanged;
            formatCombo.SelectedIndexChanged += FormatCombo_SelectedIndexChanged;
            
            // Lock previewPanel size to its TableLayoutPanel cell and enable scrolling
            this.Load += ScanForm_Load;

            // Keyboard shortcuts
            this.KeyDown += ScanForm_KeyDown;
            this.KeyPreview = true;
        }

        private void ScannerNameCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            var scanner = ScannerNameCombo.SelectedItem as ScannerInfo;
            _selectedScannerId = scanner?.Id;
        }

        private void UpdateFilename()
        {
            var extension = formatCombo.SelectedItem?.ToString() switch
            {
                "PDF (Multi-page)" => "pdf",
                "JPEG" => "jpg",
                "PNG" => "png",
                "TIFF" => "tiff",
                _ => "pdf"
            };

            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var md5 = ComputeMD5Hash(timestamp);
            var filename = $"{_folderYear}_{_folderNumYear}_{md5}.{extension}";
            filenameTextBox.Text = filename;
        }

        private string ComputeMD5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = md5.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant().Substring(0, 16);
            }
        }

        private void UpdateUI()
        {
            var hasPages = _pages.Count > 0;
            var hasCurrentPage = _currentPageIndex >= 0 && _currentPageIndex < _pages.Count;

            // Navigation buttons
            firstBtn.Enabled = hasPages && _currentPageIndex > 0;
            prevBtn.Enabled = hasPages && _currentPageIndex > 0;
            nextBtn.Enabled = hasPages && _currentPageIndex < _pages.Count - 1;
            lastBtn.Enabled = hasPages && _currentPageIndex < _pages.Count - 1;

            // Page counter
            if (hasPages)
            {
                pageNumericUpDown.Maximum = _pages.Count;
                pageNumericUpDown.Value = _currentPageIndex + 1;
                //pageLabel.Text = $"من {_currentPageIndex + 1}";
                totalPagesLabel.Text = $"Total: {_pages.Count} pages";
                var scannedCount = _pages.Count(p => !p.IsImported);
                var importedCount = _pages.Count(p => p.IsImported);
                pagesCountLabel.Text = $"Total: {_pages.Count} pages ({scannedCount} scanned, {importedCount} imported)";
            }
            else
            {
                pageNumericUpDown.Maximum = 1;
                pageNumericUpDown.Value = 1;
                //pageLabel.Text = "من 0";
                totalPagesLabel.Text = "Total: 0 pages";
                pagesCountLabel.Text = "Total: 0 pages";
            }

            // Rotation buttons
            rotateLeftBtn.Enabled = hasCurrentPage;
            rotateRightBtn.Enabled = hasCurrentPage;
            flip180Btn.Enabled = hasCurrentPage;
            resetRotationBtn.Enabled = hasCurrentPage;

            // Crop buttons
            cropBtn.Enabled = hasCurrentPage;
            applyCropBtn.Enabled = _isCropping;
            clearCropBtn.Enabled = hasCurrentPage && hasPages && _pages[_currentPageIndex].CropRegion.HasValue;

            // Save buttons
            saveAllBtn.Enabled = hasPages;
            saveCurrentBtn.Enabled = hasCurrentPage;
            retryScanBtn.Enabled = hasCurrentPage && !_isScanning;

            // Scan button - enabled if we have a scanner selected (NAPS2 Console or WIA)
            scanBtn.Enabled = !_isScanning && !string.IsNullOrEmpty(_selectedScannerId);

            // Update preview
            if (hasCurrentPage)
            {
                UpdatePreview();
            }
            else
            {
                previewPictureBox.Image = null;
            }

            // Update thumbnails
            UpdateThumbnails();
        }

        private void UpdatePreview()
        {
            // Dispose previous image
            var oldImage = previewPictureBox.Image;
            previewPictureBox.Image = null;
            oldImage?.Dispose();

            if (_currentPageIndex < 0 || _currentPageIndex >= _pages.Count)
            {
                return;
            }

            var page = _pages[_currentPageIndex];
            var displayImage = page.GetDisplayImage();

            if (displayImage == null) return;

            // Apply brightness and contrast adjustments
            Image adjustedImage = displayImage;
            int brightness = brightnessTrackBar?.Value ?? 50; // Default is 50 (neutral)
            int contrast = contrastTrackBar?.Value ?? 50; // Default is 50 (neutral)
            
            // Auto-optimize: automatically adjust brightness/contrast based on image analysis
            if (autoOptimizeCheckBox?.Checked == true)
            {
                var optimized = CalculateAutoOptimize(displayImage);
                brightness = optimized.brightness;
                contrast = optimized.contrast;
            }
            
            // Only apply if not at neutral values
            if (brightness != 50 || contrast != 50)
            {
                adjustedImage = ApplyBrightnessContrast(displayImage, brightness, contrast);
                if (adjustedImage != displayImage)
                {
                    displayImage?.Dispose();
                }
            }

            // Apply zoom and update PictureBox settings
            Image finalImage = adjustedImage;
            if (_currentZoom != 1.0)
            {
                var zoomedWidth = (int)(adjustedImage.Width * _currentZoom);
                var zoomedHeight = (int)(adjustedImage.Height * _currentZoom);
                finalImage = new Bitmap(adjustedImage, zoomedWidth, zoomedHeight);
                if (finalImage != adjustedImage)
                    adjustedImage?.Dispose();
            }

            // Set image and configure PictureBox for zooming with scrolling
            var previewPanel = previewPictureBox.Parent as Panel;
            if (previewPanel != null && finalImage != null)
            {
                int zoomedWidth = finalImage.Width;
                int zoomedHeight = finalImage.Height;
                int availableWidth = previewPanel.ClientSize.Width;
                int availableHeight = previewPanel.ClientSize.Height - 30; // Subtract zoom panel
                
                // Panel size is locked (via MaximumSize) so it won't grow beyond cell
                // AutoScroll is enabled, so content inside can scroll
                
                if (zoomedWidth <= availableWidth && zoomedHeight <= availableHeight)
                {
                    // Image fits - dock it and use Zoom mode
                    previewPictureBox.Dock = DockStyle.Fill;
                    previewPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                    previewPanel.AutoScrollMinSize = Size.Empty; // Clear scroll requirement
                    previewPictureBox.Cursor = _isCropping ? Cursors.Cross : Cursors.Default;
                }
                else
                {
                    // Image is larger than panel - enable scrolling and panning
                    previewPictureBox.Dock = DockStyle.None;
                    previewPictureBox.SizeMode = PictureBoxSizeMode.Normal;
                    previewPictureBox.Size = new Size(zoomedWidth, zoomedHeight);
                    previewPictureBox.Location = new Point(0, 30);
                    
                    // Set AutoScrollMinSize so panel knows how much space is needed
                    // Panel itself won't grow because MaximumSize is locked
                    previewPanel.AutoScrollMinSize = new Size(zoomedWidth, zoomedHeight);
                    
                    // Enable pan cursor when zoomed in
                    previewPictureBox.Cursor = _isCropping ? Cursors.Cross : Cursors.Hand;
                }
            }
            else
            {
                previewPictureBox.Dock = DockStyle.Fill;
                previewPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                if (previewPanel != null)
                {
                    previewPanel.AutoScrollMinSize = Size.Empty;
                }
            }
            
            previewPictureBox.Image = finalImage;
        }
        
        private Bitmap ApplyBrightnessContrast(Image sourceImage, int brightness, int contrast)
        {
            // Trackbar values: 0-100, where 50 is neutral (no adjustment)
            // Convert to -50 to +50 range
            int brightnessAdjust = brightness - 50;
            int contrastAdjust = contrast - 50;
            
            // If both are neutral, return original image
            if (brightnessAdjust == 0 && contrastAdjust == 0)
            {
                return new Bitmap(sourceImage);
            }
            
            var bitmap = new Bitmap(sourceImage);
            
            // Brightness: -50 to +50 maps to -127.5 to +127.5
            float brightnessValue = brightnessAdjust / 50.0f * 127.5f;
            
            // Contrast: -50 to +50 maps to 0.5x to 1.5x multiplier
            float contrastValue = 1.0f + (contrastAdjust / 50.0f * 0.5f);
            if (contrastValue < 0.1f) contrastValue = 0.1f;
            
            // Apply adjustments using ColorMatrix
            float[][] colorMatrixElements = {
                new float[] { contrastValue, 0, 0, 0, 0 },
                new float[] { 0, contrastValue, 0, 0, 0 },
                new float[] { 0, 0, contrastValue, 0, 0 },
                new float[] { 0, 0, 0, 1, 0 },
                new float[] { brightnessValue / 255.0f, brightnessValue / 255.0f, brightnessValue / 255.0f, 0, 1 }
            };
            
            var adjustedBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            using (var graphics = Graphics.FromImage(adjustedBitmap))
            {
                var colorMatrix = new System.Drawing.Imaging.ColorMatrix(colorMatrixElements);
                var imageAttributes = new System.Drawing.Imaging.ImageAttributes();
                imageAttributes.SetColorMatrix(colorMatrix);
                
                graphics.DrawImage(bitmap, 
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    0, 0, bitmap.Width, bitmap.Height,
                    GraphicsUnit.Pixel,
                    imageAttributes);
            }
            
            bitmap?.Dispose();
            return adjustedBitmap;
        }

        private void UpdateThumbnails()
        {
            // Clear existing thumbnails and dispose their images
            foreach (Control ctrl in thumbnailPanel.Controls)
            {
                if (ctrl is Panel panel)
                {
                    var picBox = panel.Controls.OfType<PictureBox>().FirstOrDefault();
                    if (picBox?.Image != null)
                    {
                        picBox.Image.Dispose();
                    }
                }
                ctrl.Dispose();
            }
            thumbnailPanel.Controls.Clear();

            for (int i = 0; i < _pages.Count; i++)
            {
                var page = _pages[i];
                var thumbnail = CreateThumbnail(page, i);
                thumbnailPanel.Controls.Add(thumbnail);
            }
        }

        private Control CreateThumbnail(ScannedPage page, int index)
        {
            var panel = new Panel
            {
                Width = 160,  // Wider for side panel
                Height = 120, // Compact height for vertical scrolling
                Margin = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = index == _currentPageIndex ? Color.LightBlue : Color.White,
                Tag = index
            };

            var picBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                Padding = new Padding(2)
            };

            // Create thumbnail image (wider for side panel)
            if (page.ImageData != null)
            {
                try
                {
                    // Maintain aspect ratio but fit to 150x100 thumbnail area
                    var aspectRatio = (double)page.ImageData.Width / page.ImageData.Height;
                    int thumbWidth, thumbHeight;
                    
                    if (aspectRatio > 1.5)
                    {
                        // Landscape - fit to width
                        thumbWidth = 150;
                        thumbHeight = (int)(150 / aspectRatio);
                    }
                    else
                    {
                        // Portrait - fit to height
                        thumbHeight = 100;
                        thumbWidth = (int)(100 * aspectRatio);
                    }
                    
                    var thumb = new Bitmap(page.ImageData, thumbWidth, thumbHeight);
                    picBox.Image = thumb;
                }
                catch
                {
                    // Image might be disposed, skip thumbnail
                }
            }

            var label = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 20,
                Text = $"Page {index + 1}" + (page.IsImported ? " [I]" : ""),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 8, FontStyle.Bold),
                BackColor = index == _currentPageIndex ? Color.LightBlue : Color.LightGray
            };

            panel.Controls.Add(picBox);
            panel.Controls.Add(label);
            panel.Controls.SetChildIndex(label, 0);

            panel.Click += (s, e) => NavigateToPage(index);
            picBox.Click += (s, e) => NavigateToPage(index);
            label.Click += (s, e) => NavigateToPage(index);

            return panel;
        }

        private void NavigateToPage(int index)
        {
            if (index >= 0 && index < _pages.Count)
            {
                _currentPageIndex = index;
                UpdateUI();
            }
        }

        // Navigation event handlers
        private void FirstBtn_Click(object sender, EventArgs e)
        {
            if (_pages.Count > 0)
            {
                _currentPageIndex = 0;
                UpdateUI();
            }
        }

        private void PrevBtn_Click(object sender, EventArgs e)
        {
            if (_currentPageIndex > 0)
            {
                _currentPageIndex--;
                UpdateUI();
            }
        }

        private void NextBtn_Click(object sender, EventArgs e)
        {
            if (_currentPageIndex < _pages.Count - 1)
            {
                _currentPageIndex++;
                UpdateUI();
            }
        }

        private void LastBtn_Click(object sender, EventArgs e)
        {
            if (_pages.Count > 0)
            {
                _currentPageIndex = _pages.Count - 1;
                UpdateUI();
            }
        }

        private void PageNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            var pageNum = (int)pageNumericUpDown.Value;
            if (pageNum >= 1 && pageNum <= _pages.Count && pageNum - 1 != _currentPageIndex)
            {
                _currentPageIndex = pageNum - 1;
                UpdateUI();
            }
        }

        // Zoom event handlers
        private void ZoomCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            var zoomText = zoomCombo.SelectedItem?.ToString() ?? "100%";
            zoomText = zoomText.Replace("%", "");
            if (double.TryParse(zoomText, out var zoom))
            {
                _currentZoom = zoom / 100.0;
                UpdatePreview();
            }
        }

        private void ZoomInBtn_Click(object sender, EventArgs e)
        {
            _currentZoom = Math.Min(_currentZoom * 1.25, 5.0);
            UpdatePreview();
            UpdateZoomCombo();
        }

        private void ZoomOutBtn_Click(object sender, EventArgs e)
        {
            _currentZoom = Math.Max(_currentZoom / 1.25, 0.1);
            UpdatePreview();
            UpdateZoomCombo();
        }

        private void FitBtn_Click(object sender, EventArgs e)
        {
            if (_currentPageIndex < 0 || _currentPageIndex >= _pages.Count)
                return;
                
            var page = _pages[_currentPageIndex];
            var sourceImage = page.GetDisplayImage();
            
            if (sourceImage != null)
            {
                var previewPanel = previewPictureBox.Parent as Panel;
                if (previewPanel != null)
                {
                    var imgWidth = sourceImage.Width;
                    var imgHeight = sourceImage.Height;
                    var availableWidth = previewPanel.ClientSize.Width;
                    var availableHeight = previewPanel.ClientSize.Height - 30; // Subtract zoom panel

                    var zoomX = (double)availableWidth / imgWidth;
                    var zoomY = (double)availableHeight / imgHeight;
                    _currentZoom = Math.Min(zoomX, zoomY);
                }
                
                sourceImage?.Dispose();
                UpdatePreview();
                UpdateZoomCombo();
            }
        }

        private void ActualSizeBtn_Click(object sender, EventArgs e)
        {
            _currentZoom = 1.0;
            UpdatePreview();
            UpdateZoomCombo();
        }

        private void PreviewPictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                ZoomInBtn_Click(sender, e);
            else
                ZoomOutBtn_Click(sender, e);
        }

        private void UpdateZoomCombo()
        {
            var percent = (int)(_currentZoom * 100);
            var zoomText = $"{percent}%";
            if (zoomCombo.Items.Contains(zoomText))
                zoomCombo.SelectedItem = zoomText;
            else
            {
                // Add custom zoom percentage
                if (!zoomCombo.Items.Cast<string>().Any(x => x.StartsWith(percent.ToString())))
                {
                    zoomCombo.Items.Add(zoomText);
                    zoomCombo.SelectedItem = zoomText;
                }
            }
        }

        // Rotation event handlers
        private void RotateLeftBtn_Click(object sender, EventArgs e)
        {
            if (_currentPageIndex >= 0 && _currentPageIndex < _pages.Count)
            {
                _pages[_currentPageIndex].Rotation = (_pages[_currentPageIndex].Rotation - 90 + 360) % 360;
                UpdateUI();
            }
        }

        private void RotateRightBtn_Click(object sender, EventArgs e)
        {
            if (_currentPageIndex >= 0 && _currentPageIndex < _pages.Count)
            {
                _pages[_currentPageIndex].Rotation = (_pages[_currentPageIndex].Rotation + 90) % 360;
                UpdateUI();
            }
        }

        private void Flip180Btn_Click(object sender, EventArgs e)
        {
            if (_currentPageIndex >= 0 && _currentPageIndex < _pages.Count)
            {
                _pages[_currentPageIndex].Rotation = (_pages[_currentPageIndex].Rotation + 180) % 360;
                UpdateUI();
            }
        }

        private void ResetRotationBtn_Click(object sender, EventArgs e)
        {
            if (_currentPageIndex >= 0 && _currentPageIndex < _pages.Count)
            {
                _pages[_currentPageIndex].Rotation = 0;
                UpdateUI();
            }
        }

        // Crop event handlers
        private Point _cropStartPoint = Point.Empty;
        private bool _isDrawingCrop = false;

        private void CropBtn_Click(object sender, EventArgs e)
        {
            if (_currentPageIndex >= 0 && _currentPageIndex < _pages.Count)
            {
                _isCropping = true;
                _isDrawingCrop = false;
                _cropSelection = null;
                UpdateUI();
                previewPictureBox.Cursor = Cursors.Cross;
            }
        }

        private void PreviewPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (_isCropping && _currentPageIndex >= 0 && _currentPageIndex < _pages.Count)
            {
                _isDrawingCrop = true;
                _cropStartPoint = e.Location;
            }
        }
        
        private void PreviewPictureBox_PanMouseDown(object sender, MouseEventArgs e)
        {
            // Only enable pan when not cropping and when zoomed in
            if (!_isCropping && _currentZoom > 1.0 && e.Button == MouseButtons.Left)
            {
                var previewPanel = previewPictureBox.Parent as Panel;
                if (previewPanel != null && previewPanel.AutoScroll)
                {
                    _isPanning = true;
                    _panStartPoint = e.Location;
                    _panStartScrollPosition = previewPanel.AutoScrollPosition;
                    previewPictureBox.Cursor = Cursors.Hand;
                }
            }
        }
        
        private void PreviewPictureBox_PanMouseMove(object sender, MouseEventArgs e)
        {
            if (_isPanning)
            {
                var previewPanel = previewPictureBox.Parent as Panel;
                if (previewPanel != null)
                {
                    // Calculate movement delta (mouse moved this much)
                    int deltaX = e.X - _panStartPoint.X;
                    int deltaY = e.Y - _panStartPoint.Y;
                    
                    // Update scroll position (reverse direction for natural panning)
                    // AutoScrollPosition returns negative values, so we need to negate
                    int currentX = -previewPanel.AutoScrollPosition.X;
                    int currentY = -previewPanel.AutoScrollPosition.Y;
                    
                    int newX = currentX - deltaX;
                    int newY = currentY - deltaY;
                    
                    // Clamp to valid scroll range
                    if (previewPanel.HorizontalScroll.Visible)
                    {
                        newX = Math.Max(0, Math.Min(previewPanel.HorizontalScroll.Maximum, newX));
                    }
                    if (previewPanel.VerticalScroll.Visible)
                    {
                        newY = Math.Max(0, Math.Min(previewPanel.VerticalScroll.Maximum, newY));
                    }
                    
                    previewPanel.AutoScrollPosition = new Point(newX, newY);
                    
                    // Update start point for smooth continuous panning
                    _panStartPoint = e.Location;
                    _panStartScrollPosition = previewPanel.AutoScrollPosition;
                }
            }
        }
        
        private void PreviewPictureBox_PanMouseUp(object sender, MouseEventArgs e)
        {
            if (_isPanning)
            {
                _isPanning = false;
                if (!_isCropping && _currentZoom > 1.0)
                {
                    previewPictureBox.Cursor = Cursors.Hand;
                }
                else
                {
                    previewPictureBox.Cursor = Cursors.Default;
                }
            }
        }

        private void PreviewPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawingCrop && _isCropping)
            {
                // Draw crop rectangle
                var rect = new Rectangle(
                    Math.Min(_cropStartPoint.X, e.X),
                    Math.Min(_cropStartPoint.Y, e.Y),
                    Math.Abs(e.X - _cropStartPoint.X),
                    Math.Abs(e.Y - _cropStartPoint.Y)
                );
                
                // Update preview with crop overlay
                // For now, we'll just store it and apply on ApplyCropBtn_Click
            }
        }

        private void PreviewPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (_isDrawingCrop && _isCropping && _currentPageIndex >= 0 && _currentPageIndex < _pages.Count)
            {
                var page = _pages[_currentPageIndex];
                if (page.ImageData != null)
                {
                    // Convert screen coordinates to image coordinates
                    var img = page.GetDisplayImage();
                    if (img != null)
                    {
                        var scaleX = (double)img.Width / previewPictureBox.Width;
                        var scaleY = (double)img.Height / previewPictureBox.Height;
                        
                        var cropRect = new Rectangle(
                            (int)(Math.Min(_cropStartPoint.X, e.X) * scaleX),
                            (int)(Math.Min(_cropStartPoint.Y, e.Y) * scaleY),
                            (int)(Math.Abs(e.X - _cropStartPoint.X) * scaleX),
                            (int)(Math.Abs(e.Y - _cropStartPoint.Y) * scaleY)
                        );
                        
                        // Ensure crop rect is within image bounds
                        cropRect = Rectangle.Intersect(cropRect, new Rectangle(0, 0, img.Width, img.Height));
                        if (cropRect.Width > 0 && cropRect.Height > 0)
                        {
                            _cropSelection = cropRect;
                            applyCropBtn.Enabled = true;
                        }
                    }
                    img?.Dispose();
                }
                
                _isDrawingCrop = false;
                previewPictureBox.Cursor = Cursors.Default;
            }
        }

        private void ApplyCropBtn_Click(object sender, EventArgs e)
        {
            if (_isCropping && _currentPageIndex >= 0 && _currentPageIndex < _pages.Count && _cropSelection.HasValue)
            {
                _pages[_currentPageIndex].CropRegion = _cropSelection;
                _isCropping = false;
                _cropSelection = null;
                previewPictureBox.Cursor = Cursors.Default;
                UpdateUI();
            }
        }

        private void ClearCropBtn_Click(object sender, EventArgs e)
        {
            if (_currentPageIndex >= 0 && _currentPageIndex < _pages.Count)
            {
                _pages[_currentPageIndex].CropRegion = null;
                _isCropping = false;
                _cropSelection = null;
                previewPictureBox.Cursor = Cursors.Default;
                UpdateUI();
            }
        }

        // Action event handlers - SCANNING using NAPS2 CLI
        private async void ScanBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedScannerId))
            {
                MessageBox.Show("Please select a scanner first.", "No Scanner", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_isScanning)
                return;

            try
            {
                _isScanning = true;
                scanBtn.Enabled = false;
                statusLabel.Text = "Scanning...";
                this.Cursor = Cursors.WaitCursor;

                // Check if using NAPS2 or WIA
                bool useNAPS2 = !string.IsNullOrEmpty(_naps2ConsolePath) && File.Exists(_naps2ConsolePath);
                
                if (useNAPS2)
                {
                    await Task.Run(() => PerformScanWithNAPS2());
                }
                else
                {
                    await Task.Run(() => PerformScanWithWIA());
                }

                if (_pages.Count > 0)
                {
                    // Navigate to last scanned page
                    _currentPageIndex = _pages.Count - 1;
                    UpdateUI();
                    statusLabel.Text = $"✓ {_pages.Count} page(s) scanned successfully";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during scanning: {ex.Message}", "Scan Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = "✗ Scan failed";
            }
            finally
            {
                _isScanning = false;
                scanBtn.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }

        private void PerformScanWithNAPS2()
        {
            // Get values from UI on the UI thread before starting background task
            string colorMode = null;
            string dpi = null;
            
            this.Invoke((MethodInvoker)(() =>
            {
                colorMode = colorCombo.SelectedItem?.ToString() switch
                {
                    "Gray" => "gray",
                    "Black&White" => "bw",
                    _ => "color"
                };
                
                dpi = DpiCombo.SelectedItem?.ToString() ?? "300";
            }));
            
            var outputPath = Path.Combine(_tempScanFolder, $"scan_{Guid.NewGuid():N}.png");

            // Build NAPS2.Console command line arguments
            // Use correct syntax: --bitdepth instead of --color, and -o for output
            bool batchMode = false;
            bool duplexMode = false;
            
            this.Invoke((MethodInvoker)(() =>
            {
                batchMode = batchCheckBox.Checked;
                duplexMode = duplexCheckBox.Checked;
            }));
            
            var args = new StringBuilder();
            args.Append($"--driver wia ");
            args.Append($"--device \"{_selectedScannerId}\" ");
            args.Append($"--dpi {dpi} ");
            args.Append($"--bitdepth {colorMode} ");
            
            if (duplexMode)
                args.Append("--source duplex ");
            else if (batchMode)
                args.Append("--source feeder ");
            else
                args.Append("--source glass ");

            args.Append($"-o \"{outputPath}\"");
            
            // Debug: Log the command being executed
            System.Diagnostics.Debug.WriteLine($"NAPS2 Command: {_naps2ConsolePath} {args}");

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _naps2ConsolePath,
                    Arguments = args.ToString(),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };

            process.Start();
            
            // Read output asynchronously with timeout to avoid blocking
            var outputBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();
            
            var outputTask = Task.Run(() =>
            {
                try
                {
                    string line;
                    while ((line = process.StandardOutput.ReadLine()) != null)
                    {
                        outputBuilder.AppendLine(line);
                        System.Diagnostics.Debug.WriteLine($"NAPS2 Output Line: {line}");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error reading output: {ex.Message}");
                }
            });
            
            var errorTask = Task.Run(() =>
            {
                try
                {
                    string line;
                    while ((line = process.StandardError.ReadLine()) != null)
                    {
                        errorBuilder.AppendLine(line);
                        System.Diagnostics.Debug.WriteLine($"NAPS2 Error Line: {line}");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error reading error stream: {ex.Message}");
                }
            });

            // Wait for process to complete with timeout (60 seconds for scanning)
            bool finished = process.WaitForExit(60000);
            
            if (!finished)
            {
                process.Kill();
                this.Invoke((MethodInvoker)(() =>
                {
                    MessageBox.Show("Scan operation timed out (60 seconds). The scanner may be busy or not responding.", 
                        "Scan Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }));
                return;
            }

            // Wait a bit for output streams to finish
            Task.WaitAll(new[] { outputTask, errorTask }, 1000);
            
            var output = outputBuilder.ToString();
            var error = errorBuilder.ToString();

            // Debug output
            System.Diagnostics.Debug.WriteLine($"NAPS2 Scan Output: {output}");
            System.Diagnostics.Debug.WriteLine($"NAPS2 Scan Error: {error}");
            System.Diagnostics.Debug.WriteLine($"NAPS2 Exit Code: {process.ExitCode}");
            System.Diagnostics.Debug.WriteLine($"Expected output file: {outputPath}");
            System.Diagnostics.Debug.WriteLine($"File exists: {File.Exists(outputPath)}");

            if (process.ExitCode == 0 && File.Exists(outputPath))
            {
                // Load scanned image
                this.Invoke((MethodInvoker)(() =>
                {
                    ImportImageFile(outputPath);
                    try { File.Delete(outputPath); } catch { }
                }));

                // If batch mode, ask if continue
                bool continueBatch = false;
                int currentPageCount = _pages.Count;
                
                this.Invoke((MethodInvoker)(() =>
                {
                    if (batchCheckBox.Checked)
                    {
                        var continueScan = MessageBox.Show(
                            $"Page {currentPageCount} scanned. Continue scanning?", 
                            "Batch Scan",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        continueBatch = continueScan == DialogResult.Yes;
                    }
                }));
                
                if (continueBatch)
                {
                    PerformScanWithNAPS2(); // Recursive for batch
                }
            }
            else
            {
                this.Invoke((MethodInvoker)(() =>
                {
                    var errorMsg = $"Scan failed (Exit Code: {process.ExitCode}).\n\n";
                    if (!string.IsNullOrWhiteSpace(error))
                        errorMsg += $"Error: {error}\n\n";
                    if (!string.IsNullOrWhiteSpace(output))
                        errorMsg += $"Output: {output}\n\n";
                    errorMsg += $"Expected file: {outputPath}\nFile exists: {File.Exists(outputPath)}";
                    MessageBox.Show(errorMsg, "Scan Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }));
            }
        }

        private void PerformScanWithWIA()
        {
            try
            {
                Type deviceManagerType = Type.GetTypeFromProgID("WIA.DeviceManager");
                if (deviceManagerType == null)
                {
                    throw new Exception("WIA DeviceManager not available");
                }

                dynamic deviceManager = Activator.CreateInstance(deviceManagerType);
                var deviceIndex = int.Parse(_selectedScannerId);
                dynamic device = deviceManager.DeviceInfos[deviceIndex].Connect();

                // Get the scanner item (usually item 1)
                dynamic item = device.Items[1];

                // Set scan properties
                var dpi = int.Parse(DpiCombo.SelectedItem?.ToString() ?? "300");
                
                // Set DPI
                SetWIAProperty(item.Properties, "HorizontalResolution", dpi);
                SetWIAProperty(item.Properties, "VerticalResolution", dpi);

                // Set color mode
                var colorMode = colorCombo.SelectedItem?.ToString() switch
                {
                    "Gray" => 2, // Grayscale
                    "Black&White" => 1, // B&W
                    _ => 0 // Color
                };
                SetWIAProperty(item.Properties, "CurrentIntent", colorMode);

                // Perform the scan - WIA_FORMAT_PNG = {B96B3CAE-0728-11D3-9D7B-0000F81EF32E}
                dynamic imageFile = item.Transfer("{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}");

                // Convert WIA image to System.Drawing.Image
                dynamic fileData = imageFile.FileData;
                byte[] imageBytes = (byte[])fileData.get_BinaryData();
                
                using (var ms = new MemoryStream(imageBytes))
                {
                    var bitmap = new Bitmap(ms);
                    var image = new Bitmap(bitmap); // Clone to avoid disposal issues

                    this.Invoke((MethodInvoker)(() =>
                    {
                        var page = new ScannedPage
                        {
                            PageNumber = _pages.Count + 1,
                            ImageData = image,
                            Rotation = 0,
                            IsDuplex = duplexCheckBox.Checked,
                            IsImported = false
                        };

                        _pages.Add(page);

                        // Update page numbers
                        for (int i = 0; i < _pages.Count; i++)
                        {
                            _pages[i].PageNumber = i + 1;
                        }
                    }));
                }

                // If batch mode, ask if continue
                if (batchCheckBox.Checked)
                {
                    var continueScan = MessageBox.Show(
                        $"Page {_pages.Count} scanned. Continue scanning?",
                        "Batch Scan",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (continueScan == DialogResult.Yes)
                    {
                        PerformScanWithWIA(); // Recursive for batch
                    }
                }
            }
            catch (Exception ex)
            {
                this.Invoke((MethodInvoker)(() =>
                {
                    MessageBox.Show($"WIA Scan Error: {ex.Message}", "Scan Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }));
            }
        }

        private void SetWIAProperty(dynamic properties, string propertyName, object value)
        {
            try
            {
                dynamic prop = properties.get_Item(propertyName);
                if (prop != null)
                {
                    prop.set_Value(value);
                }
            }
            catch
            {
                // Property might not exist, ignore
            }
        }

        // IMPORT FUNCTIONALITY
        private void ImportBtn_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "All Supported Files|*.jpg;*.jpeg;*.png;*.tiff;*.tif;*.bmp;*.gif;*.pdf|Images|*.jpg;*.jpeg;*.png;*.tiff;*.tif;*.bmp;*.gif|PDF Files|*.pdf|All Files|*.*";
                dialog.Multiselect = true;
                dialog.Title = "Import Images and PDF Files";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    ImportFiles(dialog.FileNames);
                }
            }
        }

        private void ImportFiles(string[] filePaths)
        {
            int successCount = 0;
            int failCount = 0;
            var errors = new List<string>();

            try
            {
                statusLabel.Text = "Importing files...";
                this.Cursor = Cursors.WaitCursor;

                foreach (var filePath in filePaths)
                {
                    try
                    {
                        var extension = Path.GetExtension(filePath).ToLowerInvariant();
                        
                        if (extension == ".pdf")
                        {
                            ImportPdfFile(filePath);
                            successCount++;
                        }
                        else if (IsImageFile(extension))
                        {
                            ImportImageFile(filePath);
                            successCount++;
                        }
                        else
                        {
                            failCount++;
                            errors.Add($"{Path.GetFileName(filePath)}: Unsupported file type");
                        }
                    }
                    catch (Exception ex)
                    {
                        failCount++;
                        errors.Add($"{Path.GetFileName(filePath)}: {ex.Message}");
                    }
                }

                if (successCount > 0)
                {
                    // Navigate to last imported page
                    if (_pages.Count > 0)
                    {
                        _currentPageIndex = _pages.Count - 1;
                    }
                    UpdateUI();
                }

                var message = $"Imported {successCount} page(s) successfully.";
                if (failCount > 0)
                {
                    message += $"\n{failCount} file(s) failed to import.";
                    if (errors.Count > 0 && errors.Count <= 5)
                    {
                        message += "\n\nErrors:\n" + string.Join("\n", errors);
                    }
                }

                MessageBox.Show(message, "Import Complete", MessageBoxButtons.OK, 
                    failCount > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
                
                statusLabel.Text = $"✓ {successCount} page(s) imported";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error importing files: {ex.Message}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = "✗ Import failed";
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private bool IsImageFile(string extension)
        {
            return new[] { ".jpg", ".jpeg", ".png", ".tiff", ".tif", ".bmp", ".gif" }.Contains(extension);
        }

        private void ImportImageFile(string filePath)
        {
            try
            {
                using (var originalImage = Image.FromFile(filePath))
                {
                    var image = new Bitmap(originalImage); // Clone to avoid file lock
                    
                    var page = new ScannedPage
                    {
                        PageNumber = _pages.Count + 1,
                        ImageData = image,
                        Rotation = 0,
                        IsImported = true,
                        ImportSourcePath = filePath
                    };
                    
                    _pages.Add(page);
                    
                    // Update page numbers
                    for (int i = 0; i < _pages.Count; i++)
                    {
                        _pages[i].PageNumber = i + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to import image: {ex.Message}", ex);
            }
        }

        private void ImportPdfFile(string filePath)
        {
            try
            {
                // Use PdfiumViewer directly (much faster than NAPS2)
                ImportPdfFileUsingPdfiumViewer(filePath);
                
                // Update page numbers
                for (int i = 0; i < _pages.Count; i++)
                {
                    _pages[i].PageNumber = i + 1;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"PDF import error: {ex.Message}\n{ex.StackTrace}");
                // Fallback to placeholders
                ImportPdfFileAsPlaceholder(filePath);
                
                // Update page numbers
                for (int i = 0; i < _pages.Count; i++)
                {
                    _pages[i].PageNumber = i + 1;
                }
            }
        }
        
        private void ImportPdfFileUsingPdfiumViewer(string filePath)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"PdfiumViewer: Importing PDF {Path.GetFileName(filePath)}");
                
                using (var document = PdfiumPdfDocument.Load(filePath))
                {
                    int pageCount = document.PageCount;
                    System.Diagnostics.Debug.WriteLine($"PdfiumViewer: Rendering {pageCount} pages");
                    
                    for (int i = 0; i < pageCount; i++)
                    {
                        try
                        {
                            // Render at 150 DPI (good quality, reasonable speed)
                            // PdfiumViewer Render method typically takes 4 arguments: (pageIndex, dpiX, dpiY, forPrinting)
                            Image image = null;
                            
                            // Try Render(int, int, int, bool) - most common signature
                            try
                            {
                                image = document.Render(i, 150, 150, false);
                            }
                            catch (Exception ex1)
                            {
                                System.Diagnostics.Debug.WriteLine($"Render(i, 150, 150, false) failed: {ex1.Message}");
                                
                                // Try Render(int, int, int, int) with render flags
                                try
                                {
                                    var renderMethod = document.GetType().GetMethod("Render", new[] { typeof(int), typeof(int), typeof(int), typeof(int) });
                                    if (renderMethod != null)
                                    {
                                        image = renderMethod.Invoke(document, new object[] { i, 150, 150, 0 }) as Image;
                                    }
                                }
                                catch (Exception ex2)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Render with int flags failed: {ex2.Message}");
                                    
                                    // Try finding any Render method via reflection
                                    var allRenderMethods = document.GetType().GetMethods()
                                        .Where(m => m.Name == "Render" && m.ReturnType == typeof(Image))
                                        .ToList();
                                    
                                    if (allRenderMethods.Count > 0)
                                    {
                                        foreach (var method in allRenderMethods)
                                        {
                                            try
                                            {
                                                var parameters = method.GetParameters();
                                                if (parameters.Length >= 3)
                                                {
                                                    var args = new object[parameters.Length];
                                                    args[0] = i; // page index
                                                    args[1] = 150; // dpiX
                                                    args[2] = 150; // dpiY
                                                    
                                                    // Fill remaining parameters with defaults
                                                    for (int p = 3; p < parameters.Length; p++)
                                                    {
                                                        if (parameters[p].ParameterType == typeof(bool))
                                                            args[p] = false;
                                                        else if (parameters[p].ParameterType.IsEnum || parameters[p].ParameterType == typeof(int))
                                                            args[p] = 0;
                                                        else
                                                            args[p] = parameters[p].HasDefaultValue ? parameters[p].DefaultValue : Activator.CreateInstance(parameters[p].ParameterType);
                                                    }
                                                    
                                                    image = method.Invoke(document, args) as Image;
                                                    if (image != null) break;
                                                }
                                            }
                                            catch { }
                                        }
                                    }
                                }
                            }
                            
                            if (image != null)
                            {
                                // Clone the image to avoid disposal issues
                                var bitmap = new Bitmap(image);
                                image.Dispose();
                                
                                var page = new ScannedPage
                                {
                                    PageNumber = _pages.Count + 1,
                                    ImageData = bitmap,
                                    Rotation = 0,
                                    IsImported = true,
                                    ImportSourcePath = filePath,
                                    OriginalPdfPageNumber = i + 1
                                };
                                
                                _pages.Add(page);
                                System.Diagnostics.Debug.WriteLine($"Successfully rendered page {i + 1}/{pageCount}");
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine($"Failed to render page {i + 1}, creating placeholder");
                                var placeholder = CreatePdfPlaceholderImage(filePath, i + 1, 800, 600);
                                var page = new ScannedPage
                                {
                                    PageNumber = _pages.Count + 1,
                                    ImageData = placeholder,
                                    Rotation = 0,
                                    IsImported = true,
                                    ImportSourcePath = filePath,
                                    OriginalPdfPageNumber = i + 1
                                };
                                _pages.Add(page);
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error rendering page {i + 1}: {ex.Message}");
                            // Create placeholder for this page
                            var placeholder = CreatePdfPlaceholderImage(filePath, i + 1, 800, 600);
                            var page = new ScannedPage
                            {
                                PageNumber = _pages.Count + 1,
                                ImageData = placeholder,
                                Rotation = 0,
                                IsImported = true,
                                ImportSourcePath = filePath,
                                OriginalPdfPageNumber = i + 1
                            };
                            _pages.Add(page);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"PdfiumViewer import failed: {ex.Message}");
                throw; // Re-throw to trigger fallback
            }
        }
        
        private void ImportPdfFileUsingReflection(string filePath)
        {
            try
            {
                // Try to load PdfiumViewer assembly
                Assembly pdfiumViewerAssembly = null;
                
                // First, try to find it in already loaded assemblies
                pdfiumViewerAssembly = AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(a => a.GetName().Name == "PdfiumViewer");
                
                // If not found, try to load it explicitly from the packages folder
                if (pdfiumViewerAssembly == null)
                {
                    var packagesPath = Path.Combine(Application.StartupPath, "..", "..", "packages", "PdfiumViewer.2.13.0", "lib", "net40", "PdfiumViewer.dll");
                    if (File.Exists(packagesPath))
                    {
                        pdfiumViewerAssembly = Assembly.LoadFrom(packagesPath);
                    }
                }
                
                // Also try relative to bin folder
                if (pdfiumViewerAssembly == null)
                {
                    var binPath = Path.Combine(Application.StartupPath, "PdfiumViewer.dll");
                    if (File.Exists(binPath))
                    {
                        pdfiumViewerAssembly = Assembly.LoadFrom(binPath);
                    }
                }
                
                if (pdfiumViewerAssembly == null)
                {
                    System.Diagnostics.Debug.WriteLine("PdfiumViewer assembly not found, using placeholders");
                    ImportPdfFileAsPlaceholder(filePath);
                    return;
                }
                
                var pdfDocumentType = pdfiumViewerAssembly.GetType("PdfiumViewer.PdfDocument");
                if (pdfDocumentType == null)
                {
                    System.Diagnostics.Debug.WriteLine("PdfDocument type not found in PdfiumViewer assembly");
                    ImportPdfFileAsPlaceholder(filePath);
                    return;
                }
                
                // Try to find Load method - could be static or instance
                MethodInfo loadMethod = pdfDocumentType.GetMethod("Load", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string) }, null);
                if (loadMethod == null)
                {
                    // Try instance constructor instead
                    var constructor = pdfDocumentType.GetConstructor(new[] { typeof(string) });
                    if (constructor != null)
                    {
                        var document = constructor.Invoke(new object[] { filePath }) as IDisposable;
                        if (document != null)
                        {
                            RenderPdfPages(document, pdfDocumentType, filePath);
                            return;
                        }
                    }
                    System.Diagnostics.Debug.WriteLine("PdfDocument.Load method or constructor not found");
                    ImportPdfFileAsPlaceholder(filePath);
                    return;
                }
                
                // Load the document
                var doc = loadMethod.Invoke(null, new object[] { filePath });
                if (doc == null)
                {
                    System.Diagnostics.Debug.WriteLine("Failed to load PDF document");
                    ImportPdfFileAsPlaceholder(filePath);
                    return;
                }
                
                using (var document = doc as IDisposable)
                {
                    if (document == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Document does not implement IDisposable");
                        ImportPdfFileAsPlaceholder(filePath);
                        return;
                    }
                    
                    RenderPdfPages(document, pdfDocumentType, filePath);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error importing PDF with PdfiumViewer: {ex.Message}\n{ex.StackTrace}");
                // If PdfiumViewer is not available or fails, create placeholders using PdfSharp
                ImportPdfFileAsPlaceholder(filePath);
            }
        }
        
        private void RenderPdfPages(object document, Type pdfDocumentType, string filePath)
        {
            var pageCountProperty = pdfDocumentType.GetProperty("PageCount");
            if (pageCountProperty == null)
            {
                System.Diagnostics.Debug.WriteLine("PageCount property not found");
                ImportPdfFileAsPlaceholder(filePath);
                return;
            }
            
            int pageCount = (int)pageCountProperty.GetValue(document);
            System.Diagnostics.Debug.WriteLine($"PdfiumViewer: Rendering {pageCount} pages from {Path.GetFileName(filePath)}");
            
            // Try different Render method signatures
            // PdfiumViewer Render methods can have different signatures
            MethodInfo renderMethod = null;
            
            // Try: Render(int page, int dpiX, int dpiY)
            renderMethod = pdfDocumentType.GetMethod("Render", new[] { typeof(int), typeof(int), typeof(int) });
            
            // Try: Render(int page, int dpiX, int dpiY, bool forPrinting)
            if (renderMethod == null)
            {
                renderMethod = pdfDocumentType.GetMethod("Render", new[] { typeof(int), typeof(int), typeof(int), typeof(bool) });
            }
            
            // Try: Render(int page, int dpiX, int dpiY, PdfRenderFlags flags)
            if (renderMethod == null)
            {
                var methods = pdfDocumentType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(m => m.Name == "Render" && m.GetParameters().Length >= 3)
                    .ToList();
                
                if (methods.Count > 0)
                {
                    // Try the first overload that matches
                    foreach (var method in methods)
                    {
                        var parameters = method.GetParameters();
                        if (parameters.Length >= 3 && 
                            parameters[0].ParameterType == typeof(int) && 
                            parameters[1].ParameterType == typeof(int) && 
                            parameters[2].ParameterType == typeof(int))
                        {
                            renderMethod = method;
                            break;
                        }
                    }
                }
            }
            
            if (renderMethod == null)
            {
                System.Diagnostics.Debug.WriteLine("Render method not found. Available methods:");
                var allMethods = pdfDocumentType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                foreach (var method in allMethods)
                {
                    System.Diagnostics.Debug.WriteLine($"  {method.Name}({string.Join(", ", method.GetParameters().Select(p => p.ParameterType.Name))})");
                }
                ImportPdfFileAsPlaceholder(filePath);
                return;
            }
            
            System.Diagnostics.Debug.WriteLine($"Found Render method with {renderMethod.GetParameters().Length} parameters");
            
            for (int i = 0; i < pageCount; i++)
            {
                try
                {
                    object imageObj = null;
                    var parameters = renderMethod.GetParameters();
                    
                    // Try different parameter combinations based on the method signature
                    if (parameters.Length == 3)
                    {
                        // Render(int page, int dpiX, int dpiY)
                        imageObj = renderMethod.Invoke(document, new object[] { i, 150, 150 });
                    }
                    else if (parameters.Length == 4)
                    {
                        var param4Type = parameters[3].ParameterType;
                        if (param4Type == typeof(bool))
                        {
                            // Render(int page, int dpiX, int dpiY, bool forPrinting)
                            imageObj = renderMethod.Invoke(document, new object[] { i, 150, 150, false });
                        }
                        else if (param4Type.IsEnum || param4Type == typeof(int))
                        {
                            // Render(int page, int dpiX, int dpiY, PdfRenderFlags/int flags)
                            imageObj = renderMethod.Invoke(document, new object[] { i, 150, 150, 0 });
                        }
                        else
                        {
                            // Try with default value for the 4th parameter
                            imageObj = renderMethod.Invoke(document, new object[] { i, 150, 150, Activator.CreateInstance(param4Type) });
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Unexpected Render method signature with {parameters.Length} parameters");
                    }
                    
                    if (imageObj is Image image)
                    {
                        using (image)
                        {
                            var bitmap = new Bitmap(image);
                            var page = new ScannedPage
                            {
                                PageNumber = _pages.Count + 1,
                                ImageData = bitmap,
                                Rotation = 0,
                                IsImported = true,
                                ImportSourcePath = filePath,
                                OriginalPdfPageNumber = i + 1
                            };
                            _pages.Add(page);
                            System.Diagnostics.Debug.WriteLine($"Successfully rendered page {i + 1}");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Render returned null or wrong type for page {i + 1}");
                        var placeholder = CreatePdfPlaceholderImage(filePath, i + 1, 800, 600);
                        var page = new ScannedPage
                        {
                            PageNumber = _pages.Count + 1,
                            ImageData = placeholder,
                            Rotation = 0,
                            IsImported = true,
                            ImportSourcePath = filePath,
                            OriginalPdfPageNumber = i + 1
                        };
                        _pages.Add(page);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error rendering page {i + 1}: {ex.Message}");
                    // Fall through to placeholder
                    var placeholder = CreatePdfPlaceholderImage(filePath, i + 1, 800, 600);
                    var page = new ScannedPage
                    {
                        PageNumber = _pages.Count + 1,
                        ImageData = placeholder,
                        Rotation = 0,
                        IsImported = true,
                        ImportSourcePath = filePath,
                        OriginalPdfPageNumber = i + 1
                    };
                    _pages.Add(page);
                }
            }
        }

        private void ImportPdfFileAsPlaceholder(string filePath)
        {
            // Fallback: Create placeholder images for each page
            var document = PdfReader.Open(filePath, PdfDocumentOpenMode.ReadOnly);
            
            for (int i = 0; i < document.PageCount; i++)
            {
                var pdfPage = document.Pages[i];
                var width = Math.Max((int)pdfPage.Width.Point, 800);
                var height = Math.Max((int)pdfPage.Height.Point, 600);
                
                var bitmap = CreatePdfPlaceholderImage(filePath, i + 1, width, height);
                
                var page = new ScannedPage
                {
                    PageNumber = _pages.Count + 1,
                    ImageData = bitmap,
                    Rotation = 0,
                    IsImported = true,
                    ImportSourcePath = filePath,
                    OriginalPdfPageNumber = i + 1
                };
                
                _pages.Add(page);
            }
            
            document.Close();
        }

        private Bitmap CreatePdfPlaceholderImage(string filePath, int pageNumber, int width, int height)
        {
            var bitmap = new Bitmap(width, height);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
                g.DrawString($"PDF Page {pageNumber}", new Font("Arial", 16, FontStyle.Bold), Brushes.Black, 10, 10);
                g.DrawString($"From: {Path.GetFileName(filePath)}", new Font("Arial", 10), Brushes.Gray, 10, 40);
                g.DrawString("(Rendering failed)", new Font("Arial", 9), Brushes.LightGray, 10, 60);
            }
            return bitmap;
        }

        // SAVE FUNCTIONALITY
        private void SaveAllBtn_Click(object sender, EventArgs e)
        {
            if (_pages.Count == 0)
            {
                MessageBox.Show("No pages to save.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var format = formatCombo.SelectedItem?.ToString() ?? "PDF (Multi-page)";
                
                if (format == "PDF (Multi-page)")
                {
                    SaveAllAsPdf();
                }
                else
                {
                    SaveAllAsImages(format);
                }
                
                MessageBox.Show($"Saved {_pages.Count} page(s) successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving: {ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveAllAsPdf()
        {
            SavePath = Path.Combine(saveLocationTextBox.Text, filenameTextBox.Text);
            var directory = Path.GetDirectoryName(SavePath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var document = new PdfDocument();

            foreach (var page in _pages)
            {
                try
                {
                    var displayImage = page.GetDisplayImage();
                    if (displayImage == null) continue;

                    var pdfPage = new PdfPage
                    {
                        Width = XUnit.FromPoint(displayImage.Width),
                        Height = XUnit.FromPoint(displayImage.Height)
                    };
                    document.Pages.Add(pdfPage);

                    using (var xGraphics = XGraphics.FromPdfPage(pdfPage))
                    {
                        using (var ms = new MemoryStream())
                        {
                            displayImage.Save(ms, ImageFormat.Png);
                            ms.Position = 0;
                            var xImage = XImage.FromStream(ms);
                            xGraphics.DrawImage(xImage, 0, 0, pdfPage.Width, pdfPage.Height);
                        }
                    }

                    displayImage?.Dispose();
                }
                catch (Exception ex)
                {
                    // Log error but continue with other pages
                    System.Diagnostics.Debug.WriteLine($"Error saving page {page.PageNumber}: {ex.Message}");
                }
            }

            document.Save(SavePath);
            document.Close();
        }

        private void SaveAllAsImages(string format)
        {
            var extension = format switch
            {
                "JPEG" => ".jpg",
                "PNG" => ".png",
                "TIFF" => ".tiff",
                _ => ".png"
            };

            var baseFilename = Path.GetFileNameWithoutExtension(filenameTextBox.Text);
            var directory = saveLocationTextBox.Text;
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var imageFormat = format switch
            {
                "JPEG" => ImageFormat.Jpeg,
                "PNG" => ImageFormat.Png,
                "TIFF" => ImageFormat.Tiff,
                _ => ImageFormat.Png
            };

            for (int i = 0; i < _pages.Count; i++)
            {
                var page = _pages[i];
                var displayImage = page.GetDisplayImage();
                if (displayImage == null) continue;

                var filename = $"{baseFilename}_page{i + 1}{extension}";
                var filePath = Path.Combine(directory, filename);

                displayImage.Save(filePath, imageFormat);
                displayImage?.Dispose();
            }

            SavePath = Path.Combine(directory, $"{baseFilename}_page1{extension}");
        }

        private void SaveCurrentBtn_Click(object sender, EventArgs e)
        {
            if (_currentPageIndex < 0 || _currentPageIndex >= _pages.Count)
            {
                MessageBox.Show("No current page to save.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var format = formatCombo.SelectedItem?.ToString() ?? "PDF (Multi-page)";
                var page = _pages[_currentPageIndex];
                var displayImage = page.GetDisplayImage();
                
                if (displayImage == null)
                {
                    MessageBox.Show("No image data to save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var baseFilename = Path.GetFileNameWithoutExtension(filenameTextBox.Text);
                var directory = saveLocationTextBox.Text;
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                if (format == "PDF (Multi-page)")
                {
                    var filePath = Path.Combine(directory, $"{baseFilename}_page{_currentPageIndex + 1}.pdf");
                    var document = new PdfDocument();
                    var pdfPage = new PdfPage
                    {
                        Width = XUnit.FromPoint(displayImage.Width),
                        Height = XUnit.FromPoint(displayImage.Height)
                    };
                    document.Pages.Add(pdfPage);

                    using (var xGraphics = XGraphics.FromPdfPage(pdfPage))
                    {
                        using (var ms = new MemoryStream())
                        {
                            displayImage.Save(ms, ImageFormat.Png);
                            ms.Position = 0;
                            var xImage = XImage.FromStream(ms);
                            xGraphics.DrawImage(xImage, 0, 0, pdfPage.Width, pdfPage.Height);
                        }
                    }

                    document.Save(filePath);
                    document.Close();
                    SavePath = filePath;
                }
                else
                {
                    var extension = format switch
                    {
                        "JPEG" => ".jpg",
                        "PNG" => ".png",
                        "TIFF" => ".tiff",
                        _ => ".png"
                    };
                    var filePath = Path.Combine(directory, $"{baseFilename}_page{_currentPageIndex + 1}{extension}");
                    
                    var imageFormat = format switch
                    {
                        "JPEG" => ImageFormat.Jpeg,
                        "PNG" => ImageFormat.Png,
                        "TIFF" => ImageFormat.Tiff,
                        _ => ImageFormat.Png
                    };
                    
                    displayImage.Save(filePath, imageFormat);
                    SavePath = filePath;
                }

                displayImage?.Dispose();
                MessageBox.Show("Page saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving: {ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            if (_pages.Count > 0)
            {
                var result = MessageBox.Show("Discard all scanned pages?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                    return;
            }

            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private async void RetryScanBtn_Click(object sender, EventArgs e)
        {
            if (_currentPageIndex < 0 || _currentPageIndex >= _pages.Count)
                return;

            if (string.IsNullOrEmpty(_selectedScannerId))
            {
                MessageBox.Show("Please select a scanner first.", "No Scanner", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var result = MessageBox.Show("Re-scan current page? The existing page will be replaced.", "Confirm", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                    return;

                _isScanning = true;
                retryScanBtn.Enabled = false;
                statusLabel.Text = "Re-scanning...";
                this.Cursor = Cursors.WaitCursor;

                // Check if using NAPS2 or WIA
                bool useNAPS2 = !string.IsNullOrEmpty(_naps2ConsolePath) && File.Exists(_naps2ConsolePath);

                await Task.Run(() =>
                {
                    if (useNAPS2)
                    {
                        RetryScanWithNAPS2();
                    }
                    else
                    {
                        RetryScanWithWIA();
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during re-scan: {ex.Message}", "Scan Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = "✗ Re-scan failed";
            }
            finally
            {
                _isScanning = false;
                retryScanBtn.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }

        private void RetryScanWithNAPS2()
        {
            // Get values from UI on the UI thread
            string colorMode = null;
            string dpi = null;
            
            this.Invoke((MethodInvoker)(() =>
            {
                colorMode = colorCombo.SelectedItem?.ToString() switch
                {
                    "Gray" => "gray",
                    "Black&White" => "bw",
                    _ => "color"
                };
                
                dpi = DpiCombo.SelectedItem?.ToString() ?? "300";
            }));
            
            var outputPath = Path.Combine(_tempScanFolder, $"rescan_{Guid.NewGuid():N}.png");

            var args = new StringBuilder();
            args.Append($"--driver wia ");
            args.Append($"--device \"{_selectedScannerId}\" ");
            args.Append($"--dpi {dpi} ");
            args.Append($"--bitdepth {colorMode} ");
            args.Append($"--source glass ");
            args.Append($"-o \"{outputPath}\"");

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _naps2ConsolePath,
                    Arguments = args.ToString(),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            process.Start();
            
            // Read output with timeout
            var outputBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();
            
            var outputTask = Task.Run(() =>
            {
                try
                {
                    while (!process.StandardOutput.EndOfStream)
                    {
                        var line = process.StandardOutput.ReadLine();
                        if (line != null)
                            outputBuilder.AppendLine(line);
                    }
                }
                catch { }
            });
            
            var errorTask = Task.Run(() =>
            {
                try
                {
                    while (!process.StandardError.EndOfStream)
                    {
                        var line = process.StandardError.ReadLine();
                        if (line != null)
                            errorBuilder.AppendLine(line);
                    }
                }
                catch { }
            });

            bool finished = process.WaitForExit(60000);
            
            if (!finished)
            {
                process.Kill();
                this.Invoke((MethodInvoker)(() =>
                {
                    MessageBox.Show("Re-scan operation timed out.", "Scan Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    statusLabel.Text = "✗ Re-scan failed (timeout)";
                }));
                return;
            }

            Task.WaitAll(new[] { outputTask, errorTask }, 1000);
            var output = outputBuilder.ToString();
            var error = errorBuilder.ToString();

            if (process.ExitCode == 0 && File.Exists(outputPath))
            {
                this.Invoke((MethodInvoker)(() =>
                {
                    // Dispose old image
                    _pages[_currentPageIndex].ImageData?.Dispose();
                    
                    // Replace with new scan
                    using (var img = Image.FromFile(outputPath))
                    {
                        _pages[_currentPageIndex].ImageData = new Bitmap(img);
                        _pages[_currentPageIndex].Rotation = 0;
                        _pages[_currentPageIndex].CropRegion = null;
                        _pages[_currentPageIndex].IsImported = false;
                    }
                    
                    try { File.Delete(outputPath); } catch { }
                    UpdateUI();
                    statusLabel.Text = "✓ Page re-scanned successfully";
                }));
            }
            else
            {
                this.Invoke((MethodInvoker)(() =>
                {
                    var errorMsg = $"Re-scan failed (Exit Code: {process.ExitCode})";
                    if (!string.IsNullOrWhiteSpace(error))
                        errorMsg += $"\nError: {error}";
                    MessageBox.Show(errorMsg, "Scan Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    statusLabel.Text = "✗ Re-scan failed";
                }));
            }
        }

        private void RetryScanWithWIA()
        {
            try
            {
                Type deviceManagerType = Type.GetTypeFromProgID("WIA.DeviceManager");
                if (deviceManagerType == null)
                {
                    throw new Exception("WIA DeviceManager not available");
                }

                dynamic deviceManager = Activator.CreateInstance(deviceManagerType);
                var deviceIndex = int.Parse(_selectedScannerId);
                dynamic device = deviceManager.DeviceInfos[deviceIndex].Connect();

                dynamic item = device.Items[1];

                var dpi = int.Parse(DpiCombo.SelectedItem?.ToString() ?? "300");
                SetWIAProperty(item.Properties, "HorizontalResolution", dpi);
                SetWIAProperty(item.Properties, "VerticalResolution", dpi);

                var colorMode = colorCombo.SelectedItem?.ToString() switch
                {
                    "Gray" => 2,
                    "Black&White" => 1,
                    _ => 0
                };
                SetWIAProperty(item.Properties, "CurrentIntent", colorMode);

                dynamic imageFile = item.Transfer("{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}");

                dynamic fileData = imageFile.FileData;
                byte[] imageBytes = (byte[])fileData.get_BinaryData();
                
                using (var ms = new MemoryStream(imageBytes))
                {
                    var bitmap = new Bitmap(ms);
                    var image = new Bitmap(bitmap);

                    this.Invoke((MethodInvoker)(() =>
                    {
                        // Dispose old image
                        _pages[_currentPageIndex].ImageData?.Dispose();
                        
                        // Replace with new scan
                        _pages[_currentPageIndex].ImageData = image;
                        _pages[_currentPageIndex].Rotation = 0;
                        _pages[_currentPageIndex].CropRegion = null;
                        _pages[_currentPageIndex].IsImported = false;
                        
                        UpdateUI();
                        statusLabel.Text = "✓ Page re-scanned successfully";
                    }));
                }
            }
            catch (Exception ex)
            {
                this.Invoke((MethodInvoker)(() =>
                {
                    MessageBox.Show($"WIA Re-scan Error: {ex.Message}", "Scan Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    statusLabel.Text = "✗ Re-scan failed";
                }));
            }
        }

        private void BrowseBtn_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select folder to save scanned documents";
                if (!string.IsNullOrEmpty(saveLocationTextBox.Text))
                    dialog.SelectedPath = saveLocationTextBox.Text;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    saveLocationTextBox.Text = dialog.SelectedPath;
                }
            }
        }

        // Settings event handlers
        private void BrightnessTrackBar_ValueChanged(object sender, EventArgs e)
        {
            // Disable auto-optimize when manually adjusting
            if (autoOptimizeCheckBox != null)
            {
                autoOptimizeCheckBox.Checked = false;
            }
            
            // Apply brightness to preview in real-time
            if (_currentPageIndex >= 0 && _currentPageIndex < _pages.Count)
            {
                UpdatePreview();
            }
        }

        private void ContrastTrackBar_ValueChanged(object sender, EventArgs e)
        {
            // Disable auto-optimize when manually adjusting
            if (autoOptimizeCheckBox != null)
            {
                autoOptimizeCheckBox.Checked = false;
            }
            
            // Apply contrast to preview in real-time
            if (_currentPageIndex >= 0 && _currentPageIndex < _pages.Count)
            {
                UpdatePreview();
            }
        }
        
        private void AutoOptimizeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (autoOptimizeCheckBox.Checked && _currentPageIndex >= 0 && _currentPageIndex < _pages.Count)
            {
                var page = _pages[_currentPageIndex];
                var displayImage = page.GetDisplayImage();
                if (displayImage != null)
                {
                    var optimized = CalculateAutoOptimize(displayImage);
                    brightnessTrackBar.Value = optimized.brightness;
                    contrastTrackBar.Value = optimized.contrast;
                    displayImage?.Dispose();
                }
            }
            UpdatePreview();
        }
        
        private (int brightness, int contrast) CalculateAutoOptimize(Image image)
        {
            // Analyze image to determine optimal brightness and contrast
            // This is a simple implementation - can be enhanced
            var bitmap = new Bitmap(image);
            long totalBrightness = 0;
            long pixelCount = 0;
            int minBrightness = 255;
            int maxBrightness = 0;
            
            // Sample pixels (every 10th pixel for performance)
            for (int y = 0; y < bitmap.Height; y += 10)
            {
                for (int x = 0; x < bitmap.Width; x += 10)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    int pixelBrightness = (int)((pixel.R + pixel.G + pixel.B) / 3.0);
                    totalBrightness += pixelBrightness;
                    pixelCount++;
                    if (pixelBrightness < minBrightness) minBrightness = pixelBrightness;
                    if (pixelBrightness > maxBrightness) maxBrightness = pixelBrightness;
                }
            }
            
            bitmap.Dispose();
            
            if (pixelCount == 0) return (50, 50); // Neutral values
            
            int avgBrightness = (int)(totalBrightness / pixelCount);
            int brightnessRange = maxBrightness - minBrightness;
            
            // Calculate optimal brightness (target: ~128 average)
            int brightnessAdjust = 128 - avgBrightness;
            // Map to trackbar range: -50 to +50
            int optimalBrightness = 50 + (int)(brightnessAdjust / 2.55f);
            optimalBrightness = Math.Max(0, Math.Min(100, optimalBrightness));
            
            // Calculate optimal contrast (target: good range)
            // If range is small, increase contrast; if range is large, reduce contrast
            int contrastAdjust = 128 - brightnessRange;
            // Map to trackbar range: -50 to +50
            int optimalContrast = 50 + (int)(contrastAdjust / 5.0f);
            optimalContrast = Math.Max(0, Math.Min(100, optimalContrast));
            
            return (optimalBrightness, optimalContrast);
        }

        private void FormatCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFilename();
        }
        
        private void ScanForm_Load(object sender, EventArgs e)
        {
            // Lock previewPanel to its allocated TableLayoutPanel cell size
            // This prevents the panel from growing and causing column scrolling
            var previewPanel = previewPictureBox.Parent as Panel;
            if (previewPanel != null)
            {
                // Wait for layout to complete, then lock the size
                this.Shown += (s, args) =>
                {
                    // Lock the size to allocated size (after layout is complete)
                    var currentSize = previewPanel.Size;
                    previewPanel.MaximumSize = currentSize;
                    // Enable AutoScroll for scrolling within the cell
                    previewPanel.AutoScroll = true;
                };
            }
        }

        // Keyboard shortcuts
        private void ScanForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.R)
            {
                RotateRightBtn_Click(sender, e);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.L)
            {
                RotateLeftBtn_Click(sender, e);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.F)
            {
                Flip180Btn_Click(sender, e);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.Oemplus || (e.Control && e.KeyCode == Keys.Add))
            {
                ZoomInBtn_Click(sender, e);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.OemMinus || (e.Control && e.KeyCode == Keys.Subtract))
            {
                ZoomOutBtn_Click(sender, e);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Left)
            {
                PrevBtn_Click(sender, e);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Right)
            {
                NextBtn_Click(sender, e);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Home)
            {
                FirstBtn_Click(sender, e);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.End)
            {
                LastBtn_Click(sender, e);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete)
            {
                DeleteCurrentPage();
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                SaveAllBtn_Click(sender, e);
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.I)
            {
                ImportBtn_Click(sender, e);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                CancelBtn_Click(sender, e);
                e.Handled = true;
            }
        }

        private void DeleteCurrentPage()
        {
            if (_currentPageIndex >= 0 && _currentPageIndex < _pages.Count)
            {
                var result = MessageBox.Show("Delete current page?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    _pages[_currentPageIndex].ImageData?.Dispose();
                    _pages.RemoveAt(_currentPageIndex);

                    if (_currentPageIndex >= _pages.Count)
                        _currentPageIndex = _pages.Count - 1;
                    else if (_currentPageIndex < 0)
                        _currentPageIndex = -1;

                    // Update page numbers
                    for (int i = 0; i < _pages.Count; i++)
                    {
                        _pages[i].PageNumber = i + 1;
                    }

                    UpdateUI();
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Clean up images
            foreach (var page in _pages)
            {
                page.ImageData?.Dispose();
            }
            _pages.Clear();

            // Clean up temp folder
            try
            {
                if (Directory.Exists(_tempScanFolder))
                {
                    Directory.Delete(_tempScanFolder, true);
                }
            }
            catch { }

            base.OnFormClosing(e);
        }
    }
}
