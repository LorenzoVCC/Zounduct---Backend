using Microsoft.AspNetCore.Mvc;
using Zounduct.Application.Repositories;
using Zounduct.Domain.Entities;

namespace Zounduct.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventRepository _repo;

    public EventsController(IEventRepository repo)
    {
        _repo = repo;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateEventDto dto)
    {
        var ev = new Event
        {
            Tipo = dto.Tipo,
            Payload = dto.Payload,
            Timestamp = DateTime.UtcNow
        };

        await _repo.AddAsync(ev);
        return StatusCode(201);
    }


    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var eventos = await _repo.GetAllAsync();

        var porTipo = eventos
            .GroupBy(e => e.Tipo)
            .ToDictionary(g => g.Key, g => g.Count());

        var porDia = eventos
            .GroupBy(e => e.Timestamp.Date.ToString("yyyy-MM-dd"))
            .ToDictionary(g => g.Key, g => g.Count());

        var porTipoPorDia = eventos
            .GroupBy(e => e.Tipo)
            .ToDictionary(
                g => g.Key,
                g => g.GroupBy(e => e.Timestamp.Date.ToString("yyyy-MM-dd"))
                      .ToDictionary(dg => dg.Key, dg => dg.Count())
            );

        return Ok(new
        {
            porTipo,
            porDia,
            porTipoPorDia
        });
    }

    [HttpGet("recent")]
    public async Task<IActionResult> GetRecent([FromQuery] int limit = 50)
    {
        var eventos = await _repo.GetAllAsync();

        var recientes = eventos
            .OrderByDescending(e => e.Timestamp)
            .Take(limit)
            .Select(e => new
            {
                e.Tipo,
                e.Payload,
                e.Timestamp
            })
            .ToList();

        return Ok(recientes);
    }
}

public record CreateEventDto(string Tipo, string Payload);
