namespace AdForm_SQL_API.Models
{
    public class OrderDistributionResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Data recieved";
        public List<OrderDistribution>? OrderDistributions { get; set; }
        public class OrderDistribution
        {
            public string CustomerCity { get; set; } = "";
            public int OrderCount { get; set; }
            public float TotalAmount { get; set; }
        }
    }
}
