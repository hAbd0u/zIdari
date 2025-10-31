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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ScannerNameCombo = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DpiCombo = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.colorCombo = new System.Windows.Forms.ComboBox();
            this.batchCheckBox = new System.Windows.Forms.CheckBox();
            this.duplexCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.importBtn = new System.Windows.Forms.Button();
            this.scanBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 643);
            this.tableLayoutPanel1.TabIndex = 0;
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
            this.tableLayoutPanel2.Controls.Add(this.importBtn, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.scanBtn, 0, 7);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(611, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 8;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.837073F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.485244F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.120427F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.85006F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.68713F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 36.84652F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.20189F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.971654F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(186, 637);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "الإعدادت";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ScannerNameCombo);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(180, 54);
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
            this.ScannerNameCombo.Size = new System.Drawing.Size(174, 21);
            this.ScannerNameCombo.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.DpiCombo);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 100);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(180, 52);
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
            this.DpiCombo.Size = new System.Drawing.Size(174, 21);
            this.DpiCombo.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.colorCombo);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 158);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(180, 56);
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
            this.colorCombo.Size = new System.Drawing.Size(174, 21);
            this.colorCombo.TabIndex = 0;
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
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.batchCheckBox);
            this.groupBox4.Controls.Add(this.duplexCheckBox);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(3, 220);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(180, 93);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "طريقة المسح";
            // 
            // importBtn
            // 
            this.importBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.importBtn.Location = new System.Drawing.Point(3, 553);
            this.importBtn.Name = "importBtn";
            this.importBtn.Size = new System.Drawing.Size(180, 33);
            this.importBtn.TabIndex = 7;
            this.importBtn.Text = "إستيراد ملف";
            this.importBtn.UseVisualStyleBackColor = true;
            // 
            // scanBtn
            // 
            this.scanBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scanBtn.Location = new System.Drawing.Point(3, 592);
            this.scanBtn.Name = "scanBtn";
            this.scanBtn.Size = new System.Drawing.Size(180, 42);
            this.scanBtn.TabIndex = 8;
            this.scanBtn.Text = "مسح";
            this.scanBtn.UseVisualStyleBackColor = true;
            // 
            // ScanForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 643);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ScanForm";
            this.Text = "ScanForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

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
    }
}