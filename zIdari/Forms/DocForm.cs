using System;
using System.Windows.Forms;
using zIdari.Model;
using zIdari.Service;

namespace zIdari.Forms
{
    public partial class DocForm : Form
    {
        private readonly DocumentService _svc;
        private readonly Document _existing; // null => add

        public DocForm(DocumentService svc, Document existing = null)
        {
            InitializeComponent();
            _svc = svc;
            _existing = existing;
        }

        private void DocForm_Load(object sender, EventArgs e)
        {
            if (_existing != null)
            {
                typeTxt.Text = _existing.Type;
                titleTxt.Text = _existing.Title;
            }
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            var doc = new Document
            {
                DocumentId = _existing?.DocumentId ?? 0,
                Type = typeTxt.Text?.Trim(),
                Title = titleTxt.Text?.Trim(),
                // no description in schema; keep descTxt as auxiliary if desired
            };

            if (_existing == null)
            {
                var (ok, errors, id) = _svc.Add(doc);
                if (!ok)
                {
                    MessageBox.Show(string.Join("\n", errors), "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                var (ok, errors) = _svc.Update(doc);
                if (!ok)
                {
                    MessageBox.Show(string.Join("\n", errors), "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
