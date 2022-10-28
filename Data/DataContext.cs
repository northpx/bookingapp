using API1.DatingApp.API.Data.Entities;
using Microsoft.EntityFrameworkCore;
 
namespace API1.DatingApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
       
        public DbSet<User> AppUsers { get; set; }
    }
}
