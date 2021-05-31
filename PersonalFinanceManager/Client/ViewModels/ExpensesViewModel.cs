using PersonalFinanceManager.Client.Abstract;
using PersonalFinanceManager.Client.Enums;
using PersonalFinanceManager.Client.Services;

namespace PersonalFinanceManager.Client.ViewModels
{
    public class ExpensesViewModel : StatementsBaseViewModel
    {
        public override StatementType Type => StatementType.Expense;

        public ExpensesViewModel(CategoryManager categoryManager,
            AddViewModel addViewModel,
            EditCategoriesViewModel editCategoriesViewModel) : base(addViewModel, categoryManager, editCategoriesViewModel)
        {
        }
    }
}
