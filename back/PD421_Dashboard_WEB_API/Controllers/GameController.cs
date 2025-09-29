using Microsoft.AspNetCore.Mvc;
using PD421_Dashboard_WEB_API.BLL.Dtos.Game;
using PD421_Dashboard_WEB_API.BLL.Services.Game;
using PD421_Dashboard_WEB_API.Extensions;
using PD421_Dashboard_WEB_API.Settings;

namespace PD421_Dashboard_WEB_API.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IWebHostEnvironment _environment;

        public GameController(IGameService gameService, IWebHostEnvironment environment)
        {
            _gameService = gameService;
            _environment = environment;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CreateGameDto dto)
        {
            string rootPath = _environment.ContentRootPath;
            string imagesPath = Path.Combine(rootPath, StaticFilesSettings.StorageDirectory, StaticFilesSettings.ImagesDirectory);
            var response = await _gameService.CreateAsync(dto, imagesPath);
            return this.ToActionResult(response);
        }
    }
}
