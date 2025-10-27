using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using zIdari.Repository;
using zIdari.Service;

// Forms/CorpBranchGradForm.cs
namespace zIdari.Forms
{
    public partial class CorpBranchGradForm : Form
    {
        private CorpBranchGradService _svc;
        private BindingSource _bs = new BindingSource();
        private string _currentType = "corp"; // Default to corps

        public CorpBranchGradForm()
        {
            InitializeComponent();
        }

        private void CorpBranchGradForm_Load(object sender, EventArgs e)
        {
            // Initialize service
            var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "kwin4rh.db");
            try
            {
                var repo = new CorpBranchGradRepository(dbPath);
                _svc = new CorpBranchGradService(repo);
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show($"{ex.Message}\n\nExpected path:\n{dbPath}", "خطأ في قاعدة البيانات",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ غير متوقع: {ex.Message}", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            SetupGrid();
            LoadGrid(_currentType);
        }

        private void SetupGrid()
        {
            cbpGridView.AutoGenerateColumns = false;
            cbpGridView.ReadOnly = true;
            cbpGridView.AllowUserToAddRows = false;
            cbpGridView.AllowUserToDeleteRows = false;
            cbpGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            cbpGridView.MultiSelect = false;

            // Set data property names
            LawNumCol.DataPropertyName = nameof(CorpBranchGradGridRow.LawNumCol);
            TitleCol.DataPropertyName = nameof(CorpBranchGradGridRow.TitleCol);

            // Add hidden column for CsgId
            if (!cbpGridView.Columns.Contains("CsgId_Key"))
            {
                cbpGridView.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "CsgId_Key",
                    DataPropertyName = nameof(CorpBranchGradGridRow.CsgId),
                    Visible = false
                });
            }

            cbpGridView.ContextMenuStrip = cbgContextMenu;
            cbpGridView.DataSource = _bs;
        }

        private void LoadGrid(string type)
        {
            _currentType = type;
            var rows = _svc.GetByType(type);
            _bs.DataSource = rows;

            // Update form title based on type
            this.Text = type switch
            {
                "corp" => "إدارة الأسلاك",
                "branche" => "إدارة الفروع",
                "fonction" => "إدارة الرتب",
                _ => "إدارة البيانات"
            };
        }

        // Menu item clicks
        private void CorpsMenuItem_Click(object sender, EventArgs e)
        {
            LoadGrid("corp");
        }

        private void BranchesMenuItem_Click(object sender, EventArgs e)
        {
            LoadGrid("branche");
        }

        private void GradesMenuItem_Click(object sender, EventArgs e)
        {
            LoadGrid("fonction");
        }

        // Context menu opening
        private void cbgContextMenu_Opening(object sender, CancelEventArgs e)
        {
            Point clientPos = cbpGridView.PointToClient(Cursor.Position);
            DataGridView.HitTestInfo hit = cbpGridView.HitTest(clientPos.X, clientPos.Y);

            bool clickedOnRow = (hit.Type == DataGridViewHitTestType.Cell ||
                                 hit.Type == DataGridViewHitTestType.RowHeader) &&
                                 hit.RowIndex >= 0;

            editMenuItem.Enabled = clickedOnRow;
            deleteMenuItem.Enabled = clickedOnRow;
        }

        // Mouse down handler for row selection
        private void cbpGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                cbpGridView.ClearSelection();
                cbpGridView.Rows[e.RowIndex].Selected = true;
                cbpGridView.CurrentCell = cbpGridView.Rows[e.RowIndex].Cells[0];
            }
        }

        // Double-click handler
        private void cbpGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                EditSelectedRecord();
        }

        // Add record
        private void addMenuItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new CorpsBranchPositionAEForm(_svc, _currentType))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    LoadGrid(_currentType);
                }
            }
        }

        // Edit record
        private void editMenuItem_Click(object sender, EventArgs e)
        {
            EditSelectedRecord();
        }

        private void EditSelectedRecord()
        {
            var sel = GetSelectedRow();
            if (sel == null)
            {
                MessageBox.Show("لم يتم اختيار صف.", "تعديل",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var entity = _svc.GetById(sel.CsgId);
            if (entity == null)
            {
                MessageBox.Show("السجل غير موجود.", "تعديل",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var dlg = new CorpsBranchPositionAEForm(_svc, entity))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    LoadGrid(_currentType);
                }
            }
        }

        // Delete record
        private void deleteMenuItem_Click(object sender, EventArgs e)
        {
            var sel = GetSelectedRow();
            if (sel == null)
            {
                MessageBox.Show("لم يتم اختيار صف.", "حذف",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"حذف السجل:\n{sel.TitleCol}?",
                "تأكيد الحذف",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (confirm != DialogResult.Yes) return;

            try
            {
                _svc.Delete(sel.CsgId);
                LoadGrid(_currentType);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"لا يمكن حذف السجل.\n\n{ex.Message}",
                    "خطأ في الحذف", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper to get selected row
        private CorpBranchGradGridRow GetSelectedRow()
        {
            if (cbpGridView.CurrentRow?.DataBoundItem is CorpBranchGradGridRow r1)
                return r1;
            if (cbpGridView.SelectedRows.Count > 0 &&
                cbpGridView.SelectedRows[0].DataBoundItem is CorpBranchGradGridRow r2)
                return r2;
            return null;
        }
    }
}