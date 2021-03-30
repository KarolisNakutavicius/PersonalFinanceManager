﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Shared.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public IList<Statement> Statements { get; set; }

    }
}
