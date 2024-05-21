using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using SmartAllowanceImport.Utils;
using System.Transactions;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Data.Entity;

namespace SmartAllowanceImport.Import
{
    public partial class FrmFileImport : Telerik.WinControls.UI.RadForm
    {
        SmartAllowanceImportEntities dbContext = new SmartAllowanceImportEntities();
        Excel.Application _excelApp;
        Excel.Workbooks _excelWorkbooks;
        Excel.Workbook _excelWorkbook;
        object _misValue = System.Reflection.Missing.Value;
        List<TransactionImport> _transactionImportDataList;
        List<AllowanceConfiguration> configurations;
        string _objectType = string.Empty;
        string _importFileName = string.Empty;
        string _fileSaveName = string.Empty;
        int _fileCount = 0;

        public FrmFileImport()
        {
            InitializeComponent();
        }

        private void FrmFileImport_FormClosing(object sender, FormClosingEventArgs e)
        {
            dbContext.Dispose();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog _fileDialog = new OpenFileDialog();
            _fileDialog.Filter = "Excel files (*.xlsx)|*.xlsx;*.xls";
            _fileDialog.ShowDialog();

            if (!string.IsNullOrEmpty(_fileDialog.FileName))
                txtFilePath.Text = _fileDialog.FileName.ToString().Trim();            
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilePath.Text.Trim()))
                return;

