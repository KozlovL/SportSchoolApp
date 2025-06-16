using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SportSchoolApp.Models;

// Контроллер для функционала тренера
namespace SportSchoolApp.Controllers
{
    [Authorize(Roles = UserRoles.Coach)]
    public class CoachController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CoachController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Список спортсменов тренера
        public async Task<IActionResult> Athletes()
        {
            var coach = await _userManager.GetUserAsync(User);
            if (coach == null)
            {
                return NotFound("Тренер не найден.");
            }

            var athletes = await _context.Users
                .Where(u => u.CoachId == coach.Id)
                .ToListAsync();

            return View(athletes);
        }

        // Страница учета посещаемости
        public async Task<IActionResult> Attendance(int? sessionId)
        {
            var coach = await _userManager.GetUserAsync(User);
            if (coach == null) return NotFound("Тренер не найден.");

            var scheduleSessions = await _context.Schedules
                .Where(s => s.CoachId == coach.Id && s.EventType == EventType.Training)
                .Select(s => new TrainingSession
                {
                    Id = s.Id,
                    ScheduleId = s.Id,
                    CoachId = coach.Id,
                    StartTime = s.StartDateTime,
                    EndTime = s.EndDateTime,
                    Location = s.Location,
                    Description = s.Title
                })
                .ToListAsync();

            var regularSessions = await _context.TrainingSessions
                .Where(t => t.CoachId == coach.Id)
                .Distinct()
                .ToListAsync();

            var availableSessions = scheduleSessions
                .UnionBy(regularSessions, s => s.ScheduleId ?? s.Id)
                .OrderByDescending(t => t.StartTime)
                .ToList();

            if (!availableSessions.Any())
            {
                return View(new AttendanceViewModel
                {
                    AvailableSessions = new List<TrainingSession>(),
                    Athletes = new List<AthleteAttendance>()
                });
            }

            var session = sessionId.HasValue
                ? availableSessions.FirstOrDefault(s => s.Id == sessionId) ??
                regularSessions.FirstOrDefault(t => t.Id == sessionId)
                : availableSessions.FirstOrDefault();

            if (session == null) return NotFound("Занятие не найдено.");

            if (session.ScheduleId.HasValue && session.Id == session.ScheduleId)
            {
                var existingSession = await _context.TrainingSessions
                    .FirstOrDefaultAsync(t => t.ScheduleId == session.ScheduleId);

                if (existingSession == null)
                {
                    existingSession = new TrainingSession
                    {
                        ScheduleId = session.ScheduleId,
                        CoachId = coach.Id,
                        StartTime = session.StartTime,
                        EndTime = session.EndTime,
                        Location = session.Location,
                        Description = session.Description
                    };
                    _context.TrainingSessions.Add(existingSession);
                    await _context.SaveChangesAsync();
                }
                session = existingSession;
            }

            var athletes = await _context.Users
                .Where(u => u.CoachId == coach.Id)
                .ToListAsync();

            var attendances = await _context.Attendances
                .Where(a => a.TrainingSessionId == session.Id)
                .ToListAsync();

            var model = new AttendanceViewModel
            {
                Session = session,
                Athletes = athletes.Select(a => new AthleteAttendance
                {
                    AthleteId = a.Id,
                    AthleteName = a.FullName,
                    WasPresent = attendances.FirstOrDefault(at => at.AthleteId == a.Id)?.WasPresent ?? true,
                    Notes = attendances.FirstOrDefault(at => at.AthleteId == a.Id)?.Notes ?? string.Empty
                }).ToList(),
                AvailableSessions = availableSessions
            };

            return View(model);
        }

        // Сохранение данных о посещаемости спортсменов
        [HttpPost]
        public async Task<IActionResult> SaveAttendance(AttendanceViewModel model)
        {
            if (model == null || model.Session == null || model.Athletes == null)
            {
                return BadRequest("Неверные данные запроса.");
            }

            var coach = await _userManager.GetUserAsync(User);
            if (coach == null) return NotFound("Тренер не найден.");

            if (model.Session.ScheduleId.HasValue &&
                !await _context.TrainingSessions.AnyAsync(t => t.ScheduleId == model.Session.ScheduleId))
            {
                _context.TrainingSessions.Add(new TrainingSession
                {
                    ScheduleId = model.Session.ScheduleId,
                    CoachId = coach.Id,
                    StartTime = model.Session.StartTime,
                    EndTime = model.Session.EndTime,
                    Location = model.Session.Location,
                    Description = model.Session.Description
                });
                await _context.SaveChangesAsync();
            }

            foreach (var athlete in model.Athletes)
            {
                if (athlete == null || string.IsNullOrEmpty(athlete.AthleteId)) continue;

                var attendance = await _context.Attendances
                    .FirstOrDefaultAsync(a =>
                        a.TrainingSessionId == model.Session.Id &&
                        a.AthleteId == athlete.AthleteId);

                if (attendance == null)
                {
                    attendance = new Attendance
                    {
                        TrainingSessionId = model.Session.Id,
                        AthleteId = athlete.AthleteId,
                        Date = DateTime.Now,
                        WasPresent = athlete.WasPresent,
                        Notes = athlete.Notes ?? string.Empty
                    };
                    _context.Attendances.Add(attendance);
                }
                else
                {
                    attendance.WasPresent = athlete.WasPresent;
                    attendance.Notes = athlete.Notes ?? string.Empty;
                    _context.Attendances.Update(attendance);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Attendance), new { sessionId = model.Session.Id });
        }
    }
}