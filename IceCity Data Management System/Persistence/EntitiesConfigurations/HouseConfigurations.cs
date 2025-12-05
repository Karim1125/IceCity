namespace IceCity_Data_Management_System.Persistence.EntitiesConfigurations;

public class HouseConfiguration : IEntityTypeConfiguration<House>
{
    public void Configure(EntityTypeBuilder<House> builder)
    {
        builder.ToTable("Houses");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.OwnerName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(h => h.Heaters).IsRequired();
        builder.Property(h => h.HoursPerDay).IsRequired();

        builder.Property(h => h.OutsideTemperature)
            .HasColumnType("decimal(5,2)");

        
        builder
            .HasDiscriminator<string>("HouseType")
            .HasValue<House>("House")
            .HasValue<CatShelter>("CatShelter");

        
        builder
            .HasMany(h => h.SensorReadings)
            .WithOne(r => r.House)
            .HasForeignKey(r => r.HouseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
