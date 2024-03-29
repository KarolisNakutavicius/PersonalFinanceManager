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
        public DateTime DateTime { get; set; }
        public string Details { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
