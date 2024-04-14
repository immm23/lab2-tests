using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OOPLab1.Models;
using System.Linq;

namespace OOPLab1.Controllers
{
    public class PillsController : Controller
    {
        private readonly PillsContext _context;

        public PillsController(PillsContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var pills = _context.Pills.Include(p => p.ClassNavigation)
                .Include(p => p.Illnes)
                .Include(P => P.Pharmasies);

            ViewData["Class"] = new SelectList(await _context.PillClasses.ToListAsync(), "Id", "Name");
            ViewData["Pharmasies"] = new MultiSelectList(await _context.Pharmasies.ToListAsync(),
                "Id", "Name");
            ViewData["Illnes"] = new MultiSelectList(await _context.Ilnesses.ToListAsync(),
                "Id", "Name");

            return View(await pills.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || _context.Pills is null)
            {
                return NotFound();
            }

            var pill = await _context.Pills
                .Include(p => p.ClassNavigation)
                .Include(p => p.Illnes)
                .Include(p => p.Pharmasies)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pill is null)
            {
                return NotFound();
            }

            return View(pill);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create()
        {
            ViewData["Class"] = new SelectList(await _context.PillClasses.ToListAsync(), "Id", "Name");
            ViewData["Pharmasies"] = new MultiSelectList(await _context.Pharmasies.ToListAsync(), 
                "Id", "Name");
            ViewData["Illnes"] = new MultiSelectList(await _context.Ilnesses.ToListAsync(), 
                "Id", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Class,SelectedPharmasies,SelectedIllnes,SideEffects,ExpiryDate")] Pill pill)
        {
            pill.ClassNavigation = await _context.PillClasses.FirstOrDefaultAsync(p => p.Id == pill.Class);
            if (ModelState.IsValid)
            {
                pill.Id = await _context.Pills.MaxAsync(c => (int?)c.Id) + 1 ?? 1;
                _context.Add(pill);

                List<Ilness> ilnesses = new List<Ilness>();
                foreach (var item in pill.SelectedIllnes ?? new List<int>().ToArray())
                {
                    var dbItem = await _context.Ilnesses.FirstOrDefaultAsync(p => p.Id == item);
                    ilnesses.Add(dbItem);
                }

                List<Pharmasy> pharmasies = new List<Pharmasy>();
                foreach (var item in pill?.SelectedPharmasies ?? new List<int>().ToArray())
                {
                    var dbItem = await _context.Pharmasies.FirstOrDefaultAsync(p => p.Id == item);
                    pharmasies.Add(dbItem);
                }

                ilnesses.ForEach(i => pill.Illnes.Add(i));
                pharmasies.ForEach(i => pill.Pharmasies.Add(i));
                await _context.AddAsync(pill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Class"] = new SelectList(await _context.PillClasses.ToListAsync(), 
                "Id", "Name");
            ViewData["Pharmasies"] = new MultiSelectList(await _context.Pharmasies.ToListAsync(),
                "Id", "Name", pill.SelectedPharmasies);
            ViewData["Illnes"] = new MultiSelectList(await _context.Ilnesses.ToListAsync(),
                "Id", "Name", pill.SelectedIllnes);
            return View(pill);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null || _context.Pills is null)
            {
                return NotFound();
            }

            var pill = await _context.Pills.Include(p => p.Illnes)
                .Include(p => p.Pharmasies)
                .Include(p => p.ClassNavigation)
                .FirstAsync(p => p.Id == id);
            if (pill is null)
            {
                return NotFound();
            }

            List<int> selectedPharmasies = new List<int>();
            List<int> selectedIllnesses = new List<int>();

            pill.Illnes.ToList().ForEach(p => selectedIllnesses.Add(p.Id));
            pill.Pharmasies.ToList().ForEach(p => selectedPharmasies.Add(p.Id));


            ViewData["Class"] = new SelectList(await _context.PillClasses.ToListAsync(),
                "Id", "Name");
            ViewData["Pharmasies"] = new MultiSelectList(await _context.Pharmasies.ToListAsync(),
                "Id", "Name", selectedPharmasies);
            ViewData["Illnes"] = new MultiSelectList(await _context.Ilnesses.ToListAsync(),
                "Id", "Name", selectedIllnesses);

            return View(pill);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Class, SelectedPharmasies, SelectedIllnes, SideEffects, ExpiryDate")] Pill pill)
        {
            pill.ClassNavigation = _context.PillClasses.FirstOrDefault(p => p.Id == pill.Class);
            if (id != pill.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    List<Ilness> ilnesses = new List<Ilness>();
                    if(pill.SelectedIllnes is not null)
                    {
                        foreach (var item in pill.SelectedIllnes)
                        {
                            var dbItem = _context.Ilnesses.First(p => p.Id == item);
                            ilnesses.Add(dbItem);
                        }
                    }

                    List<Pharmasy> pharmasies = new List<Pharmasy>();
                    if (pill.SelectedPharmasies is not null)
                    {
                        foreach (var item in pill.SelectedPharmasies)
                        {

                            var dbItem = _context.Pharmasies.First(p => p.Id == item);
                            pharmasies.Add(dbItem);
                        }

                    }

                    pill.Illnes.Clear();
                    pill.Pharmasies.Clear();
                    ilnesses.ForEach(p => pill.Illnes.Add(p));
                    pharmasies.ForEach(p => pill.Pharmasies.Add(p));

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PillExists(pill.Id))
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
            ViewData["Class"] = new SelectList(await _context.PillClasses.ToListAsync(),
                "Id", "Name");
            ViewData["Pharmasies"] = new MultiSelectList(await _context.Pharmasies.ToListAsync(),
                "Id", "Name", pill.SelectedPharmasies);
            ViewData["Illnes"] = new MultiSelectList(await _context.Ilnesses.ToListAsync(),
                "Id", "Name", pill.SelectedIllnes);
            return View(pill);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || _context.Pills is null)
            {
                return NotFound();
            }

            var pill = await _context.Pills
                .Include(p => p.ClassNavigation)
                .Include(p => p.Illnes)
                .Include(p => p.Pharmasies)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pill is null)
            {
                return NotFound();
            }

            return View(pill);
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pills is null)
            {
                return Problem("Entity set 'PillsContext.Pills'  is null.");
            }
            var pill = await _context.Pills.FindAsync(id);
            if (pill is not null)
            {
                _context.Pills.Remove(pill);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PillExists(int id)
        {
          return (_context.Pills?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
