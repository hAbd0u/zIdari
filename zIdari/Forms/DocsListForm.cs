using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using zIdari.Model;
using zIdari.Repository;
using zIdari.Service;

namespace zIdari.Forms
{
    public partial class DocsListForm : Form
    {
        private DocumentService _svc;
        private BindingSource _bs = new BindingSource();

        public DocsListForm()
        {
            InitializeComponent();
        }

        private void DocsListForm_Load(object sender, EventArgs e)
        {
            var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "kwin4rh.db");
            var repo = new DocumentRepository(dbPath);
            _svc = new DocumentService(repo);

            docsGrid.AutoGenerateColumns = false;
            docsGrid.ReadOnly = true;
            docsGrid.AllowUserToAddRows = false;
            docsGrid.AllowUserToDeleteRows = false;
            docsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            docsGrid.MultiSelect = false;

            TypeCol.DataPropertyName = nameof(Document.Type);
            TitleCol.DataPropertyName = nameof(Document.Title);

            docsGrid.ContextMenuStrip = docsContextMenu;
            docsGrid.DataSource = _bs;

            LoadGrid();
        }

        private void LoadGrid(string search = null)
        {
            var rows = _svc.GetAll(search);
            _bs.DataSource = rows;
        }

        private void docsContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Point clientPos = docsGrid.PointToClient(Cursor.Position);
            DataGridView.HitTestInfo hit = docsGrid.HitTest(clientPos.X, clientPos.Y);
            bool clickedOnRow = (hit.Type == DataGridViewHitTestType.Cell || hit.Type == DataGridViewHitTestType.RowHeader) && hit.RowIndex >= 0;

            editMenuItem.Enabled = clickedOnRow;
            deleteMenuItem.Enabled = clickedOnRow;
        }

        private void docsGrid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                docsGrid.ClearSelection();
                docsGrid.Rows[e.RowIndex].Selected = true;
                docsGrid.CurrentCell = docsGrid.Rows[e.RowIndex].Cells[0];
            }
        }

        private Document GetSelected()
        {
            if (_bs.Current is Document d) return d;
            if (docsGrid.CurrentRow?.DataBoundItem is Document d1) return d1;
            if (docsGrid.SelectedRows.Count > 0 && docsGrid.SelectedRows[0].DataBoundItem is Document d2) return d2;
            return null;
        }

        private void addMenuItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new DocForm(_svc))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    LoadGrid();
                }
            }
        }

        private void editMenuItem_Click(object sender, EventArgs e)
        {
            var sel = GetSelected();
            if (sel == null)
            {
                MessageBox.Show("لم يتم اختيار صف.", "تعديل", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var entity = _svc.GetById(sel.DocumentId);
            if (entity == null)
            {
                MessageBox.Show("السجل غير موجود.", "تعديل", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var dlg = new DocForm(_svc, entity))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    LoadGrid();
                }
            }
        }

        private void deleteMenuItem_Click(object sender, EventArgs e)
        {
            var sel = GetSelected();
            if (sel == null)
            {
                MessageBox.Show("لم يتم اختيار صف.", "حذف", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show($"حذف الوثيقة:\n{sel.Title}?", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _svc.Delete(sel.DocumentId);
                LoadGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"لا يمكن حذف السجل.\n\n{ex.Message}", "خطأ في الحذف", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
