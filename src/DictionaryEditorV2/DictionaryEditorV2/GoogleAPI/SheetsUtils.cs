

using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using SheetData = Google.Apis.Sheets.v4.Data;

using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using DriveData = Google.Apis.Drive.v3.Data;

using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Npgsql;

namespace DictionaryEditorV2.GoogleAPI
{
    internal class SheetsUtils
    {
        private SheetsService? sheetsService = null;

        string credentialFileName = Properties.Settings.Default.DriveApplicationName;
        string applicationName = Properties.Settings.Default.DriveCredentialFileName;

        private static SheetsUtils instance = null;
        private static readonly object lockObj = new object();

        public static SheetsUtils Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new SheetsUtils();
                        }
                    }
                }
                return instance;
            }
        }

        private SheetsUtils()
        {
            InitialiseSheetsService();
        }

        #region Trace
        private void Trace(string text, System.Drawing.Color color,
        [CallerLineNumber] int lineNumber = 0,
        [CallerMemberName] string caller = null)
        {
            Tracing.TraceInfo(string.Format("{0}#{1}", caller == null ? "???" : caller, lineNumber), text);
        }
        private void TraceError(
                string text,
                [CallerLineNumber] int lineNumber = 0,
                [CallerMemberName] string caller = null)
        {
            Tracing.TraceError(string.Format("{0}#{1}", caller == null ? "???" : caller, lineNumber), text);
        }
        #endregion Trace

        #region Public Methods

        #region Initializers
        private void InitialiseSheetsService()
        {
            try
            {
                string[] Scopes = { SheetsService.Scope.Spreadsheets };
                var credential = GoogleCredential.FromStream(new FileStream(credentialFileName, FileMode.Open)).CreateScoped(Scopes);
                sheetsService = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = applicationName,
                });
            }
            catch (Exception ex)
            {
                TraceError(ex.Message);
            }

        }

        #endregion Initializers

        #region Sheets
        public bool ClearSheet(string spreadsheetId, int sheetId = 0, bool valuesOnly = true)
        {
            lock (lockObj)
            {
                bool result = false;

                if (sheetsService != null)
                {
                    try
                    {
                        List<Request> requests = new List<Request>();
                        requests.AddRange(GetClearSheetRequests(sheetId, valuesOnly));

                        var requestBody = new BatchUpdateSpreadsheetRequest();
                        requestBody.Requests = requests;

                        sheetsService.Spreadsheets.BatchUpdate(requestBody, spreadsheetId).Execute();

                        result = true;
                    }
                    catch (Exception ex)
                    {
                        TraceError(ex.Message);
                    }
                }

                return result;
            }
        }

        public void SetSheetBanding(string sheetId)
        {
            lock (lockObj)
            {

                if (sheetsService != null)
                {
                    try
                    {
                        // Clear sheet
                        //if (clear)
                        // sheetUtils.ClearSheet(sheetId, 0, false);

                        // set column widths
                        List<ColumnWidth> columnWidths = new List<ColumnWidth>();
                        columnWidths.Add(new ColumnWidth(0, 0, 1, 75));
                        columnWidths.Add(new ColumnWidth(0, 1, 3, 100));
                        columnWidths.Add(new ColumnWidth(0, 3, 4, 600));
                        SetColumnWidths(sheetId, columnWidths);

                        try
                        {
                            var deleteBanding = new SheetData.Request
                            {
                                DeleteBanding = new SheetData.DeleteBandingRequest
                                {
                                    BandedRangeId = 1
                                }
                            };
                            var requestBody1 = new Google.Apis.Sheets.v4.Data.BatchUpdateSpreadsheetRequest();
                            var requests1 = new List<Request>();
                            requests1.Add(deleteBanding);
                            requestBody1.Requests = requests1;

                            sheetsService.Spreadsheets.BatchUpdate(requestBody1, sheetId).Execute();
                        }
                        catch { }



                        System.Drawing.Color color1 = System.Drawing.Color.LightGreen;
                        System.Drawing.Color color2 = System.Drawing.Color.White;
                        System.Drawing.Color color3 = System.Drawing.Color.LightSkyBlue;

                        var banding = new SheetData.Request
                        {
                            AddBanding = new SheetData.AddBandingRequest
                            {
                                BandedRange = new SheetData.BandedRange
                                {
                                    BandedRangeId = 1,
                                    Range = new SheetData.GridRange
                                    {
                                        StartRowIndex = 0,
                                        StartColumnIndex = 0,
                                    },
                                    RowProperties = new SheetData.BandingProperties
                                    {
                                        FirstBandColorStyle = new ColorStyle
                                        {
                                            RgbColor = new SheetData.Color
                                            {
                                                Blue = (float)color1.B / 255.0f,
                                                Green = (float)color1.G / 255.0f,
                                                Red = (float)color1.R / 255.0f,
                                                Alpha = (float)1
                                            }
                                        },
                                        SecondBandColorStyle = new ColorStyle
                                        {
                                            RgbColor = new SheetData.Color
                                            {
                                                Blue = (float)color2.B / 255.0f,
                                                Green = (float)color2.G / 255.0f,
                                                Red = (float)color2.R / 255.0f,
                                                Alpha = (float)1
                                            }
                                        }
                                    }
                                },
                            }
                        };

                        var requestBody = new SheetData.BatchUpdateSpreadsheetRequest();

                        var requests = new List<Request>();
                        requests.Add(banding);
                        requestBody.Requests = requests;

                        sheetsService.Spreadsheets.BatchUpdate(requestBody, sheetId).Execute();
                    }
                    catch (Exception ex)
                    {
                        TraceError(ex.Message);
                    }
                }
            }
        }
        public void GetSheetData(CloudSheetDef cloudSheetDef)
        {
            lock (lockObj)
            {

                if (sheetsService != null)
                {
                    try
                    {
                        string range = "sheet1!A:D";

                        SpreadsheetsResource.ValuesResource.GetRequest request =
                        sheetsService.Spreadsheets.Values.Get(cloudSheetDef.SheetId, range);
                        request.MajorDimension = SpreadsheetsResource.ValuesResource.GetRequest.MajorDimensionEnum.ROWS;

                        var response = request.Execute();
                        IList<IList<object>> values = response.Values;
                        if (values != null && values.Count > 0)
                        {
                            for (int i = 0; i < values.Count; i += 2)
                            {
                                var row1 = values[i];
                                var row2 = values[i + 1];
                                string status = row2[0].ToString();
                                if (status == Enum.GetName(typeof(SheetStatus), SheetStatus.Pending))
                                    continue;

                                SheetStatus sheetStatus = SheetStatus.Approved;
                                if (status == Enum.GetName(typeof(SheetStatus), SheetStatus.Reviewed))
                                    sheetStatus = SheetStatus.Reviewed;

                                string strongs = row1[0].ToString().Trim();
                                string gloss = row2[2].ToString();
                                string text = row2[3].ToString();

                                SheetsImporter.Instance.UpdateTranslation(sheetStatus, strongs, gloss, text, cloudSheetDef);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        TraceError(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spreadsheetId"></param>
        /// <param name="sheetId"></param>
        /// <param name="userEnteredFormat">
        ///     CellFormat:
        ///         NumberFormat
        ///         BackgroundColor
        ///         Borders
        ///         Padding
        ///         HorizontalAlignment
        ///         VerticalAlignment
        ///         WrapStrategy
        ///         TextDirection
        ///         TextFormat
        ///         
        /// </param>
        /// <param name="startRow"></param>
        /// <param name="endRow"></param>
        /// <param name="startColumn"></param>
        /// <param name="endColumn"></param>
        /// <returns></returns>
        public bool SetCellFormat(string spreadsheetId,
            UserEnteredFormat? userEnteredFormat = null,
            int sheetId = 0, int? startRow = null, int? endRow = null, int? startColumn = null, int? endColumn = null)
        {
            lock (lockObj)
            {
                bool result = false;
                if (sheetsService != null)
                {
                    try
                    {
                        var requests = new List<Request>();
                        requests.AddRange(GetRepeatCellRequests(userEnteredFormat, sheetId, startRow, endRow, startColumn, endColumn));

                        var requestBody = new SheetData.BatchUpdateSpreadsheetRequest();
                        requestBody.Requests = requests;

                        sheetsService.Spreadsheets.BatchUpdate(requestBody, spreadsheetId).Execute();

                        result = true;
                    }
                    catch (Exception ex)
                    {
                        TraceError(ex.Message);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spreadsheetId"></param>
        /// <param name="values"></param>
        /// <param name="sheetId"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public bool UpdateRowData(string spreadsheetId, List<List<object>> values,
            int sheetId = 0, int? rowIndex = null, int? columnIndex = null)
        {
            lock (lockObj)
            {
                bool result = false;
                if (sheetsService != null)
                {
                    try
                    {
                        var requests = new List<Request>();
                        requests.AddRange(GetUpdateCellRequests(values, sheetId, rowIndex, columnIndex));

                        var requestBody = new SheetData.BatchUpdateSpreadsheetRequest();
                        requestBody.Requests = requests;

                        sheetsService.Spreadsheets.BatchUpdate(requestBody, spreadsheetId).Execute();

                        result = true;
                    }
                    catch (Exception ex)
                    {
                        TraceError(ex.Message);
                    }
                }

                return result;
            }
        }

        public UserEnteredFormat? GetCellFormat(
        NumberFormat? numberFormat = null,
        System.Drawing.Color? backgroundColor = null,
        SheetData.Borders? borders = null,
        SheetData.Padding? padding = null,
        SheetHorizontalAlignment horizontalAlignment = SheetHorizontalAlignment.HORIZONTAL_ALIGN_UNSPECIFIED,
        SheetVerticalAlignment verticalAlignment = SheetVerticalAlignment.VERTICAL_ALIGN_UNSPECIFIED,
        SheetWrapStrategy wrapStrategy = SheetWrapStrategy.WRAP_STRATEGY_UNSPECIFIED,
        SheetTextDirection textDirection = SheetTextDirection.TEXT_DIRECTION_UNSPECIFIED,
        SheetData.TextFormat? textFormat = null
        )
        {
            lock (this)
            {
                UserEnteredFormat? userEnteredFormat = null;
                string fields = string.Empty;
                CellFormat cellFormat = new CellFormat();

                if (numberFormat != null)
                {
                    cellFormat.NumberFormat = numberFormat;
                    fields += "UserEnteredFormat." + "NumberFormat,";
                }

                if (backgroundColor != null)
                {
                    System.Drawing.Color color = (System.Drawing.Color)backgroundColor;
                    cellFormat.BackgroundColor = new SheetData.Color()
                    {
                        Blue = (float)color.B / 255.0f,
                        Green = (float)color.G / 255.0f,
                        Red = (float)color.R / 255.0f,
                        Alpha = (float)1
                    };
                    fields += "UserEnteredFormat." + "BackgroundColor,";
                }

                if (borders != null)
                {
                    cellFormat.Borders = borders;
                    fields += "UserEnteredFormat." + "Borders,";
                }
                if (padding != null)
                {
                    cellFormat.Padding = padding;
                    fields += "UserEnteredFormat." + "Padding,";
                }

                if (horizontalAlignment != SheetHorizontalAlignment.HORIZONTAL_ALIGN_UNSPECIFIED)
                {
                    cellFormat.HorizontalAlignment = Enum.GetName(typeof(SheetHorizontalAlignment), horizontalAlignment);
                    fields += "UserEnteredFormat." + "HorizontalAlignment,";
                }
                if (verticalAlignment != SheetVerticalAlignment.VERTICAL_ALIGN_UNSPECIFIED)
                {
                    cellFormat.VerticalAlignment = Enum.GetName(typeof(SheetVerticalAlignment), verticalAlignment);
                    fields += "UserEnteredFormat." + "VerticalAlignment,";
                }
                if (wrapStrategy != SheetWrapStrategy.WRAP_STRATEGY_UNSPECIFIED)
                {
                    cellFormat.WrapStrategy = Enum.GetName(typeof(SheetWrapStrategy), wrapStrategy);
                    fields += "UserEnteredFormat." + "WrapStrategy,";
                }

                if (textDirection != SheetTextDirection.TEXT_DIRECTION_UNSPECIFIED)
                {
                    cellFormat.TextDirection = Enum.GetName(typeof(SheetTextDirection), textDirection);
                    fields += "UserEnteredFormat." + "TextDirection,";
                }

                if (textFormat != null)
                {
                    cellFormat.TextFormat = textFormat;
                    fields += "UserEnteredFormat." + "TextFormat,";
                }

                if (fields != string.Empty)
                {
                    userEnteredFormat = new UserEnteredFormat(cellFormat, fields);
                }
                return userEnteredFormat;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="foregroundColor"></param>
        /// <param name="fontFamily"></param>
        /// <param name="fontSize"></param>
        /// <param name="bold"></param>
        /// <param name="italic"></param>
        /// <param name="strikethrough"></param>
        /// <param name="underline"></param>
        /// <returns></returns>
        public SheetData.TextFormat? GetTextFormat(
            System.Drawing.Color? foregroundColor = null,
            string? fontFamily = null,
            int? fontSize = null,
            bool? bold = null,
            bool? italic = null,
            bool? strikethrough = null,
            bool? underline = null
           )
        {
            lock (lockObj)
            {
                SheetData.TextFormat? textFormat = null;

                if (foregroundColor != null ||
                    fontFamily != null || fontSize != null || bold != null ||
                    italic != null || strikethrough != null || underline != null)
                {
                    textFormat = new SheetData.TextFormat();

                    if (foregroundColor != null)
                    {
                        System.Drawing.Color color = (System.Drawing.Color)foregroundColor;
                        textFormat.ForegroundColor = new SheetData.Color()
                        {
                            Blue = (float)color.B / 255.0f,
                            Green = (float)color.G / 255.0f,
                            Red = (float)color.R / 255.0f,
                            Alpha = (float)1
                        };
                    }

                    if (fontFamily != null) { textFormat.FontFamily = fontFamily; }
                    if (fontSize != null) { textFormat.FontSize = fontSize; }
                    if (bold != null) { textFormat.Bold = bold; }
                    if (italic != null) { textFormat.Italic = italic; }
                    if (strikethrough != null) { textFormat.Strikethrough = strikethrough; }
                    if (underline != null) { textFormat.Underline = underline; }
                }

                return textFormat;
            }
        }


        public bool SetColumnWidths(string spreadsheetId, List<ColumnWidth> columnWidths)
        {
            lock (lockObj)
            {
                bool result = false;

                if (sheetsService != null && columnWidths != null && columnWidths.Count > 0)
                {
                    try
                    {
                        List<Request> requests = new List<Request>();
                        requests.AddRange(GetColumnWidthsRequests(columnWidths));
                        var requestBody = new BatchUpdateSpreadsheetRequest();
                        requestBody.Requests = requests;

                        sheetsService.Spreadsheets.BatchUpdate(requestBody, spreadsheetId).Execute();

                        result = true;
                    }
                    catch (Exception ex)
                    {
                        TraceError(ex.Message);
                    }
                }
                return result;
            }
        }

        public List<Request> GetColumnWidthsRequests(List<ColumnWidth> columnWidths)
        {
            lock (lockObj)
            {
                var requests = new List<Request>();

                foreach (var columnWidth in columnWidths)
                {
                    Request request = new Request
                    {
                        UpdateDimensionProperties = new UpdateDimensionPropertiesRequest
                        {
                            Range = new SheetData.DimensionRange
                            {
                                SheetId = columnWidth.SheetId,
                                Dimension = "COLUMNS",
                                StartIndex = columnWidth.StartIndex,
                                EndIndex = columnWidth.EndIndex
                            },
                            Properties = new DimensionProperties
                            {
                                PixelSize = columnWidth.PixelSize
                            },
                            Fields = "PixelSize"
                        }
                    };
                    requests.Add(request);
                }
                return requests;
            }
        }
        public bool SetRowBGColor(string spreadsheetId, int sheetId, int row, SheetData.Color color)
        {
            lock (lockObj)
            {
                bool result = false;

                if (sheetsService != null)
                {
                    try
                    {
                        List<Request> requests = new List<Request>();
                        requests.AddRange(GetRowBGColorRequests(sheetId, row, color));

                        var requestBody = new BatchUpdateSpreadsheetRequest();
                        requestBody.Requests = requests;

                        sheetsService.Spreadsheets.BatchUpdate(requestBody, spreadsheetId).Execute();

                        result = true;
                    }
                    catch (Exception ex)
                    {
                        TraceError(ex.Message);
                    }
                }

                return result;
            }
        }

        public List<Request> GetRowBGColorRequests(int sheetId, int row, SheetData.Color color)
        {
            lock (lockObj)
            {
                Request request = new Request
                {
                    RepeatCell = new SheetData.RepeatCellRequest
                    {
                        Range = new SheetData.GridRange
                        {
                            SheetId = sheetId,
                            StartRowIndex = row,
                            EndRowIndex = row + 1,
                            StartColumnIndex = 0,
                        },
                        Cell = new SheetData.CellData
                        {
                            UserEnteredFormat = new SheetData.CellFormat
                            {
                                BackgroundColor = color
                            }
                        },
                        Fields = "UserEnteredFormat.BackgroundColor"
                    }
                };

                List<Request> requests = new List<Request>() { request };

                return requests;
            }
        }

        public bool SetRowVerticalAlignment(string spreadsheetId, int sheetId, int row, SheetVerticalAlignment vAlignment)
        {
            lock (lockObj)
            {
                bool result = false;

                if (sheetsService != null)
                {
                    try
                    {
                        var requests = new List<Request>();
                        requests.AddRange(GetRowVerticalAlignmentRequests(sheetId, row, vAlignment));

                        var requestBody = new Google.Apis.Sheets.v4.Data.BatchUpdateSpreadsheetRequest();
                        requestBody.Requests = requests;

                        sheetsService.Spreadsheets.BatchUpdate(requestBody, spreadsheetId).Execute();

                        result = true;
                    }
                    catch (Exception ex)
                    {
                        TraceError(ex.Message);
                    }
                }

                return result;
            }
        }

        public List<Request> GetRowVerticalAlignmentRequests(int sheetId, int row, SheetVerticalAlignment vAlignment)
        {
            lock (lockObj)
            {
                Request request = new Request
                {
                    RepeatCell = new SheetData.RepeatCellRequest
                    {
                        Range = new SheetData.GridRange
                        {
                            SheetId = sheetId,
                            StartRowIndex = row,
                            EndRowIndex = row + 1,
                            StartColumnIndex = 0,
                        },
                        Cell = new SheetData.CellData
                        {
                            UserEnteredFormat = new SheetData.CellFormat
                            {
                                VerticalAlignment = Enum.GetName(typeof(SheetVerticalAlignment), vAlignment)
                            }
                        },
                        Fields = "UserEnteredFormat.VerticalAlignment"
                    }
                };
                List<Request> requests = new List<Request>() { request };

                return requests;
            }
        }

        public bool SetColumnHorizontalAlignment(string spreadsheetId, int sheetId, int column, SheetHorizontalAlignment hAlignment)
        {
            lock (lockObj)
            {
                bool result = false;

                if (sheetsService != null)
                {
                    try
                    {
                        var requests = new List<Request>();
                        requests.AddRange(GetColumnHorizontalAlignmentRequests(sheetId, column, hAlignment));

                        var requestBody = new Google.Apis.Sheets.v4.Data.BatchUpdateSpreadsheetRequest();
                        requestBody.Requests = requests;

                        sheetsService.Spreadsheets.BatchUpdate(requestBody, spreadsheetId).Execute();

                        result = true;
                    }
                    catch (Exception ex)
                    {
                        TraceError(ex.Message);
                    }
                }

                return result;
            }
        }
        public List<Request> GetColumnHorizontalAlignmentRequests(int sheetId, int column, SheetHorizontalAlignment hAlignment)
        {
            lock (lockObj)
            {
                Request request = new Request
                {
                    RepeatCell = new SheetData.RepeatCellRequest
                    {
                        Range = new SheetData.GridRange
                        {
                            SheetId = sheetId,
                            StartRowIndex = 0,
                            StartColumnIndex = column,
                            EndColumnIndex = column + 1,
                        },
                        Cell = new SheetData.CellData
                        {
                            UserEnteredFormat = new SheetData.CellFormat
                            {
                                HorizontalAlignment = Enum.GetName(typeof(SheetHorizontalAlignment), hAlignment)
                            }
                        },
                        Fields = "UserEnteredFormat.HorizontalAlignment"
                    }
                };
                List<Request> requests = new List<Request>() { request };

                return requests;
            }
        }
        public bool SetColumnTextDirection(string spreadsheetId, int sheetId, int column, SheetTextDirection direction)
        {
            lock (lockObj)
            {
                bool result = false;

                if (sheetsService != null)
                {
                    try
                    {
                        var requests = new List<Request>();
                        requests.AddRange(GetColumnTextDirectionRequests(sheetId, column, direction));

                        var requestBody = new Google.Apis.Sheets.v4.Data.BatchUpdateSpreadsheetRequest();
                        requestBody.Requests = requests;

                        sheetsService.Spreadsheets.BatchUpdate(requestBody, spreadsheetId).Execute();

                        result = true;
                    }
                    catch (Exception ex)
                    {
                        TraceError(ex.Message);
                    }
                }

                return result;
            }
        }
        public List<Request> GetColumnTextDirectionRequests(int sheetId, int column, SheetTextDirection direction)
        {
            lock (lockObj)
            {
                Request request = new Request
                {
                    RepeatCell = new SheetData.RepeatCellRequest
                    {
                        Range = new SheetData.GridRange
                        {
                            SheetId = sheetId,
                            StartRowIndex = 0,
                            StartColumnIndex = column,
                            EndColumnIndex = column + 1,
                        },
                        Cell = new SheetData.CellData
                        {
                            UserEnteredFormat = new SheetData.CellFormat
                            {
                                TextDirection = Enum.GetName(typeof(SheetTextDirection), direction)
                            }
                        },
                        Fields = "UserEnteredFormat.TextDirection"
                    }
                };
                List<Request> requests = new List<Request>() { request };

                return requests;
            }
        }
        public bool ProtectRow(string spreadsheetId, int sheetId, int row)
        {
            lock (lockObj)
            {
                bool result = false;

                if (sheetsService != null)
                {
                    try
                    {
                        var requests = new List<Request>();
                        requests.AddRange(GetProtectRowRequests(sheetId, row));

                        var requestBody = new Google.Apis.Sheets.v4.Data.BatchUpdateSpreadsheetRequest();
                        requestBody.Requests = requests;

                        sheetsService.Spreadsheets.BatchUpdate(requestBody, spreadsheetId).Execute();

                        result = true;
                    }
                    catch (Exception ex)
                    {
                        TraceError(ex.Message);
                    }
                }

                return result;
            }
        }
        public List<Request> GetProtectRowRequests(int sheetId, int row)
        {
            lock (lockObj)
            {
                Request request = new Request
                {
                    AddProtectedRange = new AddProtectedRangeRequest
                    {
                        ProtectedRange = new ProtectedRange
                        {
                            Range = new GridRange
                            {
                                SheetId = sheetId,
                                StartRowIndex = row,
                                EndRowIndex = row + 1,
                                StartColumnIndex = 0,
                            },
                            Description = "Protected Range",
                            WarningOnly = false
                        }
                    }
                };
                List<Request> requests = new List<Request>() { request };

                return requests;
            }
        }
        public bool SetWrapStrategy(string spreadsheetId, int sheetId, SheetWrapStrategy wrapStrategy)
        {
            lock (lockObj)
            {
                bool result = false;

                if (sheetsService != null)
                {
                    try
                    {
                        var requests = new List<Request>();
                        requests.AddRange(GetTextWrapRequests(sheetId, wrapStrategy));

                        var requestBody = new Google.Apis.Sheets.v4.Data.BatchUpdateSpreadsheetRequest();
                        requestBody.Requests = requests;

                        sheetsService.Spreadsheets.BatchUpdate(requestBody, spreadsheetId).Execute();

                        result = true;
                    }
                    catch (Exception ex)
                    {
                        TraceError(ex.Message);
                    }
                }

                return result;
            }
        }
        public List<Request> GetTextWrapRequests(int sheetId, SheetWrapStrategy wrapStrategy)
        {
            lock (lockObj)
            {
                Request request = new Request
                {
                    RepeatCell = new SheetData.RepeatCellRequest
                    {
                        Range = new SheetData.GridRange
                        {
                            SheetId = sheetId,
                            StartRowIndex = 0,
                            StartColumnIndex = 0,
                        },
                        Cell = new SheetData.CellData
                        {
                            UserEnteredFormat = new SheetData.CellFormat
                            {
                                WrapStrategy = Enum.GetName(typeof(SheetWrapStrategy), wrapStrategy),
                            }
                        },
                        Fields = "UserEnteredFormat.WrapStrategy"
                    }
                };
                List<Request> requests = new List<Request>() { request };

                return requests;
            }
        }

        #endregion Sheets

        #endregion Public Methods

        #region Private Methods
        private List<Request> GetRepeatCellRequests(
            UserEnteredFormat? userEnteredFormat = null,
            int sheetId = 0, int? startRow = null, int? endRow = null, int? startColumn = null, int? endColumn = null)
        {
            SheetData.GridRange range = new GridRange();
            if (startRow != null) range.StartRowIndex = startRow;
            if (endRow != null) range.EndRowIndex = endRow;
            if (startColumn != null) range.StartColumnIndex = startColumn;
            if (endColumn != null) range.EndColumnIndex = endColumn;
            if (sheetId > 0) range.SheetId = sheetId;

            SheetData.CellData cell = new SheetData.CellData();
            string fields = string.Empty;
            if (userEnteredFormat != null)
            {
                cell.UserEnteredFormat = userEnteredFormat.CellFormat;
                fields += userEnteredFormat.Fields;
            }

            Request request = new Request
            {
                RepeatCell = new SheetData.RepeatCellRequest
                {
                    Range = range,
                    Cell = cell,
                    Fields = fields
                }
            };
            List<Request> requests = new List<Request>() { request };

            return requests;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// List of RowData containing 
        ///         List of CellData Containing
        ///     
        /// <param name="sheetId"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        private List<Request> GetUpdateCellRequests(List<List<object>> values,
                int sheetId = 0, int? rowIndex = null, int? columnIndex = null)
        {
            SheetData.GridCoordinate start = new GridCoordinate();
            if (rowIndex != null) start.RowIndex = rowIndex;
            if (columnIndex != null) start.ColumnIndex = columnIndex;
            if (sheetId > 0) start.SheetId = sheetId;

            List<SheetData.RowData> rowsData = new List<SheetData.RowData>();
            for (int i = 0; i < values.Count; i++)
            {
                List<object> list = values[i];
                rowsData.Add(new SheetData.RowData
                {
                    Values = GetRowData(list)
                });
            }

            Request request = new Request
            {
                UpdateCells = new SheetData.UpdateCellsRequest
                {
                    Start = start,
                    Rows = rowsData,
                    Fields = "userEnteredValue, dataValidation"
                }
            };
            List<Request> requests = new List<Request>() { request };

            return requests;
        }

        private List<SheetData.CellData> GetRowData(List<object> values)
        {
            List<SheetData.CellData> rowData = new List<SheetData.CellData>();

            foreach (object value in values)
            {
                SheetData.CellData cellData = new SheetData.CellData();
                if (value != null && value is SheetStatus)
                {
                    string v = Enum.GetName(typeof(SheetStatus), value);
                    cellData.UserEnteredValue = GetExtendedValue(v);
                    cellData.DataValidation = GetDataValidationRule(v);
                }
                else
                {
                    cellData.UserEnteredValue = GetExtendedValue(value);
                    if (value is bool)
                    {
                        cellData.DataValidation = GetDataValidationRule(value);
                    }
                }
                rowData.Add(cellData);
                //rowData.Add(new SheetData.CellData()
                //{
                //    UserEnteredValue = GetExtendedValue(value),
                //    dataValidation = GetDataValidationRule(value)
                //});

            }

            return rowData;
        }

        public SheetData.ExtendedValue GetExtendedValue(object value)
        {
            string fields = string.Empty;
            ExtendedValue extendedValue = new ExtendedValue();

            switch (value)
            {
                case string:
                    extendedValue.StringValue = (string)value;
                    break;
                case bool:
                    extendedValue.BoolValue = (bool)value;
                    break;
                case sbyte:
                case byte:
                case short:
                case ushort:
                case int:
                case uint:
                case long:
                case ulong:
                case float:
                case double:
                case decimal:
                    extendedValue.NumberValue = Convert.ToDouble(value);
                    break;

            }

            return extendedValue;
        }

        public SheetData.DataValidationRule GetDataValidationRule(object value)
        {
            DataValidationRule dataValidationRule = new DataValidationRule();

            if (value is bool)
            {
                dataValidationRule.Condition = new BooleanCondition
                {
                    Type = "BOOLEAN",
                };
                dataValidationRule.ShowCustomUi = true;
                dataValidationRule.Strict = true;
            }
            else if (value is string)
            {
                dataValidationRule.Condition = new BooleanCondition
                {
                    Type = "ONE_OF_LIST",
                    Values = new List<ConditionValue>()
                            {
                                new ConditionValue() { UserEnteredValue = Enum.GetName(typeof(SheetStatus), SheetStatus.Pending)},
                                new ConditionValue() { UserEnteredValue = Enum.GetName(typeof(SheetStatus), SheetStatus.Reviewed) },
                                new ConditionValue() { UserEnteredValue = Enum.GetName(typeof(SheetStatus), SheetStatus.Approved) }
                            }
                };
                dataValidationRule.ShowCustomUi = true;
                dataValidationRule.Strict = true;
            }


            return dataValidationRule;
        }

        public List<Request> GetClearSheetRequests(int sheetId, bool valuesOnly = true)
        {
            Request request = new Request
            {
                UpdateCells = new UpdateCellsRequest
                {
                    Range = new GridRange
                    {
                        SheetId = sheetId
                    },
                    Fields = valuesOnly ? "userEnteredValue" : "*"
                }
            };
            List<Request> requests = new List<Request>() { request };
            return requests;
        }

        #endregion Private Methods
    }
}
