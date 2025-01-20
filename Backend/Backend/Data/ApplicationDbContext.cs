using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options): base(options)
        {
            
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<DetailTransaction> DetailTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<DetailTransaction>()
            //    .HasOne(dt => dt.Transaction)
            //    .WithMany(d => d.DetailTransactions)
            //    .HasForeignKey(dt => dt.TransactionId);

            //modelBuilder.Entity<DetailTransaction>()
            //    .HasOne(dt => dt.Book)
            //    .WithMany(b => b.DetailTransactions)
            //    .HasForeignKey(dt => dt.BookCode);
        }
    }
}
