namespace zIdari.Forms
{
    partial class CorpsBranchPositionForm
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
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.CorpsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BranchesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GradesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cbpGridView = new System.Windows.Forms.DataGridView();
            this.LawNumCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TitleCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cbgContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbpGridView)).BeginInit();
            this.cbgContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CorpsMenuItem,
            this.BranchesMenuItem,
            this.GradesMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
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
            // cbpGridView
            // 
            this.cbpGridView.AllowUserToAddRows = false;
            this.cbpGridView.AllowUserToDeleteRows = false;
            this.cbpGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.cbpGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cbpGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LawNumCol,
            this.TitleCol});
            this.cbpGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbpGridView.Location = new System.Drawing.Point(0, 28);
            this.cbpGridView.MultiSelect = false;
            this.cbpGridView.Name = "cbpGridView";
            this.cbpGridView.ReadOnly = true;
            this.cbpGridView.RowTemplate.Height = 30;
            this.cbpGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.cbpGridView.Size = new System.Drawing.Size(784, 433);
            this.cbpGridView.TabIndex = 1;
            this.cbpGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.cbpGridView_CellDoubleClick);
            this.cbpGridView.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.cbpGridView_CellMouseDown);
            // 
            // LawNumCol
            // 
            this.LawNumCol.HeaderText = "رقم القانون";
            this.LawNumCol.Name = "LawNumCol";
            this.LawNumCol.ReadOnly = true;
            // 
            // TitleCol
            // 
            this.TitleCol.HeaderText = "العنوان";
            this.TitleCol.Name = "TitleCol";
            this.TitleCol.ReadOnly = true;
            // 
            // cbgContextMenu
            // 
            this.cbgContextMenu.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.cbgContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addMenuItem,
            this.editMenuItem,
            this.deleteMenuItem});
            this.cbgContextMenu.Name = "cbgContextMenu";
            this.cbgContextMenu.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbgContextMenu.Size = new System.Drawing.Size(104, 70);
            this.cbgContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.cbgContextMenu_Opening);
            // 
            // addMenuItem
            // 
            this.addMenuItem.Name = "addMenuItem";
            this.addMenuItem.Size = new System.Drawing.Size(103, 22);
            this.addMenuItem.Text = "أضف";
            this.addMenuItem.Click += new System.EventHandler(this.addMenuItem_Click);
            // 
            // editMenuItem
            // 
            this.editMenuItem.Name = "editMenuItem";
            this.editMenuItem.Size = new System.Drawing.Size(103, 22);
            this.editMenuItem.Text = "تعديل";
            this.editMenuItem.Click += new System.EventHandler(this.editMenuItem_Click);
            // 
            // deleteMenuItem
            // 
            this.deleteMenuItem.Name = "deleteMenuItem";
            this.deleteMenuItem.Size = new System.Drawing.Size(103, 22);
            this.deleteMenuItem.Text = "حذف";
            this.deleteMenuItem.Click += new System.EventHandler(this.deleteMenuItem_Click);
            // 
            // CorpsBranchPositionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.cbpGridView);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "CorpsBranchPositionForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "إدارة الأسلاك والفروع والرتب";
            this.Load += new System.EventHandler(this.CorpsBranchPositionForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbpGridView)).EndInit();
            this.cbgContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem CorpsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem BranchesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem GradesMenuItem;
        private System.Windows.Forms.DataGridView cbpGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn LawNumCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn TitleCol;
        private System.Windows.Forms.ContextMenuStrip cbgContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteMenuItem;
    }
}