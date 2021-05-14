using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Shared.Models
{
    public class Budget
    {
        public int BudgetId { get; set; }
        public int Amount { get; set; }
        public string Name { get; set; }
        public List<Category> Categories { get; set; }
    }
}
