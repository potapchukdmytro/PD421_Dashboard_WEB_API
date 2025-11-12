using Microsoft.AspNetCore.Mvc;
using PD421_Dashboard_WEB_API.BLL.Dtos.Genre;
using PD421_Dashboard_WEB_API.BLL.Services;
using PD421_Dashboard_WEB_API.BLL.Services.Genre;
using PD421_Dashboard_WEB_API.Extensions;
using System.Net;

namespace PD421_Dashboard_WEB_API.Controllers
{
    [ApiController]
    [Route("api/genre")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;
        private readonly ILogger<GenreController> _logger;

        public GenreController(IGenreService genreService, ILogger<GenreController> logger)
        {
            _genreService = genreService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateGenreDto dto)
        {
            var response = await _genreService.CreateAsync(dto);
            return this.ToActionResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateGenreDto dto)
        {
            var response = await _genreService.UpdateAsync(dto);
            return this.ToActionResult(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                var validResponse = new ServiceResponse
                {
                    Message = "Id не вказано",
                    IsSuccess = false,
                    HttpStatusCode = HttpStatusCode.BadRequest
                };
                return this.ToActionResult(validResponse);
            }

            var response = await _genreService.DeleteAsync(id);
            return this.ToActionResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var response = await _genreService.GetAllAsync();
            return this.ToActionResult(response);
        }

        [HttpGet("by-name")]
        public async Task<IActionResult> GetByNameAsync(string name)
        {
            return Ok();
        }

        [HttpGet("by-id")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var response = await _genreService.GetByIdAsync(id);
            return this.ToActionResult(response);
        }
    }
}
