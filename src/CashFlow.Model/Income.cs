namespace CashFlow.Model
{
    public class Income : Entity
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public int OrderNumber { get; set; }
        public string UserId { get; set; }
    }
}
