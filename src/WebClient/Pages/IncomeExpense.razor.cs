using System.Collections.Generic;
using System.Linq;
using Radzen.Blazor;

namespace WebClient.Pages
{
    public partial class IncomeExpense
    {
        private float totalCahsFlow => totalIncome - totalExpense;
        private float totalIncome;
        private float totalExpense;

        protected override void OnInitialized()
        {
            totalIncome = incomes.Sum(x => x.Price);
            totalExpense = expenses.Sum(x => x.Price);
            base.OnInitialized();
        }

        public class Income
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public float Price { get; set; }
        }
        private RadzenGrid<Income> incomesGrid;
        private List<Income> incomes = new List<Income>
        {
        new Income{Id = 1, Name = "Зарплата", Price = 100000}
        };

        private void InsertRowIncome()
        {
            var income = new Income();
            var id = (incomes.OrderByDescending(x => x.Id).First()?.Id ?? 0) + 1;
            income.Id = id;
            incomesGrid.InsertRow(income);
        }

        private void OnUpdateRowIncome(Income income)
        {
            //TODO Научиться правильно пределять объект
            foreach (Income item in incomes)
            {
                if (item.Id == income.Id)
                {
                    item.Name = income.Name;
                    item.Price = income.Price;
                }
            }
            totalIncome = incomes.Sum(x => x.Price);
        }

        private void OnCreateRowIncome(Income income)
        {
            incomes.Add(income);
            totalIncome = incomes.Sum(x => x.Price);
        }

        private void EditRowIncome(Income income)
        {
            incomesGrid.EditRow(income);
        }

        private void SaveRowIncome(Income income)
        {
            incomesGrid.UpdateRow(income);
        }

        private void CancelEditIncome(Income income)
        {
            incomesGrid.CancelEditRow(income);
        }

        private void DeleteRowIncome(Income income)
        {
            //TODO Научиться правильно пределять объект
            if (incomes.Contains(income))
            {
                incomes.Remove(income);
                incomesGrid.Reload();
                totalIncome = incomes.Sum(x => x.Price);
            }
            else
            {
                incomesGrid.CancelEditRow(income);
            }
        }

        public class Expense
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public float Price { get; set; }
        }
        private RadzenGrid<Expense> expensesGrid;
        private List<Expense> expenses = new List<Expense>
    {
        new Expense{Id = 1, Name = "Квартира", Price = 15000},
        new Expense{Id = 2, Name = "Продукты", Price = 15000},
        new Expense{Id = 3, Name = "Прочие расходы", Price = 10000}
    };

        private void InsertRowExpense()
        {
            var expense = new Expense();
            var id = (expenses.OrderByDescending(x => x.Id).First()?.Id ?? 0) + 1;
            expense.Id = id;
            expensesGrid.InsertRow(expense);
        }

        private void OnUpdateRowExpense(Expense expense)
        {
            //TODO Научиться правильно пределять объект
            foreach (Expense item in expenses)
            {
                if (item.Id == expense.Id)
                {
                    item.Name = expense.Name;
                    item.Price = expense.Price;
                }
            }
            totalExpense = expenses.Sum(x => x.Price);
        }

        private void OnCreateRowExpense(Expense expense)
        {
            expenses.Add(expense);
            totalExpense = expenses.Sum(x => x.Price);
        }

        private void EditRowExpense(Expense expense)
        {
            expensesGrid.EditRow(expense);
        }

        private void SaveRowExpense(Expense expense)
        {
            expensesGrid.UpdateRow(expense);
        }

        private void CancelEditExpense(Expense expense)
        {
            expensesGrid.CancelEditRow(expense);
        }

        private void DeleteRowExpense(Expense expense)
        {
            //TODO Научиться правильно пределять объект
            if (expenses.Contains(expense))
            {
                expenses.Remove(expense);
                expensesGrid.Reload();
                totalExpense = expenses.Sum(x => x.Price);
            }
            else
            {
                expensesGrid.CancelEditRow(expense);
            }
        }
    }
}
