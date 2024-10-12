using Microsoft.EntityFrameworkCore;
using PLAYOUT.Models.Domain;

namespace PLAYOUT.Data
{
    public class PlayOutDbContext: DbContext
    {

        public PlayOutDbContext(DbContextOptions<PlayOutDbContext> options) : base(options)
        {
        }

        public DbSet<Canal> Canales { get; set; }
        public DbSet<Musical> Musicales { get; set;}
        public DbSet<Programacion> Programaciones { get; set; }
        public DbSet<Spot> Spots { get; set; }

    }
}
