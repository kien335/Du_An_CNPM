using Microsoft.EntityFrameworkCore;
using Dự_Án_CNPM.Models;

namespace Dự_Án_CNPM.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Card> Cards { get; set; } = null!;
    public DbSet<ParkingSlot> ParkingSlots { get; set; } = null!;
    public DbSet<PricingRule> PricingRules { get; set; } = null!;
    public DbSet<ParkingSession> ParkingSessions { get; set; } = null!;
    public DbSet<Subscription> Subscriptions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed Admin user
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 1,
            Username = "admin",
            PasswordHash = "admin123", // Trong thực tế nên hash
            Role = "Admin",
            CreatedAt = new DateTime(2026, 3, 28)
        });

        // Seed Parking Slots (A01-A10, B01-B10)
        var slots = new List<ParkingSlot>();
        for (int i = 1; i <= 10; i++) 
        {
            slots.Add(new ParkingSlot { SlotId = $"A{i:D2}", Status = "Empty" });
            slots.Add(new ParkingSlot { SlotId = $"B{i:D2}", Status = "Empty" });
        }
        modelBuilder.Entity<ParkingSlot>().HasData(slots);

        // Seed some RFID Cards
        var cards = new List<Card>();
        for (int i = 1; i <= 10; i++)
        {
            cards.Add(new Card { CardId = $"C{i:D2}", Status = "Available", Note = "Thẻ thử nghiệm" });
        }
        modelBuilder.Entity<Card>().HasData(cards);
    }
}
