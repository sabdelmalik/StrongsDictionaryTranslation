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
    public enum DictionaryType
    {
        SPREADSHEETS,
        TAB_SEPARATED_TEXT
    }

    public partial class ConfigurationDialog : Form
    {
        public ConfigurationDialog()
        {
            InitializeComponent();
            rbtnSpreadsheets.Checked = true;
        }

        private void ConfigurationDialog_Load(object sender, EventArgs e)
        {
           
        }

        public DictionaryType DictionaryToUse
        {
            get
            {
                return rbtnSpreadsheets.Checked ? DictionaryType.SPREADSHEETS : DictionaryType.TAB_SEPARATED_TEXT;
            }
            set
            {
                rbtnSpreadsheets.Checked = (value == DictionaryType.SPREADSHEETS);
                rbtnTabSeparatedText.Checked = (value == DictionaryType.TAB_SEPARATED_TEXT);
            }
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

        public string TranslatedSpreadsheetsFolder
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

        public string SourceTabSeparatedText
        {
            get
            {
                return tbSourceTabSeparatedText.Text;
            }
            set
            {
                tbSourceTabSeparatedText.Text = value;
            }
        }

        public string TranslatedTabSeparatedText
        {
            get
            {
                return tbTranslatedTabSeparatedText.Text;
            }
            set
            {
                tbTranslatedTabSeparatedText.Text = value;
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
                if (i == -1)
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

        private void btnSourceTabSeparatedText_Click(object sender, EventArgs e)
        {
            GetFile("Select Source Text File", this.tbSourceTabSeparatedText);
        }

        private void btnTranslatedTabSeparatedText_Click(object sender, EventArgs e)
        {
            GetFile("Select Translated Text File", this.tbTranslatedTabSeparatedText);
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

        private void rbtnTabSeparatedText_CheckedChanged(object sender, EventArgs e)
        {
            SetDictionaryType();
        }

        private void rbtnSpreadsheets_CheckedChanged(object sender, EventArgs e)
        {
            SetDictionaryType();
        }

        private void SetDictionaryType()
        {
            if (rbtnSpreadsheets.Checked)
            {
                tbSourceSpreadsheets.Enabled = true;
                tbTranslatedSpreadsheets.Enabled = true;
                btnSourceSpreadsheets.Enabled = true;
                btnTranslatedSpreadsheets.Enabled = true;

                tbSourceTabSeparatedText.Enabled = false;
                tbTranslatedTabSeparatedText.Enabled = false;
                btnSourceTabSeparatedText.Enabled = false;
                btnTranslatedTabSeparatedText.Enabled = false;
            }
            else
            {
                tbSourceSpreadsheets.Enabled = false;
                tbTranslatedSpreadsheets.Enabled = false;
                btnSourceSpreadsheets.Enabled = false;
                btnTranslatedSpreadsheets.Enabled = false;

                tbSourceTabSeparatedText.Enabled = true;
                tbTranslatedTabSeparatedText.Enabled = true;
                btnSourceTabSeparatedText.Enabled = true;
                btnTranslatedTabSeparatedText.Enabled = true;
            }
        }

    }
}
