using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashFlow.Model.Dto.Request;
using CashFlow.Services.Interfaces;
using CashFlow.Client.Web.Services;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace CashFlow.Client.Web.Pages
{
    public partial class IncomeExpense
    {
        [Inject]
        private IFormatProvider _provider { get; set; }

        [Inject]
        private HttpInterceptorService _interceptor { get; set; }

        [Inject]
        private IIncomeService _incomeService { get; set; }

        [Inject]
        private IExpenseService _expenseService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

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
            try
            {
                //TODO: Обрабатывать Exception авторизации 
                _incomes = (List<IncomeDto>)await _incomeService.GetAllIncomeForUserAsync(string.Empty);
                _expenses = (List<ExpenseDto>)await _expenseService.GetAllExpenseForUserAsync(string.Empty);
            }
            catch (Exception _)
            {
                NavigationManager.NavigateTo("/login");
            }
            
            TotalIncome = _incomes.Sum(x => x.Price);
            TotalExpense = _expenses.Sum(x => x.Price);
            await base.OnInitializedAsync();
        }

        public void Dispose() => _interceptor.DisposeEvent();

        private void CalculateTotalCashFlow()
        {
            _titleTotalCashFlow = $"{string.Format(_provider, "{0:C} ", _totalIncome - _totalExpense)}";
        }

        private void InsertRowIncome()
        {
            _incomesGrid.InsertRow(new IncomeDto { OrderNumber = (_incomes.OrderByDescending(x => x.OrderNumber).FirstOrDefault()?.OrderNumber ?? 0) + 1 });
        }

        private async Task OnUpdateRowIncome(IncomeDto income)
        {
            IncomeDto edit = _incomes.FirstOrDefault(x => x.Id == income.Id);
            if (edit != null)
            {
                edit.Name = income.Name;
                edit.Price = income.Price;
                try
                {
                    //TODO: Обрабатывать Exception авторизации 
                    await _incomeService.UpdateIncomeAsync(edit);
                }
                catch (Exception _)
                {
                    NavigationManager.NavigateTo("/login");
                }
            }

            TotalIncome = _incomes.Sum(x => x.Price);
        }

        private async Task OnCreateRowIncome(IncomeDto income)
        {
            try
            {
                //TODO: Обрабатывать Exception авторизации 
                income = await _incomeService.CreateIncomeForUserAsync(income, string.Empty);
            }
            catch (Exception _)
            {
                NavigationManager.NavigateTo("/login");
            }
            
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
                try
                {
                    //TODO: Обрабатывать Exception авторизации 
                    await _incomeService.RemoveIncomeAsync(income.Id);
                }
                catch (Exception _)
                {
                    NavigationManager.NavigateTo("/login");
                }
            }
            else
            {
                _incomesGrid.CancelEditRow(income);
            }
        }

        private RadzenGrid<ExpenseDto> _expensesGrid;
        private List<ExpenseDto> _expenses = new List<ExpenseDto>();
        
        private void InsertRowExpense()
        {
            _expensesGrid.InsertRow(new ExpenseDto{ OrderNumber = (_expenses.OrderByDescending(x => x.OrderNumber).FirstOrDefault()?.OrderNumber ?? 0) + 1 });
        }

        private async Task OnUpdateRowExpense(ExpenseDto expense)
        {
            ExpenseDto edit = _expenses.FirstOrDefault(x => x.Id == expense.Id);
            if (edit != null)
            {
                edit.Name = expense.Name;
                edit.Price = expense.Price;
                try
                {
                    //TODO: Обрабатывать Exception авторизации 
                    await _expenseService.UpdateExpenseAsync(edit);
                }
                catch (Exception _)
                {
                    NavigationManager.NavigateTo("/login");
                }
            }

            TotalExpense = _expenses.Sum(x => x.Price);
        }

        private async Task OnCreateRowExpense(ExpenseDto expense)
        {
            try
            {
                //TODO: Обрабатывать Exception авторизации 
                expense = await _expenseService.CreateExpenseForUserAsync(expense, string.Empty);
            }
            catch (Exception _)
            {
                NavigationManager.NavigateTo("/login");
            }
            
            _expenses.Add(expense);
            TotalExpense = _expenses.Sum(x => x.Price);
            await _expensesGrid.Reload();
        }

        private void EditRowExpense(ExpenseDto expense)
        {
            _expensesGrid.EditRow(expense);
        }

        private void SaveRowExpense(ExpenseDto expense)
        {
            _expensesGrid.UpdateRow(expense);
        }

        private void CancelEditExpense(ExpenseDto expense)
        {
            _expensesGrid.CancelEditRow(expense);
        }

        private async Task DeleteRowExpense(ExpenseDto expense)
        {
            if (_expenses.Contains(expense))
            {
                _expenses.Remove(expense);
                TotalExpense = _expenses.Sum(x => x.Price);
                await _expensesGrid.Reload();
                try
                {
                    //TODO: Обрабатывать Exception авторизации 
                    await _expenseService.RemoveExpenseAsync(expense.Id);
                }
                catch (Exception _)
                {
                    NavigationManager.NavigateTo("/login");
                }
            }
            else
            {
                _expensesGrid.CancelEditRow(expense);
            }
        }
    }
}
