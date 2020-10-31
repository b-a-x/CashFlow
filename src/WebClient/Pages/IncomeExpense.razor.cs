using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using WebClient.Services;

namespace WebClient.Pages
{
    public partial class IncomeExpense
    {
        [Inject]
        private IFormatProvider provider { get; set; }

        [Inject]
        public HttpInterceptorService interceptor { get; set; }

        private string titleTotalCashFlow;
        private float totalIncome;
        private float totalExpense;
        public float TotalIncome
        {
            get => totalIncome;
            set
            {
                totalIncome = value;
                CalculateTotalCashFlow();
            }
        }
        public float TotalExpense
        {
            get => totalExpense;
            set
            {
                totalExpense = value;
                CalculateTotalCashFlow();
            }
        }

        protected override Task OnInitializedAsync()
        {
            interceptor.RegisterEvent();
            TotalIncome = incomes.Sum(x => x.Price);
            TotalExpense = expenses.Sum(x => x.Price);
            return base.OnInitializedAsync();
        }

        public void Dispose() => interceptor.DisposeEvent();

        private void CalculateTotalCashFlow()
        {
            titleTotalCashFlow = $"{string.Format(provider, "{0:C} ", totalIncome - totalExpense)}";
        }

        public class Income
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public float Price { get; set; }
            public int OrderNumber { get; set; }
        }
        
        private RadzenGrid<Income> incomesGrid;
        private List<Income> incomes = new List<Income>
        {
            new Income{Id = Guid.NewGuid().ToString(), Name = "Зарплата", Price = 100000, OrderNumber = 1},
            new Income{Id = Guid.NewGuid().ToString(), Name = "Дивиденты по акциям компании Apple", Price = 250, OrderNumber = 2}
        };

        private void InsertRowIncome()
        {
            var income = new Income();
            var id = (incomes.OrderByDescending(x => x.OrderNumber).First()?.OrderNumber ?? 0) + 1;
            income.OrderNumber = id;
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
            TotalIncome = incomes.Sum(x => x.Price);
        }

        private void OnCreateRowIncome(Income income)
        {
            incomes.Add(income);
            TotalIncome = incomes.Sum(x => x.Price);
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
                TotalIncome = incomes.Sum(x => x.Price);
            }
            else
            {
                incomesGrid.CancelEditRow(income);
            }
        }

        public class Expense
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public float Price { get; set; }
            public int OrderNumber { get; set; }
        }
        private RadzenGrid<Expense> expensesGrid;
        private List<Expense> expenses = new List<Expense>
        {
            new Expense{Id = Guid.NewGuid().ToString(), Name = "Квартира", Price = 15000, OrderNumber = 1},
            new Expense{Id = Guid.NewGuid().ToString(), Name = "Продукты", Price = 15000, OrderNumber = 2},
            new Expense{Id = Guid.NewGuid().ToString(), Name = "Прочие расходы", Price = 10000, OrderNumber = 3},
            new Expense{Id = Guid.NewGuid().ToString(), Name = "Платеж по ипотеке", Price = 30000, OrderNumber = 4}
        };

        private void InsertRowExpense()
        {
            var expense = new Expense();
            var id = (expenses.OrderByDescending(x => x.OrderNumber).First()?.OrderNumber ?? 0) + 1;
            expense.OrderNumber = id;
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
            TotalExpense = expenses.Sum(x => x.Price);
        }

        private void OnCreateRowExpense(Expense expense)
        {
            expenses.Add(expense);
            TotalExpense = expenses.Sum(x => x.Price);
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
                TotalExpense = expenses.Sum(x => x.Price);
            }
            else
            {
                expensesGrid.CancelEditRow(expense);
            }
        }
    }
}
