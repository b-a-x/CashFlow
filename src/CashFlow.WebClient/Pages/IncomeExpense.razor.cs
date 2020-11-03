using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashFlow.Model.Dto.Request;
using CashFlow.Services.Interfaces;
using CashFlow.WebClient.Services;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace CashFlow.WebClient.Pages
{
    public partial class IncomeExpense
    {
        [Inject]
        private IFormatProvider _provider { get; set; }

        [Inject]
        private HttpInterceptorService _interceptor { get; set; }

        [Inject]
        private IIncomeService _incomeService { get; set; }

        private string _titleTotalCashFlow;
        private float _totalIncome;
        private float _totalExpense;

        private List<IncomeDto> _incomes = new List<IncomeDto>();
        private RadzenGrid<IncomeDto> _incomesGrid;

        public float TotalIncome
        {
            get => _totalIncome;
            set
            {
                _totalIncome = value;
                CalculateTotalCashFlow();
            }
        }
        public float TotalExpense
        {
            get => _totalExpense;
            set
            {
                _totalExpense = value;
                CalculateTotalCashFlow();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            _interceptor.RegisterEvent();
            _incomes = (List<IncomeDto>)await _incomeService.GetAllIncomeForUserAsync(string.Empty);
            TotalIncome = _incomes.Sum(x => x.Price);
            TotalExpense = expenses.Sum(x => x.Price);
            await base.OnInitializedAsync();
        }

        public void Dispose() => _interceptor.DisposeEvent();

        private void CalculateTotalCashFlow()
        {
            _titleTotalCashFlow = $"{string.Format(_provider, "{0:C} ", _totalIncome - _totalExpense)}";
        }

        private void InsertRowIncome()
        {
            var income = new IncomeDto
            {
                OrderNumber = (_incomes.OrderByDescending(x => x.OrderNumber).FirstOrDefault()?.OrderNumber ?? 0) + 1
            };

            _incomesGrid.InsertRow(income);
        }

        private async Task OnUpdateRowIncome(IncomeDto income)
        {
            IncomeDto edit = _incomes.FirstOrDefault(x => x.Id == income.Id);
            if (edit != null)
            {
                edit.Name = income.Name;
                edit.Price = income.Price;
                await _incomeService.UpdateIncomeAsync(edit);
            }

            TotalIncome = _incomes.Sum(x => x.Price);
        }

        private async Task OnCreateRowIncome(IncomeDto income)
        {
            income = await _incomeService.CreateIncomeAsync(income);
            _incomes.Add(income);
            TotalIncome = _incomes.Sum(x => x.Price);
            await _incomesGrid.Reload();
        }

        private void EditRowIncome(IncomeDto income)
        {
            _incomesGrid.EditRow(income);
        }

        private void SaveRowIncome(IncomeDto income)
        {
            _incomesGrid.UpdateRow(income);
        }

        private void CancelEditIncome(IncomeDto income)
        {
            _incomesGrid.CancelEditRow(income);
        }

        private async Task DeleteRowIncome(IncomeDto income)
        {
            if (_incomes.Contains(income))
            {
                _incomes.Remove(income);
                TotalIncome = _incomes.Sum(x => x.Price);
                await _incomesGrid.Reload();
                await _incomeService.RemoveIncomeAsync(income.Id);
            }
            else
            {
                _incomesGrid.CancelEditRow(income);
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
            var id = (expenses.OrderByDescending(x => x.OrderNumber).FirstOrDefault()?.OrderNumber ?? 0) + 1;
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
