﻿using PD421_Dashboard_WEB_API.DAL.Entitites;

namespace PD421_Dashboard_WEB_API.BLL.Dtos.Game
{
    public class GameImageDto
    {
        public string? Id { get; set; }
        public string? ImagePath { get; set; }
        public bool IsMain { get; set; } = false;
    }
}
