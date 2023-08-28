using Microsoft.EntityFrameworkCore;
using MQTTManager.DB.Model;

namespace MQTTManager.DB
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<BrokerConfigurationModel> BrokerConfig { get; set; }
        public DbSet<MessageLogModel> MessageLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BrokerConfigurationModel>(entity =>
            {
                entity.HasKey(e => e.Id); // Ustala Id jako klucz główny

                entity.Property(e => e.Name) // Konfiguracja właściwości Name
                    .IsRequired() // Wymaga, żeby pole Name było zawsze uzupełnione
                    .HasMaxLength(100); // Ustala maksymalną długość na 100 znaków

                // Konfigurujemy pole Port jako pole wymagane
                entity.Property(e => e.Port)
                    .IsRequired();

                // Konfigurujemy pole Authorization jako pole wymagane i mapujemy na typ wyliczeniowy
                entity.Property(e => e.Authorization)
                    .IsRequired()
                    .HasConversion<string>(); // Konwersja typu wyliczeniowego na string

            });

            modelBuilder.Entity<MessageLogModel>(entity =>
            {
                // Ustalamy Id jako klucz główny
                entity.HasKey(e => e.Id);

                // Konfigurujemy pole Topic jako pole wymagane
                entity.Property(e => e.Topic)
                    .IsRequired()
                    .HasMaxLength(255); // Zakładam, że chcesz ograniczyć długość tematu do 255 znaków

                // Konfigurujemy pole Payload jako pole wymagane
                entity.Property(e => e.Payload)
                    .IsRequired();

                // Konfigurujemy pole Time jako pole wymagane
                entity.Property(e => e.Time)
                    .IsRequired();
            });
        }
    }
}