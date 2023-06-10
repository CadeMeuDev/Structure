using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Structure.WebAppMvc.Data;

namespace Structure.WebAppMvc.Controllers
{
    [ApiController]
    [Route("api/v1/locations")]
    public class LocationsApiController : ControllerBase
    {
        private readonly StructureContext _context;

        public LocationsApiController(StructureContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var location = await _context.Location
                .Select(l => new
                {
                    l.Name,
                    l.Acronym
                })
                .ToListAsync();
            return Ok(location);
        }

        [HttpGet("{acronym}")]
        public async Task<IActionResult> Get(string acronym)
        {
            var location = await _context.Location
                .Include(l => l.Parent)
                .Include(l => l.Parent.Parent)
                .Include(l => l.Parent.Parent.Parent)
                .Include(l => l.Children)
                .ThenInclude(l => l.Children)
                .ThenInclude(l => l.Children)
                .Select(l => new
                {
                    l.Acronym,
                    l.Name,
                    Parent = new
                    {
                        l.Parent.Name,
                        l.Parent.Acronym,
                        Parent = new
                        {
                            l.Parent.Parent.Name,
                            l.Parent.Parent.Acronym,
                            Parent = new
                            {
                                l.Parent.Parent.Parent.Name,
                                l.Parent.Parent.Parent.Acronym
                            },
                        },
                    },
                    Children = l.Children.Select(l => new
                    {
                        l.Name,
                        l.Acronym,
                        Children = l.Children.Select(l => new
                        {
                            l.Name,
                            l.Acronym,
                            Children = l.Children.Select(l => new
                            {
                                l.Name,
                                l.Acronym,
                            })
                        })
                    })
                })
                .FirstOrDefaultAsync(m => m.Acronym == acronym);
            return Ok(location);
        }
    }
}