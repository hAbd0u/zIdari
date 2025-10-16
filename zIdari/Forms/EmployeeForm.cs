// Forms/EmployeeForm.cs
using System;
using System.ComponentModel; // for LicenseManager
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using zIdari.Model;
using zIdari.Service;

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
                    case TextBox tb:
                        tb.TextChanged -= MarkDirty;
                        tb.TextChanged += MarkDirty;
                        break;
                    case ComboBox cb:
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

        private void EmployeeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // If nothing changed, just close.
            if (!_isDirty) return;

            var res = MessageBox.Show(
                "Save changes before closing?",
                "Confirm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);

            if (res == DialogResult.Yes)
            {
                // Attempt to save; if validation fails, keep the form open.
                try
                {
                    var entity = BuildFromUI();
                    var (ok, errors) = _svc.AddOrUpdate(entity);
                    if (!ok)
                    {
                        MessageBox.Show(string.Join("\n", errors), "Validation",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true; // stay open to fix mistakes
                        return;
                    }

                    // Notify parent if they’re listening
                    EmployeeSaved?.Invoke(this, entity);

                    _isDirty = false; // saved, allow close
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Save Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true; // keep form open on exception
                }
            }
            else
            {
                // No => discard, allow close
                _isDirty = false;
            }
        }

    }
}