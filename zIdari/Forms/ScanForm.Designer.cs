namespace zIdari.Forms
{
    partial class ScanForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.thumbnailPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.previewPanel = new System.Windows.Forms.Panel();
            this.previewPictureBox = new System.Windows.Forms.PictureBox();
            this.zoomPanel = new System.Windows.Forms.Panel();
            this.zoomCombo = new System.Windows.Forms.ComboBox();
            this.zoomInBtn = new System.Windows.Forms.Button();
            this.zoomOutBtn = new System.Windows.Forms.Button();
            this.fitBtn = new System.Windows.Forms.Button();
            this.actualSizeBtn = new System.Windows.Forms.Button();
            this.rotationPanel = new System.Windows.Forms.Panel();
            this.rotateLeftBtn = new System.Windows.Forms.Button();
            this.rotateRightBtn = new System.Windows.Forms.Button();
            this.flip180Btn = new System.Windows.Forms.Button();
            this.resetRotationBtn = new System.Windows.Forms.Button();
            this.cropPanel = new System.Windows.Forms.Panel();
            this.totalPagesLabel = new System.Windows.Forms.Label();
            this.firstBtn = new System.Windows.Forms.Button();
            this.lastBtn = new System.Windows.Forms.Button();
            this.cropBtn = new System.Windows.Forms.Button();
            this.prevBtn = new System.Windows.Forms.Button();
            this.applyCropBtn = new System.Windows.Forms.Button();
            this.clearCropBtn = new System.Windows.Forms.Button();
            this.pageNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.nextBtn = new System.Windows.Forms.Button();
            this.filenamePanel = new System.Windows.Forms.Panel();
            this.filenameLabel = new System.Windows.Forms.Label();
            this.filenameTextBox = new System.Windows.Forms.TextBox();
            this.saveLocationPanel = new System.Windows.Forms.Panel();
            this.saveLocationLabel = new System.Windows.Forms.Label();
            this.saveLocationTextBox = new System.Windows.Forms.TextBox();
            this.browseBtn = new System.Windows.Forms.Button();
            this.actionButtonsPanel = new System.Windows.Forms.Panel();
            this.saveAllBtn = new System.Windows.Forms.Button();
            this.saveCurrentBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.retryScanBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.colorCombo = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DpiCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ScannerNameCombo = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.batchCheckBox = new System.Windows.Forms.CheckBox();
            this.duplexCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.brightnessTrackBar = new System.Windows.Forms.TrackBar();
            this.contrastTrackBar = new System.Windows.Forms.TrackBar();
            this.autoOptimizeCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.formatCombo = new System.Windows.Forms.ComboBox();
            this.importBtn = new System.Windows.Forms.Button();
            this.scanBtn = new System.Windows.Forms.Button();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.pagesCountLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel1.SuspendLayout();
            this.previewPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).BeginInit();
            this.zoomPanel.SuspendLayout();
            this.rotationPanel.SuspendLayout();
            this.cropPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageNumericUpDown)).BeginInit();
            this.filenamePanel.SuspendLayout();
            this.saveLocationPanel.SuspendLayout();
            this.actionButtonsPanel.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.brightnessTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contrastTrackBar)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 132F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel1.Controls.Add(this.thumbnailPanel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.previewPanel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1206, 872);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // thumbnailPanel
            // 
            this.thumbnailPanel.AutoScroll = true;
            this.thumbnailPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.thumbnailPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.thumbnailPanel.Location = new System.Drawing.Point(1077, 3);
            this.thumbnailPanel.Name = "thumbnailPanel";
            this.thumbnailPanel.Size = new System.Drawing.Size(126, 866);
            this.thumbnailPanel.TabIndex = 5;
            this.thumbnailPanel.WrapContents = false;
            // 
            // previewPanel
            // 
            this.previewPanel.Controls.Add(this.previewPictureBox);
            this.previewPanel.Controls.Add(this.zoomPanel);
            this.previewPanel.Controls.Add(this.rotationPanel);
            this.previewPanel.Controls.Add(this.cropPanel);
            this.previewPanel.Controls.Add(this.filenamePanel);
            this.previewPanel.Controls.Add(this.saveLocationPanel);
            this.previewPanel.Controls.Add(this.actionButtonsPanel);
            this.previewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewPanel.Location = new System.Drawing.Point(253, 3);
            this.previewPanel.Name = "previewPanel";
            this.previewPanel.Size = new System.Drawing.Size(818, 866);
            this.previewPanel.TabIndex = 1;
            // 
            // previewPictureBox
            // 
            this.previewPictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.previewPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.previewPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewPictureBox.Location = new System.Drawing.Point(0, 30);
            this.previewPictureBox.Name = "previewPictureBox";
            this.previewPictureBox.Size = new System.Drawing.Size(818, 691);
            this.previewPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.previewPictureBox.TabIndex = 0;
            this.previewPictureBox.TabStop = false;
            // 
            // zoomPanel
            // 
            this.zoomPanel.Controls.Add(this.zoomCombo);
            this.zoomPanel.Controls.Add(this.zoomInBtn);
            this.zoomPanel.Controls.Add(this.zoomOutBtn);
            this.zoomPanel.Controls.Add(this.fitBtn);
            this.zoomPanel.Controls.Add(this.actualSizeBtn);
            this.zoomPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.zoomPanel.Location = new System.Drawing.Point(0, 0);
            this.zoomPanel.Name = "zoomPanel";
            this.zoomPanel.Size = new System.Drawing.Size(818, 30);
            this.zoomPanel.TabIndex = 1;
            // 
            // zoomCombo
            // 
            this.zoomCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.zoomCombo.FormattingEnabled = true;
            this.zoomCombo.Location = new System.Drawing.Point(100, 3);
            this.zoomCombo.Name = "zoomCombo";
            this.zoomCombo.Size = new System.Drawing.Size(80, 21);
            this.zoomCombo.TabIndex = 0;
            // 
            // zoomInBtn
            // 
            this.zoomInBtn.Location = new System.Drawing.Point(186, 3);
            this.zoomInBtn.Name = "zoomInBtn";
            this.zoomInBtn.Size = new System.Drawing.Size(30, 23);
            this.zoomInBtn.TabIndex = 1;
            this.zoomInBtn.Text = "+";
            this.zoomInBtn.UseVisualStyleBackColor = true;
            // 
            // zoomOutBtn
            // 
            this.zoomOutBtn.Location = new System.Drawing.Point(222, 3);
            this.zoomOutBtn.Name = "zoomOutBtn";
            this.zoomOutBtn.Size = new System.Drawing.Size(30, 23);
            this.zoomOutBtn.TabIndex = 2;
            this.zoomOutBtn.Text = "-";
            this.zoomOutBtn.UseVisualStyleBackColor = true;
            // 
            // fitBtn
            // 
            this.fitBtn.Location = new System.Drawing.Point(258, 3);
            this.fitBtn.Name = "fitBtn";
            this.fitBtn.Size = new System.Drawing.Size(60, 23);
            this.fitBtn.TabIndex = 3;
            this.fitBtn.Text = "ملئ";
            this.fitBtn.UseVisualStyleBackColor = true;
            // 
            // actualSizeBtn
            // 
            this.actualSizeBtn.Location = new System.Drawing.Point(324, 3);
            this.actualSizeBtn.Name = "actualSizeBtn";
            this.actualSizeBtn.Size = new System.Drawing.Size(70, 23);
            this.actualSizeBtn.TabIndex = 4;
            this.actualSizeBtn.Text = "الحجم الأصلي";
            this.actualSizeBtn.UseVisualStyleBackColor = true;
            // 
            // rotationPanel
            // 
            this.rotationPanel.Controls.Add(this.rotateLeftBtn);
            this.rotationPanel.Controls.Add(this.rotateRightBtn);
            this.rotationPanel.Controls.Add(this.flip180Btn);
            this.rotationPanel.Controls.Add(this.cropBtn);
            this.rotationPanel.Controls.Add(this.resetRotationBtn);
            this.rotationPanel.Controls.Add(this.clearCropBtn);
            this.rotationPanel.Controls.Add(this.applyCropBtn);
            this.rotationPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rotationPanel.Location = new System.Drawing.Point(0, 721);
            this.rotationPanel.Name = "rotationPanel";
            this.rotationPanel.Size = new System.Drawing.Size(818, 30);
            this.rotationPanel.TabIndex = 2;
            // 
            // rotateLeftBtn
            // 
            this.rotateLeftBtn.Location = new System.Drawing.Point(3, 3);
            this.rotateLeftBtn.Name = "rotateLeftBtn";
            this.rotateLeftBtn.Size = new System.Drawing.Size(80, 23);
            this.rotateLeftBtn.TabIndex = 0;
            this.rotateLeftBtn.Text = "↻ تدوير يسار";
            this.rotateLeftBtn.UseVisualStyleBackColor = true;
            // 
            // rotateRightBtn
            // 
            this.rotateRightBtn.Location = new System.Drawing.Point(89, 3);
            this.rotateRightBtn.Name = "rotateRightBtn";
            this.rotateRightBtn.Size = new System.Drawing.Size(80, 23);
            this.rotateRightBtn.TabIndex = 1;
            this.rotateRightBtn.Text = "↺ تدوير يمين";
            this.rotateRightBtn.UseVisualStyleBackColor = true;
            // 
            // flip180Btn
            // 
            this.flip180Btn.Location = new System.Drawing.Point(175, 3);
            this.flip180Btn.Name = "flip180Btn";
            this.flip180Btn.Size = new System.Drawing.Size(80, 23);
            this.flip180Btn.TabIndex = 2;
            this.flip180Btn.Text = "🔄 قلب 180°";
            this.flip180Btn.UseVisualStyleBackColor = true;
            // 
            // resetRotationBtn
            // 
            this.resetRotationBtn.Location = new System.Drawing.Point(261, 3);
            this.resetRotationBtn.Name = "resetRotationBtn";
            this.resetRotationBtn.Size = new System.Drawing.Size(70, 23);
            this.resetRotationBtn.TabIndex = 3;
            this.resetRotationBtn.Text = "↺ إعادة ";
            this.resetRotationBtn.UseVisualStyleBackColor = true;
            // 
            // cropPanel
            // 
            this.cropPanel.Controls.Add(this.totalPagesLabel);
            this.cropPanel.Controls.Add(this.firstBtn);
            this.cropPanel.Controls.Add(this.lastBtn);
            this.cropPanel.Controls.Add(this.prevBtn);
            this.cropPanel.Controls.Add(this.pageNumericUpDown);
            this.cropPanel.Controls.Add(this.nextBtn);
            this.cropPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cropPanel.Location = new System.Drawing.Point(0, 751);
            this.cropPanel.Name = "cropPanel";
            this.cropPanel.Size = new System.Drawing.Size(818, 30);
            this.cropPanel.TabIndex = 3;
            // 
            // totalPagesLabel
            // 
            this.totalPagesLabel.AutoSize = true;
            this.totalPagesLabel.Location = new System.Drawing.Point(483, 7);
            this.totalPagesLabel.Name = "totalPagesLabel";
            this.totalPagesLabel.Size = new System.Drawing.Size(42, 13);
            this.totalPagesLabel.TabIndex = 6;
            this.totalPagesLabel.Text = "0 صفحة";
            // 
            // firstBtn
            // 
            this.firstBtn.Location = new System.Drawing.Point(443, 2);
            this.firstBtn.Name = "firstBtn";
            this.firstBtn.Size = new System.Drawing.Size(34, 23);
            this.firstBtn.TabIndex = 0;
            this.firstBtn.Text = "►►";
            this.firstBtn.UseVisualStyleBackColor = true;
            // 
            // lastBtn
            // 
            this.lastBtn.Location = new System.Drawing.Point(275, 2);
            this.lastBtn.Name = "lastBtn";
            this.lastBtn.Size = new System.Drawing.Size(34, 23);
            this.lastBtn.TabIndex = 5;
            this.lastBtn.Text = "◄◄";
            this.lastBtn.UseVisualStyleBackColor = true;
            // 
            // cropBtn
            // 
            this.cropBtn.Location = new System.Drawing.Point(737, 1);
            this.cropBtn.Name = "cropBtn";
            this.cropBtn.Size = new System.Drawing.Size(60, 23);
            this.cropBtn.TabIndex = 0;
            this.cropBtn.Text = "قص";
            this.cropBtn.UseVisualStyleBackColor = true;
            // 
            // prevBtn
            // 
            this.prevBtn.Location = new System.Drawing.Point(405, 2);
            this.prevBtn.Name = "prevBtn";
            this.prevBtn.Size = new System.Drawing.Size(34, 23);
            this.prevBtn.TabIndex = 1;
            this.prevBtn.Text = "►";
            this.prevBtn.UseVisualStyleBackColor = true;
            // 
            // applyCropBtn
            // 
            this.applyCropBtn.Location = new System.Drawing.Point(661, 1);
            this.applyCropBtn.Name = "applyCropBtn";
            this.applyCropBtn.Size = new System.Drawing.Size(70, 23);
            this.applyCropBtn.TabIndex = 1;
            this.applyCropBtn.Text = "تطبيق";
            this.applyCropBtn.UseVisualStyleBackColor = true;
            // 
            // clearCropBtn
            // 
            this.clearCropBtn.Location = new System.Drawing.Point(585, 1);
            this.clearCropBtn.Name = "clearCropBtn";
            this.clearCropBtn.Size = new System.Drawing.Size(70, 23);
            this.clearCropBtn.TabIndex = 2;
            this.clearCropBtn.Text = "إلغاء القص";
            this.clearCropBtn.UseVisualStyleBackColor = true;
            // 
            // pageNumericUpDown
            // 
            this.pageNumericUpDown.Location = new System.Drawing.Point(357, 5);
            this.pageNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pageNumericUpDown.Name = "pageNumericUpDown";
            this.pageNumericUpDown.Size = new System.Drawing.Size(40, 20);
            this.pageNumericUpDown.TabIndex = 3;
            this.pageNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nextBtn
            // 
            this.nextBtn.Location = new System.Drawing.Point(315, 2);
            this.nextBtn.Name = "nextBtn";
            this.nextBtn.Size = new System.Drawing.Size(34, 23);
            this.nextBtn.TabIndex = 4;
            this.nextBtn.Text = "◄";
            this.nextBtn.UseVisualStyleBackColor = true;
            // 
            // filenamePanel
            // 
            this.filenamePanel.Controls.Add(this.filenameLabel);
            this.filenamePanel.Controls.Add(this.filenameTextBox);
            this.filenamePanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.filenamePanel.Location = new System.Drawing.Point(0, 781);
            this.filenamePanel.Name = "filenamePanel";
            this.filenamePanel.Size = new System.Drawing.Size(818, 25);
            this.filenamePanel.TabIndex = 6;
            // 
            // filenameLabel
            // 
            this.filenameLabel.AutoSize = true;
            this.filenameLabel.Location = new System.Drawing.Point(840, 5);
            this.filenameLabel.Name = "filenameLabel";
            this.filenameLabel.Size = new System.Drawing.Size(52, 13);
            this.filenameLabel.TabIndex = 0;
            this.filenameLabel.Text = "Filename:";
            // 
            // filenameTextBox
            // 
            this.filenameTextBox.Location = new System.Drawing.Point(3, 3);
            this.filenameTextBox.Name = "filenameTextBox";
            this.filenameTextBox.ReadOnly = true;
            this.filenameTextBox.Size = new System.Drawing.Size(794, 20);
            this.filenameTextBox.TabIndex = 1;
            // 
            // saveLocationPanel
            // 
            this.saveLocationPanel.Controls.Add(this.saveLocationLabel);
            this.saveLocationPanel.Controls.Add(this.saveLocationTextBox);
            this.saveLocationPanel.Controls.Add(this.browseBtn);
            this.saveLocationPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.saveLocationPanel.Location = new System.Drawing.Point(0, 806);
            this.saveLocationPanel.Name = "saveLocationPanel";
            this.saveLocationPanel.Size = new System.Drawing.Size(818, 25);
            this.saveLocationPanel.TabIndex = 7;
            // 
            // saveLocationLabel
            // 
            this.saveLocationLabel.AutoSize = true;
            this.saveLocationLabel.Location = new System.Drawing.Point(820, 5);
            this.saveLocationLabel.Name = "saveLocationLabel";
            this.saveLocationLabel.Size = new System.Drawing.Size(79, 13);
            this.saveLocationLabel.TabIndex = 0;
            this.saveLocationLabel.Text = "Save Location:";
            // 
            // saveLocationTextBox
            // 
            this.saveLocationTextBox.Location = new System.Drawing.Point(130, 3);
            this.saveLocationTextBox.Name = "saveLocationTextBox";
            this.saveLocationTextBox.Size = new System.Drawing.Size(684, 20);
            this.saveLocationTextBox.TabIndex = 1;
            // 
            // browseBtn
            // 
            this.browseBtn.Location = new System.Drawing.Point(3, 3);
            this.browseBtn.Name = "browseBtn";
            this.browseBtn.Size = new System.Drawing.Size(121, 20);
            this.browseBtn.TabIndex = 2;
            this.browseBtn.Text = "إختر مسار";
            this.browseBtn.UseVisualStyleBackColor = true;
            // 
            // actionButtonsPanel
            // 
            this.actionButtonsPanel.Controls.Add(this.saveAllBtn);
            this.actionButtonsPanel.Controls.Add(this.saveCurrentBtn);
            this.actionButtonsPanel.Controls.Add(this.cancelBtn);
            this.actionButtonsPanel.Controls.Add(this.retryScanBtn);
            this.actionButtonsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.actionButtonsPanel.Location = new System.Drawing.Point(0, 831);
            this.actionButtonsPanel.Name = "actionButtonsPanel";
            this.actionButtonsPanel.Size = new System.Drawing.Size(818, 35);
            this.actionButtonsPanel.TabIndex = 8;
            // 
            // saveAllBtn
            // 
            this.saveAllBtn.Location = new System.Drawing.Point(3, 5);
            this.saveAllBtn.Name = "saveAllBtn";
            this.saveAllBtn.Size = new System.Drawing.Size(100, 25);
            this.saveAllBtn.TabIndex = 0;
            this.saveAllBtn.Text = "حفظ الكل";
            this.saveAllBtn.UseVisualStyleBackColor = true;
            // 
            // saveCurrentBtn
            // 
            this.saveCurrentBtn.Location = new System.Drawing.Point(109, 5);
            this.saveCurrentBtn.Name = "saveCurrentBtn";
            this.saveCurrentBtn.Size = new System.Drawing.Size(100, 25);
            this.saveCurrentBtn.TabIndex = 1;
            this.saveCurrentBtn.Text = "حفظ الحالية";
            this.saveCurrentBtn.UseVisualStyleBackColor = true;
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(631, 4);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(80, 25);
            this.cancelBtn.TabIndex = 2;
            this.cancelBtn.Text = "إلغاء";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // retryScanBtn
            // 
            this.retryScanBtn.Location = new System.Drawing.Point(717, 5);
            this.retryScanBtn.Name = "retryScanBtn";
            this.retryScanBtn.Size = new System.Drawing.Size(80, 25);
            this.retryScanBtn.TabIndex = 3;
            this.retryScanBtn.Text = "إعادة مسح";
            this.retryScanBtn.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox3, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.groupBox2, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.groupBox4, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.groupBox5, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.groupBox6, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.importBtn, 0, 7);
            this.tableLayoutPanel2.Controls.Add(this.scanBtn, 0, 8);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 9;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(244, 866);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.colorCombo);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 241);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(238, 85);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "الألوان";
            // 
            // colorCombo
            // 
            this.colorCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.colorCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.colorCombo.FormattingEnabled = true;
            this.colorCombo.Location = new System.Drawing.Point(3, 16);
            this.colorCombo.Name = "colorCombo";
            this.colorCombo.Size = new System.Drawing.Size(232, 21);
            this.colorCombo.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.DpiCombo);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 150);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(238, 85);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "الدقة";
            // 
            // DpiCombo
            // 
            this.DpiCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DpiCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DpiCombo.FormattingEnabled = true;
            this.DpiCombo.Location = new System.Drawing.Point(3, 16);
            this.DpiCombo.Name = "DpiCombo";
            this.DpiCombo.Size = new System.Drawing.Size(232, 21);
            this.DpiCombo.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(238, 56);
            this.label1.TabIndex = 0;
            this.label1.Text = "الإعدادت";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ScannerNameCombo);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 59);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(238, 85);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "إسم الجهاز";
            // 
            // ScannerNameCombo
            // 
            this.ScannerNameCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScannerNameCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ScannerNameCombo.FormattingEnabled = true;
            this.ScannerNameCombo.Location = new System.Drawing.Point(3, 16);
            this.ScannerNameCombo.Name = "ScannerNameCombo";
            this.ScannerNameCombo.Size = new System.Drawing.Size(232, 21);
            this.ScannerNameCombo.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.batchCheckBox);
            this.groupBox4.Controls.Add(this.duplexCheckBox);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(3, 332);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(238, 107);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "طريقة المسح";
            // 
            // batchCheckBox
            // 
            this.batchCheckBox.AutoSize = true;
            this.batchCheckBox.Location = new System.Drawing.Point(81, 28);
            this.batchCheckBox.Name = "batchCheckBox";
            this.batchCheckBox.Size = new System.Drawing.Size(93, 17);
            this.batchCheckBox.TabIndex = 4;
            this.batchCheckBox.Text = "متعدد الصفحات";
            this.batchCheckBox.UseVisualStyleBackColor = true;
            // 
            // duplexCheckBox
            // 
            this.duplexCheckBox.AutoSize = true;
            this.duplexCheckBox.Location = new System.Drawing.Point(99, 51);
            this.duplexCheckBox.Name = "duplexCheckBox";
            this.duplexCheckBox.Size = new System.Drawing.Size(75, 17);
            this.duplexCheckBox.TabIndex = 5;
            this.duplexCheckBox.Text = "من الجهتين";
            this.duplexCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.brightnessTrackBar);
            this.groupBox5.Controls.Add(this.contrastTrackBar);
            this.groupBox5.Controls.Add(this.autoOptimizeCheckBox);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(3, 445);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(238, 164);
            this.groupBox5.TabIndex = 9;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "الإعدادات المتقدمة";
            // 
            // brightnessTrackBar
            // 
            this.brightnessTrackBar.Location = new System.Drawing.Point(6, 17);
            this.brightnessTrackBar.Maximum = 100;
            this.brightnessTrackBar.Name = "brightnessTrackBar";
            this.brightnessTrackBar.Size = new System.Drawing.Size(168, 45);
            this.brightnessTrackBar.TabIndex = 0;
            this.brightnessTrackBar.TickFrequency = 10;
            this.brightnessTrackBar.Value = 50;
            // 
            // contrastTrackBar
            // 
            this.contrastTrackBar.Location = new System.Drawing.Point(8, 62);
            this.contrastTrackBar.Maximum = 100;
            this.contrastTrackBar.Name = "contrastTrackBar";
            this.contrastTrackBar.Size = new System.Drawing.Size(168, 45);
            this.contrastTrackBar.TabIndex = 1;
            this.contrastTrackBar.TickFrequency = 10;
            this.contrastTrackBar.Value = 50;
            // 
            // autoOptimizeCheckBox
            // 
            this.autoOptimizeCheckBox.AutoSize = true;
            this.autoOptimizeCheckBox.Location = new System.Drawing.Point(81, 109);
            this.autoOptimizeCheckBox.Name = "autoOptimizeCheckBox";
            this.autoOptimizeCheckBox.Size = new System.Drawing.Size(90, 17);
            this.autoOptimizeCheckBox.TabIndex = 2;
            this.autoOptimizeCheckBox.Text = "تحسين تلقائي";
            this.autoOptimizeCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.formatCombo);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(3, 615);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(238, 85);
            this.groupBox6.TabIndex = 10;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "الصيغة";
            // 
            // formatCombo
            // 
            this.formatCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formatCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.formatCombo.FormattingEnabled = true;
            this.formatCombo.Location = new System.Drawing.Point(3, 16);
            this.formatCombo.Name = "formatCombo";
            this.formatCombo.Size = new System.Drawing.Size(232, 21);
            this.formatCombo.TabIndex = 0;
            // 
            // importBtn
            // 
            this.importBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.importBtn.Location = new System.Drawing.Point(3, 706);
            this.importBtn.Name = "importBtn";
            this.importBtn.Size = new System.Drawing.Size(238, 62);
            this.importBtn.TabIndex = 7;
            this.importBtn.Text = "إستيراد ملف";
            this.importBtn.UseVisualStyleBackColor = true;
            // 
            // scanBtn
            // 
            this.scanBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scanBtn.Location = new System.Drawing.Point(3, 774);
            this.scanBtn.Name = "scanBtn";
            this.scanBtn.Size = new System.Drawing.Size(238, 89);
            this.scanBtn.TabIndex = 8;
            this.scanBtn.Text = "مسح";
            this.scanBtn.UseVisualStyleBackColor = true;
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.pagesCountLabel});
            this.statusBar.Location = new System.Drawing.Point(0, 872);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(1206, 22);
            this.statusBar.TabIndex = 2;
            this.statusBar.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(81, 17);
            this.statusLabel.Text = "Scanner ready";
            // 
            // pagesCountLabel
            // 
            this.pagesCountLabel.Name = "pagesCountLabel";
            this.pagesCountLabel.Size = new System.Drawing.Size(79, 17);
            this.pagesCountLabel.Text = "Total: 0 pages";
            // 
            // ScanForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1206, 894);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusBar);
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.Name = "ScanForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "مسح وثيقة";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.previewPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).EndInit();
            this.zoomPanel.ResumeLayout(false);
            this.rotationPanel.ResumeLayout(false);
            this.cropPanel.ResumeLayout(false);
            this.cropPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageNumericUpDown)).EndInit();
            this.filenamePanel.ResumeLayout(false);
            this.filenamePanel.PerformLayout();
            this.saveLocationPanel.ResumeLayout(false);
            this.saveLocationPanel.PerformLayout();
            this.actionButtonsPanel.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.brightnessTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contrastTrackBar)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox ScannerNameCombo;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox DpiCombo;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox colorCombo;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox batchCheckBox;
        private System.Windows.Forms.CheckBox duplexCheckBox;
        private System.Windows.Forms.Button importBtn;
        private System.Windows.Forms.Button scanBtn;
        private System.Windows.Forms.Panel previewPanel;
        private System.Windows.Forms.PictureBox previewPictureBox;
        private System.Windows.Forms.Button firstBtn;
        private System.Windows.Forms.Button prevBtn;
        private System.Windows.Forms.NumericUpDown pageNumericUpDown;
        private System.Windows.Forms.Button nextBtn;
        private System.Windows.Forms.Button lastBtn;
        private System.Windows.Forms.Label totalPagesLabel;
        private System.Windows.Forms.FlowLayoutPanel thumbnailPanel;
        private System.Windows.Forms.Panel zoomPanel;
        private System.Windows.Forms.ComboBox zoomCombo;
        private System.Windows.Forms.Button zoomInBtn;
        private System.Windows.Forms.Button zoomOutBtn;
        private System.Windows.Forms.Button fitBtn;
        private System.Windows.Forms.Button actualSizeBtn;
        private System.Windows.Forms.Panel rotationPanel;
        private System.Windows.Forms.Button rotateLeftBtn;
        private System.Windows.Forms.Button rotateRightBtn;
        private System.Windows.Forms.Button flip180Btn;
        private System.Windows.Forms.Button resetRotationBtn;
        private System.Windows.Forms.Panel cropPanel;
        private System.Windows.Forms.Button cropBtn;
        private System.Windows.Forms.Button applyCropBtn;
        private System.Windows.Forms.Button clearCropBtn;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TrackBar brightnessTrackBar;
        private System.Windows.Forms.TrackBar contrastTrackBar;
        private System.Windows.Forms.CheckBox autoOptimizeCheckBox;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ComboBox formatCombo;
        private System.Windows.Forms.Panel filenamePanel;
        private System.Windows.Forms.Label filenameLabel;
        private System.Windows.Forms.TextBox filenameTextBox;
        private System.Windows.Forms.Panel saveLocationPanel;
        private System.Windows.Forms.Label saveLocationLabel;
        private System.Windows.Forms.TextBox saveLocationTextBox;
        private System.Windows.Forms.Button browseBtn;
        private System.Windows.Forms.Panel actionButtonsPanel;
        private System.Windows.Forms.Button saveAllBtn;
        private System.Windows.Forms.Button saveCurrentBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button retryScanBtn;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripStatusLabel pagesCountLabel;
    }
}