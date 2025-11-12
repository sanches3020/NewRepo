using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sofia.Web.Data;
using Sofia.Web.Models;

namespace Sofia.Web.Controllers;

[Route("stats")]
public class StatsController : Controller
{
    private readonly SofiaDbContext _context;

    public StatsController(SofiaDbContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(int? days)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Auth");
        }

        var userIdInt = int.Parse(userId);
        var daysBack = days ?? 30;
        var startDate = DateTime.Now.AddDays(-daysBack);

        // Общая статистика
        var totalNotes = await _context.Notes.CountAsync(n => n.UserId == userIdInt);
        var recentNotes = await _context.Notes.CountAsync(n => n.UserId == userIdInt && n.Date >= startDate);
        var totalGoals = await _context.Goals.CountAsync(g => g.UserId == userIdInt);
        var activeGoals = await _context.Goals.CountAsync(g => g.UserId == userIdInt && g.Status == GoalStatus.Active);
        var completedGoals = await _context.Goals.CountAsync(g => g.UserId == userIdInt && g.Status == GoalStatus.Completed);
        var totalEmotions = await _context.EmotionEntries.CountAsync(e => e.UserId == userIdInt);

        // Статистика эмоций
        var emotionStats = await _context.EmotionEntries
            .Where(e => e.UserId == userIdInt && e.Date >= startDate)
            .GroupBy(e => e.Emotion)
            .Select(g => new { Emotion = g.Key, Count = g.Count() })
            .ToListAsync();

        // Статистика по дням недели
        var weeklyStats = await _context.Notes
            .Where(n => n.UserId == userIdInt && n.Date >= startDate)
            .GroupBy(n => n.Date.DayOfWeek)
            .Select(g => new { DayOfWeek = g.Key, Count = g.Count() })
            .ToListAsync();

        // Статистика по часам
        var hourlyStats = await _context.Notes
            .Where(n => n.UserId == userIdInt && n.CreatedAt >= startDate)
            .GroupBy(n => n.CreatedAt.Hour)
            .Select(g => new { Hour = g.Key, Count = g.Count() })
            .ToListAsync();

        // Топ тегов
        var notesWithTags = await _context.Notes
            .Where(n => n.UserId == userIdInt && n.Date >= startDate && !string.IsNullOrEmpty(n.Tags))
            .Select(n => n.Tags)
            .ToListAsync();

        var tagStats = notesWithTags
            .SelectMany(tags => tags?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? Enumerable.Empty<string>())
            .GroupBy(tag => tag.Trim())
            .Select(g => new { Tag = g.Key, Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .Take(10)
            .ToList();

        // Топ активностей
        var activityStats = await _context.Notes
            .Where(n => n.UserId == userIdInt && n.Date >= startDate && !string.IsNullOrEmpty(n.Activity))
            .GroupBy(n => n.Activity)
            .Select(g => new { Activity = g.Key, Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .Take(10)
            .ToListAsync();

        // Статистика практик
        var practiceStats = await _context.Practices
            .Where(p => p.IsActive)
            .ToListAsync();

        // Тренды настроения (последние 7 дней)
        var moodTrends = await _context.EmotionEntries
            .Where(e => e.UserId == userIdInt && e.Date >= DateTime.Now.AddDays(-7))
            .GroupBy(e => e.Date.Date)
            .Select(g => new { 
                Date = g.Key, 
                AverageMood = g.Average(e => (int)e.Emotion),
                Count = g.Count()
            })
            .OrderBy(g => g.Date)
            .ToListAsync();

        ViewBag.DaysBack = daysBack;
        ViewBag.TotalNotes = totalNotes;
        ViewBag.RecentNotes = recentNotes;
        ViewBag.TotalGoals = totalGoals;
        ViewBag.ActiveGoals = activeGoals;
        ViewBag.CompletedGoals = completedGoals;
        ViewBag.TotalEmotions = totalEmotions;
        ViewBag.EmotionStats = emotionStats;
        ViewBag.WeeklyStats = weeklyStats;
        ViewBag.HourlyStats = hourlyStats;
        ViewBag.TagStats = tagStats;
        ViewBag.ActivityStats = activityStats;
        ViewBag.PracticeStats = practiceStats;
        ViewBag.MoodTrends = moodTrends;

        return View();
    }

    [HttpGet("export")]
    public async Task<IActionResult> Export(int? days)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return Json(new { success = false, message = "Пользователь не авторизован" });
        }

        var userIdInt = int.Parse(userId);
        var daysBack = days ?? 30;
        var startDate = DateTime.Now.AddDays(-daysBack);

        var notes = await _context.Notes
            .Where(n => n.UserId == userIdInt && n.Date >= startDate)
            .OrderByDescending(n => n.Date)
            .ToListAsync();

        var goals = await _context.Goals
            .Where(g => g.UserId == userIdInt)
            .OrderByDescending(g => g.CreatedAt)
            .ToListAsync();

        // Создаем CSV данные
        var csvContent = "Дата,Время,Эмоция,Содержание,Теги,Активность,Закреплено,Поделиться с психологом\n";
        
        foreach (var note in notes)
        {
            csvContent += $"{note.Date:yyyy-MM-dd},{note.CreatedAt:HH:mm},{note.Emotion},\"{note.Content.Replace("\"", "\"\"")}\",{note.Tags ?? ""},{note.Activity ?? ""},{note.IsPinned},{note.ShareWithPsychologist}\n";
        }

