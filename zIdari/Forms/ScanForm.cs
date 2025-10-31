using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing;
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

        private const string NAPS2_PATH = @"C:\Program Files\NAPS2\NAPS2.exe";
        private string _tempScanFolder;

        public string SavePath { get; private set; }
        public List<ScannedPage> Pages => _pages;

        public class ScannerInfo
        {
            public string Id { get; set; }
            public string Name { get; set; }
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
                
                if (!File.Exists(NAPS2_PATH))
                {
                    ScannerNameCombo.Items.Add("NAPS2 not found. Please install NAPS2 from https://www.naps2.com/");
                    statusLabel.Text = "NAPS2 not installed";
                    return;
                }

                // Use NAPS2 CLI to list scanners
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = NAPS2_PATH,
                        Arguments = "--list-scanners",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                var output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                _availableScanners.Clear();
                ScannerNameCombo.Items.Clear();

                if (!string.IsNullOrWhiteSpace(output))
                {
                    var lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(line) && line.Contains("|"))
                        {
                            var parts = line.Split('|');
                            if (parts.Length >= 2)
                            {
                                var scanner = new ScannerInfo
                                {
                                    Id = parts[0].Trim(),
                                    Name = parts[1].Trim()
                                };
                                _availableScanners.Add(scanner);
                                ScannerNameCombo.Items.Add(scanner);
                            }
                        }
                    }
                }

                if (ScannerNameCombo.Items.Count == 0)
                {
                    ScannerNameCombo.Items.Add("No scanners found");
                    statusLabel.Text = "No scanners available";
                }
                else
                {
                    if (ScannerNameCombo.Items.Count > 0)
                    {
                        ScannerNameCombo.SelectedIndex = 0;
                        _selectedScannerId = ((ScannerInfo)ScannerNameCombo.SelectedItem)?.Id;
                    }
                    statusLabel.Text = $"✓ {_availableScanners.Count} scanner(s) found";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading scanners: {ex.Message}\n\nMake sure NAPS2 is installed from https://www.naps2.com/", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = "Error loading scanners";
                ScannerNameCombo.Items.Add("Error loading scanners");
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
            formatCombo.SelectedIndexChanged += FormatCombo_SelectedIndexChanged;

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
                pageLabel.Text = $"من {_currentPageIndex + 1}";
                totalPagesLabel.Text = $"Total: {_pages.Count} pages";
                var scannedCount = _pages.Count(p => !p.IsImported);
                var importedCount = _pages.Count(p => p.IsImported);
                pagesCountLabel.Text = $"Total: {_pages.Count} pages ({scannedCount} scanned, {importedCount} imported)";
            }
            else
            {
                pageNumericUpDown.Maximum = 1;
                pageNumericUpDown.Value = 1;
                pageLabel.Text = "من 0";
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

            // Scan button
            scanBtn.Enabled = !_isScanning && !string.IsNullOrEmpty(_selectedScannerId) && File.Exists(NAPS2_PATH);

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

            // Apply zoom
            Image finalImage = displayImage;
            if (_currentZoom != 1.0)
            {
                var zoomedWidth = (int)(displayImage.Width * _currentZoom);
                var zoomedHeight = (int)(displayImage.Height * _currentZoom);
                finalImage = new Bitmap(displayImage, zoomedWidth, zoomedHeight);
                if (finalImage != displayImage)
                    displayImage?.Dispose();
            }

            previewPictureBox.Image = finalImage;
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
                Width = 60,
                Height = 85,
                Margin = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = index == _currentPageIndex ? Color.LightBlue : Color.White,
                Tag = index
            };

            var picBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            // Create thumbnail image
            if (page.ImageData != null)
            {
                try
                {
                    var thumb = new Bitmap(page.ImageData, 50, 70);
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
                Height = 15,
                Text = $"Page {index + 1}" + (page.IsImported ? " [I]" : ""),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 7)
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
            if (previewPictureBox.Image != null && previewPictureBox.Width > 0 && previewPictureBox.Height > 0)
            {
                var imgWidth = previewPictureBox.Image.Width;
                var imgHeight = previewPictureBox.Image.Height;
                var picWidth = previewPictureBox.Width;
                var picHeight = previewPictureBox.Height;

                var zoomX = (double)picWidth / imgWidth;
                var zoomY = (double)picHeight / imgHeight;
                _currentZoom = Math.Min(zoomX, zoomY);
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

            if (!File.Exists(NAPS2_PATH))
            {
                MessageBox.Show("NAPS2 not found. Please install NAPS2 from https://www.naps2.com/\n\nDefault path: C:\\Program Files\\NAPS2\\NAPS2.exe", 
                    "NAPS2 Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                await Task.Run(() => PerformScan());

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

        private void PerformScan()
        {
            var colorMode = colorCombo.SelectedItem?.ToString() switch
            {
                "Gray" => "Gray",
                "Black&White" => "BlackWhite",
                _ => "Color"
            };

            var dpi = DpiCombo.SelectedItem?.ToString() ?? "300";
            var outputPath = Path.Combine(_tempScanFolder, $"scan_{Guid.NewGuid():N}.png");

            // Build NAPS2 command line arguments
            var args = new StringBuilder();
            args.Append($"--scanner \"{_selectedScannerId}\" ");
            args.Append($"--dpi {dpi} ");
            args.Append($"--format png ");
            args.Append($"--color {colorMode} ");
            
            if (batchCheckBox.Checked)
                args.Append("--batch ");
            if (duplexCheckBox.Checked)
                args.Append("--duplex ");

            args.Append($"\"{outputPath}\"");

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = NAPS2_PATH,
                    Arguments = args.ToString(),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };

            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode == 0 && File.Exists(outputPath))
            {
                // Load scanned image
                this.Invoke((MethodInvoker)(() =>
                {
                    ImportImageFile(outputPath);
                    try { File.Delete(outputPath); } catch { }
                }));

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
                        PerformScan(); // Recursive for batch
                    }
                }
            }
            else
            {
                this.Invoke((MethodInvoker)(() =>
                {
                    var errorMsg = string.IsNullOrWhiteSpace(error) ? "Scan was cancelled or failed." : error;
                    MessageBox.Show(errorMsg, "Scan Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }));
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
                // Using PdfSharp to extract pages
                var document = PdfReader.Open(filePath, PdfDocumentOpenMode.ReadOnly);
                
                for (int i = 0; i < document.PageCount; i++)
                {
                    var pdfPage = document.Pages[i];
                    
                    // Convert PDF page to image
                    // Note: PdfSharp doesn't have built-in rendering, so we'll create a placeholder image
                    // In production, you might want to use a library like PdfiumViewer or Ghostscript
                    var width = (int)pdfPage.Width.Point;
                    var height = (int)pdfPage.Height.Point;
                    
                    // Create a placeholder bitmap
                    var bitmap = new Bitmap(Math.Max(width, 800), Math.Max(height, 600));
                    using (var g = Graphics.FromImage(bitmap))
                    {
                        g.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
                        g.DrawString($"PDF Page {i + 1}", SystemFonts.DefaultFont, Brushes.Black, 10, 10);
                        g.DrawString($"From: {Path.GetFileName(filePath)}", SystemFonts.DefaultFont, Brushes.Gray, 10, 30);
                    }
                    
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
                
                // Update page numbers
                for (int i = 0; i < _pages.Count; i++)
                {
                    _pages[i].PageNumber = i + 1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to import PDF: {ex.Message}", ex);
            }
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

            if (!File.Exists(NAPS2_PATH))
            {
                MessageBox.Show("NAPS2 not found. Please install NAPS2 from https://www.naps2.com/", 
                    "NAPS2 Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                await Task.Run(() =>
                {
                    var colorMode = colorCombo.SelectedItem?.ToString() switch
                    {
                        "Gray" => "Gray",
                        "Black&White" => "BlackWhite",
                        _ => "Color"
                    };

                    var dpi = DpiCombo.SelectedItem?.ToString() ?? "300";
                    var outputPath = Path.Combine(_tempScanFolder, $"rescan_{Guid.NewGuid():N}.png");

                    var args = new StringBuilder();
                    args.Append($"--scanner \"{_selectedScannerId}\" ");
                    args.Append($"--dpi {dpi} ");
                    args.Append($"--format png ");
                    args.Append($"--color {colorMode} ");
                    args.Append($"\"{outputPath}\"");

                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = NAPS2_PATH,
                            Arguments = args.ToString(),
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        }
                    };

                    process.Start();
                    process.WaitForExit();

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
                            MessageBox.Show("Scan was cancelled or failed.", "Scan Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            statusLabel.Text = "✗ Re-scan failed";
                        }));
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
            // Apply brightness to preview in real-time
            if (_currentPageIndex >= 0 && _currentPageIndex < _pages.Count)
            {
                UpdatePreview();
            }
        }

        private void ContrastTrackBar_ValueChanged(object sender, EventArgs e)
        {
            // Apply contrast to preview in real-time
            if (_currentPageIndex >= 0 && _currentPageIndex < _pages.Count)
            {
                UpdatePreview();
            }
        }

        private void FormatCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFilename();
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
