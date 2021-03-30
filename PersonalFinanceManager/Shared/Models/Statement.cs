﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Shared.Models
{
    public class Statement
    {
        public int StatementId { get; set; }
        public float Amount { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}