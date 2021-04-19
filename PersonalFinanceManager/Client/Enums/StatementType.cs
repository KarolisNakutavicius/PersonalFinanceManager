using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client.Enums
{
    public enum StatementType
    {
        [Description("Expenses")]
        Expense,
        [Description("Incomes")]
        Income,
        [Description("Budgets")]
        Budget
    }
}
