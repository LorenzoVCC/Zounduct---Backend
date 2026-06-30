using Microsoft.EntityFrameworkCore;
using Zounduct.Domain.Entities;

namespace Zounduct.Infrastructure.Persistence;

public class ZounductDbContext : DbContext
{
    public ZounductDbContext(DbContextOptions<ZounductDbContext> options)
        : base(options)
    {
    }

    public DbSet<Event> Events => Set<Event>();
}