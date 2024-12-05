using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Tests.Controllers
{
    [TestFixture]
    [TestOf(typeof(ItemsController))]
    public class ItemsControllerTest
    {
        private WebApplication1Context _context;
        private ItemsController _controller;

        [SetUp]
        public void SetUp()
        {
            // Setup In-Memory Database
            var options = new DbContextOptionsBuilder<WebApplication1Context>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new WebApplication1Context(options);
            
            // Ensure Items DbSet is initialized
            _context.Items = _context.Set<Item>(); 

            // Seed the in-memory database
            _context.Items.AddRange(new List<Item>
            {
                new Item { Id = 1, Name = "Item1", Price = 10.0 },
                new Item { Id = 2, Name = "Item2", Price = 20.0 }
            });
            _context.SaveChanges();

            // Initialize the controller
            _controller = new ItemsController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            // Dispose resources
            _context.Dispose();

            // Dispose of controller if necessary (this is rare but added to handle the analyzer warning)
            (_controller as IDisposable)?.Dispose();
        }
        
        [Test]
        public async Task Index_ReturnsViewResult_WithListOfItems()
        {
            // Act: Call the Index method
            var result = await _controller.Index();

            // Assert the type of the result
            Assert.IsInstanceOf<ViewResult>(result);

            // Safely cast the result to ViewResult
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);

            // Assert: Check if the model is a list of items and has the correct count
            Assert.IsAssignableFrom<List<Item>>(viewResult!.Model);
            var model = viewResult.Model as List<Item>;
            Assert.That(model?.Count, Is.EqualTo(2));
        }
    }
}
