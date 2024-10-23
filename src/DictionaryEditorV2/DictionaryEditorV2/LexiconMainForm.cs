using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

using WeifenLuo.WinFormsUI.Docking;
using System.Diagnostics;
using System.Threading;

using System.Reflection;

using System.Linq.Expressions;
using static System.Net.WebRequestMethods;
using Microsoft.VisualBasic.Logging;
using System.Xml.Linq;

using DictionaryEditorV2.Editor;
using DictionaryEditorV2.GlossReferences;
using DictionaryEditorV2.Lexicon;
using DictionaryEditorV2.GoogleAPI;

namespace DictionaryEditorV2
{
    public partial class LexiconMainForm : Form
    {
        private const bool dev = false;

        private WordReferences wordReferencesPanel;
        private EditorPanel editorPanel;
        private LexiconPanel lexiconPanel;
        private ProgressForm progressForm;


        private bool m_bSaveLayout = true;
        private DeserializeDockContent m_deserializeDockContent;

        // to save updated dictionary
        // https://stackoverflow.com/questions/36333567/saving-a-dictionaryint-object-in-c-sharp-serialization

        private ConfigurationHolder config = new ConfigurationHolder();

        private ImportExportSheets importExportSheets = new ImportExportSheets();

        public LexiconMainForm()
        {
            InitializeComponent();

            dockPanel.DocumentStyle = DocumentStyle.DockingWindow;
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);

            this.Resize += BibleTaggingForm_Resize;

            // allow key press detection
            this.KeyPreview = true;
            // handel keypress
            this.KeyDown += BibleTaggingForm_KeyDown;

        }

        #region Form Events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LexiconMainForm_Load(object sender, EventArgs e)
        {
            #region WinFormUI setup
            this.dockPanel.Theme = this.vS2013BlueTheme1;

            string configFile = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "DockPanel.config");

            if (System.IO.File.Exists(configFile))
                dockPanel.LoadFromXml(configFile, m_deserializeDockContent);


            progressForm = new ProgressForm(this);

            lexiconPanel = new LexiconPanel();
            lexiconPanel.Text = "Lexicon";
            lexiconPanel.TabText = lexiconPanel.Text;

