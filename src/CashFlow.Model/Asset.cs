namespace CashFlow.Model
{
    public class Asset : Entity
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public int OrderNumber { get; set; }
        public string UserId { get; set; }
    }
}
