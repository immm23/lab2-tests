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
    public class ClassesController : Controller
    {
        private readonly PillsContext _context;

        public ClassesController(PillsContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
              return _context.PillClasses is not null ? 
                View(await _context.PillClasses.ToListAsync()) :
                Problem("Entity set 'PillsContext.Classes'  is null.");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || _context.PillClasses is null)
            {
                return NotFound();
            }

            var pillClass = await _context.PillClasses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pillClass is null)
            {
                return NotFound();
            }

            return View(pillClass);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("Name")] PillClass pillClass)
        {
            if (ModelState.IsValid)
            {
                pillClass.Id = await _context.PillClasses.MaxAsync(c => (int?)c.Id) + 1 ?? 1;
                await _context.AddAsync(pillClass);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(pillClass);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null || _context.PillClasses is null)
            {
                return NotFound();
            }

            var pillClass = await _context.PillClasses.FindAsync(id);
            if (pillClass is null)
            {
                return NotFound();
            }
            return View(pillClass);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] PillClass pillClass)
        {
            if (id != pillClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pillClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassExists(pillClass.Id))
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
            return View(pillClass);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || _context.PillClasses is null)
            {
                return NotFound();
            }

            var pillClass = await _context.PillClasses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pillClass is null)
            {
                return NotFound();
            }

            return View(pillClass);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PillClasses is null)
            {
                return Problem("Entity set 'PillsContext.Classes'  is null.");
            }
            var pillClass = await _context.PillClasses.FindAsync(id);
            if (pillClass is not null)
            {
                _context.PillClasses.Remove(pillClass);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public bool ClassExists(int id)
        {
          return (_context.PillClasses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