            editorPanel = new EditorPanel(this, lexiconPanel);
            editorPanel.Text = "Lexicon Translation Editor";
            editorPanel.TabText = editorPanel.Text;

            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                editorPanel.MdiParent = this;
                editorPanel.Show(dockPanel, DockState.Document);
            }
            else
                editorPanel.Show(dockPanel, DockState.Document);

            //editorPanel.CloseButton = false;
            editorPanel.CloseButtonVisible = false;

            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                lexiconPanel.MdiParent = this;
                lexiconPanel.Show(dockPanel, DockState.DockTop);
            }
            else
                lexiconPanel.Show(dockPanel, DockState.DockTop);

            lexiconPanel.CloseButtonVisible = false;


            wordReferencesPanel = new WordReferences(this, lexiconPanel); //CreateNewDocument();
            wordReferencesPanel.Text = "Word References";
            wordReferencesPanel.TabText = wordReferencesPanel.Text;
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                wordReferencesPanel.MdiParent = this;
                wordReferencesPanel.Show(dockPanel, DockState.DockRight);
            }
            else
                wordReferencesPanel.Show(dockPanel, DockState.DockRight);

            if (!IsValidSettings())
            {
                if (GetSettings() != DialogResult.OK)
                {
                    CloseForm();
                    return;
                }
            }

            lexiconPanel.Initialise();
            wordReferencesPanel.CloseButtonVisible = false;


            #endregion WinFormUI setup


            Assembly assembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = assembly.GetName();
            Version version = assemblyName.Version;
            this.Text = "Lexicon Editor " + version.ToString();

            string execFolder = Path.GetDirectoryName(assembly.Location);
            Tracing.InitialiseTrace(execFolder);

            this.Closing += MainForm_Closing;

        }

        private bool IsValidSettings()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.Database)) return false;
            if (string.IsNullOrEmpty(Properties.Settings.Default.SheetsCredentialFileName)) return false;
            if (string.IsNullOrEmpty(Properties.Settings.Default.DriveCredentialFileName)) return false;

            string path = Properties.Settings.Default.DriveApplicationName;
            if (string.IsNullOrEmpty(path)) return false;
            string content = System.IO.File.ReadAllText(path);
            if (!content.Contains("private_key")) return false;

            path = Properties.Settings.Default.DriveApplicationName;
            if (string.IsNullOrEmpty(path)) return false;
            content = System.IO.File.ReadAllText(path);
            if (!content.Contains("private_key")) return false;

            return true;
        }

        private void BibleTaggingForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Modifiers == Keys.Control)
            {
                //                target.SaveUpdates();
            }
        }

        private void BibleTaggingForm_Resize(object sender, EventArgs e)
        {
            progressForm.Location = new Point(
                this.Location.X + (this.Width / 2) - (progressForm.Width / 2),
                this.Location.Y + (this.Height / 2) - (progressForm.Height / 2));
            //waitCursorAnimation.Location = new Point(
            //    (this.Width / 2) - (waitCursorAnimation.Width / 2), 
            //    (this.Height / 2) - (waitCursorAnimation.Height / 2));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (MessageBox.Show("This will close down the application. Confirm?", "Close Bible Edit", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    e.Cancel = true;
                    this.Activate();
                }
                else
                {

                    // Save the Verses Updates

                    Properties.Settings.Default.Save();

                    string configFile = Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "DockPanel.config");
                    if (m_bSaveLayout)
                        dockPanel.SaveAsXml(configFile);
                    else if (System.IO.File.Exists(configFile))
                        System.IO.File.Delete(configFile);
                }
            }
            catch (Exception ex)
            {
                Tracing.TraceException(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion Form Events

        private void CloseForm()
        {
            if (InvokeRequired)
            {
                // Call this same method but append THREAD2 to the text
                Action safeWrite = delegate { CloseForm(); };
                Invoke(safeWrite);
            }
            else
            {
                this.Close();
            }
        }

        private void StartGui()
        {
            Tracing.TraceEntry(MethodBase.GetCurrentMethod().Name);

            if (InvokeRequired)
            {
                // Call this same method but append THREAD2 to the text
                Action safeWrite = delegate { StartGui(); };
                Invoke(safeWrite);
            }
            else
            {
                //lexiconPanel.CurrentBook = Properties.Settings.Default.LastBook;
                //lexiconPanel.CurrentChapter = Properties.Settings.Default.LastChapter;
                //lexiconPanel.CurrentVerse = Properties.Settings.Default.LastVerse;
                //lexiconPanel.FireStrongChanged();
                //editorPanel.TargetDirty = false;
            }
        }

        public void WaitCursorControl(bool wait)
        {
            Tracing.TraceEntry(MethodBase.GetCurrentMethod().Name);

            if (InvokeRequired)
            {
                // Call this same method but append THREAD2 to the text
                Action safeWrite = delegate { WaitCursorControl(wait); };
                Invoke(safeWrite);
            }
            else
            {
                if (wait)
                {
                    this.Cursor = Cursors.WaitCursor;
                    //waitCursorAnimation.Visible = true;
                    //aitCursorAnimation.BringToFront();
                    progressForm.Clear();

                    //progressForm.Location = new Point((this.Width / 2) - (progressForm.Width / 2),
                    //                                  (this.Height / 2) - (progressForm.Height / 2));

                    progressForm.Show();

                    progressForm.Location = new Point(
                        this.Location.X + (this.Width / 2) - (progressForm.Width / 2),
                        this.Location.Y + (this.Height / 2) - (progressForm.Height / 2));


                    menuStrip1.Enabled = false;
                    lexiconPanel.Enabled = false;
                    editorPanel.Enabled = false;
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    //waitCursorAnimation.Visible = false;
                    progressForm.Visible = false;
                    menuStrip1.Enabled = true;
                    lexiconPanel.Enabled = true;
                    editorPanel.Enabled = true;
                }
            }
        }

        public void UpdateProgress(string label, int progress)
        {
            if (InvokeRequired)
            {
                try
                {
                    // Call this same method but append THREAD2 to the text
                    Action safeWrite = delegate { UpdateProgress(label, progress); };
                    Invoke(safeWrite);
                }
                catch (Exception ex) { }
            }
            else
            {
                progressForm.Label = label;
                progressForm.Progress = progress;
            }
        }

        private DialogResult ShowMessageBox(string text, string caption, MessageBoxButtons buttons)
        {
            DialogResult result = DialogResult.OK;

            if (InvokeRequired)
            {
                // Call this same method but append THREAD2 to the text
                //Action safeWrite = delegate { ShowMessageBox(text, caption, buttons); };
                result = (DialogResult)Invoke(new Func<DialogResult>(() => ShowMessageBox(text, caption, buttons)));
            }
            else
            {
                result = MessageBox.Show(text, caption, buttons);
            }
            return result;
        }

        #region public properties

        public ConfigurationHolder Config { get { return config; } }
        public EditorPanel EditorPanel { get { return editorPanel; } }
        public LexiconPanel LexiconEntryPanel { get { return lexiconPanel; } }

        #endregion public properties

        #region WinFormUI
        private IDockContent FindDocument(string text)
        {
            if (dockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                foreach (Form form in MdiChildren)
                    if (form.Text == text)
                        return form as IDockContent;

                return null;
            }
            else
            {
                foreach (IDockContent content in dockPanel.Documents)
                    if (content.DockHandler.TabText == text)
                        return content;

                return null;
            }
        }

        private WordReferences CreateNewDocument()
        {
            WordReferences dummyDoc = new WordReferences();

            int count = 1;
            string text = $"Document{count}";
            while (FindDocument(text) != null)
            {
                count++;
                text = $"Document{count}";
            }

            dummyDoc.Text = text;
            return dummyDoc;
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            // DummyDoc overrides GetPersistString to add extra information into persistString.
            // Any DockContent may override this value to add any needed information for deserialization.

            string[] parsedStrings = persistString.Split(new char[] { ',' });
            if (parsedStrings.Length != 3)
                return null;

            if (parsedStrings[0] != typeof(DummyDoc).ToString())
                return null;

            DummyDoc dummyDoc = new DummyDoc();
            if (parsedStrings[1] != string.Empty)
                dummyDoc.FileName = parsedStrings[1];
            if (parsedStrings[2] != string.Empty)
                dummyDoc.Text = parsedStrings[2];

            return dummyDoc;
        }

        #endregion WinFormUI


        #region Bible File Loading

        /// <summary>
        /// 
        /// </summary>
        private void GetBiblesFolder()
        {
            Tracing.TraceEntry(MethodBase.GetCurrentMethod().Name);

            if (InvokeRequired)
            {
                Action safeWrite = delegate { GetBiblesFolder(); };
                Invoke(safeWrite);
            }
            else
            {
                string folderPath = string.Empty;
                DialogResult res = folderBrowserDialog1.ShowDialog(this);
                if (res == DialogResult.OK)
                {
                    folderPath = folderBrowserDialog1.SelectedPath;
                }
                // Properties.Settings.Default.BiblesFolder = folderPath;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public string GetBibleFilePath(string directory, string title)
        {
            Tracing.TraceEntry(MethodBase.GetCurrentMethod().Name, directory, title);

            string biblePath = string.Empty;

            openFileDialog.Title = title;
            openFileDialog.InitialDirectory = directory;
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            openFileDialog.RestoreDirectory = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Read the contents of the file into a stream
                biblePath = openFileDialog.FileName;
            }

            Tracing.TraceExit(MethodBase.GetCurrentMethod().Name, biblePath);
            return biblePath;
        }


        #endregion Bible File Loading

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetSettings();
        }

        private DialogResult GetSettings()
        {
            SettingsForm settingsForm = new SettingsForm();

            DialogResult result = settingsForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                Properties.Settings.Default.Save();
            }
            return result;
        }




        private void saveUpdatedTartgetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //target.SaveUpdates();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();

            aboutForm.ShowDialog();

        }

        private void importExportSheetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (importExportSheets ==  null || importExportSheets.IsDisposed)
                importExportSheets = new ImportExportSheets();
            importExportSheets.Show();
        }
    }
}
