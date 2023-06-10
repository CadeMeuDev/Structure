using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Structure.WebAppMvc.Data;
using Structure.WebAppMvc.Models;

namespace Structure.WebAppMvc.Controllers
{
    public class LocationsController : Controller
    {
        private readonly StructureContext _context;

        public LocationsController(StructureContext context)
        {
            _context = context;
        }

        // GET: Locations
        public async Task<IActionResult> Index()
        {
            var structureContext = _context.Location.Include(l => l.Parent);
            return View(await structureContext.ToListAsync());
        }

        // GET: Locations/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Location == null)
            {
                return NotFound();
            }

            var location = await _context.Location
                .Include(l => l.Parent)
                .Include(l => l.Parent.Parent)
                .Include(l => l.Parent.Parent.Parent)
                .Include(l => l.Children)
                .ThenInclude(l => l.Children)
                .ThenInclude(l => l.Children)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // GET: Locations/Create
        public IActionResult Create()
        {
            ViewData["Parent"] = new SelectList(_context.Location, "Id", "Name");
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Acronym,ParentId,LocationType")] Location location)
        {
            location.Uuid = Guid.NewGuid();
            if (ModelState.IsValid)
            {
                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Parent"] = new SelectList(_context.Location, "Id", "Name", location.ParentId);
            return View(location);
        }

        // GET: Locations/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Location == null)
            {
                return NotFound();
            }

            var location = await _context.Location.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            ViewData["ParentId"] = new SelectList(_context.Location, "Id", "Id", location.ParentId);
            return View(location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Uuid,Name,ParentId,IsDeleted,LocationType")] Location location)
        {
            if (id != location.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(location);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(location.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentId"] = new SelectList(_context.Location, "Id", "Id", location.ParentId);
            return View(location);
        }

        // GET: Locations/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Location == null)
            {
                return NotFound();
            }

            var location = await _context.Location
                .Include(l => l.Parent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Location == null)
            {
                return Problem("Entity set 'StructureContext.Location'  is null.");
            }
            var location = await _context.Location.FindAsync(id);
            if (location != null)
            {
                _context.Location.Remove(location);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(long id)
        {
            return (_context.Location?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}