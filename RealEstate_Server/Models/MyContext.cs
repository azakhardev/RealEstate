using Microsoft.EntityFrameworkCore;

namespace RealEstate_Server.Models
{
    public class MyContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Insertion> Insertions { get; set; }

        public DbSet<Request> Requests { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<Property> Properties { get; set; }

        public DbSet<PropertyInsertion> PropertiesInsertions { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<CustomerInsertion> CustomersInsertions { get; set; }

        public DbSet<Chat> Chats { get; set; }

        public DbSet<Message> Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=mysqlstudenti.litv.sssvt.cz;database=4b2_zakharchenkoartem_db2;user=zakharchenkoarte;password=123456;SslMode=none");
        }
    }
}