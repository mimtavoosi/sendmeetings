using Microsoft.EntityFrameworkCore;
using SendReceiptsDemo.Models;

namespace SendReceiptsDemo.Data
{
    public class SendReceiptContext:DbContext
    {
        public SendReceiptContext(DbContextOptions<SendReceiptContext> options) : base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Right> Rights { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Token> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) //run just once
        {

            base.OnModelCreating(modelBuilder);

            //Next Step: Call Add-Migration and Update-Database tools for update database
        }
    }
}
