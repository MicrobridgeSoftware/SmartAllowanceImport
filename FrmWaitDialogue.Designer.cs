namespace SmartAllowanceImport
{
    partial class FrmWaitDialogue
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
            this.segmentedRingWaitingBarIndicatorElement1 = new Telerik.WinControls.UI.SegmentedRingWaitingBarIndicatorElement();
            this.radWaitingBar = new Telerik.WinControls.UI.RadWaitingBar();
            this.segmentedRingWaitingBarIndicatorElement2 = new Telerik.WinControls.UI.SegmentedRingWaitingBarIndicatorElement();
            ((System.ComponentModel.ISupportInitialize)(this.radWaitingBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // segmentedRingWaitingBarIndicatorElement1
            // 
            this.segmentedRingWaitingBarIndicatorElement1.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.segmentedRingWaitingBarIndicatorElement1.Name = "segmentedRingWaitingBarIndicatorElement1";
            this.segmentedRingWaitingBarIndicatorElement1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.segmentedRingWaitingBarIndicatorElement1.UseCompatibleTextRendering = false;
            // 
            // radWaitingBar
            // 
            this.radWaitingBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radWaitingBar.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.radWaitingBar.Location = new System.Drawing.Point(0, 0);
            this.radWaitingBar.Name = "radWaitingBar";
            this.radWaitingBar.ShowText = true;
            this.radWaitingBar.Size = new System.Drawing.Size(312, 135);
            this.radWaitingBar.TabIndex = 1;
            this.radWaitingBar.Text = "Processing your request. Please wait...............";
            this.radWaitingBar.WaitingIndicators.Add(this.segmentedRingWaitingBarIndicatorElement2);
            this.radWaitingBar.WaitingSpeed = 20;
            this.radWaitingBar.WaitingStyle = Telerik.WinControls.Enumerations.WaitingBarStyles.SegmentedRing;
            // 
            // segmentedRingWaitingBarIndicatorElement2
            // 
            this.segmentedRingWaitingBarIndicatorElement2.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.segmentedRingWaitingBarIndicatorElement2.Name = "segmentedRingWaitingBarIndicatorElement2";
            this.segmentedRingWaitingBarIndicatorElement2.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.segmentedRingWaitingBarIndicatorElement2.UseCompatibleTextRendering = false;
            // 
            // FrmWaitDialogue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 135);
            this.ControlBox = false;
            this.Controls.Add(this.radWaitingBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FrmWaitDialogue";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Wait Dialogue";
            this.Load += new System.EventHandler(this.FrmWaitDialogue_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radWaitingBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.SegmentedRingWaitingBarIndicatorElement segmentedRingWaitingBarIndicatorElement1;
        private Telerik.WinControls.UI.RadWaitingBar radWaitingBar;
        private Telerik.WinControls.UI.SegmentedRingWaitingBarIndicatorElement segmentedRingWaitingBarIndicatorElement2;
    }
}
