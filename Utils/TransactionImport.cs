using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartAllowanceImport.Utils
{
    public class TransactionImport
    {
        public string _empRef { get; set; }
        public decimal _regularOT { get; set; }
        public decimal _regularDT { get; set; }
        public decimal _regularTT { get; set; }
        public int _mealAllowance { get; set; }
        public int _eveningAllowance { get; set; }
        public int _nightAllowance { get; set; }
        public int _sundayAllowance { get; set; }
        public decimal _holidayRegular { get; set; }
        public decimal _holidayDouble { get; set; }
        public decimal _holidayTriple { get; set; }
        public decimal _shiftOT { get; set; }
        public decimal _shiftDT { get; set; }
        public decimal _overhaulOT { get; set; }
        public decimal _overhaulDT { get; set; }
        public decimal _overhaulTT { get; set; }
        public decimal _absHours { get; set; }
        public string _employeeName { get; set; }
        public string _notes { get; set; }
    }
}
