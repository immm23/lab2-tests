using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OOPLab1.Models;

namespace OOPLab1.Controllers
{
    public class IlnessesController : Controller
    {
        private readonly PillsContext _context;

        public IlnessesController(PillsContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Ilnesses.Include(p => p.Pills).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || _context.Ilnesses is null)
            {
                return NotFound();
            }

            var ilness = await _context.Ilnesses
                .Include(p => p.Pills)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ilness is null)
            {
                return NotFound();
            }

            return View(ilness);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create()
        {
            ViewData["Pills"] = new MultiSelectList(await _context.Pills.ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Symptoms,Description, PillsSelected")]
            Ilness ilness)
        {
            if (ModelState.IsValid)
            {
                List<Pill> pills = new List<Pill>();
                if(ilness.PillsSelected is not null)
                {
                    foreach (var item in ilness.PillsSelected)
                    {
                        var dbItem = await _context.Pills.FirstAsync(p => p.Id == item);
                        pills.Add(dbItem);
                    }
                }
               
                pills.ForEach(p => _context.Pills.Add(p));

                await _context.AddAsync(ilness);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Pills"] = new MultiSelectList(await _context.Pills.ToListAsync(), 
                "Id", "Name", ilness.PillsSelected);
            return View(ilness);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null || _context.Ilnesses is null)
            {
                return NotFound();
            }

            var ilness = await _context.Ilnesses
                .Include(p => p.Pills)
                .FirstAsync(p => p.Id == id);
            if (ilness is null)
            {
                return NotFound();
            }

            List<int> pillsSelected = new List<int>();
            ilness.Pills.ToList().ForEach(p => pillsSelected.Add(p.Id));

            ViewData["Pills"] = new MultiSelectList(await _context.Pills.ToListAsync(), 
                "Id", "Name", pillsSelected);
            return View(ilness);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,PillsSelected,Symptoms,Description")] Ilness ilness)
        {
            if (id != ilness.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    List<Pill> pills = new List<Pill>();

                    if(ilness.PillsSelected is not null)
                    {
                        foreach (var item in ilness.PillsSelected)
                        {
                            var dbItem = _context.Pills.First(p => p.Id == item);
                            pills.Add(dbItem);
                        }
                    }


                    ilness.Pills.Clear();
                    pills.ForEach(p => _context.Pills.Add(p));

                    _context.Update(ilness);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IlnessExists(ilness.Id))
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
            ViewData["Pills"] = new SelectList(await _context.Pills.ToListAsync(), 
                "Id", "Name");
            return View(ilness);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || _context.Ilnesses is null)
            {
                return NotFound();
            }

            var ilness = await _context.Ilnesses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ilness is null)
            {
                return NotFound();
            }

            return View(ilness);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ilnesses is null)
            {
                return Problem("Entity set 'PillsContext.Ilnesses'  is null.");
            }
            var ilness = await _context.Ilnesses.FindAsync(id);
            if (ilness is not null)
            {
                _context.Ilnesses.Remove(ilness);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IlnessExists(int id)
        {
          return (_context.Ilnesses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
