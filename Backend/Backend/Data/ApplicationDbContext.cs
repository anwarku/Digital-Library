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
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            //// Relasi One to Many, Transaction to Detail Transactions
            //modelBuilder.Entity<DetailTransaction>()
            //    .HasOne(dt => dt.Transaction)
            //    .WithMany(d => d.DetailTransactions)
            //    .HasForeignKey(dt => dt.TransactionId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //// Relasi One to Many, Book to Detail Transactions
            //modelBuilder.Entity<DetailTransaction>()
            //    .HasOne(dt => dt.Book)
            //    .WithMany(b => b.DetailTransactions)
            //    .HasForeignKey(dt => dt.BookCode)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
