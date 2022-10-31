using industriation_crm.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace industriation_crm.Server.Models
{
    public partial class DatabaseContext : DbContext
    {

        public DbSet<user> user { get; set; }
        public DbSet<roles> roles { get; set; }
        public DbSet<client> client { get; set; }
        public DbSet<product> product { get; set; }
        public DbSet<order> order { get; set; }
        public DbSet<product_to_order> product_to_order { get; set; }
        public DbSet<supplier> supplier { get; set; }
        public DbSet<supplier_order> supplier_order { get; set; }
        public DbSet<contact> contact { get; set; }
        public DbSet<delivery> delivery { get; set; }
        public DbSet<delivery_type> delivery_type { get; set; }
        public DbSet<order_pay> order_pay { get; set; }
        public DbSet<pay_status> pay_status { get; set; }
        public DbSet<delivery_period_type> delivery_period_type { get; set; }
        public DbSet<contact_phone> contact_phone { get; set; }
        public DatabaseContext()
        {
        }
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             
        }
    }
}
