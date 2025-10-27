namespace zIdari.Forms
{
    partial class CorpsBranchPositionForm
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
            this.cbpGridView = new System.Windows.Forms.DataGridView();
            this.CorpsBrnGrdIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LawNumCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TitleCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.cbpGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // cbpGridView
            // 
            this.cbpGridView.AllowUserToAddRows = false;
            this.cbpGridView.AllowUserToDeleteRows = false;
            this.cbpGridView.AllowUserToOrderColumns = true;
            this.cbpGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cbpGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CorpsBrnGrdIdCol,
            this.LawNumCol,
            this.TitleCol});
            this.cbpGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbpGridView.Location = new System.Drawing.Point(0, 0);
            this.cbpGridView.Name = "cbpGridView";
            this.cbpGridView.ReadOnly = true;
            this.cbpGridView.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbpGridView.Size = new System.Drawing.Size(800, 450);
            this.cbpGridView.TabIndex = 0;
            // 
            // CorpsBrnGrdIdCol
            // 
            this.CorpsBrnGrdIdCol.HeaderText = "الرقم";
            this.CorpsBrnGrdIdCol.Name = "CorpsBrnGrdIdCol";
            this.CorpsBrnGrdIdCol.ReadOnly = true;
            // 
            // LawNumCol
            // 
            this.LawNumCol.HeaderText = "القانون";
            this.LawNumCol.Name = "LawNumCol";
            this.LawNumCol.ReadOnly = true;
            // 
            // TitleCol
            // 
            this.TitleCol.HeaderText = "الإسم";
            this.TitleCol.Name = "TitleCol";
            this.TitleCol.ReadOnly = true;
            // 
            // CorpsBranchPositionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cbpGridView);
            this.Name = "CorpsBranchPositionForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "CorpsBranchPositionForm";
            ((System.ComponentModel.ISupportInitialize)(this.cbpGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView cbpGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn CorpsBrnGrdIdCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn LawNumCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn TitleCol;
    }
}