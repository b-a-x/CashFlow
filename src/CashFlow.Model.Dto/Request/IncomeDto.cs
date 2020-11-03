using System;
using System.Collections.Generic;
using System.Text;

namespace CashFlow.Model.Dto.Request
{
    public class IncomeDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int OrderNumber { get; set; }
    }
}
