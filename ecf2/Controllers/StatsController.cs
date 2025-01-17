using ecf2.Models;
using ecf2.Services;
using Microsoft.AspNetCore.Mvc;

namespace ecf2.Controllers
{
    public class StatsController : Controller
    {
        private readonly StatsService _statsService;

        public StatsController(StatsService statsService)
        {
            _statsService = statsService;
        }

        // GET: Stats
        public async Task<IActionResult> Index()
        {
            var stats = await _statsService.GetAsync();
            return View(stats);
        }

        // GET: Stats/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var stats = await _statsService.GetAsync(id);
            if (stats == null)
            {
                return NotFound();
            }

            return View(stats); 
        }

        // GET: Stats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stats/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EvenementId,NombreParticipants")] EvenementStat evenementStat)
        {
            if (ModelState.IsValid)
            {
                await _statsService.CreateAsync(evenementStat);
                return RedirectToAction(nameof(Index));  
            }
            return View(evenementStat);  
        }

        // GET: Stats/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var stats = await _statsService.GetAsync(id);
            if (stats == null)
            {
                return NotFound();
            }
            return View(stats);  
        }

        // POST: Stats/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EvenementId,NombreParticipants")] EvenementStat updatedStat)
        {
            if (id != updatedStat.EvenementId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingStats = await _statsService.GetAsync(id);
                if (existingStats == null)
                {
                    return NotFound();
                }

                await _statsService.UpdateAsync(id, updatedStat);
                return RedirectToAction(nameof(Index));  
            }

            return View(updatedStat);  
        }
    }
}
