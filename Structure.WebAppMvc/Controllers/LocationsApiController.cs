using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Structure.WebAppMvc.Cache;
using Structure.WebAppMvc.Data;
using Structure.WebAppMvc.ViewObjects;

namespace Structure.WebAppMvc.Controllers;

[ApiController]
[Route("api/v1/locations")]
public class LocationsApiController : ControllerBase
{
    private readonly StructureContext _context;
    private readonly IDatabase _cache;

    public LocationsApiController(StructureContext context, RedisProvider redis)
    {
        _context = context;
        _cache = redis.Cache;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var key = "location-all";
        var locationCache = await _cache.StringGetAsync(key);

        if (locationCache.HasValue)
        {
            var location = JsonSerializer.Deserialize<List<LocationResult>>(locationCache.ToString());
            return Ok(location);
        }

        var locationDb = await _context.Location
            .Select(l => (LocationResult)l)
            .ToListAsync();

        await _cache.StringSetAsync(key, JsonSerializer.Serialize(locationDb), TimeSpan.FromSeconds(120));

        return Ok(locationDb);
    }

    [HttpGet("{acronym}")]
    public async Task<IActionResult> Get(string acronym)
    {
        var key = $"location-acronym-{acronym}";
        var locationCache = await _cache.StringGetAsync(key);

        if (locationCache.HasValue)
        {
            var location = JsonSerializer.Deserialize<LocationDetailResult>(locationCache.ToString());
            return Ok(location);
        }

        var locationDb = await _context.Location
            .Where(m => m.Acronym == acronym)
            .Include(l => l.Parent)
            .Include(l => l.Children)
            .Select(l => (LocationDetailResult)l)
            .FirstOrDefaultAsync();

        await _cache.StringSetAsync(key, JsonSerializer.Serialize(locationDb), TimeSpan.FromSeconds(120));
        return Ok(locationDb);
    }
}