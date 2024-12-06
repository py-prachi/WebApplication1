using Microsoft.AspNetCore.Mvc;
using WebApplication1.Controllers;
using WebApplication1.Models;
using WebApplication1.Service;
using Moq;

namespace WebApplication1.Tests.Controllers
{
    [TestFixture]
    [TestOf(typeof(OrdersController))]
    public class OrdersControllerTest
    {
        private OrdersController _controller;
        private Mock<IOrderService> _mockOrderService;

        [SetUp]
        public void SetUp()
        {
            // Initialize Order Service mock
            _mockOrderService = new Mock<IOrderService>();
            
            // Initialize the controller with the mock service
            _controller = new OrdersController(_mockOrderService.Object);
        }

        [TearDown]
        public void TearDown()
        {
            (_controller as IDisposable)?.Dispose();
        }
        
        [Test]
        public async Task GetOrders_ReturnsViewResult_WithListOfOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { Id = 1, CustomerName = "Customer1", OrderDate = DateTime.Now, TotalAmount = 100.0 },
                new Order { Id = 2, CustomerName = "Customer2", OrderDate = DateTime.Now, TotalAmount = 200.0 }
            };
            _mockOrderService.Setup(service => service.GetOrdersAsync()).ReturnsAsync(orders);

            // Act
            var result = await _controller.GetOrders();

            // Assert the type of the result
            Assert.IsInstanceOf<ViewResult>(result);

            // Safely cast the result to ViewResult
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);

            // Assert: Check if the model is a list of orders and has the correct count
            Assert.IsAssignableFrom<List<Order>>(viewResult!.Model);
            var model = viewResult.Model as List<Order>;
            Assert.That(model?.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task CreateOrder_ValidModel_AddsOrderAndRedirectsToGetOrders()
        {
            // Arrange
            var newOrder = new Order { Id = 3, CustomerName = "NewCustomer", OrderDate = DateTime.Now, TotalAmount = 300.0 };
            _mockOrderService.Setup(service => service.AddOrderAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateOrder(newOrder);

            // Assert the type of the result
            Assert.IsInstanceOf<RedirectToActionResult>(result);

            // Safely cast the result to RedirectToActionResult
            var redirectResult = result as RedirectToActionResult;
            Assert.NotNull(redirectResult);

            // Assert: Check if the user is redirected to GetOrders
            Assert.That(redirectResult.ActionName, Is.EqualTo("GetOrders"));
        }

        [Test]
        public async Task CreateOrder_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange
            var invalidOrder = new Order { Id = 0, CustomerName = "", OrderDate = DateTime.Now, TotalAmount = -10.0 };
            _controller.ModelState.AddModelError("CustomerName", "The CustomerName field is required.");

            // Act
            var result = await _controller.CreateOrder(invalidOrder);

            // Assert the type of the result
            Assert.IsInstanceOf<ViewResult>(result);

            // Safely cast the result to ViewResult
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);

           }

        [Test]
        public void CreateOrder_ReturnsViewResult_WhenCalledWithoutModel()
        {
            // Act: Call the CreateOrder method without any model
            var result = _controller.CreateOrder();

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
