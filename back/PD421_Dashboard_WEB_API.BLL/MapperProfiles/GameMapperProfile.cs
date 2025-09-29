using AutoMapper;
using PD421_Dashboard_WEB_API.BLL.Dtos.Game;
using PD421_Dashboard_WEB_API.DAL.Entitites;

namespace PD421_Dashboard_WEB_API.BLL.MapperProfiles
{
    public class GameMapperProfile : Profile
    {
        public GameMapperProfile()
        {
            // CreateGameDto -> GameEntity
            CreateMap<CreateGameDto, GameEntity>()
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Genres, opt => opt.Ignore());
        }
    }
}
