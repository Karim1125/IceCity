namespace IceCity_Data_Management_System.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<House> Houses { get; set; }
        public DbSet<CatShelter> CatShelters { get; set; }
        public DbSet<SensorReading> SensorReadings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<House>()
        .HasDiscriminator<string>("HouseType")
        .HasValue<House>("House")
        .HasValue<CatShelter>("CatShelter");



            base.OnModelCreating(modelBuilder);
        }
    }
}
