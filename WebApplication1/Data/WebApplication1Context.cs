using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class WebApplication1Context(DbContextOptions<WebApplication1Context> options) : DbContext(options)
    {
        
        // Define DbSets for your entities (tables in the database)
        public DbSet<Item> Items { get; set; }  // Example entity
    }
}
