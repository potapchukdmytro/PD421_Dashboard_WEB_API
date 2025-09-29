using AutoMapper;
using PD421_Dashboard_WEB_API.BLL.Dtos.Game;
using PD421_Dashboard_WEB_API.BLL.Services.Storage;
using PD421_Dashboard_WEB_API.DAL.Entitites;
using PD421_Dashboard_WEB_API.DAL.Repositories.Game;

namespace PD421_Dashboard_WEB_API.BLL.Services.Game
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;

        public GameService(IGameRepository gameRepository, IMapper mapper, IStorageService storageService)
        {
            _gameRepository = gameRepository;
            _storageService = storageService;
            _mapper = mapper;
        }


        public async Task<ServiceResponse> CreateAsync(CreateGameDto dto, string imagesPath)
        {
            var entity = _mapper.Map<GameEntity>(dto);

            string gamePath = Path.Combine(imagesPath, entity.Id);
            Directory.CreateDirectory(gamePath);

            // Save main image
            var mainImageName = await _storageService.SaveImageAsync(dto.MainImage, gamePath);

            if(mainImageName == null)
            {
                return new ServiceResponse
                {
                    IsSuccess = false,
                    HttpStatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Message = "Помилка під час збереження головної картинки"
                };

            }

            var mainImage = new GameImageEntity
            {
                GameId = entity.Id,
                ImagePath = Path.Combine(entity.Id, mainImageName),
                IsMain = true
            };

            entity.Images.Add(mainImage);

            // Save images
            var imageNames = await _storageService.SaveImagesAsync(dto.Images, gamePath);

            if (imageNames.Count() > 0)
            {
                var images = imageNames.Select(name => new GameImageEntity
                {
                    GameId = entity.Id,
                    IsMain = false,
                    ImagePath = Path.Combine(entity.Id, name)
                });

                foreach (var i in images)
                {
                    entity.Images.Add(i);
                }
            }

            await _gameRepository.CreateAsync(entity);

            return new ServiceResponse
            {
                Message = $"Гру '{entity.Name}' успішно додано"
            };
        }
    }
}
