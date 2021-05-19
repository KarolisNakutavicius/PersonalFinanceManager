using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Shared.Responses
{
    public class RegisterResult
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public string Token { get; set; }
    }
}
