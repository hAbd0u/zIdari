using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using zIdari.Forms;
using zIdari.Repository;
using zIdari.Service;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace zIdari
{
    public partial class MainForm : Form
    {
        private int rightClickedRowIndex = -1;
        private EmployeeService _employeeService;
        private IEmployeeRepository _repo;
        private EmployeeService _svc;
        private BindingSource _bs = new BindingSource();
        private List<EmployeeGridRow> _allRows = new List<EmployeeGridRow>();
        
        
        public MainForm()
        {
            InitializeComponent();
            //employeesDataGV.Columns[0].Visible = false;
            //employeesDataGV.AutoGenerateColumns = false;


        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.employeesDataGV.AutoGenerateColumns = false;
            this.employeesDataGV.ContextMenuStrip = employeesContextMenu;


            var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "kwin4rh.db");

            try
            {
                _repo = new EmployeeRepository(dbPath);
                _svc = new EmployeeService(_repo);

                employeesDataGV.AutoGenerateColumns = false;

                // DataPropertyName bindings
                NumFolderCol.DataPropertyName = nameof(EmployeeGridRow.NumFolderCol);
                FullNameArCol.DataPropertyName = nameof(EmployeeGridRow.FullNameArCol);
                FullNameFrCol.DataPropertyName = nameof(EmployeeGridRow.FullNameFrCol);
                PhoneCol.DataPropertyName = nameof(EmployeeGridRow.PhoneCol);
                EmailCol.DataPropertyName = nameof(EmployeeGridRow.EmailCol);
                AddressCol.DataPropertyName = nameof(EmployeeGridRow.AddressCol);

                // Add hidden key columns if not present
                if (!employeesDataGV.Columns.Contains("FolderNum_Key"))
                {
                    employeesDataGV.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = "FolderNum_Key",
                        DataPropertyName = nameof(EmployeeGridRow.FolderNum),
                        Visible = false
                    });
                    employeesDataGV.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = "FolderNumYear_Key",
                        DataPropertyName = nameof(EmployeeGridRow.FolderNumYear),
                        Visible = false
                    });
                }

                employeesDataGV.DataSource = _bs;

                LoadGrid(); // initial
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show($"{ex.Message}\n\nExpected path:\n{dbPath}", "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadGrid(string search = null)
        {
            _allRows = _svc.GetGrid(search);
            _bs.DataSource = _allRows;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            var q = textBox1.Text?.Trim();
            LoadGrid(string.IsNullOrEmpty(q) ? null : q);
        }

        private void employeesContextMenu_Opening(object sender, CancelEventArgs e)
        {
            // Get mouse position relative to DataGridView
            Point clientPos = employeesDataGV.PointToClient(Cursor.Position);
            DataGridView.HitTestInfo hit = employeesDataGV.HitTest(clientPos.X, clientPos.Y);

            // Check if we actually clicked on a row cell
            bool clickedOnRow = (hit.Type == DataGridViewHitTestType.Cell ||
                                 hit.Type == DataGridViewHitTestType.RowHeader);

            if (!clickedOnRow)
            {
                // Clicked on empty area -> disable all or cancel menu
                editMenuItem.Enabled = false;
                delMenuItem.Enabled = false;
                demandWorkAckCertieficateMenuItem.Enabled = false;
                demandWorkCertieficateMenuItem.Enabled = false;
                printWorkAckCertieficateMenuItem.Enabled = false;
                printWorkCertieficateMenuItem.Enabled = false;

                // optionally cancel:
                // e.Cancel = true;
                return;
            }

            // Enable/disable items depending on row selection
            bool hasSelection = employeesDataGV.SelectedRows.Count > 0;

            editMenuItem.Enabled = hasSelection;
            delMenuItem.Enabled = hasSelection;
            demandWorkAckCertieficateMenuItem.Enabled = hasSelection;
            demandWorkCertieficateMenuItem.Enabled = hasSelection;
            printWorkAckCertieficateMenuItem.Enabled = hasSelection;
            printWorkCertieficateMenuItem.Enabled = hasSelection;
        }



        private void employeesDataGV_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Record where the click happened
                rightClickedRowIndex = e.RowIndex;

                if (e.RowIndex >= 0)
                {
                    employeesDataGV.ClearSelection();
                    employeesDataGV.Rows[e.RowIndex].Selected = true;
                    employeesDataGV.CurrentCell = employeesDataGV.Rows[e.RowIndex].Cells[0];
                }
                else
                {
                    employeesDataGV.ClearSelection();
                }
            }
        }

        private void addMenuItem_Click(object sender, EventArgs e)
        {
            var (nextNum, year) = _svc.GenerateNewFolderKey(null);
            var empForm = new EmployeeForm(_svc);
            empForm.SetInitialKeys(nextNum, year, lockFields: true);
            empForm.EmployeeSaved += (s, emp) => LoadGrid(textBox1.Text?.Trim());
            empForm.SetActionSender("AddEmployee");
            empForm.Show(this);
        }

        private void delMenuItem_Click(object sender, EventArgs e)
        {
            var sel = GetSelectedRow();
            if (sel == null)
            {
                MessageBox.Show("No row selected.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"Delete employee:\n{sel.FullNameArCol}  ({sel.NumFolderCol}) ?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (confirm != DialogResult.Yes) return;

            try
            {
                // Prefer service:
                _svc.Delete(sel.FolderNum, sel.FolderNumYear);

                // If you don't have a service, call repo instead:
                // _repo.Delete(sel.FolderNum, sel.FolderNumYear);

                // Refresh grid (preserve current search if you have it)
                LoadGrid(textBox1.Text?.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Couldn't delete record.\n\n{ex.Message}",
                    "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



    // Helper you likely already have:
    private EmployeeGridRow GetSelectedRow()
    {
        if (employeesDataGV.CurrentRow?.DataBoundItem is EmployeeGridRow r1) return r1;
        if (employeesDataGV.SelectedRows.Count > 0 &&
            employeesDataGV.SelectedRows[0].DataBoundItem is EmployeeGridRow r2) return r2;
        if (employeesDataGV.CurrentCell != null &&
            employeesDataGV.Rows[employeesDataGV.CurrentCell.RowIndex].DataBoundItem is EmployeeGridRow r3) return r3;
        return null;
    }

    private void EditSelectedEmployee()
    {
        var sel = GetSelectedRow();
        if (sel == null)
        {
            MessageBox.Show("No row selected.", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        // Load full entity from DB
        var entity = _repo.GetByKey(sel.FolderNum, sel.FolderNumYear);
        if (entity == null)
        {
            MessageBox.Show("Record not found.", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Open the edit form, prefilled
        using (var dlg = new zIdari.Forms.EmployeeForm(_svc, entity))
        {
            // Optional: nice title
            dlg.Text = $"Edit Employee — {sel.FullNameArCol} ({sel.NumFolderCol})";

            var result = dlg.ShowDialog(this);

            // If user chose to update on close → DialogResult.OK
            if (result == DialogResult.OK)
            {
                // Refresh and reselect the updated row
                ReloadAndReselect(entity.FolderNum, entity.FolderNumYear);
            }
        }
    }

    // Keep your search term and reselect the edited row
    private void ReloadAndReselect(int folderNum, int folderNumYear)
    {
        var q = textBox1.Text?.Trim();
        LoadGrid(string.IsNullOrWhiteSpace(q) ? null : q);

        if (_bs?.DataSource is List<EmployeeGridRow> list)
        {
            var idx = list.FindIndex(r => r.FolderNum == folderNum && r.FolderNumYear == folderNumYear);
            if (idx >= 0)
            {
                employeesDataGV.ClearSelection();

                // pick a visible column to focus (NumFolderCol or next visible)
                int targetCol = 0;
                for (int c = 0; c < employeesDataGV.Columns.Count; c++)
                {
                    if (employeesDataGV.Columns[c].Visible) { targetCol = c; break; }
                }

                employeesDataGV.CurrentCell = employeesDataGV.Rows[idx].Cells[targetCol];
                employeesDataGV.Rows[idx].Selected = true;
                employeesDataGV.FirstDisplayedScrollingRowIndex = Math.Max(0, idx);
            }
        }
    }



    private void BindEmployees(List<zIdari.Repository.EmployeeGridRow> rows)
        {
            employeesDataGV.AutoGenerateColumns = false;

            NumFolderCol.DataPropertyName = nameof(zIdari.Repository.EmployeeGridRow.NumFolderCol);
            FullNameArCol.DataPropertyName = nameof(zIdari.Repository.EmployeeGridRow.FullNameArCol);
            FullNameFrCol.DataPropertyName = nameof(zIdari.Repository.EmployeeGridRow.FullNameFrCol);
            PhoneCol.DataPropertyName = nameof(zIdari.Repository.EmployeeGridRow.PhoneCol);
            EmailCol.DataPropertyName = nameof(zIdari.Repository.EmployeeGridRow.EmailCol);
            AddressCol.DataPropertyName = nameof(zIdari.Repository.EmployeeGridRow.AddressCol);

            employeesDataGV.DataSource = rows;
        }

        private void editMenuItem_Click(object sender, EventArgs e)
        {
            EditSelectedEmployee();
        }

        private void employeesDataGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) EditSelectedEmployee();
        }

        private void CorpsMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new zIdari.Forms.CorpsBranchPositionForm("corp"))
            {
                form.ShowDialog(this);
            }
        }

        private void BranchesMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new zIdari.Forms.CorpsBranchPositionForm("branche"))
            {
                form.ShowDialog(this);
            }
        }

        private void GradesMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new zIdari.Forms.CorpsBranchPositionForm("fonction"))
            {
                form.ShowDialog(this);
            }
        }

    }
}
