using PersonalFinanceManager.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client.ViewModels
{
    public class IncomeViewModel 
    {
        public float Amount { get; set; }

        public static implicit operator IncomeViewModel(IncomeModel income)
        {
            return new IncomeViewModel
            {
                Amount = income.Amount
            };
        }
    }
}
