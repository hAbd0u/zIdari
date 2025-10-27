using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using zIdari.Model;
using zIdari.Service;

// Forms/ExperienceForm.cs
namespace zIdari.Forms
{
    public partial class ExperienceForm : Form
    {
        private readonly ExperienceService _svc;
        private readonly Experience _existing;
        private readonly int _folderNum;
        private readonly int _folderNumYear;

        public event EventHandler<Experience> ExperienceSaved;

        // For adding new experience
        public ExperienceForm(ExperienceService svc, int folderNum, int folderNumYear)
        {
            InitializeComponent();
            _svc = svc ?? throw new ArgumentNullException(nameof(svc));
            _folderNum = folderNum;
            _folderNumYear = folderNumYear;
            _existing = null;
        }

        // For editing existing experience
        public ExperienceForm(ExperienceService svc, Experience existing)
        {
            InitializeComponent();
            _svc = svc ?? throw new ArgumentNullException(nameof(svc));
            _existing = existing ?? throw new ArgumentNullException(nameof(existing));
            _folderNum = existing.FolderNum;
            _folderNumYear = existing.FolderNumYear;
        }

        // Designer-only constructor
        public ExperienceForm()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                throw new InvalidOperationException("Use ExperienceForm(ExperienceService, ...) at runtime.");
        }

        private void ExperienceForm_Load(object sender, EventArgs e)
        {
            // Set custom date format
            DateFrom.CustomFormat = "dd/MM/yyyy";
            DateTo.CustomFormat = "dd/MM/yyyy";

            if (_existing != null)
            {
                // Edit mode - fill form
                this.Text = "تعديل الخبرة المهنية";
                FillUI(_existing);
            }
            else
            {
                // Add mode
                this.Text = "إضافة خبرة مهنية";
                DateFrom.Value = DateTime.Now;
                DateTo.Value = DateTime.Now;
            }
        }

        private void FillUI(Experience exp)
        {
            CertificateNumberTxt.Text = exp.CertRef;
            InstitutionNameTxt.Text = exp.Company;
            PositionTxt.Text = exp.Position;

            if (exp.DateFrom.HasValue)
                DateFrom.Value = exp.DateFrom.Value;

            if (exp.DateTo.HasValue)
                DateTo.Value = exp.DateTo.Value;
        }

        private Experience BuildFromUI()
        {
            var exp = new Experience
            {
                ExperienceId = _existing?.ExperienceId,
                FolderNum = _folderNum,
                FolderNumYear = _folderNumYear,
                CertRef = CertificateNumberTxt.Text?.Trim(),
                Company = InstitutionNameTxt.Text?.Trim(),
                Position = PositionTxt.Text?.Trim(),
                DateFrom = DateFrom.Value.Date,
                DateTo = DateTo.Value.Date
            };

            return exp;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var entity = BuildFromUI();

                // Show confirmation dialog
                var verb = (_existing == null) ? "إضافة" : "تعديل";
                var confirmMsg = $"هل تريد {verb} هذه الخبرة المهنية؟";
                var confirmResult = MessageBox.Show(confirmMsg, "تأكيد",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if (confirmResult != DialogResult.Yes)
                    return;

                var (ok, errors) = _svc.AddOrUpdate(entity);
                if (!ok)
                {
                    MessageBox.Show(string.Join("\n", errors), "خطأ في التحقق",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ExperienceSaved?.Invoke(this, entity);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ في الحفظ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}