namespace AdForm_SQL_API.Models
{
    public class InvoiceResponse
    {
        public bool Success { get; set; } = true;
        public float TotalPrice { get; set; }
        public List<InvoiceResponseOrderProduct>? Products { get; set; }
        public string Message { get; set; } = "Data recieved";
    public class InvoiceResponseOrderProduct
        {
            public string Name { get; set; } = "";
            public string Category { get; set; } = "";
            public int Quantity { get; set; }
            public float Price { get; set; }
        }
    }
}
