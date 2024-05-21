using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace SmartAllowanceImport.Maintenance
{
    public partial class FrmAllowanceConfiguration : Telerik.WinControls.UI.RadForm
    {
        SmartAllowanceImportEntities dbContext = new SmartAllowanceImportEntities();
        List<PayrollCodeView> _payrollCodes;
        List<AllowanceConfiguration> _allowanceApplications;

        public FrmAllowanceConfiguration()
        {
            InitializeComponent();
        }

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }

        private void radLabel2_Click(object sender, EventArgs e)
        {

        }

        private void FrmAllowanceConfiguration_Load(object sender, EventArgs e)
        {
            _payrollCodes = new List<PayrollCodeView>();

            _payrollCodes = dbContext.PayrollCodeViews.AsNoTracking().ToList();

            this.tbcPayrollCode.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            RadListDataItemCollection _autoCompleteItems = this.tbcPayrollCode.AutoCompleteItems;

            foreach (string _code in _payrollCodes.Select(x => x.PayrollCode).Distinct().ToList())
            {
                _autoCompleteItems.Add(new RadListDataItem(_code));
            }

            getFormData();
        }

        public void getFormData()
        {

            _allowanceApplications = new List<AllowanceConfiguration>();
            _allowanceApplications = dbContext.AllowanceConfigurations.AsNoTracking().ToList();

            bindingSourceAllowanceConfig.DataSource = _allowanceApplications;
        }

        private void FrmAllowanceConfiguration_FormClosing(object sender, FormClosingEventArgs e)
        {
            dbContext.Dispose();
        }

        private void grdDataDisplay_CommandCellClick(object sender, GridViewCellEventArgs e)
        {
            DialogResult _verifyAction = RadMessageBox.Show("Do you want to edit this record?", Application.ProductName, MessageBoxButtons.YesNo);

            if (_verifyAction == DialogResult.Yes)
            {
                grdDataDisplay.Enabled = false;
                spnPayRate.Enabled = true;
                tbcPayrollCode.Enabled = true;
                chkActive.Enabled = true;
                txtName.Enabled = true;
            }
        }

        private void btnRevert_Click(object sender, EventArgs e)
        {
            if (grdDataDisplay.Enabled)
                return;

            DialogResult _verifyAction = RadMessageBox.Show("All unsaved data will be lost.\n" +
                                                            "Are you sure you want to revert?", Application.ProductName, MessageBoxButtons.YesNo);

            if (_verifyAction == DialogResult.Yes)
            {
                spnPayRate.Enabled = false;
                tbcPayrollCode.Enabled = false;
                chkActive.Enabled = false;
                txtName.Enabled = false;
                grdDataDisplay.Enabled = true;

                bindingSourceAllowanceConfig.MovePrevious();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (grdDataDisplay.Enabled)
                return;

            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                RadMessageBox.Show("A name is required for this Allowance!", Application.ProductName);
                return;
            }

            if (string.IsNullOrEmpty(tbcPayrollCode.Text.Trim()))
            {
                RadMessageBox.Show("Payroll code is a required field!", Application.ProductName);
                return;
            }
            else
            {
                string _payCode = tbcPayrollCode.Text.Trim();

                var _codeExist = _payrollCodes.Where(x => x.PayrollCode.Trim().Equals(_payCode)).Any();

                if (!_codeExist)
                {
                    RadMessageBox.Show("This Payroll code does not exist in SMART Pay!", Application.ProductName);
                    return;
                }
            }

            DialogResult _verifyAction = RadMessageBox.Show("Do you want to save this record?", Application.ProductName, MessageBoxButtons.YesNo);

            if (_verifyAction == DialogResult.Yes)
            {
                try
                {
                    int _allowanceConfigId = ((AllowanceConfiguration)bindingSourceAllowanceConfig.Current).AllowanceConfigId;

                    var _findAllowanceRecord = dbContext.AllowanceConfigurations.Where(c => c.AllowanceConfigId == _allowanceConfigId).FirstOrDefault();
                    _findAllowanceRecord.PayrollCode = tbcPayrollCode.Text.Trim();
                    _findAllowanceRecord.Active = chkActive.Checked == true ? true : false;
                    _findAllowanceRecord.RateFactor = (decimal)spnPayRate.Value;
                    _findAllowanceRecord.AllowanceName = txtName.Text.Trim();
                    _findAllowanceRecord.LastModifiedDate = DateTime.Now;

                    dbContext.SaveChanges();

                    grdDataDisplay.Enabled = true;
                    spnPayRate.Enabled = false;
                    tbcPayrollCode.Enabled = false;
                    chkActive.Enabled = false;
                    txtName.Enabled = false;
                    bindingSourceAllowanceConfig.Clear();
                    getFormData();
                    RadMessageBox.Show("Record created successfully!", Application.ProductName);
                }
                catch (Exception _exp)
                {
                    RadMessageBox.Show(_exp.InnerException == null ? _exp.Message : _exp.InnerException.Message);
                }
            }
        }

        private void bindingSourceAllowanceConfig_PositionChanged(object sender, EventArgs e)
        {
            if(bindingSourceAllowanceConfig.Current != null)
            {
                if (!string.IsNullOrEmpty(((AllowanceConfiguration)bindingSourceAllowanceConfig.Current).PayrollCode))
                    tbcPayrollCode.Text = ((AllowanceConfiguration)bindingSourceAllowanceConfig.Current).PayrollCode.Trim();
                else
                    tbcPayrollCode.Text = string.Empty;
            }
        }
    }
}