namespace zIdari.Forms
{
    partial class CarrierForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.carrierNameCombo = new System.Windows.Forms.ComboBox();
            this.carrierTypeCombo = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.PositionListCombo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BrancheListCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CorpListCombo = new System.Windows.Forms.ComboBox();
            this.DocSignDate = new System.Windows.Forms.DateTimePicker();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.DegreeListCombo = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ClassListCombo = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.statusRadio2 = new System.Windows.Forms.RadioButton();
            this.statusRadio = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.ScanBtn = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.DocEffectiveDate = new System.Windows.Forms.DateTimePicker();
            this.DocNumText = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.DocTitleComboList = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.PubFunctionNumText = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.PubFunctionNumDate = new System.Windows.Forms.DateTimePicker();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.FinanceControlNumText = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.FinanceControlDate = new System.Windows.Forms.DateTimePicker();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.docTypeComboList = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.carrierNameCombo);
            this.groupBox1.Controls.Add(this.carrierTypeCombo);
            this.groupBox1.Location = new System.Drawing.Point(4, 13);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox1.Size = new System.Drawing.Size(541, 42);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "نوع المسار";
            // 
            // carrierNameCombo
            // 
            this.carrierNameCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.carrierNameCombo.FormattingEnabled = true;
            this.carrierNameCombo.Location = new System.Drawing.Point(279, 16);
            this.carrierNameCombo.Margin = new System.Windows.Forms.Padding(1);
            this.carrierNameCombo.Name = "carrierNameCombo";
            this.carrierNameCombo.Size = new System.Drawing.Size(122, 21);
            this.carrierNameCombo.TabIndex = 7;
            // 
            // carrierTypeCombo
            // 
            this.carrierTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.carrierTypeCombo.FormattingEnabled = true;
            this.carrierTypeCombo.Items.AddRange(new object[] {
            "توظيف",
            "إدماج",
            "ترقية",
            "درجة",
            "توقيف",
            "إنزال في الرتبة"});
            this.carrierTypeCombo.Location = new System.Drawing.Point(412, 16);
            this.carrierTypeCombo.Margin = new System.Windows.Forms.Padding(1);
            this.carrierTypeCombo.Name = "carrierTypeCombo";
            this.carrierTypeCombo.Size = new System.Drawing.Size(122, 21);
            this.carrierTypeCombo.TabIndex = 6;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.PositionListCombo);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.BrancheListCombo);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.CorpListCombo);
            this.groupBox2.Location = new System.Drawing.Point(4, 61);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox2.Size = new System.Drawing.Size(928, 53);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "القانون الأساسي";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(447, 24);
            this.label3.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "المنصب";
            // 
            // PositionListCombo
            // 
            this.PositionListCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PositionListCombo.FormattingEnabled = true;
            this.PositionListCombo.Location = new System.Drawing.Point(268, 22);
            this.PositionListCombo.Margin = new System.Windows.Forms.Padding(1);
            this.PositionListCombo.Name = "PositionListCombo";
            this.PositionListCombo.Size = new System.Drawing.Size(177, 21);
            this.PositionListCombo.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(672, 24);
            this.label2.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "الشعبة";
            // 
            // BrancheListCombo
            // 
            this.BrancheListCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BrancheListCombo.FormattingEnabled = true;
            this.BrancheListCombo.Location = new System.Drawing.Point(495, 22);
            this.BrancheListCombo.Margin = new System.Windows.Forms.Padding(1);
            this.BrancheListCombo.Name = "BrancheListCombo";
            this.BrancheListCombo.Size = new System.Drawing.Size(177, 21);
            this.BrancheListCombo.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(889, 24);
            this.label1.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "السلك";
            // 
            // CorpListCombo
            // 
            this.CorpListCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CorpListCombo.FormattingEnabled = true;
            this.CorpListCombo.Location = new System.Drawing.Point(712, 22);
            this.CorpListCombo.Margin = new System.Windows.Forms.Padding(1);
            this.CorpListCombo.Name = "CorpListCombo";
            this.CorpListCombo.Size = new System.Drawing.Size(177, 21);
            this.CorpListCombo.TabIndex = 2;
            // 
            // DocSignDate
            // 
            this.DocSignDate.Location = new System.Drawing.Point(271, 18);
            this.DocSignDate.Margin = new System.Windows.Forms.Padding(1);
            this.DocSignDate.Name = "DocSignDate";
            this.DocSignDate.Size = new System.Drawing.Size(103, 20);
            this.DocSignDate.TabIndex = 5;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.DegreeListCombo);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.ClassListCombo);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(4, 122);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox3.Size = new System.Drawing.Size(928, 42);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "الدرجة";
            // 
            // DegreeListCombo
            // 
            this.DegreeListCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DegreeListCombo.FormattingEnabled = true;
            this.DegreeListCombo.Location = new System.Drawing.Point(729, 18);
            this.DegreeListCombo.Margin = new System.Windows.Forms.Padding(1);
            this.DegreeListCombo.Name = "DegreeListCombo";
            this.DegreeListCombo.Size = new System.Drawing.Size(63, 21);
            this.DegreeListCombo.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(792, 20);
            this.label5.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "الدرجة";
            // 
            // ClassListCombo
            // 
            this.ClassListCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ClassListCombo.FormattingEnabled = true;
            this.ClassListCombo.Location = new System.Drawing.Point(830, 18);
            this.ClassListCombo.Margin = new System.Windows.Forms.Padding(1);
            this.ClassListCombo.Name = "ClassListCombo";
            this.ClassListCombo.Size = new System.Drawing.Size(63, 21);
            this.ClassListCombo.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(892, 20);
            this.label4.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "الصنف";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.statusRadio2);
            this.groupBox4.Controls.Add(this.statusRadio);
            this.groupBox4.Location = new System.Drawing.Point(4, 176);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox4.Size = new System.Drawing.Size(928, 42);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "الحالة";
            // 
            // statusRadio2
            // 
            this.statusRadio2.AutoSize = true;
            this.statusRadio2.Location = new System.Drawing.Point(815, 16);
            this.statusRadio2.Margin = new System.Windows.Forms.Padding(1);
            this.statusRadio2.Name = "statusRadio2";
            this.statusRadio2.Size = new System.Drawing.Size(54, 17);
            this.statusRadio2.TabIndex = 1;
            this.statusRadio2.TabStop = true;
            this.statusRadio2.Text = "متربص";
            this.statusRadio2.UseVisualStyleBackColor = true;
            // 
            // statusRadio
            // 
            this.statusRadio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.statusRadio.AutoSize = true;
            this.statusRadio.Location = new System.Drawing.Point(871, 16);
            this.statusRadio.Margin = new System.Windows.Forms.Padding(1);
            this.statusRadio.Name = "statusRadio";
            this.statusRadio.Size = new System.Drawing.Size(49, 17);
            this.statusRadio.TabIndex = 0;
            this.statusRadio.TabStop = true;
            this.statusRadio.Text = "مرسم";
            this.statusRadio.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.statusRadio.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.docTypeComboList);
            this.groupBox5.Controls.Add(this.label14);
            this.groupBox5.Controls.Add(this.ScanBtn);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.DocEffectiveDate);
            this.groupBox5.Controls.Add(this.DocNumText);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.DocTitleComboList);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.DocSignDate);
            this.groupBox5.Location = new System.Drawing.Point(4, 226);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox5.Size = new System.Drawing.Size(928, 42);
            this.groupBox5.TabIndex = 13;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "الوثيقة";
            // 
            // ScanBtn
            // 
            this.ScanBtn.Location = new System.Drawing.Point(55, 10);
            this.ScanBtn.Margin = new System.Windows.Forms.Padding(1);
            this.ScanBtn.Name = "ScanBtn";
            this.ScanBtn.Size = new System.Drawing.Size(34, 24);
            this.ScanBtn.TabIndex = 21;
            this.ScanBtn.Text = "...";
            this.ScanBtn.UseVisualStyleBackColor = true;
            this.ScanBtn.Click += new System.EventHandler(this.ScanBtn_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(535, 18);
            this.label9.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "الرقم";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(199, 18);
            this.label8.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "تاريخ المفعول";
            // 
            // DocEffectiveDate
            // 
            this.DocEffectiveDate.Location = new System.Drawing.Point(96, 18);
            this.DocEffectiveDate.Margin = new System.Windows.Forms.Padding(1);
            this.DocEffectiveDate.Name = "DocEffectiveDate";
            this.DocEffectiveDate.Size = new System.Drawing.Size(103, 20);
            this.DocEffectiveDate.TabIndex = 15;
            this.DocEffectiveDate.ValueChanged += new System.EventHandler(this.DocEffectiveDate_ValueChanged);
            // 
            // DocNumText
            // 
            this.DocNumText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DocNumText.Location = new System.Drawing.Point(444, 17);
            this.DocNumText.Margin = new System.Windows.Forms.Padding(1);
            this.DocNumText.Name = "DocNumText";
            this.DocNumText.Size = new System.Drawing.Size(91, 20);
            this.DocNumText.TabIndex = 14;
            this.DocNumText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(374, 18);
            this.label6.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "تاريخ الإمضاء";
            // 
            // DocTitleComboList
            // 
            this.DocTitleComboList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DocTitleComboList.FormattingEnabled = true;
            this.DocTitleComboList.Location = new System.Drawing.Point(566, 18);
            this.DocTitleComboList.Margin = new System.Windows.Forms.Padding(1);
            this.DocTitleComboList.Name = "DocTitleComboList";
            this.DocTitleComboList.Size = new System.Drawing.Size(123, 21);
            this.DocTitleComboList.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(693, 18);
            this.label7.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "عنوان الوثيقة";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label10);
            this.groupBox6.Controls.Add(this.PubFunctionNumText);
            this.groupBox6.Controls.Add(this.label12);
            this.groupBox6.Controls.Add(this.PubFunctionNumDate);
            this.groupBox6.Location = new System.Drawing.Point(4, 277);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox6.Size = new System.Drawing.Size(928, 42);
            this.groupBox6.TabIndex = 17;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "تأشيرة الوظيف العمومي";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(893, 21);
            this.label10.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 13);
            this.label10.TabIndex = 13;
            this.label10.Text = "الرقم";
            // 
            // PubFunctionNumText
            // 
            this.PubFunctionNumText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PubFunctionNumText.Location = new System.Drawing.Point(802, 19);
            this.PubFunctionNumText.Margin = new System.Windows.Forms.Padding(1);
            this.PubFunctionNumText.Name = "PubFunctionNumText";
            this.PubFunctionNumText.Size = new System.Drawing.Size(91, 20);
            this.PubFunctionNumText.TabIndex = 14;
            this.PubFunctionNumText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(729, 22);
            this.label12.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(68, 13);
            this.label12.TabIndex = 12;
            this.label12.Text = "تاريخ الإمضاء";
            // 
            // PubFunctionNumDate
            // 
            this.PubFunctionNumDate.Location = new System.Drawing.Point(628, 19);
            this.PubFunctionNumDate.Margin = new System.Windows.Forms.Padding(1);
            this.PubFunctionNumDate.Name = "PubFunctionNumDate";
            this.PubFunctionNumDate.Size = new System.Drawing.Size(103, 20);
            this.PubFunctionNumDate.TabIndex = 5;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label11);
            this.groupBox7.Controls.Add(this.FinanceControlNumText);
            this.groupBox7.Controls.Add(this.label13);
            this.groupBox7.Controls.Add(this.FinanceControlDate);
            this.groupBox7.Location = new System.Drawing.Point(4, 325);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox7.Size = new System.Drawing.Size(928, 42);
            this.groupBox7.TabIndex = 18;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "تأشيرة المراقب المالي";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(893, 21);
            this.label11.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 13);
            this.label11.TabIndex = 13;
            this.label11.Text = "الرقم";
            // 
            // FinanceControlNumText
            // 
            this.FinanceControlNumText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FinanceControlNumText.Location = new System.Drawing.Point(802, 19);
            this.FinanceControlNumText.Margin = new System.Windows.Forms.Padding(1);
            this.FinanceControlNumText.Name = "FinanceControlNumText";
            this.FinanceControlNumText.Size = new System.Drawing.Size(91, 20);
            this.FinanceControlNumText.TabIndex = 14;
            this.FinanceControlNumText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(731, 21);
            this.label13.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(68, 13);
            this.label13.TabIndex = 12;
            this.label13.Text = "تاريخ الإمضاء";
            // 
            // FinanceControlDate
            // 
            this.FinanceControlDate.Location = new System.Drawing.Point(628, 19);
            this.FinanceControlDate.Margin = new System.Windows.Forms.Padding(1);
            this.FinanceControlDate.Name = "FinanceControlDate";
            this.FinanceControlDate.Size = new System.Drawing.Size(103, 20);
            this.FinanceControlDate.TabIndex = 5;
            // 
            // SaveBtn
            // 
            this.SaveBtn.Location = new System.Drawing.Point(4, 386);
            this.SaveBtn.Margin = new System.Windows.Forms.Padding(1);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(92, 28);
            this.SaveBtn.TabIndex = 19;
            this.SaveBtn.Text = "حفظ";
            this.SaveBtn.UseVisualStyleBackColor = true;
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(104, 386);
            this.CancelBtn.Margin = new System.Windows.Forms.Padding(1);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(92, 28);
            this.CancelBtn.TabIndex = 20;
            this.CancelBtn.Text = "إلغاء";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // docTypeComboList
            // 
            this.docTypeComboList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.docTypeComboList.FormattingEnabled = true;
            this.docTypeComboList.Location = new System.Drawing.Point(762, 18);
            this.docTypeComboList.Margin = new System.Windows.Forms.Padding(1);
            this.docTypeComboList.Name = "docTypeComboList";
            this.docTypeComboList.Size = new System.Drawing.Size(123, 21);
            this.docTypeComboList.TabIndex = 23;
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(889, 18);
            this.label14.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(30, 13);
            this.label14.TabIndex = 22;
            this.label14.Text = "النوع";
            // 
            // CarrierForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(943, 425);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "CarrierForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "CarrierForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox CorpListCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker DocSignDate;
        private System.Windows.Forms.ComboBox carrierNameCombo;
        private System.Windows.Forms.ComboBox carrierTypeCombo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox BrancheListCombo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox PositionListCombo;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox DegreeListCombo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox ClassListCombo;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton statusRadio2;
        private System.Windows.Forms.RadioButton statusRadio;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox DocTitleComboList;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox DocNumText;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker DocEffectiveDate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox PubFunctionNumText;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DateTimePicker PubFunctionNumDate;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox FinanceControlNumText;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.DateTimePicker FinanceControlDate;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button ScanBtn;
        private System.Windows.Forms.ComboBox docTypeComboList;
        private System.Windows.Forms.Label label14;
    }
}