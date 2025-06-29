﻿using AdForm_SQL_API.AdFormDB;
using AdForm_SQL_API.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using static AdForm_SQL_API.Models.InvoiceResponse;
using static AdForm_SQL_API.Models.OrderDistributionResponse;

namespace AdForm_SQL_API.Services
{
    public class AdFormService
    {
        private AdFormSqlContext _context;
        public AdFormService(AdFormSqlContext context)
        {
            _context = context; // Uses Entity Framework to avoid string queries
        }
        public InvoiceResponse GetOrders(string orderId) {
            InvoiceResponse invoice = new InvoiceResponse();
            invoice.Products = (from o in _context.Orders
                        join p in _context.Products on o.ProductId equals p.ProductId
                        where o.OrderId == orderId
                        select new InvoiceResponseOrderProduct
                        {
                            Name = p.ProductName ?? "",
                            Category = p.ProductCategory ?? "",
                            Quantity = o.Quantity,
                            Price = p.Price*o.Quantity
                        }).ToList();
            if (invoice.Products.Count() == 0)
            {
                // If no order has been found, appropriate message is issued to inform the user on the front end
                invoice.Success = false;
                invoice.Message = "Order: " + orderId + " not found";
                return invoice;
            }
            // Total price calculation is seperated from the invoice as only one is needed. And only after it is confirmed, that an order was found
            invoice.TotalPrice = invoice.Products.Sum(o => o.Price);
            return invoice;
        } 
        public OrderDistributionResponse GetOrderDistribution(string city, bool order)
        {
            OrderDistributionResponse response = new OrderDistributionResponse();
            var data = (from o in _context.Orders
                           join c in _context.Customers on o.CustomerId equals c.CustomerId
                           join p in _context.Products on o.ProductId equals p.ProductId
                           select new
                           {
                               c.Details,
                               o.OrderId,
                               Amount = p.Price * o.Quantity
                           }).AsEnumerable()  // Switch to LINQ-to-Object
               .Select(x => new {
                   City = JObject.Parse(x.Details)["city"]?.ToString(),
                   x.OrderId,
                   x.Amount
               }).ToList();
            // LINQ does not recognise JSON Parse data. Grouping, filtering and ordering is done speratly
            response.OrderDistributions = (from entry in data
                          group entry by entry.City into g
                          where city != "" ? g.Key == city : true // Filter by city only if the variable is not an empty string
                          select new OrderDistribution
                          {
                              CustomerCity = g.Key,
                              OrderCount = g.Select(x => x.OrderId).Distinct().Count(),
                              TotalAmount = g.Sum(x => x.Amount)
                          }).OrderByDescending(x => order ? x.OrderCount : 0).ToList();
                          // Descending so that the list first shows the cities with the most orders. Only sorted if variable boolean order is set to "true"
            // Checking, if the city was provided, but not found. Appropriate message to inform the front end user.
            if (response.OrderDistributions.Count() == 0 && city != "")
            {
                response.Success = false;
                response.Message = "City: " + city + " not found";
            }
            return response;
        }
    }
}
