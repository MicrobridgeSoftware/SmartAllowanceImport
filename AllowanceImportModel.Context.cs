﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SmartAllowanceImportEntities : DbContext
    {
        public SmartAllowanceImportEntities()
            : base("name=SmartAllowanceImportEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ImportedTransaction> ImportedTransactions { get; set; }
        public virtual DbSet<ImportedTransactionHeader> ImportedTransactionHeaders { get; set; }
        public virtual DbSet<AllowanceConfiguration> AllowanceConfigurations { get; set; }
        public virtual DbSet<ImportedTransactionAllowance> ImportedTransactionAllowances { get; set; }
        public virtual DbSet<PayrollCodeView> PayrollCodeViews { get; set; }
        public virtual DbSet<ExportedTransactionDetail> ExportedTransactionDetails { get; set; }
        public virtual DbSet<ExportedTransaction> ExportedTransactions { get; set; }
        public virtual DbSet<ImportedTransactionsView> ImportedTransactionsViews { get; set; }
        public virtual DbSet<AutoCompleteDataSourceView> AutoCompleteDataSourceViews { get; set; }
        public virtual DbSet<AllowancesForExportView> AllowancesForExportViews { get; set; }
        public virtual DbSet<ExportedTransactionsView> ExportedTransactionsViews { get; set; }
    }
}
