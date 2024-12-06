

using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Service;

namespace WebApplication1.Controllers
{
    public class OrdersController :  Controller
    {
        private readonly IOrderService _orderService;
        
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        
        
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetOrdersAsync();
            return View(orders);
        }
   
        
        public void Dispose()
        {
         Console.WriteLine("Controller disposed");
            
        }
        
        public IActionResult CreateOrder()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateOrder([Bind("Id, CustomerName, OrderDate, TotalAmount")] Order order)
        {
            if (ModelState.IsValid)
            {   
                Console.WriteLine($"Creating order for {order.CustomerName}");
                await _orderService.AddOrderAsync(order);
                return RedirectToAction("GetOrders");
            }
            Console.WriteLine("model state invalid");
            return View(order);
        }
        

        // public async Task<IActionResult> Edit(int id)
        // {
        //     var item = await _context.Items.FirstOrDefaultAsync(x=>x.Id == id);
        //     return View(item);
        // }
        // [HttpPost]
        // public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Price")] Item item)
        // {
        //     if(ModelState.IsValid)
        //     {
        //         _context.Update(item);
        //         await _context.SaveChangesAsync();
        //         return RedirectToAction("Index");
        //     
        //     }
        //     return View(item);
        // }

        // public async Task<IActionResult> Delete(int id)
        // {
        //     var item = await _context.Items.FirstOrDefaultAsync(x=>x.Id == id);
        //     return View(item);
        // }
        // [HttpPost, ActionName("Delete")]
        // public async Task<IActionResult> DeleteConfirmed(int id)
        // {
        //     var item = await _context.Items.FindAsync(id);
        //     if(item !=null)
        //     {
        //         _context.Items.Remove(item);
        //         await _context.SaveChangesAsync();
        //     }
        //     return RedirectToAction("Index");
        // }
        
    }
}