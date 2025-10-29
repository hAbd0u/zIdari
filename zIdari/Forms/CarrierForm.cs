using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
