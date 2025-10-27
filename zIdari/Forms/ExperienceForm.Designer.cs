namespace zIdari.Forms
{
    partial class ExperienceForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.DateTo = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.DateFrom = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.PositionTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.InstitutionNameTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CertificateNumberTxt = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.DateTo);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.DateFrom);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.PositionTxt);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.InstitutionNameTxt);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.CertificateNumberTxt);
            this.groupBox1.Location = new System.Drawing.Point(8, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(560, 180);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "الخبرة المهنية";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(493, 153);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "إلى تاريخ";
            // 
            // DateTo
            // 
            this.DateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DateTo.Location = new System.Drawing.Point(270, 150);
            this.DateTo.Name = "DateTo";
            this.DateTo.RightToLeftLayout = true;
            this.DateTo.Size = new System.Drawing.Size(217, 20);
            this.DateTo.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(493, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "من تاريخ";
            // 
            // DateFrom
            // 
            this.DateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DateFrom.Location = new System.Drawing.Point(270, 120);
            this.DateFrom.Name = "DateFrom";
            this.DateFrom.RightToLeftLayout = true;
            this.DateFrom.Size = new System.Drawing.Size(217, 20);
            this.DateFrom.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(493, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "المنصب";
            // 
            // PositionTxt
            // 
            this.PositionTxt.Location = new System.Drawing.Point(270, 90);
            this.PositionTxt.Name = "PositionTxt";
            this.PositionTxt.Size = new System.Drawing.Size(220, 20);
            this.PositionTxt.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(493, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "المؤسسة";
            // 
            // InstitutionNameTxt
            // 
            this.InstitutionNameTxt.Location = new System.Drawing.Point(270, 60);
            this.InstitutionNameTxt.Name = "InstitutionNameTxt";
            this.InstitutionNameTxt.Size = new System.Drawing.Size(220, 20);
            this.InstitutionNameTxt.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(493, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "رقم الشهادة";
            // 
            // CertificateNumberTxt
            // 
            this.CertificateNumberTxt.Location = new System.Drawing.Point(270, 30);
            this.CertificateNumberTxt.Name = "CertificateNumberTxt";
            this.CertificateNumberTxt.Size = new System.Drawing.Size(220, 20);
            this.CertificateNumberTxt.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(414, 210);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 30);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "حفظ";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(495, 210);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "إلغاء";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ExperienceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 261);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExperienceForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "الخبرة المهنية";
            this.Load += new System.EventHandler(this.ExperienceForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox CertificateNumberTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox InstitutionNameTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox PositionTxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker DateFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker DateTo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}