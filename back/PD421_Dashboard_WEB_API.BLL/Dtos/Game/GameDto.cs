﻿namespace PD421_Dashboard_WEB_API.BLL.Dtos.Game
{
    public class GameDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? Publisher { get; set; }
        public string? Developer { get; set; }
        public GameImageDto? MainImage { get; set; }
        public List<GameImageDto> Images { get; set; } = [];

    }
}
