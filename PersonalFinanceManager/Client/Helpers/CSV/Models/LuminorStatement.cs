using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client.Helpers.CSV.Models
{
    public class LuminorStatement
    {
        [Index(0)]
        public string DateTime { get; set; }

        [Index(1)]
        public string Amount { get; set; }

        [Index(2)]
        public string CD { get; set; }
    }
}