            try
            {
                FrmWaitDialogue waitDialogue = new FrmWaitDialogue();
                _transactionImportDataList = new List<TransactionImport>();

                if (!bgwTransaction.IsBusy)
                {
                    txtFileCount.Text = string.Empty;
                    _objectType = "Import";
                    bgwTransaction.RunWorkerAsync();
                    waitDialogue.ShowDialog();
                }
                else
                    RadMessageBox.Show("System currently executing a process. Your request cannot be processed at this time", Application.ProductName);                
            }
            catch(Exception _exp)
            {
                RadMessageBox.Show(_exp.InnerException == null ? _exp.Message : _exp.InnerException.Message);
            }
        }

        private void bgwTransaction_DoWork(object sender, DoWorkEventArgs e)
        {
            if (_objectType.Equals("Import"))
            {
                _excelApp = new Microsoft.Office.Interop.Excel.Application();

                _importFileName = txtFilePath.Text.Trim();

                _excelWorkbooks = _excelApp.Workbooks;

                _excelWorkbook = _excelWorkbooks.Open(_importFileName, _misValue, _misValue, _misValue, _misValue,
                                 _misValue, _misValue, _misValue, _misValue, _misValue, _misValue, _misValue,
                                 _misValue, _misValue, _misValue);

                Excel.Worksheet _myExcelWorksheet = (Excel.Worksheet)_excelWorkbook.ActiveSheet;
                _excelApp.ScreenUpdating = false;

                Excel.Range last = _myExcelWorksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
                Excel.Range range = _myExcelWorksheet.get_Range("A1", last);

                int _totalRows = last.Row;
                int lastUsedColumn = last.Column;
                int _startRow = 3;
                
                for (int _xlRow = _startRow; _xlRow <= _totalRows; _xlRow++)
                {
                    var _employeeRef = _myExcelWorksheet.get_Range("A" + _startRow.ToString(), _misValue).Value2;
                    var _employee = _myExcelWorksheet.get_Range("B" + _startRow.ToString(), _misValue).Value2;
                    var _regOvertime = _myExcelWorksheet.get_Range("D" + _startRow.ToString(), _misValue).Value2;
                    var _regDoubleTime = _myExcelWorksheet.get_Range("E" + _startRow.ToString(), _misValue).Value2;
                    var _regTripleTime = _myExcelWorksheet.get_Range("F" + _startRow.ToString(), _misValue).Value2;
                    var _mealAllowance = _myExcelWorksheet.get_Range("G" + _startRow.ToString(), _misValue).Value2;
                    var _eveningPremium = _myExcelWorksheet.get_Range("H" + _startRow.ToString(), _misValue).Value2;
                    var _nightPremium = _myExcelWorksheet.get_Range("I" + _startRow.ToString(), _misValue).Value2;
                    var _sundayPremium = _myExcelWorksheet.get_Range("J" + _startRow.ToString(), _misValue).Value2;
                    var _holidayRegular = _myExcelWorksheet.get_Range("K" + _startRow.ToString(), _misValue).Value2;
                    var _holidayDouble = _myExcelWorksheet.get_Range("L" + _startRow.ToString(), _misValue).Value2;
                    var _holidayTriple = _myExcelWorksheet.get_Range("M" + _startRow.ToString(), _misValue).Value2;
                    var _shiftOT = _myExcelWorksheet.get_Range("N" + _startRow.ToString(), _misValue).Value2;
                    var _shiftDT = _myExcelWorksheet.get_Range("O" + _startRow.ToString(), _misValue).Value2;
                    var _overhaulOT = _myExcelWorksheet.get_Range("P" + _startRow.ToString(), _misValue).Value2;
                    var _overhaulDT = _myExcelWorksheet.get_Range("Q" + _startRow.ToString(), _misValue).Value2;
                    var _overhaulTT = _myExcelWorksheet.get_Range("R" + _startRow.ToString(), _misValue).Value2;
                    var _absentHours = _myExcelWorksheet.get_Range("S" + _startRow.ToString(), _misValue).Value2;
                    var _transactionNotes = _myExcelWorksheet.get_Range("T" + _startRow.ToString(), _misValue).Value2;

                    _transactionImportDataList.Add(new TransactionImport
                    {
                        _absHours = _absentHours == null ? 0 : Convert.ToDecimal(_absentHours),
                        _employeeName = _employee == null ? "" : (string)_employee,
                        _empRef = _employeeRef == null ? "" : (string)_employeeRef,
                        _eveningAllowance = _eveningPremium == null ? 0 : Convert.ToInt32(_eveningPremium),
                        _holidayDouble = _holidayDouble == null ? 0 : Convert.ToDecimal(_holidayDouble),
                        _holidayRegular = _holidayRegular == null ? 0 : Convert.ToDecimal(_holidayRegular),
                        _holidayTriple = _holidayTriple == null ? 0 : Convert.ToDecimal(_holidayTriple),
                        _mealAllowance = _mealAllowance == null ? 0 : Convert.ToInt32(_mealAllowance),
                        _nightAllowance = _nightPremium == null ? 0 : Convert.ToInt32(_nightPremium),
                        _overhaulDT = _overhaulDT == null ? 0 : Convert.ToDecimal(_overhaulDT),
                        _overhaulOT = _overhaulOT == null ? 0 : Convert.ToDecimal(_overhaulOT),
                        _overhaulTT = _overhaulTT == null ? 0 : Convert.ToDecimal(_overhaulTT),
                        _regularDT = _regDoubleTime == null ? 0 : Convert.ToDecimal(_regDoubleTime),
                        _regularOT = _regOvertime == null ? 0 : Convert.ToDecimal(_regOvertime),
                        _regularTT = _regTripleTime == null ? 0 : Convert.ToDecimal(_regTripleTime),
                        _shiftDT = _shiftDT == null ? 0 : Convert.ToDecimal(_shiftDT),
                        _shiftOT = _shiftOT == null ? 0 : Convert.ToDecimal(_shiftOT),
                        _sundayAllowance = _sundayPremium == null ? 0 : Convert.ToInt32(_sundayPremium),
                        _notes = _transactionNotes == null ? "" : (string)_transactionNotes
                    });

                    _startRow++;
                }
            }
            
            if(_objectType.Equals("DatabaseUpdate"))
            {
                using (var transcope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.RepeatableRead }))
                {
                    ImportedTransactionHeader transactionHeader = new ImportedTransactionHeader();
                    transactionHeader.ImportedFilePath = _importFileName;
                    transactionHeader.DateImported = DateTime.Now;
                    transactionHeader.RecordCount = _fileCount;
                    transactionHeader.IsExported = false;
                    transactionHeader.NewFileName = _fileSaveName;
                    dbContext.ImportedTransactionHeaders.Add(transactionHeader);
                    dbContext.SaveChanges();

                    int _newTransactionHeaderId = transactionHeader.ImportedTransactionHeaderId;

                    foreach (TransactionImport transaction in _transactionImportDataList)
                    {
                        ImportedTransaction importedTransaction = new ImportedTransaction();
                        importedTransaction.ImportedTransactionHeaderId = _newTransactionHeaderId;
                        importedTransaction.EmployeeReference = transaction._empRef;
                        importedTransaction.RegularOvertime = transaction._regularOT;
                        importedTransaction.RegularDoubleTime = transaction._regularDT;
                        importedTransaction.RegularTripleTime = transaction._regularTT;
                        importedTransaction.MealAllowance = transaction._mealAllowance;
                        importedTransaction.EveningPremium = transaction._eveningAllowance;
                        importedTransaction.NightPremium = transaction._nightAllowance;
                        importedTransaction.SundayPremium = transaction._sundayAllowance;
                        importedTransaction.HolidayRegularHours = transaction._holidayRegular;
                        importedTransaction.HolidayDoubleTimeHours = transaction._holidayDouble;
                        importedTransaction.HolidayTripleTimeHours = transaction._holidayTriple;
                        importedTransaction.ShiftOvertime = transaction._shiftOT;
                        importedTransaction.ShiftDoubleTime = transaction._shiftDT;
                        importedTransaction.OverhaulOvertime = transaction._overhaulOT;
                        importedTransaction.OverhaulDoubleTime = transaction._overhaulDT;
                        importedTransaction.OverhaulTripleTime = transaction._overhaulTT;
                        importedTransaction.AbsentHours = transaction._absHours;
                        importedTransaction.Notes = transaction._notes;
                        dbContext.ImportedTransactions.Add(importedTransaction);
                        
                        if(transaction._regularOT > 0)
                        {
                            var _allowanceValue = configurations.Where(x => x.ObjectReference == "RegularOT").FirstOrDefault();

                            if (_allowanceValue != null)
                            {
                                ImportedTransactionAllowance allowance = new ImportedTransactionAllowance();
                                allowance.AllowanceConfigId = _allowanceValue.AllowanceConfigId;
                                allowance.ImportedTransactionId = importedTransaction.ImportedTransactionId;
                                allowance.Amount = transaction._regularOT;                                
                                importedTransaction.ImportedTransactionAllowances.Add(allowance);                                
                            }
                        }

                        if (transaction._regularDT > 0)
                        {
                            var _allowanceValue = configurations.Where(x => x.ObjectReference == "RegularDT").FirstOrDefault();

                            if (_allowanceValue != null)
                            {
                                ImportedTransactionAllowance allowance = new ImportedTransactionAllowance();
                                allowance.AllowanceConfigId = _allowanceValue.AllowanceConfigId;
                                allowance.ImportedTransactionId = importedTransaction.ImportedTransactionId;
                                allowance.Amount = transaction._regularDT;
                                importedTransaction.ImportedTransactionAllowances.Add(allowance);
                            }
                        }

                        if (transaction._regularTT > 0)
                        {
                            var _allowanceValue = configurations.Where(x => x.ObjectReference == "RegularTT").FirstOrDefault();

                            if (_allowanceValue != null)
                            {
                                ImportedTransactionAllowance allowance = new ImportedTransactionAllowance();
                                allowance.AllowanceConfigId = _allowanceValue.AllowanceConfigId;
                                allowance.ImportedTransactionId = importedTransaction.ImportedTransactionId;
                                allowance.Amount = transaction._regularTT;
                                importedTransaction.ImportedTransactionAllowances.Add(allowance);
                            }
                        }

                        if (transaction._mealAllowance > 0)
                        {
                            var _allowanceValue = configurations.Where(x => x.ObjectReference == "MealAllowance").FirstOrDefault();

                            if (_allowanceValue != null)
                            {
                                ImportedTransactionAllowance allowance = new ImportedTransactionAllowance();
                                allowance.AllowanceConfigId = _allowanceValue.AllowanceConfigId;
                                allowance.ImportedTransactionId = importedTransaction.ImportedTransactionId;
                                allowance.Amount = transaction._mealAllowance;
                                importedTransaction.ImportedTransactionAllowances.Add(allowance);
                            }
                        }

                        if (transaction._eveningAllowance > 0)
                        {
                            var _allowanceValue = configurations.Where(x => x.ObjectReference == "EveningPremium").FirstOrDefault();

                            if (_allowanceValue != null)
                            {
                                ImportedTransactionAllowance allowance = new ImportedTransactionAllowance();
                                allowance.AllowanceConfigId = _allowanceValue.AllowanceConfigId;
                                allowance.ImportedTransactionId = importedTransaction.ImportedTransactionId;
                                allowance.Amount = transaction._eveningAllowance;
                                importedTransaction.ImportedTransactionAllowances.Add(allowance);
                            }
                        }

                        if (transaction._nightAllowance > 0)
                        {
                            var _allowanceValue = configurations.Where(x => x.ObjectReference == "NightPremium").FirstOrDefault();

                            if (_allowanceValue != null)
                            {
                                ImportedTransactionAllowance allowance = new ImportedTransactionAllowance();
                                allowance.AllowanceConfigId = _allowanceValue.AllowanceConfigId;
                                allowance.ImportedTransactionId = importedTransaction.ImportedTransactionId;
                                allowance.Amount = transaction._nightAllowance;
                                importedTransaction.ImportedTransactionAllowances.Add(allowance);
                            }
                        }

                        if (transaction._sundayAllowance > 0)
                        {
                            var _allowanceValue = configurations.Where(x => x.ObjectReference == "SundayPremium").FirstOrDefault();

                            if (_allowanceValue != null)
                            {
                                ImportedTransactionAllowance allowance = new ImportedTransactionAllowance();
                                allowance.AllowanceConfigId = _allowanceValue.AllowanceConfigId;
                                allowance.ImportedTransactionId = importedTransaction.ImportedTransactionId;
                                allowance.Amount = transaction._sundayAllowance;
                                importedTransaction.ImportedTransactionAllowances.Add(allowance);
                            }
                        }

                        if (transaction._holidayRegular > 0)
                        {
                            var _allowanceValue = configurations.Where(x => x.ObjectReference == "HolidayRegular").FirstOrDefault();

                            if (_allowanceValue != null)
                            {
                                ImportedTransactionAllowance allowance = new ImportedTransactionAllowance();
                                allowance.AllowanceConfigId = _allowanceValue.AllowanceConfigId;
                                allowance.ImportedTransactionId = importedTransaction.ImportedTransactionId;
                                allowance.Amount = transaction._holidayRegular;
                                importedTransaction.ImportedTransactionAllowances.Add(allowance);
                            }
                        }

                        if (transaction._holidayDouble > 0)
                        {
                            var _allowanceValue = configurations.Where(x => x.ObjectReference == "HolidayDoubleTime").FirstOrDefault();

                            if (_allowanceValue != null)
                            {
                                ImportedTransactionAllowance allowance = new ImportedTransactionAllowance();
                                allowance.AllowanceConfigId = _allowanceValue.AllowanceConfigId;
                                allowance.ImportedTransactionId = importedTransaction.ImportedTransactionId;
                                allowance.Amount = transaction._holidayDouble;
                                importedTransaction.ImportedTransactionAllowances.Add(allowance);
                            }
                        }

                        if (transaction._holidayTriple > 0)
                        {
                            var _allowanceValue = configurations.Where(x => x.ObjectReference == "HolidayTripleTime").FirstOrDefault();

                            if (_allowanceValue != null)
                            {
                                ImportedTransactionAllowance allowance = new ImportedTransactionAllowance();
                                allowance.AllowanceConfigId = _allowanceValue.AllowanceConfigId;
                                allowance.ImportedTransactionId = importedTransaction.ImportedTransactionId;
                                allowance.Amount = transaction._holidayTriple;
                                importedTransaction.ImportedTransactionAllowances.Add(allowance);
                            }
                        }

                        if (transaction._shiftOT > 0)
                        {
                            var _allowanceValue = configurations.Where(x => x.ObjectReference == "ShiftOvertime").FirstOrDefault();

                            if (_allowanceValue != null)
                            {
                                ImportedTransactionAllowance allowance = new ImportedTransactionAllowance();
                                allowance.AllowanceConfigId = _allowanceValue.AllowanceConfigId;
                                allowance.ImportedTransactionId = importedTransaction.ImportedTransactionId;
                                allowance.Amount = transaction._shiftOT;
                                importedTransaction.ImportedTransactionAllowances.Add(allowance);
                            }
                        }

                        if (transaction._shiftDT > 0)
                        {
                            var _allowanceValue = configurations.Where(x => x.ObjectReference == "ShiftDoubleTime").FirstOrDefault();

                            if (_allowanceValue != null)
                            {
                                ImportedTransactionAllowance allowance = new ImportedTransactionAllowance();
                                allowance.AllowanceConfigId = _allowanceValue.AllowanceConfigId;
                                allowance.ImportedTransactionId = importedTransaction.ImportedTransactionId;
                                allowance.Amount = transaction._shiftDT;
                                importedTransaction.ImportedTransactionAllowances.Add(allowance);
                            }
                        }

                        if (transaction._overhaulOT > 0)
                        {
                            var _allowanceValue = configurations.Where(x => x.ObjectReference == "OverhaulOT").FirstOrDefault();

                            if (_allowanceValue != null)
                            {
                                ImportedTransactionAllowance allowance = new ImportedTransactionAllowance();
                                allowance.AllowanceConfigId = _allowanceValue.AllowanceConfigId;
                                allowance.ImportedTransactionId = importedTransaction.ImportedTransactionId;
                                allowance.Amount = transaction._overhaulOT;
                                importedTransaction.ImportedTransactionAllowances.Add(allowance);
                            }
                        }

                        if (transaction._overhaulDT > 0)
                        {
                            var _allowanceValue = configurations.Where(x => x.ObjectReference == "OverhaulDT").FirstOrDefault();

                            if (_allowanceValue != null)
                            {
                                ImportedTransactionAllowance allowance = new ImportedTransactionAllowance();
                                allowance.AllowanceConfigId = _allowanceValue.AllowanceConfigId;
                                allowance.ImportedTransactionId = importedTransaction.ImportedTransactionId;
                                allowance.Amount = transaction._overhaulDT;
                                importedTransaction.ImportedTransactionAllowances.Add(allowance);
                            }
                        }

                        if (transaction._overhaulTT > 0)
                        {
                            var _allowanceValue = configurations.Where(x => x.ObjectReference == "OverhaulTT").FirstOrDefault();

                            if (_allowanceValue != null)
                            {
                                ImportedTransactionAllowance allowance = new ImportedTransactionAllowance();
                                allowance.AllowanceConfigId = _allowanceValue.AllowanceConfigId;
                                allowance.ImportedTransactionId = importedTransaction.ImportedTransactionId;
                                allowance.Amount = transaction._overhaulTT;
                                importedTransaction.ImportedTransactionAllowances.Add(allowance);
                            }
                        }

                        if (transaction._absHours != 0)
                        {
                            var _allowanceValue = configurations.Where(x => x.ObjectReference == "SalaryReduction").FirstOrDefault();

                            if (_allowanceValue != null)
                            {
                                ImportedTransactionAllowance allowance = new ImportedTransactionAllowance();
                                allowance.AllowanceConfigId = _allowanceValue.AllowanceConfigId;
                                allowance.ImportedTransactionId = importedTransaction.ImportedTransactionId;
                                allowance.Amount = transaction._absHours;
                                importedTransaction.ImportedTransactionAllowances.Add(allowance);
                            }
                        }
                    }

                    dbContext.SaveChanges();

                    string _applicationFolderPath = AppDomain.CurrentDomain.BaseDirectory;
                    string _exportFolderPath = _applicationFolderPath + "ImportedTransactions";
                    string _exportDirectory = _exportFolderPath;
                    string currentTime = DateTime.Now.ToString("_MMM_dd_yyyy_HH_mm_ss");
                    _exportFolderPath = _exportFolderPath + "\\" + _fileSaveName + currentTime + Path.GetExtension(_importFileName.Trim());
                    
                    bool _destinationPathExist = Directory.Exists(_exportDirectory);

                    if (_destinationPathExist)
                        File.Copy(_importFileName, _exportFolderPath);                    

                    transcope.Complete();
                }
            }
        }
               
        private void bgwTransaction_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_objectType.Equals("Import"))
            {
                _excelWorkbooks.Close();
                _excelApp.Quit();

                Marshal.ReleaseComObject(_excelWorkbooks);
                Marshal.ReleaseComObject(_excelWorkbooks);
                Marshal.ReleaseComObject(_excelApp);
                
                grdDataDisplay.DataSource = _transactionImportDataList;
                txtFileCount.Text = string.Format("{0:N0}", _transactionImportDataList.Count);
            }

            if (_objectType.Equals("DatabaseUpdate"))
            {
                grdDataDisplay.Rows.Clear();
                txtFileCount.Text = string.Empty;
                txtFileName.Text = string.Empty;
                txtFilePath.Text = string.Empty;

                RadMessageBox.Show("Database update successfully!", Application.ProductName);
            }

            GC.Collect();

            for (int index = Application.OpenForms.Count - 1; index >= 0; index--)
            {
                if (Application.OpenForms[index].Name == "FrmWaitDialogue")
                    Application.OpenForms[index].Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (grdDataDisplay.Rows.Count <= 0)
                return;

            if (string.IsNullOrEmpty(txtFileName.Text.Trim()))
            {
                RadMessageBox.Show("Enter a name to save this file!", Application.ProductName);
                return;
            }
            else
                _fileSaveName = txtFileName.Text.Trim();


            DialogResult _verifyAction = RadMessageBox.Show("Are you sure you want to save this import?", Application.ProductName, MessageBoxButtons.YesNo);

            if (_verifyAction == DialogResult.Yes)
            {
                try
                {
                    FrmWaitDialogue waitDialogue = new FrmWaitDialogue();

                    if (!bgwTransaction.IsBusy)
                    {
                        _fileCount = Convert.ToInt32(txtFileCount.Text.Trim());
                        _objectType = "DatabaseUpdate";
                        bgwTransaction.RunWorkerAsync();
                        waitDialogue.ShowDialog();
                    }
                    else
                        RadMessageBox.Show("System currently executing a process. Your request cannot be processed at this time", Application.ProductName);
                }
                catch(Exception _exp)
                {
                    RadMessageBox.Show(_exp.InnerException == null ? _exp.Message : _exp.InnerException.Message);
                }
            }
        }

        private void FrmFileImport_Load(object sender, EventArgs e)
        {
            configurations = new List<AllowanceConfiguration>();

            configurations = dbContext.AllowanceConfigurations.Where(x=> x.Active).AsNoTracking().ToList();
        }
    }
}