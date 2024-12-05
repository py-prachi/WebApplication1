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
            // // Setup In-Memory Database
            // var options = new DbContextOptionsBuilder<WebApplication1Context>()
            //     .UseInMemoryDatabase(databaseName: "TestDatabase")
            //     .Options;
            //
            
            // Setup a fresh in-memory database for every test
            var options = new DbContextOptionsBuilder<WebApplication1Context>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a unique database name
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
        
        [Test]
        public async Task Create_ValidModel_AddsItemAndRedirectsToIndex()
        {
            // Arrange
            var newItem = new Item { Id = 3, Name = "NewItem", Price = 30.0 };

            // Act
            var result = await _controller.Create(newItem);

            // Assert the type of the result
            Assert.IsInstanceOf<RedirectToActionResult>(result);

            // Safely cast the result to RedirectToActionResult
            var redirectResult = result as RedirectToActionResult;
            Assert.NotNull(redirectResult);

            // Assert: Check if the user is redirected to Index
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));

            // Assert: Check if the item was added to the database
            var items = _context.Items.ToList();
            Assert.That(items.Count, Is.EqualTo(3)); // Initial 2 + New item
            Assert.That(items.Any(i => i.Name == "NewItem" && i.Price == 30.0));
        }

        [Test]
        public async Task Create_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange
            var invalidItem = new Item { Id = 0, Name = "", Price = -10.0 }; // Invalid model
            _controller.ModelState.AddModelError("Name", "The Name field is required.");

            // Act
            var result = await _controller.Create(invalidItem);

            // Assert the type of the result
            Assert.IsInstanceOf<ViewResult>(result);

            // Safely cast the result to ViewResult
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);

            // Assert: Check if the returned model is the same as the one passed in
            Assert.That(viewResult.Model, Is.SameAs(invalidItem));

            // Assert: Ensure the item is not added to the database
            var items = _context.Items.ToList();
            Assert.That(items.Count, Is.EqualTo(2)); // Only initial 2 items
        }
        
        // Test for Create() returning a View when no model is provided
        [Test]
        public void Create_ReturnsViewResult()
        {
            // Act: Call the Create method without any model
            var result = _controller.Create();

            // Assert the type of the result
            Assert.IsInstanceOf<ViewResult>(result);

            // Safely cast the result to ViewResult
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);

            // Optionally: Check if the returned view has the correct name (if set)
            Assert.Null(viewResult.ViewName); // Assumes no specific view name is set
        }
    }
}
