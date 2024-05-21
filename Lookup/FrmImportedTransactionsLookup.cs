using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.Export;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Export;

namespace SmartAllowanceImport.Lookup
{
    public partial class FrmImportedTransactionsLookup : Telerik.WinControls.UI.RadForm
    {
        SmartAllowanceImportEntities dbContext = new SmartAllowanceImportEntities();
        List<ImportedTransactionsView> _importedTransactions;
        List<AutoCompleteDataSourceView> dataSourceViews;

        public FrmImportedTransactionsLookup()
        {
            InitializeComponent();
        }

        private void radLabel2_Click(object sender, EventArgs e)
        {

        }

        private void FrmImportedTransactionsLookup_Load(object sender, EventArgs e)
        {
            dtpStartDate.Value = DateTime.Today.Date;
            dataSourceViews = new List<AutoCompleteDataSourceView>();

            dataSourceViews = dbContext.AutoCompleteDataSourceViews.Where(x => x.Source.Trim().Equals("Import")).AsNoTracking().ToList();

            var _distinctPaths = dataSourceViews.Select(x => x.FilePath.Trim()).Distinct().ToList();

            if (_distinctPaths.Count > 0)
            {
                this.tbcPath.AutoCompleteMode = AutoCompleteMode.Suggest;
                RadListDataItemCollection _autoCompleteItems = this.tbcPath.AutoCompleteItems;

                foreach (string _description in _distinctPaths)
                {
                    _autoCompleteItems.Add(new RadListDataItem(_description));
                }
            }

            var _distinctFileName = dataSourceViews.Select(x => x.FileName.Trim()).Distinct().ToList();

            if (_distinctFileName.Count > 0)
            {
                this.tbcName.AutoCompleteMode = AutoCompleteMode.Suggest;
                RadListDataItemCollection _autoCompleteItems = this.tbcName.AutoCompleteItems;

                foreach (string _description in _distinctFileName)
                {
                    _autoCompleteItems.Add(new RadListDataItem(_description));
                }
            }

            GroupDescriptor groupDescriptor = new GroupDescriptor();
            groupDescriptor.GroupNames.Add("DateImported", ListSortDirection.Ascending);
            this.grdDataDisplay.GroupDescriptors.Add(groupDescriptor);
            grdDataDisplay.MasterTemplate.ExpandAllGroups();
        }

        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            dtpEndDate.MinDate = dtpStartDate.Value;
        }

        private void rdbnDate_CheckStateChanged(object sender, EventArgs e)
        {
            if (rdbnDate.IsChecked)
            {
                dtpStartDate.Enabled = true;
                dtpEndDate.Enabled = true;
            }
            else
            {
                dtpStartDate.Enabled = false;
                dtpEndDate.Enabled = false;
            }
        }

        private void rdbnName_CheckStateChanged(object sender, EventArgs e)
        {
            if (rdbnName.IsChecked)
                tbcName.Enabled = true;
            else
                tbcName.Enabled = false;
        }

        private void rdbnPath_CheckStateChanged(object sender, EventArgs e)
        {
            if (rdbnPath.IsChecked)
                tbcPath.Enabled = true;
            else
                tbcPath.Enabled = false;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (grdDataDisplay.Rows.Count <= 0)
                return;

            if (!pdf.IsChecked && !excel.IsChecked)
            {
                RadMessageBox.Show("Kindly select the format in which you would like to export the result set", Application.ProductName);
                return;
            }

            string _fileExtension = string.Empty;

            if (pdf.IsChecked)
                _fileExtension = ".pdf";
            else
                _fileExtension = ".xlsx";

            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "Select Path to save this export";
            folder.ShowDialog();
            string path = string.Empty;

            if (!string.IsNullOrEmpty(folder.SelectedPath))
                path = folder.SelectedPath;
            else
            {
                RadMessageBox.Show("A path must be selected to facilitate this export!", Application.ProductName);
                return;
            }

            string _destinationPath = path;

            string _saveAs = "Imported Transactions";

            string fullpath = _destinationPath + "\\" + _saveAs + _fileExtension;

            bool fileExist = File.Exists(fullpath);

            if (fileExist)
            {
                RadMessageBox.Show("A file containing the same name already exist in this location", Application.ProductName);
                return;
            }

            if (_fileExtension.Equals(".xlsx"))
            {
                GridViewSpreadExport spreadExporter = new GridViewSpreadExport(this.grdDataDisplay);
                SpreadExportRenderer exportRenderer = new SpreadExportRenderer();

                spreadExporter.HiddenColumnOption = Telerik.WinControls.UI.Export.HiddenOption.DoNotExport;
                spreadExporter.SheetName = _saveAs;
                spreadExporter.ExportVisualSettings = true;
                spreadExporter.FileExportMode = FileExportMode.CreateOrOverrideFile;

                spreadExporter.RunExport(fullpath, exportRenderer);
            }
            else if (_fileExtension.Equals(".pdf"))
            {
                //GridViewPdfExport pdfExporter = new GridViewPdfExport(this.grdDataDisplay);
                //pdfExporter.FileExtension = ".pdf";
                //pdfExporter.HiddenColumnOption = Telerik.WinControls.UI.Export.HiddenOption.DoNotExport;
                //pdfExporter.SummariesExportOption = SummariesOption.ExportAll;
                //pdfExporter.ExportVisualSettings = true;
                //pdfExporter.PageSize = new SizeF(162, 162);
                //pdfExporter.FitToPageWidth = true;

                //pdfExporter.RunExportAsync(fullpath, new Telerik.WinControls.Export.PdfExportRenderer());

                ExportToPDF exporter = new ExportToPDF(this.grdDataDisplay);
                exporter.FileExtension = "pdf";
                exporter.HiddenColumnOption = Telerik.WinControls.UI.Export.HiddenOption.DoNotExport;
                exporter.ExportVisualSettings = true;
                exporter.PageTitle = "Imported Transaction Details";
                exporter.PdfExportSettings.PageHeight = 216;
                exporter.PdfExportSettings.PageWidth = 356;
                exporter.FitToPageWidth = true;
                exporter.RunExport(fullpath);
            }

            RadMessageBox.Show("File exported successfully!", Application.ProductName);
        }

        private void FrmImportedTransactionsLookup_FormClosing(object sender, FormClosingEventArgs e)
        {
            dbContext.Dispose();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!rdbnDate.IsChecked && !rdbnName.IsChecked && !rdbnPath.IsChecked)
            {
                RadMessageBox.Show("Select an option then press 'Search' to retreive the desired results!", Application.ProductName);
                return;
            }

            _importedTransactions = new List<ImportedTransactionsView>();

            if (rdbnDate.IsChecked)
                _importedTransactions = dbContext.ImportedTransactionsViews.Where(x => DbFunctions.TruncateTime(x.DateImported) >= dtpStartDate.Value && DbFunctions.TruncateTime(x.DateImported) <= dtpEndDate.Value).AsNoTracking().ToList();
            else if (rdbnName.IsChecked)
                _importedTransactions = dbContext.ImportedTransactionsViews.Where(x => x.NewFileName.Trim().ToUpper().Equals(tbcName.Text.Trim().ToUpper())).AsNoTracking().ToList();
            else if (rdbnPath.IsChecked)
                _importedTransactions = dbContext.ImportedTransactionsViews.Where(x => x.ImportedFilePath.Trim().ToUpper().Equals(tbcPath.Text.Trim().ToUpper())).AsNoTracking().ToList();

            grdDataDisplay.DataSource = _importedTransactions;
        }
    }
}