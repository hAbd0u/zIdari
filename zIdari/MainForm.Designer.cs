namespace zIdari
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LawsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CorpsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BranchesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GradesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.docMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.employeesDataGV = new System.Windows.Forms.DataGridView();
            this.NumFolderCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FullNameArCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FullNameFrCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DepartementCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServiceCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GradeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PhoneCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EmailCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AddressCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.employeesContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.delMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.demandWorkAckCertieficateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.demandWorkCertieficateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printWorkAckCertieficateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printWorkCertieficateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.employeesDataGV)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.employeesContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenuItem,
            this.LawsMenuItem,
            this.CorpsMenuItem,
            this.BranchesMenuItem,
            this.GradesMenuItem,
            this.docMenuItem,
            this.scanToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(1, 1, 0, 1);
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(962, 26);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // FileMenuItem
            // 
            this.FileMenuItem.Name = "FileMenuItem";
            this.FileMenuItem.Size = new System.Drawing.Size(59, 24);
            this.FileMenuItem.Text = "الملف";
            // 
            // LawsMenuItem
            // 
            this.LawsMenuItem.Name = "LawsMenuItem";
            this.LawsMenuItem.Size = new System.Drawing.Size(70, 24);
            this.LawsMenuItem.Text = "القوانين";
            // 
            // CorpsMenuItem
            // 
            this.CorpsMenuItem.Name = "CorpsMenuItem";
            this.CorpsMenuItem.Size = new System.Drawing.Size(69, 24);
            this.CorpsMenuItem.Text = "الأسلاك";
            this.CorpsMenuItem.Click += new System.EventHandler(this.CorpsMenuItem_Click);
            // 
            // BranchesMenuItem
            // 
            this.BranchesMenuItem.Name = "BranchesMenuItem";
            this.BranchesMenuItem.Size = new System.Drawing.Size(66, 24);
            this.BranchesMenuItem.Text = "الشعب";
            this.BranchesMenuItem.Click += new System.EventHandler(this.BranchesMenuItem_Click);
            // 
            // GradesMenuItem
            // 
            this.GradesMenuItem.Name = "GradesMenuItem";
            this.GradesMenuItem.Size = new System.Drawing.Size(77, 24);
            this.GradesMenuItem.Text = "المناصب";
            this.GradesMenuItem.Click += new System.EventHandler(this.GradesMenuItem_Click);
            // 
            // docMenuItem
            // 
            this.docMenuItem.Name = "docMenuItem";
            this.docMenuItem.Size = new System.Drawing.Size(55, 24);
            this.docMenuItem.Text = "وثيقة";
            this.docMenuItem.Click += new System.EventHandler(this.docMenuItem_Click);
            // 
            // scanToolStripMenuItem
            // 
            this.scanToolStripMenuItem.Name = "scanToolStripMenuItem";
            this.scanToolStripMenuItem.Size = new System.Drawing.Size(50, 24);
            this.scanToolStripMenuItem.Text = "scan";
            this.scanToolStripMenuItem.Click += new System.EventHandler(this.scanToolStripMenuItem_Click);
            // 
            // employeesDataGV
            // 
            this.employeesDataGV.AllowUserToAddRows = false;
            this.employeesDataGV.AllowUserToDeleteRows = false;
            this.employeesDataGV.AllowUserToOrderColumns = true;
            this.employeesDataGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.employeesDataGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.employeesDataGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NumFolderCol,
            this.FullNameArCol,
            this.FullNameFrCol,
            this.DepartementCol,
            this.ServiceCol,
            this.GradeCol,
            this.PhoneCol,
            this.EmailCol,
            this.AddressCol});
            this.employeesDataGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.employeesDataGV.Location = new System.Drawing.Point(1, 22);
            this.employeesDataGV.Margin = new System.Windows.Forms.Padding(1);
            this.employeesDataGV.Name = "employeesDataGV";
            this.employeesDataGV.ReadOnly = true;
            this.employeesDataGV.RowHeadersWidth = 102;
            this.employeesDataGV.RowTemplate.Height = 40;
            this.employeesDataGV.Size = new System.Drawing.Size(960, 396);
            this.employeesDataGV.TabIndex = 1;
            this.employeesDataGV.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.employeesDataGV_CellDoubleClick);
            this.employeesDataGV.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.employeesDataGV_CellMouseDown);
            // 
            // NumFolderCol
            // 
            this.NumFolderCol.HeaderText = "رقم الملف";
            this.NumFolderCol.MinimumWidth = 12;
            this.NumFolderCol.Name = "NumFolderCol";
            this.NumFolderCol.ReadOnly = true;
            // 
            // FullNameArCol
            // 
            this.FullNameArCol.HeaderText = "اللقب و الإسم";
            this.FullNameArCol.MinimumWidth = 12;
            this.FullNameArCol.Name = "FullNameArCol";
            this.FullNameArCol.ReadOnly = true;
            // 
            // FullNameFrCol
            // 
            this.FullNameFrCol.HeaderText = "Nom et prenom";
            this.FullNameFrCol.MinimumWidth = 12;
            this.FullNameFrCol.Name = "FullNameFrCol";
            this.FullNameFrCol.ReadOnly = true;
            // 
            // DepartementCol
            // 
            this.DepartementCol.HeaderText = "القسم";
            this.DepartementCol.MinimumWidth = 12;
            this.DepartementCol.Name = "DepartementCol";
            this.DepartementCol.ReadOnly = true;
            // 
            // ServiceCol
            // 
            this.ServiceCol.HeaderText = "المصلحة";
            this.ServiceCol.MinimumWidth = 12;
            this.ServiceCol.Name = "ServiceCol";
            this.ServiceCol.ReadOnly = true;
            // 
            // GradeCol
            // 
            this.GradeCol.HeaderText = "الرتبة";
            this.GradeCol.MinimumWidth = 12;
            this.GradeCol.Name = "GradeCol";
            this.GradeCol.ReadOnly = true;
            // 
            // PhoneCol
            // 
            this.PhoneCol.HeaderText = "الهاتف";
            this.PhoneCol.MinimumWidth = 12;
            this.PhoneCol.Name = "PhoneCol";
            this.PhoneCol.ReadOnly = true;
            // 
            // EmailCol
            // 
            this.EmailCol.HeaderText = "البريد الإلكتروني";
            this.EmailCol.MinimumWidth = 12;
            this.EmailCol.Name = "EmailCol";
            this.EmailCol.ReadOnly = true;
            // 
            // AddressCol
            // 
            this.AddressCol.HeaderText = "العنوان";
            this.AddressCol.MinimumWidth = 12;
            this.AddressCol.Name = "AddressCol";
            this.AddressCol.ReadOnly = true;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(1, 1);
            this.textBox1.Margin = new System.Windows.Forms.Padding(1);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(960, 24);
            this.textBox1.TabIndex = 2;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.employeesDataGV, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 26);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(962, 419);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // employeesContextMenu
            // 
            this.employeesContextMenu.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.employeesContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addMenuItem,
            this.editMenuItem,
            this.delMenuItem,
            this.demandWorkAckCertieficateMenuItem,
            this.demandWorkCertieficateMenuItem,
            this.printWorkAckCertieficateMenuItem,
            this.printWorkCertieficateMenuItem});
            this.employeesContextMenu.Name = "employeesContextMenu";
            this.employeesContextMenu.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.employeesContextMenu.Size = new System.Drawing.Size(188, 158);
            this.employeesContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.employeesContextMenu_Opening);
            // 
            // addMenuItem
            // 
            this.addMenuItem.Name = "addMenuItem";
            this.addMenuItem.Size = new System.Drawing.Size(187, 22);
            this.addMenuItem.Text = "أضف";
            this.addMenuItem.Click += new System.EventHandler(this.addMenuItem_Click);
            // 
            // editMenuItem
            // 
            this.editMenuItem.Name = "editMenuItem";
            this.editMenuItem.Size = new System.Drawing.Size(187, 22);
            this.editMenuItem.Text = "تعديل";
            this.editMenuItem.Click += new System.EventHandler(this.editMenuItem_Click);
            // 
            // delMenuItem
            // 
            this.delMenuItem.Name = "delMenuItem";
            this.delMenuItem.Size = new System.Drawing.Size(187, 22);
            this.delMenuItem.Text = "حذف";
            this.delMenuItem.Click += new System.EventHandler(this.delMenuItem_Click);
            // 
            // demandWorkAckCertieficateMenuItem
            // 
            this.demandWorkAckCertieficateMenuItem.Name = "demandWorkAckCertieficateMenuItem";
            this.demandWorkAckCertieficateMenuItem.Size = new System.Drawing.Size(187, 22);
            this.demandWorkAckCertieficateMenuItem.Text = "طلب شهادة إقرار عمل";
            // 
            // demandWorkCertieficateMenuItem
            // 
            this.demandWorkCertieficateMenuItem.Name = "demandWorkCertieficateMenuItem";
            this.demandWorkCertieficateMenuItem.Size = new System.Drawing.Size(187, 22);
            this.demandWorkCertieficateMenuItem.Text = "طلب شهادة عمل";
            // 
            // printWorkAckCertieficateMenuItem
            // 
            this.printWorkAckCertieficateMenuItem.Name = "printWorkAckCertieficateMenuItem";
            this.printWorkAckCertieficateMenuItem.Size = new System.Drawing.Size(187, 22);
            this.printWorkAckCertieficateMenuItem.Text = "طباعة شهادة إقرار عمل";
            // 
            // printWorkCertieficateMenuItem
            // 
            this.printWorkCertieficateMenuItem.Name = "printWorkCertieficateMenuItem";
            this.printWorkCertieficateMenuItem.Size = new System.Drawing.Size(187, 22);
            this.printWorkCertieficateMenuItem.Text = "طباعة شهادة عمل";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(962, 445);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "MainForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "الكـwيـــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــــ" +
    "ــــــــــــــــــــــــن للموارد البشرية 1.0";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.employeesDataGV)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.employeesContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LawsMenuItem;
        private System.Windows.Forms.DataGridView employeesDataGV;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn NumFolderCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn FullNameArCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn FullNameFrCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn DepartementCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServiceCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn GradeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn PhoneCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn EmailCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn AddressCol;
        private System.Windows.Forms.ContextMenuStrip employeesContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editMenuItem;
        private System.Windows.Forms.ToolStripMenuItem delMenuItem;
        private System.Windows.Forms.ToolStripMenuItem demandWorkCertieficateMenuItem;
        private System.Windows.Forms.ToolStripMenuItem demandWorkAckCertieficateMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printWorkAckCertieficateMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printWorkCertieficateMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CorpsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem BranchesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem GradesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem docMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scanToolStripMenuItem;
    }
}

