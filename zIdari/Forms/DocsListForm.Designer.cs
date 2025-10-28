namespace zIdari.Forms
{
    partial class DocsListForm
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
            this.docsGrid = new System.Windows.Forms.DataGridView();
            this.TypeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TitleCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.docsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.docsGrid)).BeginInit();
            this.docsContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // docsGrid
            // 
            this.docsGrid.AllowUserToAddRows = false;
            this.docsGrid.AllowUserToDeleteRows = false;
            this.docsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.docsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.docsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TypeCol,
            this.TitleCol});
            this.docsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.docsGrid.Location = new System.Drawing.Point(0, 0);
            this.docsGrid.MultiSelect = false;
            this.docsGrid.Name = "docsGrid";
            this.docsGrid.ReadOnly = true;
            this.docsGrid.RowTemplate.Height = 30;
            this.docsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.docsGrid.Size = new System.Drawing.Size(784, 461);
            this.docsGrid.TabIndex = 0;
            this.docsGrid.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.docsGrid_CellMouseDown);
            // 
            // TypeCol
            // 
            this.TypeCol.HeaderText = "النوع";
            this.TypeCol.Name = "TypeCol";
            this.TypeCol.ReadOnly = true;
            // 
            // TitleCol
            // 
            this.TitleCol.HeaderText = "العنوان";
            this.TitleCol.Name = "TitleCol";
            this.TitleCol.ReadOnly = true;
            // 
            // docsContextMenu
            // 
            this.docsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addMenuItem,
            this.editMenuItem,
            this.deleteMenuItem});
            this.docsContextMenu.Name = "docsContextMenu";
            this.docsContextMenu.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.docsContextMenu.Size = new System.Drawing.Size(100, 70);
            this.docsContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.docsContextMenu_Opening);
            // 
            // addMenuItem
            // 
            this.addMenuItem.Name = "addMenuItem";
            this.addMenuItem.Size = new System.Drawing.Size(99, 22);
            this.addMenuItem.Text = "أضف";
            this.addMenuItem.Click += new System.EventHandler(this.addMenuItem_Click);
            // 
            // editMenuItem
            // 
            this.editMenuItem.Name = "editMenuItem";
            this.editMenuItem.Size = new System.Drawing.Size(99, 22);
            this.editMenuItem.Text = "تعديل";
            this.editMenuItem.Click += new System.EventHandler(this.editMenuItem_Click);
            // 
            // deleteMenuItem
            // 
            this.deleteMenuItem.Name = "deleteMenuItem";
            this.deleteMenuItem.Size = new System.Drawing.Size(99, 22);
            this.deleteMenuItem.Text = "حذف";
            this.deleteMenuItem.Click += new System.EventHandler(this.deleteMenuItem_Click);
            // 
            // DocsListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.docsGrid);
            this.Name = "DocsListForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "قائمة الوثائق";
            this.Load += new System.EventHandler(this.DocsListForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.docsGrid)).EndInit();
            this.docsContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView docsGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn TypeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn TitleCol;
        private System.Windows.Forms.ContextMenuStrip docsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteMenuItem;
    }
}