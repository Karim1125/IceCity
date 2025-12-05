namespace IceCity_Data_Management_System.Persistence.EntitiesConfigurations;

public class SensorReadingConfiguration : IEntityTypeConfiguration<SensorReading>
{
    public void Configure(EntityTypeBuilder<SensorReading> builder)
    {
        builder.ToTable("SensorReadings");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Temperature)
            .HasColumnType("decimal(5,2)");

        builder.Property(s => s.Timestamp)
            .IsRequired();

        builder
            .HasOne(s => s.House)
            .WithMany(h => h.SensorReadings)
            .HasForeignKey(s => s.HouseId);
    }
}
