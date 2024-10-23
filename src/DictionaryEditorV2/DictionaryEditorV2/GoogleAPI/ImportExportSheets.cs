using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DictionaryEditorV2.GoogleAPI
{
    public partial class ImportExportSheets : Form
    {
        public ImportExportSheets()
        {
            InitializeComponent();
        }

        private void ImportExportSheets_Load(object sender, EventArgs e)
        {
            btnExport.Enabled = false; btnImport.Enabled = false;
            btnExport.Paint += button_Paint;
            btnExport.MouseDown += button_MouseDown;
            btnExport.MouseUp += button_MouseUp;

            new Thread(() => { Initialise(); }).Start();
        }


        #region buttons appearance
        private bool blnButtonDown = false;

        private void button_Paint(object? sender, PaintEventArgs e)
        {
            if (blnButtonDown == false)
            {
                ControlPaint.DrawBorder(e.Graphics, (sender as System.Windows.Forms.Button).ClientRectangle,
                    System.Drawing.SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
                    System.Drawing.SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
                    System.Drawing.SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset,
                    System.Drawing.SystemColors.ControlLightLight, 5, ButtonBorderStyle.Outset);
            }
            else
            {
                ControlPaint.DrawBorder(e.Graphics, (sender as System.Windows.Forms.Button).ClientRectangle,
                    System.Drawing.SystemColors.ControlLightLight, 5, ButtonBorderStyle.Inset,
                    System.Drawing.SystemColors.ControlLightLight, 5, ButtonBorderStyle.Inset,
                    System.Drawing.SystemColors.ControlLightLight, 5, ButtonBorderStyle.Inset,
                    System.Drawing.SystemColors.ControlLightLight, 5, ButtonBorderStyle.Inset);
            }
        }

        private void button_MouseDown(object? sender, System.Windows.Forms.MouseEventArgs e)
        {
            blnButtonDown = true;
            (sender as System.Windows.Forms.Button).Invalidate();
        }

        private void button_MouseUp(object? sender, System.Windows.Forms.MouseEventArgs e)
        {
            blnButtonDown = false;
            (sender as System.Windows.Forms.Button).Invalidate();

        }
        #endregion buttons appearance

        private void Initialise()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => { Initialise(); }));
            }
            else
            {
                List<CloudSheetDef> sheetDefs = DriveUtils.Instance.GetCloudSheets();
                Dictionary<string, DbSheetDef> sheetIds = DriveUtils.Instance.GetSheetInfoFromDB();
                foreach (CloudSheetDef sheetDef in sheetDefs)
                {
                    if (!sheetIds.Keys.Contains(sheetDef.Name))
                        sheetDef.MissingFromDB = true;
                    else if (sheetIds[sheetDef.Name].SheetId != sheetDef.SheetId)
                        sheetDef.IdChanged = false;
                }

                lbSheets.Items.Clear();
                lbSheets.Items.AddRange(sheetDefs.ToArray());
            }
        }


        private void lbSheets_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnExport.Enabled = true; btnImport.Enabled = true;

            traceBox.Clear();
            WriteTrace("Selected Sheets:", Color.Black);
            foreach (var item in lbSheets.SelectedItems)
            {
                CloudSheetDef sd = (CloudSheetDef)item;
                WriteTrace(string.Format("'{0}', Last updated: {1}, By: {2}",
                                            sd.Name,
                                            sd.ModifiedTimestamp,
                                            sd.LastModifyingUserName), Color.Black);
            }
        }

        private void WriteTrace(string text, System.Drawing.Color color)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { WriteTrace(text, color); }));
            }
            else
            {
                traceBox.SelectionColor = color;
                if (text.Length > 0)
                {
                    //string txt = string.Format("{0}: {1}v", DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss.fff"), text);
                    //traceBox.AppendText(txt);
                    traceBox.SelectedText = text + "\r\n"; // txt;
                }
                else
                {
                    traceBox.AppendText("\r\n");
                }
                traceBox.ScrollToCaret();
                System.Windows.Forms.Application.DoEvents();
            }

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to overwrite the cloud sheets?", "Warning", MessageBoxButtons.YesNo);
            if (result == DialogResult.No) return;

            var selectedSheets = lbSheets.SelectedItems;
            bool firstSheet = true;
            foreach (CloudSheetDef item in selectedSheets)
            {
                WriteTrace("Updating " + item.Name + "...", Color.Blue);

                SheetsUtils.Instance.ClearSheet(item.SheetId);

                SheetsUtils.Instance.SetSheetBanding(item.SheetId);

                SheetUploader.Instance.PopulateSheet(item);

                if (firstSheet) firstSheet = false;
                else Thread.Sleep(2500); // throttle so google does not complain
            }
            WriteTrace("All Done!", Color.Green);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            var selectedSheets = lbSheets.SelectedItems;
            bool firstSheet = true;
            foreach (CloudSheetDef item in selectedSheets)
            {
                WriteTrace("Importing " + item.Name + "...", Color.Blue);

                SheetsUtils.Instance.GetSheetData(item);
            }
            WriteTrace("All Done!", Color.Green);

        }
    }
}

