using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BSChallenger.Server.Models;
using BSChallenger.Server.Models.API;

namespace BSChallenger.Server.API
{
    public class Rankings1Controller : Controller
    {
        private readonly Database _context;

        public Rankings1Controller(Database context)
        {
            _context = context;
        }

        // GET: Rankings1
        public async Task<IActionResult> Index()
        {
              return View(await _context.Rankings.ToListAsync());
        }

        // GET: Rankings1/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Rankings == null)
            {
                return NotFound();
            }

            var ranking = await _context.Rankings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ranking == null)
            {
                return NotFound();
            }

            return View(ranking);
        }

        // GET: Rankings1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rankings1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,IconURL")] Ranking ranking)
        {
            if (ModelState.IsValid)
            {
                ranking.Id = Guid.NewGuid();
                _context.Add(ranking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ranking);
        }

        // GET: Rankings1/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Rankings == null)
            {
                return NotFound();
            }

            var ranking = await _context.Rankings.FindAsync(id);
            if (ranking == null)
            {
                return NotFound();
            }
            return View(ranking);
        }

        // POST: Rankings1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,IconURL")] Ranking ranking)
        {
            if (id != ranking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ranking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RankingExists(ranking.Id))
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
            return View(ranking);
        }

        // GET: Rankings1/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Rankings == null)
            {
                return NotFound();
            }

            var ranking = await _context.Rankings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ranking == null)
            {
                return NotFound();
            }

            return View(ranking);
        }

        // POST: Rankings1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Rankings == null)
            {
                return Problem("Entity set 'Database.Rankings'  is null.");
            }
            var ranking = await _context.Rankings.FindAsync(id);
            if (ranking != null)
            {
                _context.Rankings.Remove(ranking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RankingExists(Guid id)
        {
          return _context.Rankings.Any(e => e.Id == id);
        }
    }
}
