using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Client.Contracts
{
    public interface IViewModel
    {
        Task OnInit();
    }
}
