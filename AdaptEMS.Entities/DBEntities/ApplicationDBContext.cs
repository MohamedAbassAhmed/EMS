using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AdaptEMS.Entities.DBEntities
{
    public class ApplicationDBContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext()
        {
            Database.EnsureCreated();

        }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=.;Database=EMS;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");
            base.OnConfiguring(optionsBuilder);

        }
      
        public DbSet<EMSLog> EMSLogs { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<LeaveOrder> LeaveOrders { get; set; }
    }
}
