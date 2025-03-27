using Microsoft.EntityFrameworkCore;
using Presyotect.Features.Configuration.Models;
using Presyotect.Features.Establishments.Models;
using Presyotect.Features.Monitoring.Models;
using Presyotect.Features.Products.Models;
using Presyotect.Features.Users.Models;

namespace Presyotect.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<Establishment> Establishments { get; set; }
    public DbSet<MonitoredPrice> MonitoredPrices { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Personnel> Personnel { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Classification> Classifications { get; set; }
    public DbSet<MonitoringSchedule> MonitoringSchedules { get; set; }
}