using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Service;


namespace WebApplication1.Tests.Service
{
    [TestFixture]
    public class OrderServiceTest
    {
        private WebApplication1Context _context;
        private OrderService _service;

        [SetUp]
        public void SetUp()
        {
            // Setup In-Memory Database for the Service
            var options = new DbContextOptionsBuilder<WebApplication1Context>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a unique database name
                .Options;

            _context = new WebApplication1Context(options);
            _service = new OrderService(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetOrdersAsync_ReturnsListOfOrders()
        {
            // Arrange
            var order = new Order { Id = 1, CustomerName = "Customer1", OrderDate = DateTime.Now, TotalAmount = 100.0 };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetOrdersAsync();

            // Assert: Check if the list contains the order
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].CustomerName, Is.EqualTo("Customer1"));
        }

        [Test]
        public async Task AddOrderAsync_AddsOrderToDatabase()
        {
            // Arrange
            var newOrder = new Order { Id = 2, CustomerName = "NewCustomer", OrderDate = DateTime.Now, TotalAmount = 150.0 };

            // Act
            await _service.AddOrderAsync(newOrder);

            // Assert: Check if the order was added to the database
            var order = await _context.Orders.FindAsync(2);
            Assert.NotNull(order);
            Assert.That(order.CustomerName, Is.EqualTo("NewCustomer"));
        }
    }
}
