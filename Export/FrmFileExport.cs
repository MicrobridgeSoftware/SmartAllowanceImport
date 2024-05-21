using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;

namespace SmartAllowanceImport.Export
{
    public partial class FrmFileExport : Telerik.WinControls.UI.RadForm
    {
        SmartAllowanceImportEntities dbContext = new SmartAllowanceImportEntities();
        List<AllowancesForExportView> _allowanceExport;
        bool _exportMode = false;
        private List<string> CSVContent;

        public FrmFileExport()
        {
            InitializeComponent();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (grdDataDisplay.Rows.Count <= 0)
                return;

            if (string.IsNullOrEmpty(txtFilePath.Text.Trim()))
            {
                RadMessageBox.Show("Select a destination in which the export should be saved", Application.ProductName);
                return;
            }

            if (string.IsNullOrEmpty(txtFileName.Text.Trim()))
            {
                RadMessageBox.Show("Enter 'save as' name for the file!", Application.ProductName);
                return;
            }

            DialogResult ConfirmExport = RadMessageBox.Show("Do you want to export employee allowances for this period?", Application.ProductName, MessageBoxButtons.YesNo);

            if (ConfirmExport == System.Windows.Forms.DialogResult.Yes)
            {
                string path = string.Empty;

                path = txtFilePath.Text.Trim();

                path = path + "\\" + txtFileName.Text.Trim() + ".csv";

                bool fileExist = File.Exists(path);

                if (fileExist)
                {
                    FileInfo file = new FileInfo(path);

                    Process[] AllProcesses = Process.GetProcessesByName("excel");

                    foreach (Process ExcelProcess in AllProcesses)
                    {
                        RadMessageBox.Show("Excel file(s) are currently opened \n" +
                                           "Close all opened files and try again", Application.ProductName);
                        return;
                    }

                    string currentTime = DateTime.Now.ToString("_MMM_dd_yyyy_HH_mm_ss");
                    string newfileName = file.Name.Replace(".csv", "") + currentTime + ".csv";
                    File.Copy(path, newfileName, true);
                    File.Delete(path);
                }

                FrmWaitDialogue _waitDialogue = new FrmWaitDialogue();

                List<object> bgwarguments = new List<object>();
                bgwarguments.Add("Export");
                bgwarguments.Add(path);

                if (!bgwExport.IsBusy)
                {
                    _exportMode = true;
                    bgwExport.RunWorkerAsync(bgwarguments);
                    _waitDialogue.ShowDialog();
                }
                else
                    RadMessageBox.Show("File cannot be exported at this time!", Application.ProductName);
            }
        }

        public void ConvertDatatoCSV(Dictionary<string, List<string>> dict, string path)
        {
            StringBuilder sb = new StringBuilder();
            CSVContent = new List<string>();

            String csv = String.Join(",", dict.Select(d => d.Key));
            sb.Append(csv + Environment.NewLine);

            int count = -1;
            foreach (var data in _allowanceExport)
            {
                count++;
                String csv2 = String.Join(",", dict.Select(d => string.Join(",", d.Value.Skip(count).Take(1))));

                sb.Append(csv2 + Environment.NewLine);

                CSVContent.Add(csv2);
            }

            File.WriteAllText(path, sb.ToString());

            string _applicationFolderPath = AppDomain.CurrentDomain.BaseDirectory;
            string _exportFolderPath = _applicationFolderPath + "ExportPayrollTransactions";
            string currentTime = DateTime.Now.ToString("_HH_mm_ss");
            _exportFolderPath = _exportFolderPath + "\\" + txtFileName.Text.Trim() + "_T_" + currentTime + ".csv";

            File.WriteAllText(_exportFolderPath, sb.ToString());
        }

        private void FrmFileExport_Load(object sender, EventArgs e)
        {
            FrmWaitDialogue _waitDialogue = new FrmWaitDialogue();

            List<object> bgwarguments = new List<object>();
            bgwarguments.Add("FormLoad");
            bgwarguments.Add(string.Empty);

            bgwExport.RunWorkerAsync(bgwarguments);

            _waitDialogue.ShowDialog();
        }

        private void bgwExport_DoWork(object sender, DoWorkEventArgs e)
        {
            List<object> _argumentList = e.Argument as List<object>;

            string _mode = _argumentList[0].ToString();
            string _path = _argumentList[1].ToString();

            if (_mode.Equals("Export") && !string.IsNullOrEmpty(_path))
            {
                Dictionary<string, List<string>> data = new Dictionary<string, List<string>>();

                data.Add("EmployeeRef", new List<string> { });
                data.Add("PayrollCode", new List<string> { });
                data.Add("DutyCode", new List<string> { });
                data.Add("Rate", new List<string> { });
                data.Add("Quantity", new List<string> { });
                data.Add("Amount", new List<string> { });
                data.Add("TransactionDate", new List<string> { });
                data.Add("GlAccount", new List<string> { });
                data.Add("Location", new List<string> { });
                data.Add("Branch", new List<string> { });
                data.Add("Department", new List<string> { });
                data.Add("Notes", new List<string> { });

                foreach (var _data in _allowanceExport)
                {
                    data["EmployeeRef"].Add(_data.EmployeeRef.Trim());
                    data["PayrollCode"].Add(_data.PayrollCode.Trim());
                    data["DutyCode"].Add(string.Empty);
                    data["Rate"].Add(_data.Rate.ToString());
                    data["Quantity"].Add(_data.Quantity.ToString());
                    data["Amount"].Add(string.Empty);
                    data["TransactionDate"].Add(DateTime.Today.Date.ToString("dd/MM/yyyy"));
                    data["GlAccount"].Add(string.Empty);
                    data["Location"].Add(string.Empty);
                    data["Branch"].Add(string.Empty);
                    data["Department"].Add(string.Empty);
                    data["Notes"].Add(_data.Notes.Trim());
                }

                ConvertDatatoCSV(data, _path);
                UpdateJobLogExportStatus();
            }
            else            
                _allowanceExport = dbContext.AllowancesForExportViews.ToList();
            
        }

