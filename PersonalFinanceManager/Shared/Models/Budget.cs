using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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


        // Property needed for UI. Need To refactor this
        private string _newCategory;
        [NotMapped]
        public string NewCategoryName
        {
            get
            {
                if (string.IsNullOrEmpty(_newCategory) && Categories != null)
                    return Categories.First().Name;
                return _newCategory;
            }
            set
            {
                _newCategory = value;
            }
        }
    }
}
