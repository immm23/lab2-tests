using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OOPLab1.Models;


namespace OOPLab1.Controllers;

#region Attribues
[ExcludeFromCodeCoverage]
#endregion
public class PharmasiesController : Controller
{
    private readonly PillsContext _context;

    public PharmasiesController(PillsContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Pharmasies.Include(p => p.Pills).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null || _context.Pharmasies is null)
        {
            return NotFound();
        }

        var pharmasy = await _context.Pharmasies
            .Include(p => p.Pills)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (pharmasy is null)
        {
            return NotFound();
        }

        return View(pharmasy);
    }


    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create()
    {
        ViewData["Pills"] = new MultiSelectList(await _context.Pills.ToListAsync(), 
            "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create([Bind("Id,Name,Adress, SelectedPills,PhoneNumber,OwnerName")] Pharmasy pharmasy)
    {
        if (ModelState.IsValid)
        {
            List<Pill> pills = new List<Pill>();

            if(pharmasy.SelectedPills is not null)
            {
                foreach (var item in pharmasy.SelectedPills)
                {
                    var dbItem =await  _context.Pills.FirstAsync(p => p.Id == item);
                    pills.Add(dbItem);
                }
            }

            pills.ForEach(p => _context.Pills.Add(p));
            await _context.AddAsync(pharmasy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["Pills"] = new MultiSelectList(await _context.Pills.ToListAsync(), 
            "Id", "Name", pharmasy.SelectedPills);
        return View(pharmasy);
    }

    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null || _context.Pharmasies is null)
        {
            return NotFound();
        }

        var pharmasy = await _context.Pharmasies
            .Include(p => p.Pills)
            .FirstAsync(p => p.Id == id);
        if (pharmasy is null)
        {
            return NotFound();
        }

        List<int> pillsSelected = new List<int>();
        pharmasy.Pills.ToList().ForEach(p => pillsSelected.Add(p.Id));

        ViewData["Pills"] = new MultiSelectList(await _context.Pills.ToListAsync(),
            "Id", "Name", pillsSelected);
        return View(pharmasy);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Adress,SelectedPills, PhoneNumber,OwnerName")] Pharmasy pharmasy)
    {
        if (id != pharmasy.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                List<Pill> pills = new List<Pill>();

                if(pharmasy.SelectedPills is not null)
                {
                    foreach (var item in pharmasy.SelectedPills)
                    {
                        var dbItem = _context.Pills.First(p => p.Id == item);
                        pills.Add(dbItem);
                    }
                }
               

                pharmasy.Pills.Clear();
                pills.ForEach(p => _context.Pills.Add(p));

                _context.Update(pharmasy);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PharmasyExists(pharmasy.Id))
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
        return View(pharmasy);
    }

    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null || _context.Pharmasies is null)
        {
            return NotFound();
        }

        var pharmasy = await _context.Pharmasies
            .FirstOrDefaultAsync(m => m.Id == id);
        if (pharmasy is null)
        {
            return NotFound();
        }

        return View(pharmasy);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Pharmasies is null)
        {
            return Problem("Entity set 'PillsContext.Pharmasies'  is null.");
        }
        var pharmasy = await _context.Pharmasies.FindAsync(id);
        if (pharmasy is not null)
        {
            _context.Pharmasies.Remove(pharmasy);
        }
        
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PharmasyExists(int id)
    {
      return (_context.Pharmasies?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
