using Zounduct.Domain.Entities;

namespace Zounduct.Application.Repositories;

public interface IEventRepository
{
    Task AddAsync(Event ev);
    Task<List<Event>> GetAllAsync();
}