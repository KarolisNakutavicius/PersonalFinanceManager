using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Shared.Models
{
    public class User : IdentityUser<int>
    {
        //public bool IsAuthenticated { get; set; }
        public IList<Statement> Statements { get; set; }
    }
}
