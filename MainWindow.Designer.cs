namespace SmartAllowanceImport
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.WinControls.UI.CarouselEllipsePath carouselEllipsePath1 = new Telerik.WinControls.UI.CarouselEllipsePath();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.radCarousel = new Telerik.WinControls.UI.RadCarousel();
            this.btnMaintenance = new Telerik.WinControls.UI.RadButtonElement();
            this.ellipseShape1 = new Telerik.WinControls.EllipseShape();
            this.btnImport = new Telerik.WinControls.UI.RadButtonElement();
            this.btnImportedData = new Telerik.WinControls.UI.RadButtonElement();
            this.btnExportedData = new Telerik.WinControls.UI.RadButtonElement();
            this.btnExport = new Telerik.WinControls.UI.RadButtonElement();
            ((System.ComponentModel.ISupportInitialize)(this.radCarousel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radCarousel
            // 
            carouselEllipsePath1.Center = new Telerik.WinControls.UI.Point3D(50D, 50D, 0D);
            carouselEllipsePath1.FinalAngle = -100D;
            carouselEllipsePath1.InitialAngle = -90D;
            carouselEllipsePath1.U = new Telerik.WinControls.UI.Point3D(-20D, -17D, -50D);
            carouselEllipsePath1.V = new Telerik.WinControls.UI.Point3D(30D, -25D, -60D);
            carouselEllipsePath1.ZScale = 500D;
            this.radCarousel.CarouselPath = carouselEllipsePath1;
            this.radCarousel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radCarousel.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.btnMaintenance,
            this.btnImport,
            this.btnImportedData,
            this.btnExportedData,
            this.btnExport});
            this.radCarousel.Location = new System.Drawing.Point(0, 0);
            this.radCarousel.Name = "radCarousel";
            this.radCarousel.Size = new System.Drawing.Size(784, 475);
            this.radCarousel.TabIndex = 1;
            this.radCarousel.Text = "radCarousel1";
            ((Telerik.WinControls.UI.RadRepeatButtonElement)(this.radCarousel.GetChildAt(0).GetChildAt(3))).DisplayStyle = Telerik.WinControls.DisplayStyle.None;
            ((Telerik.WinControls.UI.RadRepeatButtonElement)(this.radCarousel.GetChildAt(0).GetChildAt(3))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            ((Telerik.WinControls.UI.RadRepeatButtonElement)(this.radCarousel.GetChildAt(0).GetChildAt(4))).DisplayStyle = Telerik.WinControls.DisplayStyle.None;
            ((Telerik.WinControls.UI.RadRepeatButtonElement)(this.radCarousel.GetChildAt(0).GetChildAt(4))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // btnMaintenance
            // 
            this.btnMaintenance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMaintenance.Image = global::SmartAllowanceImport.Properties.Resources.iconfinder_package_system_1442;
            this.btnMaintenance.Name = "btnMaintenance";
            this.btnMaintenance.Shape = this.ellipseShape1;
            this.btnMaintenance.Text = "Maintenance";
            this.btnMaintenance.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnMaintenance.Click += new System.EventHandler(this.btnMaintenance_Click);
            // 
            // ellipseShape1
            // 
            this.ellipseShape1.IsRightToLeft = false;
            // 
            // btnImport
            // 
            this.btnImport.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImport.Image = global::SmartAllowanceImport.Properties.Resources.fileImport;
            this.btnImport.Name = "btnImport";
            this.btnImport.Shape = this.ellipseShape1;
            this.btnImport.Text = "File Import";
            this.btnImport.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnImportedData
            // 
            this.btnImportedData.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportedData.Image = global::SmartAllowanceImport.Properties.Resources.view_doc;
            this.btnImportedData.Name = "btnImportedData";
            this.btnImportedData.Shape = this.ellipseShape1;
            this.btnImportedData.Text = "View Imported Data";
            this.btnImportedData.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnImportedData.Click += new System.EventHandler(this.btnImportedData_Click);
            // 
            // btnExportedData
            // 
            this.btnExportedData.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportedData.Image = global::SmartAllowanceImport.Properties.Resources.view_doc;
            this.btnExportedData.Name = "btnExportedData";
            this.btnExportedData.Shape = this.ellipseShape1;
            this.btnExportedData.Text = "View Exported Data";
            this.btnExportedData.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExportedData.TextOrientation = System.Windows.Forms.Orientation.Horizontal;
            this.btnExportedData.Click += new System.EventHandler(this.btnExportedData_Click);
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Image = global::SmartAllowanceImport.Properties.Resources.FileExport;
            this.btnExport.Name = "btnExport";
            this.btnExport.Shape = this.ellipseShape1;
            this.btnExport.Text = "File Export";
            this.btnExport.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 475);
            this.Controls.Add(this.radCarousel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "MainWindow";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "SMART Allowance Import";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radCarousel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadCarousel radCarousel;
        private Telerik.WinControls.UI.RadButtonElement btnMaintenance;
        private Telerik.WinControls.UI.RadButtonElement btnExport;
        private Telerik.WinControls.UI.RadButtonElement btnImport;
        private Telerik.WinControls.EllipseShape ellipseShape1;
        private Telerik.WinControls.UI.RadButtonElement btnImportedData;
        private Telerik.WinControls.UI.RadButtonElement btnExportedData;
    }
}
