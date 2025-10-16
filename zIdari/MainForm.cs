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
            var empForm = new EmployeeForm(_svc);
            empForm.EmployeeSaved += (s, emp) => LoadGrid(textBox1.Text?.Trim());
            empForm.SetActionSender("AddEmployee");
            empForm.Show(this);
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
    }
}
