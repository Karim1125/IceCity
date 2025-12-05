namespace IceCity_Data_Management_System.Persistence.EntitiesConfigurations;

public class CatShelterConfiguration : IEntityTypeConfiguration<CatShelter>
{
    public void Configure(EntityTypeBuilder<CatShelter> builder)
    {
        builder.Property(c => c.CatName)
            .IsRequired()
            .HasMaxLength(50);
    }
}
