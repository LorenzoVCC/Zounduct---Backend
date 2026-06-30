using Microsoft.EntityFrameworkCore;
using Zounduct.Application.Repositories;
using Zounduct.Domain.Entities;
using Zounduct.Infrastructure.Persistence;

namespace Zounduct.Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly ZounductDbContext _context;

    public EventRepository(ZounductDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Event ev)
    {
        await _context.Events.AddAsync(ev);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Event>> GetAllAsync()
    {
        return await _context.Events.ToListAsync();
    }
}