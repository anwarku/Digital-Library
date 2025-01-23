using AutoMapper;

namespace Backend.Mapper
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(config =>
            {
                config.AddProfile(new EfToDtoMappingProfile());
                config.AddProfile(new DtoToEfMappingProfile());
            });
        }
    }
}
