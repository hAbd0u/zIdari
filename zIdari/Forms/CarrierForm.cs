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
using zIdari.Repository;

namespace zIdari.Forms
{
    public partial class CarrierForm : Form
    {
        // Dictionary to map carrier types to their corresponding name options
        private readonly Dictionary<string, string[]> _carrierTypeOptions = new Dictionary<string, string[]>
        {
            { "توظيف", new[] { "إختبار", "شهادة", "إمتحان مهني" } },
            { "درجة", new string[0] },
            { "ترقية", new[] { "شهادة", "تكوين قبل الترقية", "إمتحان مهني", "سبيل الإختبار" } },
            { "توقيف", new[] { "التقاعد", "وفاة", "إستقالة", "عزل", "فسخ العقد", "النقل", "تسريح" } },
            { "إدماج", new string[0] },
            { "إنزال في الرتبة", new string[0] }
        };

        public CarrierForm()
        {
            InitializeComponent();

            // Wire up the event handler
            carrierTypeCombo.SelectedIndexChanged += CarrierTypeCombo_SelectedIndexChanged;
            docTypeComboList.SelectedIndexChanged += DocTypeComboList_SelectedIndexChanged;
            
            // Wire up Load event
            this.Load += CarrierForm_Load;
        }

        private void CarrierForm_Load(object sender, EventArgs e)
        {
            LoadDocumentTypes();
            LoadCorpsList();
            LoadBranchesList();
            LoadPositionsList();
        }

        private void DocTypeComboList_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDocumentTitles();
        }

        private void LoadDocumentTypes()
        {
            try
            {
                var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "kwin4rh.db");
                var repo = new DocumentRepository(dbPath);
                var types = repo.GetDistinctTypes();

                docTypeComboList.Items.Clear();
                docTypeComboList.Items.Add(""); // Add empty option
                docTypeComboList.Items.AddRange(types.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في تحميل أنواع الوثائق: {ex.Message}", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDocumentTitles()
        {
            try
            {
                // Clear the titles combo
                DocTitleComboList.Items.Clear();
                DocTitleComboList.Items.Add(""); // Add empty option

                // Get selected type
                string selectedType = docTypeComboList.SelectedItem?.ToString();
                
                // If no type selected or empty, just leave the titles combo with empty option
                if (string.IsNullOrWhiteSpace(selectedType))
                {
                    return;
                }

                // Load titles for the selected type
                var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "kwin4rh.db");
                var repo = new DocumentRepository(dbPath);
                var titles = repo.GetTitlesByType(selectedType);

                if (titles != null && titles.Count > 0)
                {
                    DocTitleComboList.Items.AddRange(titles.ToArray());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في تحميل عناوين الوثائق: {ex.Message}", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCorpsList()
        {
            try
            {
                var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "kwin4rh.db");
                var repo = new CorpBranchGradRepository(dbPath);
                var corps = repo.GetDistinctTitlesByType("corp");

                CorpListCombo.Items.Clear();
                CorpListCombo.Items.Add(""); // Add empty option
                
                if (corps != null && corps.Count > 0)
                {
                    CorpListCombo.Items.AddRange(corps.ToArray());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في تحميل الأسلاك: {ex.Message}", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBranchesList()
        {
            try
            {
                var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "kwin4rh.db");
                var repo = new CorpBranchGradRepository(dbPath);
                var branches = repo.GetDistinctTitlesByType("branche");

                BrancheListCombo.Items.Clear();
                BrancheListCombo.Items.Add(""); // Add empty option
                
                if (branches != null && branches.Count > 0)
                {
                    BrancheListCombo.Items.AddRange(branches.ToArray());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في تحميل الشعب: {ex.Message}", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPositionsList()
        {
            try
            {
                var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "kwin4rh.db");
                var repo = new CorpBranchGradRepository(dbPath);
                var positions = repo.GetDistinctTitlesByType("fonction");

                PositionListCombo.Items.Clear();
                PositionListCombo.Items.Add(""); // Add empty option
                
                if (positions != null && positions.Count > 0)
                {
                    PositionListCombo.Items.AddRange(positions.ToArray());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في تحميل المناصب: {ex.Message}", "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarrierTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected carrier type
            string selectedType = carrierTypeCombo.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedType) || !_carrierTypeOptions.ContainsKey(selectedType))
            {
                // Hide the combo if no type is selected or type is not found
                carrierNameCombo.Visible = false;
                return;
            }

            // Get the options for the selected type
            string[] options = _carrierTypeOptions[selectedType];

            if (options.Length == 0)
            {
                // Hide the combo if there are no options
                carrierNameCombo.Visible = false;
                carrierNameCombo.Items.Clear();
                carrierNameCombo.SelectedIndex = -1;
            }
            else
            {
                // Show the combo and populate it with options
                carrierNameCombo.Visible = true;
                carrierNameCombo.Items.Clear();
                carrierNameCombo.Items.AddRange(options);

                // Optionally select the first item
                if (carrierNameCombo.Items.Count > 0)
                {
                    carrierNameCombo.SelectedIndex = 0;
                }
            }
        }

        private void ScanBtn_Click(object sender, EventArgs e)
        {

        }

        private void DocEffectiveDate_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
