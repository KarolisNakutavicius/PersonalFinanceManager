﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceManager.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Server.Contexts
{
    public class FinanceManagerContext : IdentityDbContext<User>
    {
        public override DbSet<User> Users { get; set; }

        public DbSet<Statement> Statements { get; set; }

        public DbSet<IncomeModel> Incomes { get; set; }

        public DbSet<Expense> Expenses { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Budget> Budgets { get; set; }

        public FinanceManagerContext(DbContextOptions<FinanceManagerContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Statement>()
                .HasDiscriminator<string>("statement_type")
                .HasValue<IncomeModel>("income")
                .HasValue<Expense>("expense")
                .IsComplete(true);

            modelBuilder.Entity<Statement>()
                .HasOne(s => s.User)
                .WithMany(u => u.Statements)
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<Statement>()
                .HasOne(s => s.Category)
                .WithMany(c => c.Statements)
                .HasForeignKey(s => s.CategoryId);

            modelBuilder.Entity<Budget>().Property(b => b.BudgetId).ValueGeneratedOnAdd();

            modelBuilder.Entity<Budget>()
                .HasMany(b => b.Categories)
                .WithMany(c => c.Budgets);
        }
    }
}
