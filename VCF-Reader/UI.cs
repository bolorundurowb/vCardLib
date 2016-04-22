using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Thought.vCards;

namespace VCF_Reader
{
    public partial class UI : Form
    {
        private List<DataGridViewRow> allRows;

        public UI()
        {
            InitializeComponent();
            allRows = new List<DataGridViewRow>();
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.LastFilePath))
                if (Properties.Settings.Default.LastFilePath.Contains(".vcf"))
                {
                    LoadVCF(Properties.Settings.Default.LastFilePath);
                    lbl_file_path.Text = Properties.Settings.Default.LastFilePath;
                }
        }

        private void btn_browse_Click(object sender, EventArgs e)
        {
            ofd_pick_file.ShowDialog();
            if (!string.IsNullOrWhiteSpace(ofd_pick_file.FileName))
            {
                lbl_file_path.Text = ofd_pick_file.FileName;
                LoadVCF(ofd_pick_file.FileName);
            }
        }

        private void UI_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (lbl_file_path.Text.Contains(".vcf"))
            {
                Properties.Settings.Default.LastFilePath = lbl_file_path.Text;
                Properties.Settings.Default.Save();
            }
        }

        private void LoadVCF(string filePath)
        {
            dgv_display.Rows.Clear();
            vCard vcard = new vCard(filePath);
            object[] row = new object[5];
            row[0] = vcard.FamilyName;
            row[1] = vcard.GivenName;
            row[2] = vcard.EmailAddresses.Count > 0 ? vcard.EmailAddresses[0].ToString() : "";
            row[3] = vcard.Phones.Count() > 0 ? vcard.Phones[0].FullNumber : "";
            row[4] = vcard.Phones.Count() > 1 ? vcard.Phones[1].FullNumber : "";
            dgv_display.Rows.Add(row);

            foreach (DataGridViewRow dRow in dgv_display.Rows)
            {
                allRows.Add(dRow);
            }
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            List<DataGridViewRow> matchedRows = new List<DataGridViewRow>();
            foreach (var row in allRows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value.ToString().ToLower().Contains(txt_search.Text.ToLower()))
                        matchedRows.Add(row);
                }
            }
            dgv_display.Rows.Clear();
            foreach(var row in matchedRows)
            {
                if (!dgv_display.Rows.Contains(row))
                    dgv_display.Rows.Add(row);
            }
        }
    }
}
