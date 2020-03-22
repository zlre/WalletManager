namespace WalletManager.Data
{
    using Microsoft.EntityFrameworkCore;

    public class WalletManagerDBContext : DbContext
    {
        public WalletManagerDBContext(DbContextOptions<WalletManagerDBContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<WalletCurrency> WalletCurrencies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Id)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Name)
                .IsUnique();

            modelBuilder.Entity<Wallet>()
                .HasKey(w => w.Id);

            modelBuilder.Entity<Wallet>()
                .HasIndex(w => w.Id)
                .IsUnique();

            modelBuilder.Entity<Wallet>()
                .Property(w => w.Id)
                .IsRequired();


            modelBuilder.Entity<User>()
                .HasMany(u => u.Wallets)
                .WithOne(w => w.User);

            modelBuilder.Entity<Wallet>()
                .HasOne(w => w.User)
                .WithMany(u => u.Wallets);

            modelBuilder.Entity<Currency>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Currency>()
                .HasIndex(c => c.Id)
                .IsUnique();

            modelBuilder.Entity<Currency>()
                .Property(c => c.Id)
                .IsRequired();

            modelBuilder.Entity<Currency>()
                .HasIndex(c => c.Name)
                .IsUnique();


            modelBuilder.Entity<WalletCurrency>()
                .HasKey(wc => new { wc.WalletId, wc.CurrencyId });

            modelBuilder.Entity<WalletCurrency>()
                .HasOne(wc => wc.Wallet)
                .WithMany(wc => wc.WalletCurrencies)
                .HasForeignKey(wc => wc.WalletId);

            modelBuilder.Entity<WalletCurrency>()
                .HasOne(wc => wc.Currency)
                .WithMany(wc => wc.WalletCurrencies)
                .HasForeignKey(wc => wc.CurrencyId);

        }
    }
}
