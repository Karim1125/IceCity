namespace IceCity_Data_Management_System.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // House
            CreateMap<House, HouseDto>().ReverseMap();
            CreateMap<CreateHouseDto, House>();
            CreateMap<UpdateHouseDto, House>();
            CreateMap<CatShelter, HouseDto>();

            // Cat Shelter
            CreateMap<CatShelter, CatShelterDto>().ReverseMap();
            CreateMap<CreateCatShelterDto, CatShelter>();

            // Sensor Reading
            CreateMap<SensorReading, SensorReadingDto>().ReverseMap();
            CreateMap<CreateSensorReadingDto, SensorReading>();
        }
    }
}
