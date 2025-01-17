using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ecf2.Data;
using ecf2.Models;
using ecf2.Services;
using System.Drawing.Printing;

namespace ecf2.Controllers
{
    public class EvenementController : Controller
    {
        private readonly ecf2Context _context;
        private readonly StatsService _statsService;
        private const int PageSize = 5;

        public EvenementController(ecf2Context context, StatsService statsService)
        {
            _context = context;
            _statsService = statsService;
        }

        // GET: Evenement
        public IActionResult Index(int page = 1)
        {
            var totalItems = _context.Evenement.Count(); 

            var evenements = _context.Evenement
                .OrderBy(e => e.Id)  
                .Skip((page - 1) * PageSize) 
                .Take(PageSize) 
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / PageSize);

            return View(evenements);
        }

        // GET: Evenement/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evenement = await _context.Evenement
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evenement == null)
            {
                return NotFound();
            }

            var participants = await _context.Participant
                .Where(p => p.EvenementId == id)
                .ToListAsync();
            ViewBag.ParticipantCount = participants.Count;

            ViewBag.Participants = participants;

            return View(evenement);
        }


        // GET: Evenement/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Evenement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Lieu,Date")] Evenement evenement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(evenement);
                await _context.SaveChangesAsync();

                var evenementStat = new EvenementStat
                {
                    EvenementId = evenement.Id,
                    NombreParticipants = 0
                };

                await _statsService.CreateAsync(evenementStat);

                return RedirectToAction(nameof(Index));
            }
            return View(evenement);
        }

        // GET: Evenement/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evenement = await _context.Evenement.FindAsync(id);
            if (evenement == null)
            {
                return NotFound();
            }
            return View(evenement);
        }

        // POST: Evenement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Lieu,Date")] Evenement evenement)
        {
            if (id != evenement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evenement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EvenementExists(evenement.Id))
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
            return View(evenement);
        }

        // GET: Evenement/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evenement = await _context.Evenement
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evenement == null)
            {
                return NotFound();
            }

            return View(evenement);
        }

        private bool EvenementExists(int id)
        {
            return _context.Evenement.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> Inscrire(int EvenementId, string NomParticipant)
        {
            if (string.IsNullOrWhiteSpace(NomParticipant))
            {
                ModelState.AddModelError("NomParticipant", "Le nom du participant est requis.");
                return RedirectToAction("Details", new { id = EvenementId });
            }

            var participant = new Participant
            {
                EvenementId = EvenementId,
                Nom = NomParticipant
            };

            _context.Participant.Add(participant);
            await _context.SaveChangesAsync();

            var evenementStat = await _statsService.GetAsync(EvenementId);
            if (evenementStat != null)
            {
                evenementStat.NombreParticipants += 1;
                await _statsService.UpdateAsync(EvenementId, evenementStat);
            }

            return RedirectToAction("Details", new { id = EvenementId });
        }


        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evenement = await _context.Evenement
                .FirstOrDefaultAsync(m => m.Id == id);

            if (evenement == null)
            {
                return NotFound();
            }

            var participants = await _context.Participant
                .Where(p => p.EvenementId == id)
                .ToListAsync();
            _context.Participant.RemoveRange(participants);
            _context.Evenement.Remove(evenement);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
