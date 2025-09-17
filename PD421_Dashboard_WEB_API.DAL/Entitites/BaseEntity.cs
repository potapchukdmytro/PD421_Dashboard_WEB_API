﻿namespace PD421_Dashboard_WEB_API.DAL.Entitites
{
    public interface IBaseEntity<TId>
    {
        TId Id { get; set; }
        DateTime CreatedAt { get; set; }
    }

    public class BaseEntity<TId> : IBaseEntity<TId>
    {
        public virtual TId Id { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
