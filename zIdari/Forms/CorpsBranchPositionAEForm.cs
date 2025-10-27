using System;
using System.ComponentModel;
using System.Windows.Forms;
using zIdari.Model;
using zIdari.Service;

// Forms/CorpsBranchPositionAEForm.cs
namespace zIdari.Forms
{
    public partial class CorpsBranchPositionAEForm : Form
    {
        private readonly CorpBranchGradService _svc;
        private readonly CorpBranchGrad _existing;
        private readonly string _type;
        private int? _csgId;

        public event EventHandler<CorpBranchGrad> RecordSaved;

        // For adding new record
        public CorpsBranchPositionAEForm(CorpBranchGradService svc, string type)
        {
            InitializeComponent();
            _svc = svc ?? throw new ArgumentNullException(nameof(svc));
            _type = type ?? throw new ArgumentNullException(nameof(type));
            _existing = null;
            _csgId = null;
        }

        // For editing existing record
        public CorpsBranchPositionAEForm(CorpBranchGradService svc, CorpBranchGrad existing)
        {
            InitializeComponent();
            _svc = svc ?? throw new ArgumentNullException(nameof(svc));
            _existing = existing ?? throw new ArgumentNullException(nameof(existing));
            _type = existing.Type;
            _csgId = existing.CsgId;
        }

        // Designer-only constructor
        public CorpsBranchPositionAEForm()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
                throw new InvalidOperationException("Use CorpsBranchPositionAEForm(CorpBranchGradService, ...) at runtime.");
        }

        private void CorpsBranchPositionAEForm_Load(object sender, EventArgs e)
        {
            if (_existing != null)
            {
                // Edit mode - fill form
                this.Text = GetFormTitle(_type, isEdit: true);
                FillUI(_existing);
            }
            else
            {
                // Add mode
                this.Text = GetFormTitle(_type, isEdit: false);
            }
        }

        private string GetFormTitle(string type, bool isEdit)
        {
            string entityName = type switch
            {
                "corp" => "السلك",
                "branche" => "الفرع",
                "fonction" => "الرتبة",
                _ => "البيانات"
            };

            string verb = isEdit ? "تعديل" : "إضافة";
            return $"{verb} {entityName}";
        }

        private void FillUI(CorpBranchGrad cbg)
        {
            LawNameTxt.Text = cbg.LawNum;
            TitleTxt.Text = cbg.Title;
        }

        private CorpBranchGrad BuildFromUI()
        {
            var cbg = new CorpBranchGrad
            {
                CsgId = _csgId,
                Type = _type,
                LawNum = LawNameTxt.Text?.Trim(),
                Title = TitleTxt.Text?.Trim()
            };

            return cbg;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var entity = BuildFromUI();

                // Show confirmation dialog
                var verb = (_existing == null) ? "إضافة" : "تعديل";
                var confirmMsg = $"هل تريد {verb} هذا السجل؟";
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

                RecordSaved?.Invoke(this, entity);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ في الحفظ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}