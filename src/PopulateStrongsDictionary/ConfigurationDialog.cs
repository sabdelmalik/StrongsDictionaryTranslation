using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PopulateStrongsDictionary
{
    public partial class ConfigurationDialog : Form
    {
        public ConfigurationDialog()
        {
            InitializeComponent();
        }

        public string SourceSpreadsheetsFolder
        {
            get
            {
                return tbSourceSpreadsheets.Text;
            }
            set
            {
                tbSourceSpreadsheets.Text = value;
            }
        }

        public string SourceTranslatedSpreadsheetsFolder
        {
            get
            {
                return tbTranslatedSpreadsheets.Text;
            }
            set
            {
                tbTranslatedSpreadsheets.Text = value;
            }
        }

        public string TaggedBibleFile
        {
            get
            {
                return tbTaggedBible.Text;
            }
            set
            {
                tbTaggedBible.Text = value;
            }
        }

        public int SelectedLanguage
        {
            get
            {
                int i = this.comboBoxLanguage.SelectedIndex;
                if(i == -1)
                {
                    return 0;
                }

                Language l = (Language)this.comboBoxLanguage.Items[i];

                return l.ID;
            }

            set
            {
                for (int i = 0; i < this.comboBoxLanguage.Items.Count; i++)
                {
                    if (value == ((Language)this.comboBoxLanguage.Items[i]).ID)
                    {
                        this.comboBoxLanguage.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        public void AddLanguage(Language language)
        {
            comboBoxLanguage.Items.Add(language);
        }
        private void btnSourceSpreadsheets_Click(object sender, EventArgs e)
        {
            GetFolder("Select Source Spreadsheets Folder", this.tbSourceSpreadsheets);
        }

        private void btnTranslatedSpreadsheets_Click(object sender, EventArgs e)
        {
            GetFolder("Select Translated Spreadsheets Folder", this.tbTranslatedSpreadsheets);
        }

        private void btnTaggedBible_Click(object sender, EventArgs e)
        {
            GetFile("Select Bible File", this.tbTaggedBible);
        }

        private void GetFolder(string caption, TextBox target)
        {
            folderBrowserDialog1.Description = caption;

            if (!string.IsNullOrEmpty(target.Text))
            {
                folderBrowserDialog1.SelectedPath = target.Text;
            }


            DialogResult result = folderBrowserDialog1.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                target.Text = folderBrowserDialog1.SelectedPath;
            }

        }

        private void GetFile(string caption, TextBox target)
        {
            openFileDialog1.Title = caption;

            if (!string.IsNullOrEmpty(target.Text))
            {
                openFileDialog1.InitialDirectory = Path.GetDirectoryName(target.Text);
            }


            DialogResult result = openFileDialog1.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                target.Text = openFileDialog1.FileName;
            }

        }
    }
}
