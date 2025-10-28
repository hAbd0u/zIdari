namespace zIdari.Forms
{
    partial class DocsListForm
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
            this.docsGridView = new System.Windows.Forms.DataGridView();
            this.DocIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocTypeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TitleCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.docsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // docsGridView
            // 
            this.docsGridView.AllowUserToAddRows = false;
            this.docsGridView.AllowUserToDeleteRows = false;
            this.docsGridView.AllowUserToOrderColumns = true;
            this.docsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.docsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DocIdCol,
            this.DocTypeCol,
            this.TitleCol});
            this.docsGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.docsGridView.Location = new System.Drawing.Point(0, 0);
            this.docsGridView.Name = "docsGridView";
            this.docsGridView.ReadOnly = true;
            this.docsGridView.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.docsGridView.RowHeadersWidth = 102;
            this.docsGridView.RowTemplate.Height = 40;
            this.docsGridView.Size = new System.Drawing.Size(1844, 777);
            this.docsGridView.TabIndex = 0;
            // 
            // DocIdCol
            // 
            this.DocIdCol.HeaderText = "الرقم";
            this.DocIdCol.MinimumWidth = 12;
            this.DocIdCol.Name = "DocIdCol";
            this.DocIdCol.ReadOnly = true;
            this.DocIdCol.Width = 250;
            // 
            // DocTypeCol
            // 
            this.DocTypeCol.HeaderText = "نوع الوثيقة";
            this.DocTypeCol.MinimumWidth = 12;
            this.DocTypeCol.Name = "DocTypeCol";
            this.DocTypeCol.ReadOnly = true;
            this.DocTypeCol.Width = 250;
            // 
            // TitleCol
            // 
            this.TitleCol.HeaderText = "عنوان الوثيقة";
            this.TitleCol.MinimumWidth = 12;
            this.TitleCol.Name = "TitleCol";
            this.TitleCol.ReadOnly = true;
            this.TitleCol.Width = 250;
            // 
            // DocsListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1844, 777);
            this.Controls.Add(this.docsGridView);
            this.Name = "DocsListForm";
            this.Text = "DocsForm";
            ((System.ComponentModel.ISupportInitialize)(this.docsGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView docsGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocIdCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocTypeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn TitleCol;
    }
}