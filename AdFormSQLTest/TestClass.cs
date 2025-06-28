using AdForm_SQL_API.AdFormDB;
using AdForm_SQL_API.Controllers;
using AdForm_SQL_API.Models;
using AdForm_SQL_API.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AdFormSQLTest
{
    public class TestClass
    {
        List<Order> orders = new List<Order>
        {
            new Order { OrderId = "ord1", CustomerId = 1, ProductId = 1, Quantity = 2 },
            new Order { OrderId = "ord2", CustomerId = 2, ProductId = 2, Quantity = 3 },
            new Order { OrderId = "ord3", CustomerId = 2, ProductId = 1, Quantity = 4 }
        };
        List<Product> products = new List<Product>
        {
            new Product { ProductId = 1, ProductName = "Awesome Plastic Shirt", ProductCategory = "Tools", Price = 10 },
            new Product { ProductId = 2, ProductName = "Cake", ProductCategory = "Food", Price = 5.12f }
        };
        List<Customer> customers = new List<Customer>
        {
            new Customer { CustomerId = 1, FirstName = "Sonya", LastName = "Tillman", Email = "S.T@gmail.com", Details = "{\"country\": \"Tunisia\", \"city\": \"East Belleland\"}" },
            new Customer { CustomerId = 2, FirstName = "Alan", LastName = "Zeke", Email = "A.Z@gmail.com", Details = "{\"country\": \"US\", \"city\": \"New York\"}" }
        };
        public static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return mockSet;
        }
        [Fact]
        public void GetOrder()
        {
            var mockOrderSet = CreateMockDbSet(orders);
            var mockProductSet = CreateMockDbSet(products);

            var mockContext = new Mock<AdFormSqlContext>();
            mockContext.Setup(c => c.Orders).Returns(mockOrderSet.Object);
            mockContext.Setup(c => c.Products).Returns(mockProductSet.Object);

            AdFormService service = new AdFormService(mockContext.Object);

            InvoiceResponse result = service.GetOrders("ord1");

            Assert.True(result.Success);
        }
        [Fact]
        public void GetOrderProductName()
        {
            var mockOrderSet = CreateMockDbSet(orders);
            var mockProductSet = CreateMockDbSet(products);

            var mockContext = new Mock<AdFormSqlContext>();
            mockContext.Setup(c => c.Orders).Returns(mockOrderSet.Object);
            mockContext.Setup(c => c.Products).Returns(mockProductSet.Object);

            AdFormService service = new AdFormService(mockContext.Object);

            InvoiceResponse result = service.GetOrders("ord1");

            Assert.Equal("Awesome Plastic Shirt", result.Products[0].Name);
        }
        [Fact]
        public void GetOrderTotalPrice()
        {
            var mockOrderSet = CreateMockDbSet(orders);
            var mockProductSet = CreateMockDbSet(products);

            var mockContext = new Mock<AdFormSqlContext>();
            mockContext.Setup(c => c.Orders).Returns(mockOrderSet.Object);
            mockContext.Setup(c => c.Products).Returns(mockProductSet.Object);

            AdFormService service = new AdFormService(mockContext.Object);

            InvoiceResponse result = service.GetOrders("ord1");

            Assert.Equal(20, result.TotalPrice);
        }
        [Fact]
        public void GetOrderNotFound()
        {
            var mockOrderSet = CreateMockDbSet(orders);
            var mockProductSet = CreateMockDbSet(products);

            var mockContext = new Mock<AdFormSqlContext>();
            mockContext.Setup(c => c.Orders).Returns(mockOrderSet.Object);
            mockContext.Setup(c => c.Products).Returns(mockProductSet.Object);

            AdFormService service = new AdFormService(mockContext.Object);

            InvoiceResponse result = service.GetOrders("ord");

            Assert.Equal("Order not found", result.Message);
        }
        [Fact]
        public void GetOrderDistribution()
        {
            var mockOrderSet = CreateMockDbSet(orders);
            var mockProductSet = CreateMockDbSet(products);
            var mockCustomerSet = CreateMockDbSet(customers);

            var mockContext = new Mock<AdFormSqlContext>();
            mockContext.Setup(c => c.Orders).Returns(mockOrderSet.Object);
            mockContext.Setup(c => c.Products).Returns(mockProductSet.Object);
            mockContext.Setup(c => c.Customers).Returns(mockCustomerSet.Object);

            AdFormService service = new AdFormService(mockContext.Object);

            OrderDistributionResponse result = service.GetOrderDistribution("", false);

            Assert.Equal(2, result.OrderDistributions.Count());
        }
        [Fact]
        public void GetOrderDistributionOrdered()
        {
            var mockOrderSet = CreateMockDbSet(orders);
            var mockProductSet = CreateMockDbSet(products);
            var mockCustomerSet = CreateMockDbSet(customers);

            var mockContext = new Mock<AdFormSqlContext>();
            mockContext.Setup(c => c.Orders).Returns(mockOrderSet.Object);
            mockContext.Setup(c => c.Products).Returns(mockProductSet.Object);
            mockContext.Setup(c => c.Customers).Returns(mockCustomerSet.Object);

            AdFormService service = new AdFormService(mockContext.Object);

            OrderDistributionResponse result = service.GetOrderDistribution("", true);

            Assert.True(result.OrderDistributions[0].OrderCount > result.OrderDistributions[1].OrderCount);
        }
        [Fact]
        public void GetOrderDistributionByCity()
        {
            var mockOrderSet = CreateMockDbSet(orders);
            var mockProductSet = CreateMockDbSet(products);
            var mockCustomerSet = CreateMockDbSet(customers);

            var mockContext = new Mock<AdFormSqlContext>();
            mockContext.Setup(c => c.Orders).Returns(mockOrderSet.Object);
            mockContext.Setup(c => c.Products).Returns(mockProductSet.Object);
            mockContext.Setup(c => c.Customers).Returns(mockCustomerSet.Object);

            AdFormService service = new AdFormService(mockContext.Object);

            OrderDistributionResponse result = service.GetOrderDistribution("New York", false);

            Assert.Equal(2, result.OrderDistributions[0].OrderCount);
        }
    }
}