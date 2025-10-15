using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using zIdari.Forms;

namespace zIdari
{
    public partial class MainForm : Form
    {
        private int rightClickedRowIndex = -1;
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


            this.employeesDataGV.Rows.Add(1, "Ali");
            this.employeesDataGV.Rows.Add(2, "Sara");
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
            EmployeeForm empForm = new EmployeeForm();
            empForm.Show();
        }
    }
}
