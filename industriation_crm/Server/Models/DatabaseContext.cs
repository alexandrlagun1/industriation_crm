using industriation_crm.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace industriation_crm.Server.Models
{
    public partial class DatabaseContext : DbContext
    {
        public DbSet<progress_client_discount> progress_client_discount { get; set; }
        public DbSet<pay_status> pay_status { get; set; }
        public DbSet<category> category { get; set; }
        public DbSet<user> user { get; set; }
        public DbSet<roles> roles { get; set; }
        public DbSet<order_status> order_status { get; set; }
        public DbSet<client> client { get; set; }
        public DbSet<product> product { get; set; }
        public DbSet<order> order { get; set; }
        public DbSet<product_to_order> product_to_order { get; set; }
        public DbSet<supplier_order> supplier_order { get; set; }
        public DbSet<order_check> order_check { get; set; }
        public DbSet<contact> contact { get; set; }
        public DbSet<delivery> delivery { get; set; }
        public DbSet<delivery_type> delivery_type { get; set; }
        public DbSet<order_pay> order_pay { get; set; }
        public DbSet<task> task { get; set; }
        public DbSet<delivery_period_type> delivery_period_type { get; set; }
        public DbSet<contact_phone> contact_phone { get; set; }
        public DbSet<order_history> order_history { get; set; }
        public DbSet<call_history> call_history { get; set; }
        public DbSet<stage> stage { get; set; }
        public DbSet<supplier_order_status> supplier_order_status { get; set; }
        public DbSet<user_notifications> user_notifications { get; set; }
        public DatabaseContext()
        {
        }
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("Server=91.109.201.60; Port=3306; Database=industriation_crm; Uid=alexandr; Pwd=P@ssw0rd; charset=utf8mb4;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             
        }
    }
}
