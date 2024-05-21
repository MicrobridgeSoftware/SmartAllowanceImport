//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmartAllowanceImport
{
    using System;
    using System.Collections.Generic;
    
    public partial class ImportedTransaction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ImportedTransaction()
        {
            this.ImportedTransactionAllowances = new HashSet<ImportedTransactionAllowance>();
            this.ExportedTransactionDetails = new HashSet<ExportedTransactionDetail>();
        }
    
        public int ImportedTransactionId { get; set; }
        public int ImportedTransactionHeaderId { get; set; }
        public string EmployeeReference { get; set; }
        public decimal RegularOvertime { get; set; }
        public decimal RegularDoubleTime { get; set; }
        public decimal RegularTripleTime { get; set; }
        public int MealAllowance { get; set; }
        public int EveningPremium { get; set; }
        public int NightPremium { get; set; }
        public int SundayPremium { get; set; }
        public decimal HolidayRegularHours { get; set; }
        public decimal HolidayDoubleTimeHours { get; set; }
        public decimal HolidayTripleTimeHours { get; set; }
        public decimal ShiftOvertime { get; set; }
        public decimal ShiftDoubleTime { get; set; }
        public decimal OverhaulOvertime { get; set; }
        public decimal OverhaulDoubleTime { get; set; }
        public decimal OverhaulTripleTime { get; set; }
        public decimal AbsentHours { get; set; }
        public string Notes { get; set; }
    
        public virtual ImportedTransactionHeader ImportedTransactionHeader { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ImportedTransactionAllowance> ImportedTransactionAllowances { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExportedTransactionDetail> ExportedTransactionDetails { get; set; }
    }
}
