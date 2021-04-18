using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Shared.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string ColorHex { get; set; }

        public int StatementId { get; set; }
        public Statement Statement { get; set; }
    }
}
