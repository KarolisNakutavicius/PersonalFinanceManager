using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client.Helpers.CSV
{
    public class IsExpenseConverter : ConverterBase
    {
        public override object StringToField(string @from)
        {
            return @from == "C";
        }
    }
}
