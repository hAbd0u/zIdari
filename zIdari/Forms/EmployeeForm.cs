// Forms/EmployeeForm.cs
using System;
using System.ComponentModel; // for LicenseManager
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using zIdari.Model;
using zIdari.Service;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace zIdari.Forms
{
    public partial class EmployeeForm : Form
    {
        private string _actionSender = "";
        private readonly EmployeeService _svc;
        private readonly Employee _existing; // null = Add, not null = Edit

        public event EventHandler<Employee> EmployeeSaved;

        private bool _isDirty = false;
        private bool _initializing = false;

        private ExperienceService _expSvc;
        private BindingSource _expBS = new BindingSource();
        private ContextMenuStrip _experienceContextMenu;

        // RUNTIME: call this for Add, or pass an existing entity for Edit.
        public EmployeeForm(EmployeeService svc, Employee existing = null)
        {
            InitializeComponent();
            _svc = svc ?? throw new ArgumentNullException(nameof(svc));
            _existing = existing;

            InitRuntimeUi();
        }

        // DESIGNER-ONLY: allows the VS designer to open the form
        public EmployeeForm()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                throw new InvalidOperationException("Use EmployeeForm(EmployeeService) at runtime.");
            InitRuntimeUi(); // safe basic UI init (no DB/service calls)
        }

        private void InitRuntimeUi()
        {
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(800, 600);

            // Hook FormClosing once
            this.FormClosing -= EmployeeForm_FormClosing;
            this.FormClosing += EmployeeForm_FormClosing;
        }

        private void EmployeeForm_Load(object sender, EventArgs e)
        {
            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.ItemSize = new Size(0, 1);
            tabControl1.SizeMode = TabSizeMode.Fixed;

            _initializing = true; // suppress dirty while populating

            // Example: years for FolderNumYear
            if (folderNumCombo.Items.Count == 0)
            {
                int year = DateTime.Now.Year;
                for (int y = year; y >= 1980; y--)
                    folderNumCombo.Items.Add(y.ToString());
            }

            if (_existing != null)
                FillUI(_existing);

            // After fields are populated, wire up change listeners
            WireDirtyTracking(this);

            _initializing = false; // user changes from here on mark dirty
            _isDirty = false;      // clean slate after initial load

            SetupExperienceGrid();

            if (_existing != null)
                LoadExperienceGrid();
        }


        public void SetActionSender(string action)
        {
            switch (action)
            {
                case "AddEmployee":
                case "EditEmployee":
                case "DeleteEmployee":
                    _actionSender = action;
                    break;
            }
        }

        private void FillUI(Employee e)
        {
            folderNumTxt.Text = e.FolderNum.ToString(CultureInfo.InvariantCulture);
            if (e.FolderNumYear > 0) folderNumCombo.SelectedItem = e.FolderNumYear.ToString();

            fnameTxt.Text = e.Fname;
            lnameTxt.Text = e.Lname;
            fnameFrTxt.Text = e.FnameFr;
            lnameFrTxt.Text = e.LnameFr;

            fatherNameTxt.Text = e.FatherName;
            motherNameTxt.Text = e.MotherName;

            if (e.Birth.HasValue) birthDatePicker.Value = e.Birth.Value;
            birthWilayaCombo.Text = e.Wilaya;

            sexCombo.SelectedIndex = e.Sex ? 0 : 1; // 0=Male, 1=Female (adjust to your UI)

            addressTxt.Text = e.Address;
            phoneTxt.Text = e.Phone;
            EmailTxt.Text = e.Email;

            relationCombo.Text = e.Relation;
            husbandTxt.Text = e.HusbandName;

            if (e.ActDate.HasValue) actDatePicker.Value = e.ActDate.Value;
            if (e.ActNum.HasValue) actNumTxt.Text = e.ActNum.Value.ToString(CultureInfo.InvariantCulture);
        }

        private Employee BuildFromUI()
        {
            if (!int.TryParse(folderNumTxt.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var folderNum))
                throw new InvalidOperationException("FolderNum must be an integer.");

            if (folderNumCombo.SelectedItem == null ||
                !int.TryParse(folderNumCombo.SelectedItem.ToString(), out var folderNumYear))
                throw new InvalidOperationException("Please select FolderNumYear.");

            var e = new Employee
            {
                FolderNum = folderNum,
                FolderNumYear = folderNumYear,
                Fname = fnameTxt.Text?.Trim(),
                Lname = lnameTxt.Text?.Trim(),
                FnameFr = fnameFrTxt.Text?.Trim(),
                LnameFr = lnameFrTxt.Text?.Trim(),
                FatherName = fatherNameTxt.Text?.Trim(),
                MotherName = motherNameTxt.Text?.Trim(),
                Birth = birthDatePicker.Value.Date,
                Wilaya = birthWilayaCombo.Text?.Trim(),
                Sex = (sexCombo.SelectedIndex == 0),
                Address = addressTxt.Text?.Trim(),
                Phone = phoneTxt.Text?.Trim(),
                Email = EmailTxt.Text?.Trim(),
                Relation = relationCombo.Text?.Trim(),
                HusbandName = husbandTxt.Text?.Trim(),
                ActDate = actDatePicker.Value.Date,
                ActNum = string.IsNullOrWhiteSpace(actNumTxt.Text)
                                ? (int?)null
                                : int.Parse(actNumTxt.Text, CultureInfo.InvariantCulture)
            };

            return e;
        }

        private void WireDirtyTracking(Control root)
        {
            foreach (Control c in root.Controls)
            {
                switch (c)
                {
                    case System.Windows.Forms.TextBox tb:
                        tb.TextChanged -= MarkDirty;
                        tb.TextChanged += MarkDirty;
                        break;
                    case System.Windows.Forms.ComboBox cb:
                        cb.TextChanged -= MarkDirty;
                        cb.TextChanged += MarkDirty;
                        cb.SelectedIndexChanged -= MarkDirty;
                        cb.SelectedIndexChanged += MarkDirty;
                        break;
                    case DateTimePicker dtp:
                        dtp.ValueChanged -= MarkDirty;
                        dtp.ValueChanged += MarkDirty;
                        break;
                    case CheckBox chk:
                        chk.CheckedChanged -= MarkDirty;
                        chk.CheckedChanged += MarkDirty;
                        break;
                    case RadioButton rb:
                        rb.CheckedChanged -= MarkDirty;
                        rb.CheckedChanged += MarkDirty;
                        break;
                    case NumericUpDown nud:
                        nud.ValueChanged -= MarkDirty;
                        nud.ValueChanged += MarkDirty;
                        break;
                }

                if (c.HasChildren)
                    WireDirtyTracking(c);
            }
        }

        private void MarkDirty(object sender, EventArgs e)
        {
            if (_initializing) return;
            _isDirty = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var entity = BuildFromUI();

                var (ok, errors) = _svc.AddOrUpdate(entity);
                if (!ok)
                {
                    MessageBox.Show(string.Join("\n", errors), "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                EmployeeSaved?.Invoke(this, entity);   // for modeless usage
                this.DialogResult = DialogResult.OK;   // for modal usage
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Save Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // In EmployeeForm.cs
        private void EmployeeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // only prompt if user changed something
            if (_existing == null && !_isDirty) return; // only skip for Add with no changes

            // choose verb based on add vs edit
            var verb = (_existing == null) ? "save this new employee" : "update this employee";
            var res = MessageBox.Show($"Do you want to {verb}?",
                                      "Confirm",
                                      MessageBoxButtons.YesNo,
                                      MessageBoxIcon.Question,
                                      MessageBoxDefaultButton.Button1);

            if (res == DialogResult.Yes)
            {
                try
                {
                    var entity = BuildFromUI();
                    var (ok, errors) = _svc.AddOrUpdate(entity);
                    if (!ok)
                    {
                        MessageBox.Show(string.Join("\n", errors), "Validation",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true; // keep form open to fix issues
                        return;
                    }

                    EmployeeSaved?.Invoke(this, entity); // modeless listeners
                    this.DialogResult = DialogResult.OK; // modal caller refreshes
                    _isDirty = false; // saved
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Save Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                }
            }
            else
            {
                // No → discard changes
                _isDirty = false;
            }
        }

        private void EmployeeTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            tabControl1.SelectedIndex = e.Node.Index;
        }

        public void SetInitialKeys(int folderNum, int folderNumYear, bool lockFields = true)
        {
            // Ensure the year combo is populated
            if (folderNumCombo.Items.Count == 0)
            {
                int yearNow = DateTime.Now.Year;
                for (int y = yearNow; y >= 1980; y--) folderNumCombo.Items.Add(y.ToString());
            }

            _initializing = true;               // don't mark dirty while we prefill
            folderNumTxt.Text = folderNum.ToString();

            var yearText = folderNumYear.ToString();
            if (!folderNumCombo.Items.Contains(yearText))
                folderNumCombo.Items.Add(yearText);
            folderNumCombo.SelectedItem = yearText;

            if (lockFields)
            {
                folderNumTxt.ReadOnly = true;
                folderNumCombo.Enabled = false;
            }

            _initializing = false;
            _isDirty = false;                   // start clean; user edits will flip this to true
        }








        private void InitializeExperienceService()
        {
            var dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "kwin4rh.db");
            var expRepo = new zIdari.Repository.ExperienceRepository(dbPath);
            _expSvc = new ExperienceService(expRepo);
        }

        // Add this method to set up the experience grid and context menu:

        private void SetupExperienceGrid()
        {
            // Initialize service if not already done
            if (_expSvc == null)
                InitializeExperienceService();

            // Set up DataGridView
            experienceGridView.AutoGenerateColumns = false;
            experienceGridView.ReadOnly = true;
            experienceGridView.AllowUserToAddRows = false;
            experienceGridView.AllowUserToDeleteRows = false;
            experienceGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            experienceGridView.MultiSelect = false;

            // Set data property names
            CertNumCol.DataPropertyName = nameof(zIdari.Repository.ExperienceGridRow.CertNumCol);
            CompanyCol.DataPropertyName = nameof(zIdari.Repository.ExperienceGridRow.CompanyCol);
            DateFromCol.DataPropertyName = nameof(zIdari.Repository.ExperienceGridRow.DateFromCol);
            DateToCol.DataPropertyName = nameof(zIdari.Repository.ExperienceGridRow.DateToCol);
            PositionCol.DataPropertyName = nameof(zIdari.Repository.ExperienceGridRow.PositionCol);

            // Add hidden column for ExperienceId
            if (!experienceGridView.Columns.Contains("ExperienceId_Key"))
            {
                experienceGridView.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "ExperienceId_Key",
                    DataPropertyName = nameof(zIdari.Repository.ExperienceGridRow.ExperienceId),
                    Visible = false
                });
            }

            // Create context menu
            _experienceContextMenu = new ContextMenuStrip();

            var addItem = new ToolStripMenuItem("أضف خبرة");
            addItem.Click += ExperienceAddMenuItem_Click;

            var editItem = new ToolStripMenuItem("تعديل");
            editItem.Name = "editExpItem";
            editItem.Click += ExperienceEditMenuItem_Click;

            var deleteItem = new ToolStripMenuItem("حذف");
            deleteItem.Name = "deleteExpItem";
            deleteItem.Click += ExperienceDeleteMenuItem_Click;

            _experienceContextMenu.Items.AddRange(new ToolStripItem[] { addItem, editItem, deleteItem });
            _experienceContextMenu.Opening += ExperienceContextMenu_Opening;

            experienceGridView.ContextMenuStrip = _experienceContextMenu;
            experienceGridView.DataSource = _expBS;

            // Handle double-click
            experienceGridView.CellDoubleClick += experienceGridView_CellDoubleClick;
            experienceGridView.CellMouseDown += experienceGridView_CellMouseDown;
        }

        // Add this method to load experiences:

        private void LoadExperienceGrid()
        {
            if (_expSvc == null || _existing == null) return;

            var rows = _expSvc.GetGrid(_existing.FolderNum, _existing.FolderNumYear);
            _expBS.DataSource = rows;
        }

        // Context menu opening handler:

        private void ExperienceContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Point clientPos = experienceGridView.PointToClient(Cursor.Position);
            DataGridView.HitTestInfo hit = experienceGridView.HitTest(clientPos.X, clientPos.Y);

            bool clickedOnRow = (hit.Type == DataGridViewHitTestType.Cell ||
                                 hit.Type == DataGridViewHitTestType.RowHeader) &&
                                 hit.RowIndex >= 0;

            var editItem = _experienceContextMenu.Items["editExpItem"] as ToolStripMenuItem;
            var deleteItem = _experienceContextMenu.Items["deleteExpItem"] as ToolStripMenuItem;

            if (editItem != null) editItem.Enabled = clickedOnRow;
            if (deleteItem != null) deleteItem.Enabled = clickedOnRow;
        }

        // Mouse down handler for row selection:

        private void experienceGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                experienceGridView.ClearSelection();
                experienceGridView.Rows[e.RowIndex].Selected = true;
                experienceGridView.CurrentCell = experienceGridView.Rows[e.RowIndex].Cells[0];
            }
        }

        // Double-click handler:

        private void experienceGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                EditSelectedExperience();
        }

        // Add experience:

        private void ExperienceAddMenuItem_Click(object sender, EventArgs e)
        {
            if (_existing == null)
            {
                MessageBox.Show("يجب حفظ الموظف أولاً قبل إضافة الخبرات.", "تنبيه",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var dlg = new ExperienceForm(_expSvc, _existing.FolderNum, _existing.FolderNumYear))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    LoadExperienceGrid();
                }
            }
        }

        // Edit experience:

        private void ExperienceEditMenuItem_Click(object sender, EventArgs e)
        {
            EditSelectedExperience();
        }

        private void EditSelectedExperience()
        {
            var sel = GetSelectedExperience();
            if (sel == null)
            {
                MessageBox.Show("لم يتم اختيار صف.", "تعديل",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var entity = _expSvc.GetById(sel.ExperienceId);
            if (entity == null)
            {
                MessageBox.Show("السجل غير موجود.", "تعديل",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var dlg = new ExperienceForm(_expSvc, entity))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    LoadExperienceGrid();
                }
            }
        }

        // Delete experience:

        private void ExperienceDeleteMenuItem_Click(object sender, EventArgs e)
        {
            var sel = GetSelectedExperience();
            if (sel == null)
            {
                MessageBox.Show("لم يتم اختيار صف.", "حذف",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"حذف الخبرة في:\n{sel.CompanyCol}?",
                "تأكيد الحذف",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (confirm != DialogResult.Yes) return;

            try
            {
                _expSvc.Delete(sel.ExperienceId);
                LoadExperienceGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"لا يمكن حذف السجل.\n\n{ex.Message}",
                    "خطأ في الحذف", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper to get selected experience:

        private zIdari.Repository.ExperienceGridRow GetSelectedExperience()
        {
            if (experienceGridView.CurrentRow?.DataBoundItem is zIdari.Repository.ExperienceGridRow r1)
                return r1;
            if (experienceGridView.SelectedRows.Count > 0 &&
                experienceGridView.SelectedRows[0].DataBoundItem is zIdari.Repository.ExperienceGridRow r2)
                return r2;
            return null;
        }


    }
}