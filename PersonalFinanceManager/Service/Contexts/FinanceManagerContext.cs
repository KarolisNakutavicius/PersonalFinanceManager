using Microsoft.EntityFrameworkCore;
using PersonalFinanceManager.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Server.Contexts
{
    public class FinanceManagerContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Statement> Statements { get; set; }

        public DbSet<IncomeModel> Incomes { get; set; }

        public DbSet<Expense> Expenses { get; set; }

        public FinanceManagerContext(DbContextOptions<FinanceManagerContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(ba => ba.UserID).ValueGeneratedOnAdd();
            modelBuilder.Entity<Statement>().Property(ba => ba.StatementId).ValueGeneratedOnAdd();

            modelBuilder.Entity<Statement>()
                .HasDiscriminator<string>("statement_type")
                .HasValue<IncomeModel>("income")
                .HasValue<Expense>("expense")
                .IsComplete(true);

            modelBuilder.Entity<Statement>()
                .HasOne(s => s.User)
                .WithMany(u => u.Statements)
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserID = 1,
                    Email = "karolis.nakutavicius@stud.mif.vu.lt"
                });


            modelBuilder.Entity<IncomeModel>().HasData(
                new IncomeModel
                {
                    StatementId = 1,
                    UserId = 1,
                    Amount = 100,
                });
        }
    }
}