        private void UpdateJobLogExportStatus()
        {
            using (var transcope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.RepeatableRead }))
            {
                try
                {
                    ExportedTransaction _exportTransactions = new ExportedTransaction();
                    _exportTransactions.FileName = txtFileName.Text.Trim();
                    _exportTransactions.ExportedPath = txtFilePath.Text.Trim();
                    _exportTransactions.EmployeeCount = Convert.ToInt32(txtFileCount.Text);
                    _exportTransactions.DateExported = DateTime.Now;
                    dbContext.ExportedTransactions.Add(_exportTransactions);
                    dbContext.SaveChanges();

                    int _exportedTransId = _exportTransactions.ExportedTransactionId;

                    var _distinctHeaderIds = _allowanceExport.Select(x => x.ImportedTransactionHeaderId).Distinct().ToList();

                    foreach (int _headerId in _distinctHeaderIds)
                    {
                        var _getHeaderRow = dbContext.ImportedTransactionHeaders.Where(x => x.ImportedTransactionHeaderId == _headerId).FirstOrDefault();

                        if (_getHeaderRow != null)
                            _getHeaderRow.IsExported = true;

                        var _fetchAllowances = _allowanceExport.Where(x => x.ImportedTransactionHeaderId == _headerId).ToList();

                        foreach (var allowance in _fetchAllowances)
                        {
                            ExportedTransactionDetail transactionDetail = new ExportedTransactionDetail();
                            transactionDetail.ExportedTransactionId = _exportedTransId;
                            transactionDetail.ImportedTransactionId = allowance.ImportedTransactionId;
                            transactionDetail.AllowanceConfigId = allowance.AllowanceConfigId;
                            transactionDetail.Amount = Convert.ToDecimal(allowance.Rate);
                            transactionDetail.PayrollCode = allowance.PayrollCode;
                            transactionDetail.ExportedQuantity = allowance.Quantity;
                            dbContext.ExportedTransactionDetails.Add(transactionDetail);
                        }
                    }

                    dbContext.SaveChanges();
                    transcope.Complete();
                }
                catch (Exception _exp)
                {
                    RadMessageBox.Show(_exp.InnerException == null ? _exp.Message : _exp.InnerException.Message);
                }
            }
        }

        private void bgwExport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            for (int index = Application.OpenForms.Count - 1; index >= 0; index--)
            {
                if (Application.OpenForms[index].Name == "FrmWaitDialogue")
                    Application.OpenForms[index].Close();
            }

            if (_exportMode)
            {
                RadMessageBox.Show(txtFileName.Text.Trim() + " exported Successfully ", Application.ProductName);
                grdDataDisplay.Rows.Clear();
            }
            else
            {
                grdDataDisplay.GroupDescriptors.Remove("EmployeeFullName");
                grdDataDisplay.SummaryRowsBottom.Clear();

                grdDataDisplay.DataSource = _allowanceExport;

                var _distinctList = _allowanceExport.Select(x => x.EmployeeId).Distinct().ToList();
                txtFileCount.Text = string.Format("{0:N0}", _distinctList.Count);

                if (_allowanceExport.Count > 0)
                {
                    GroupDescriptor groupDescriptor = new GroupDescriptor();
                    groupDescriptor.GroupNames.Add("EmployeeFullName", ListSortDirection.Ascending);
                    this.grdDataDisplay.GroupDescriptors.Add(groupDescriptor);
                    grdDataDisplay.MasterTemplate.ExpandAllGroups();

                    //GridViewSummaryItem summaryItem = new GridViewSummaryItem("Rate", "Total = {0:N2}", GridAggregateFunction.Sum);
                    //GridViewSummaryRowItem summaryRowItem = new GridViewSummaryRowItem();
                    //summaryRowItem.Add(summaryItem);
                    //this.grdDataDisplay.SummaryRowsBottom.Add(summaryRowItem);
                }
            }

            _exportMode = false;
        }

        private void grdDataDisplay_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            //if (e.CellElement is GridSummaryCellElement)
            //{
            //    e.CellElement.ForeColor = Color.Red;
            //    e.CellElement.TextAlignment = ContentAlignment.MiddleRight;
            //    e.CellElement.Font = new Font("Segoe UI", 8.0f, FontStyle.Bold);
            //}
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "Select export destination path";
            folder.ShowDialog();

            if (!string.IsNullOrEmpty(folder.SelectedPath))
            {
                txtFilePath.Text = folder.SelectedPath;
            }
        }
    }
}