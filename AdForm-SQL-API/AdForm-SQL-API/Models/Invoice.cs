namespace AdForm_SQL_API.Models
{
    public class Invoice
    {
        public float TotalPrice { get; set; }
        public List<OrderProduct> Products { get; set; }
        public class OrderProduct
        {
            public string Name { get; set; }
            public string Category { get; set; }
            public int Quantity { get; set; }
            public float Price { get; set; }
        }
    }
}
