using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SportSchoolApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SportSchoolApp.Controllers
{
    // Контроллер для расписания тренировок и соревнований
    [Authorize]
    public class ScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ScheduleController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Список событий расписания
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var schedules = await _context.Schedules
                .Include(s => s.Coach)
                .OrderBy(s => s.StartDateTime)
                .ToListAsync();

            return View(schedules);
        }

        // Создание нового события (только для админов)
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Create()
        {
            var coaches = await _userManager.GetUsersInRoleAsync(UserRoles.Coach);
            ViewBag.Coaches = new SelectList(coaches, "Id", "FullName");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(schedule);
                await _context.SaveChangesAsync();
                
                if (schedule.EventType == EventType.Training)
                {
                    var trainingSession = new TrainingSession
                    {
                        ScheduleId = schedule.Id,
                        CoachId = schedule.CoachId ?? throw new InvalidOperationException("CoachId не найдено в БД"),
                        StartTime = schedule.StartDateTime,
                        EndTime = schedule.EndDateTime,
                        Location = schedule.Location,
                        Description = schedule.Title
                    };
                    _context.TrainingSessions.Add(trainingSession);
                    await _context.SaveChangesAsync();
                }
                
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.Coaches = await _userManager.GetUsersInRoleAsync(UserRoles.Coach);
            return View(schedule);
        }

        // Редактирование нового события (только для админов)
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            var coaches = await _userManager.GetUsersInRoleAsync(UserRoles.Coach);
            ViewBag.Coaches = new SelectList(coaches, "Id", "FullName");
            return View(schedule);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Schedule schedule)
        {
            if (id != schedule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(schedule.Id))
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

            ViewBag.Coaches = await _userManager.GetUsersInRoleAsync(UserRoles.Coach);
            return View(schedule);
        }

        // Удаление нового события (только для админов)
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Coach)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = UserRoles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule != null)
            {
                _context.Schedules.Remove(schedule);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Проверка существования события
        private bool ScheduleExists(int id)
        {
            return _context.Schedules.Any(e => e.Id == id);
        }
        
        // Фильтрация расписания в зависимости от роли пользователя
        private IQueryable<Schedule> ApplyRoleBasedFilter(IQueryable<Schedule> query)
        {
            var user = _userManager.GetUserAsync(User).Result;

            if (user == null)
            {
                return query; 
            }

            if (User.IsInRole(UserRoles.Coach))
            {
                return query.Where(s => s.CoachId == user.Id);
            }
            else if (User.IsInRole(UserRoles.Athlete))
            {
                return query.Where(s =>
                    (user.CoachId != null && s.CoachId == user.CoachId) ||
                    s.EventType == EventType.Competition
                );
            }

            return query;
        }
    }
}