        var fileName = $"sofia_export_{DateTime.Now:yyyy-MM-dd}.csv";
        var bytes = System.Text.Encoding.UTF8.GetBytes(csvContent);
        
        return File(bytes, "text/csv", fileName);
    }

    [HttpGet("insights")]
    public async Task<IActionResult> Insights()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Auth");
        }

        var userIdInt = int.Parse(userId);
        var last30Days = DateTime.Now.AddDays(-30);
        
        // Структура для инсайтов по типам
        var insights = new List<dynamic>();
        
        // 📈 Тренд - анализ по дням недели
        var weeklyEmotions = await _context.EmotionEntries
            .Where(e => e.UserId == userIdInt && e.Date >= last30Days)
            .ToListAsync();
        
        if (weeklyEmotions.Any())
        {
            var mondayEmotions = weeklyEmotions
                .Where(e => e.Date.DayOfWeek == DayOfWeek.Monday)
                .GroupBy(e => e.Emotion)
                .Select(g => new { Emotion = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .FirstOrDefault();
            
            if (mondayEmotions != null && mondayEmotions.Count > 2)
            {
                insights.Add(new {
                    Type = "📈 Тренд",
                    Text = $"Вы чаще отмечаете {GetEmotionName(mondayEmotions.Emotion).ToLower()} по понедельникам"
                });
            }
        }
        
        // 🔁 Повторение - анализ регулярности заметок
        var notesByDate = await _context.Notes
            .Where(n => n.UserId == userIdInt && n.Date >= last30Days)
            .OrderBy(n => n.Date)
            .Select(n => n.Date.Date)
            .Distinct()
            .ToListAsync();
        
        if (notesByDate.Count >= 3)
        {
            var intervals = new List<int>();
            for (int i = 1; i < notesByDate.Count; i++)
            {
                var days = (notesByDate[i] - notesByDate[i-1]).Days;
                intervals.Add(days);
            }
            
            if (intervals.Any())
            {
                var avgInterval = (int)intervals.Average();
                if (avgInterval > 0 && avgInterval <= 3)
                {
                    insights.Add(new {
                        Type = "🔁 Повторение",
                        Text = $"Вы создаете заметки стабильно каждые {avgInterval} {(avgInterval == 1 ? "день" : avgInterval < 5 ? "дня" : "дней")}"
                    });
                }
            }
        }
        
        // ⏰ Влияние времени - анализ по часам
        var hourlyEmotions = await _context.Notes
            .Where(n => n.UserId == userIdInt && n.CreatedAt >= last30Days)
            .ToListAsync();
        
        if (hourlyEmotions.Any())
        {
            var eveningNotes = hourlyEmotions
                .Where(n => n.CreatedAt.Hour >= 18 && n.CreatedAt.Hour < 22)
                .ToList();
            
            var morningNotes = hourlyEmotions
                .Where(n => n.CreatedAt.Hour >= 6 && n.CreatedAt.Hour < 12)
                .ToList();
            
            if (eveningNotes.Count > morningNotes.Count * 1.5 && eveningNotes.Count > 5)
            {
                var avgMood = eveningNotes
                    .Select(n => (int)n.Emotion)
                    .DefaultIfEmpty(0)
                    .Average();
                
                if (avgMood >= 3) // Радостно или выше
                {
                    insights.Add(new {
                        Type = "⏰ Влияние времени",
                        Text = "Лучшее настроение — в вечернее время"
                    });
                }
            }
        }
        
        // 🎯 Цель и поведение - связь целей с эмоциями
        var goals = await _context.Goals
            .Where(g => g.UserId == userIdInt && g.CreatedAt >= last30Days)
            .OrderByDescending(g => g.CreatedAt)
            .ToListAsync();
        
        if (goals.Any())
        {
            var recentGoal = goals.First();
            var goalDate = recentGoal.CreatedAt.Date;
            var afterGoalEmotions = await _context.EmotionEntries
                .Where(e => e.UserId == userIdInt && e.Date >= goalDate && e.Emotion == EmotionType.Calm)
                .CountAsync();
            
            var beforeGoalEmotions = await _context.EmotionEntries
                .Where(e => e.UserId == userIdInt && e.Date < goalDate && e.Date >= goalDate.AddDays(-7) && e.Emotion == EmotionType.Calm)
                .CountAsync();
            
            if (afterGoalEmotions > beforeGoalEmotions && afterGoalEmotions > 2)
            {
                insights.Add(new {
                    Type = "🎯 Цель и поведение",
                    Text = $"После постановки цели \"{recentGoal.Title}\" вы стали чаще отмечать спокойствие"
                });
            }
        }
        
        // ⚠️ Отклонения - резкие изменения
        var last5Days = DateTime.Now.AddDays(-5);
        var recentNotes = await _context.Notes
            .Where(n => n.UserId == userIdInt && n.Date >= last5Days)
            .CountAsync();
        
        var previous5Days = await _context.Notes
            .Where(n => n.UserId == userIdInt && n.Date >= last5Days.AddDays(-5) && n.Date < last5Days)
            .CountAsync();
        
        if (previous5Days > 0 && recentNotes < previous5Days * 0.5)
        {
            insights.Add(new {
                Type = "⚠️ Отклонения",
                Text = "Резкое снижение активности за последние 5 дней"
            });
        }

        ViewBag.Insights = insights;
        
        // Если запрос через AJAX, возвращаем JSON
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" || Request.Query["format"] == "json")
        {
            return Json(new { insights = insights.Select(i => new { type = i.Type, text = i.Text }).ToList() });
        }
        
        return View();
    }

    [HttpGet("report")]
    public async Task<IActionResult> GenerateReport(int? days, string format)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Auth");
        }

        var userIdInt = int.Parse(userId);
        var daysBack = days ?? 30;
        var startDate = DateTime.Now.AddDays(-daysBack);
        var endDate = DateTime.Now;

        // Собираем данные для отчета
        var notes = await _context.Notes
            .Where(n => n.UserId == userIdInt && n.Date >= startDate && n.Date <= endDate)
            .OrderByDescending(n => n.Date)
            .ToListAsync();

        var goals = await _context.Goals
            .Where(g => g.UserId == userIdInt && (g.Date >= startDate || g.Status == GoalStatus.Active))
            .ToListAsync();

        var practices = await _context.Practices
            .Where(p => p.IsActive)
            .ToListAsync();

        // Анализ эмоций
        var emotionStats = await _context.EmotionEntries
            .Where(e => e.UserId == userIdInt && e.Date >= startDate && e.Date <= endDate)
            .GroupBy(e => e.Emotion)
            .Select(g => new { Emotion = g.Key, Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .ToListAsync();

        // Анализ активности
        var activityStats = notes
            .Where(n => !string.IsNullOrEmpty(n.Activity))
            .GroupBy(n => n.Activity)
            .Select(g => new { Activity = g.Key, Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .Take(10)
            .ToList();

        // Анализ тегов
        var tagStats = notes
            .Where(n => !string.IsNullOrEmpty(n.Tags))
            .SelectMany(n => n.Tags?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? Enumerable.Empty<string>())
            .GroupBy(tag => tag.Trim())
            .Select(g => new { Tag = g.Key, Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .Take(15)
            .ToList();

        // Статистика целей
        var goalStats = new
        {
            Total = goals.Count,
            Active = goals.Count(g => g.Status == GoalStatus.Active),
            Completed = goals.Count(g => g.Status == GoalStatus.Completed),
            AverageProgress = goals.Where(g => g.Status == GoalStatus.Active).Any() ? goals.Where(g => g.Status == GoalStatus.Active).Average(g => g.Progress) : 0
        };

        // Тренды настроения
        var moodTrends = await _context.EmotionEntries
            .Where(e => e.UserId == userIdInt && e.Date >= startDate && e.Date <= endDate)
            .GroupBy(e => e.Date.Date)
            .Select(g => new { 
                Date = g.Key, 
                AverageMood = g.Average(e => (int)e.Emotion),
                Count = g.Count()
            })
            .OrderBy(g => g.Date)
            .ToListAsync();

        var reportData = new
        {
            Period = new { Start = startDate, End = endDate, Days = daysBack },
            Summary = new
            {
                TotalNotes = notes.Count,
                TotalGoals = goals.Count,
                ActiveGoals = goalStats.Active,
                CompletedGoals = goalStats.Completed,
                AverageMood = moodTrends.Any() ? moodTrends.Average(m => m.AverageMood) : 0,
                MostFrequentEmotion = emotionStats.FirstOrDefault()?.Emotion,
                MostFrequentActivity = activityStats.FirstOrDefault()?.Activity
            },
            EmotionStats = emotionStats,
            ActivityStats = activityStats,
            TagStats = tagStats,
            GoalStats = goalStats,
            MoodTrends = moodTrends,
            Notes = notes.Take(50), // Последние 50 заметок
            Goals = goals,
            Practices = practices,
            GeneratedAt = DateTime.Now,
            Version = "1.0"
        };

        if (format == "json")
        {
            var json = System.Text.Json.JsonSerializer.Serialize(reportData, new System.Text.Json.JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
            
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            return File(bytes, "application/json", $"sofia_report_{DateTime.Now:yyyy-MM-dd}.json");
        }
        else if (format == "pdf")
        {
            // Для PDF отчета создадим HTML и вернем его
            ViewBag.ReportData = reportData;
            ViewBag.Format = "pdf";
            return View("Report");
        }
        else
        {
            // HTML отчет
            ViewBag.ReportData = reportData;
            ViewBag.Format = "html";
            return View("Report");
        }
    }

    private string GetEmotionName(EmotionType emotion)
    {
        return emotion switch
        {
            EmotionType.VerySad => "очень грустно",
            EmotionType.Sad => "грустно",
            EmotionType.Neutral => "нейтрально",
            EmotionType.Happy => "радостно",
            EmotionType.VeryHappy => "очень радостно",
            EmotionType.Anxious => "тревожно",
            EmotionType.Calm => "спокойно",
            EmotionType.Excited => "взволнованно",
            EmotionType.Frustrated => "раздражённо",
            EmotionType.Grateful => "благодарно",
            _ => emotion.ToString()
        };
    }
}


