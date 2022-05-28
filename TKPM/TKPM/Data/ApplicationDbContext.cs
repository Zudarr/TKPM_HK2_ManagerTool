using Microsoft.EntityFrameworkCore;
using TKPM.Models;
namespace TKPM.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
        }
        public DbSet<DaiLy> DaiLys { get; set; }
    }
}
