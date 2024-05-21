using SmartAllowanceImport.Export;
using SmartAllowanceImport.Import;
using SmartAllowanceImport.Lookup;
using SmartAllowanceImport.Maintenance;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace SmartAllowanceImport
{
    public partial class MainWindow : Telerik.WinControls.UI.RadForm
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            CarouselBezierPath bezierPath = new CarouselBezierPath();
            bezierPath.FirstPoint = new Point3D(10, 20, 0);
            bezierPath.CtrlPoint1 = new Point3D(14, 76, 70);
            bezierPath.CtrlPoint2 = new Point3D(86, 76, 70);
            bezierPath.LastPoint = new Point3D(90, 20, 0);
            this.radCarousel.CarouselPath = bezierPath;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            this.radCarousel.AnimationsToApply = Animations.None;

            FrmFileExport _fileExport = new FrmFileExport();
            _fileExport.ShowDialog();
        }

        private void btnMaintenance_Click(object sender, EventArgs e)
        {
            this.radCarousel.AnimationsToApply = Animations.None;

            FrmAllowanceConfiguration _allowanceConfiguration = new FrmAllowanceConfiguration();
            _allowanceConfiguration.ShowDialog();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            this.radCarousel.AnimationsToApply = Animations.None;

            FrmFileImport _fileImport = new FrmFileImport();
            //_fileImport.MdiParent = this;
            _fileImport.ShowDialog();
        }

        private void btnImportedData_Click(object sender, EventArgs e)
        {
            this.radCarousel.AnimationsToApply = Animations.None;

            FrmImportedTransactionsLookup _transactionsLookup = new FrmImportedTransactionsLookup();
            _transactionsLookup.ShowDialog();
        }

        private void btnExportedData_Click(object sender, EventArgs e)
        {
            this.radCarousel.AnimationsToApply = Animations.None;

            FrmExportedTransactionsLookup _transactionsLookup = new FrmExportedTransactionsLookup();
            _transactionsLookup.ShowDialog();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}