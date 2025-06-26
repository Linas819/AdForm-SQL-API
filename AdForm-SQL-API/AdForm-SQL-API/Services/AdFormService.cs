using AdForm_SQL_API.AdFormDB;
using AdForm_SQL_API.Models;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using static AdForm_SQL_API.Models.Invoice;

namespace AdForm_SQL_API.Services
{
    public class AdFormService
    {
        private AdFormSqlContext _context;
        public AdFormService(AdFormSqlContext context)
        {
            _context = context;
        }
        public Invoice GetOrders(string orderId) {
            Invoice invoice = new Invoice();
            invoice.Products = (from o in _context.Orders
                        join p in _context.Products on o.ProductId equals p.ProductId
                        where o.OrderId == orderId
                        select new OrderProduct
                        {
                            Name = p.ProductName,
                            Category = p.ProductCategory,
                            Quantity = o.Quantity,
                            Price = p.Price*o.Quantity
                        }).ToList();
            // Total price calculation is seperated from the invoice as only one is needed
            invoice.TotalPrice = invoice.Products.Sum(o => o.Price);
            return invoice;
        } 
        public List<OrderDistribution> GetOrderDistributio(string city, bool order)
        {
            var rawData = (from o in _context.Orders
                           join c in _context.Customers on o.CustomerId equals c.CustomerId
                           join p in _context.Products on o.ProductId equals p.ProductId
                           select new
                           {
                               c.Details,
                               o.OrderId,
                               Amount = p.Price * o.Quantity
                           }).AsEnumerable()  // Switch to LINQ to Objects
               .Select(x => new {
                   City = JObject.Parse(x.Details)["city"]?.ToString(),
                   x.OrderId,
                   x.Amount
               });
            // LINQ does not recognise JSON Parse data. Grouping, filtering and ordering is done speratly
            return (from entry in rawData
                          group entry by entry.City into g
                          where city != "" ? g.Key == city : true // Filter by city only if the variable is not an empty string
                          select new OrderDistribution
                          {
                              CustomerCity = g.Key,
                              OrderCount = g.Select(x => x.OrderId).Distinct().Count(),
                              TotalAmount = g.Sum(x => x.Amount)
                          }).OrderByDescending(x => order ? x.OrderCount : 0).ToList(); 
                          // Descending so that first show the cities with the most orders. Only sorted if order bool is set to "true"
        }
    }
}
