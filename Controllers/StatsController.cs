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
        var daysBack = days ?? 30;
        var startDate = DateTime.Now.AddDays(-daysBack);

        // Общая статистика
        var totalNotes = await _context.Notes.CountAsync();
        var recentNotes = await _context.Notes.CountAsync(n => n.CreatedAt >= startDate);
        var totalGoals = await _context.Goals.CountAsync();
        var activeGoals = await _context.Goals.CountAsync(g => g.Status == GoalStatus.Active);
        var completedGoals = await _context.Goals.CountAsync(g => g.Status == GoalStatus.Completed);

        // Статистика эмоций
        var emotionStats = await _context.Notes
            .Where(n => n.CreatedAt >= startDate)
            .GroupBy(n => n.Emotion)
            .Select(g => new { Emotion = g.Key, Count = g.Count() })
            .ToListAsync();

        // Статистика по дням недели
        var weeklyStats = await _context.Notes
            .Where(n => n.CreatedAt >= startDate)
            .GroupBy(n => n.CreatedAt.DayOfWeek)
            .Select(g => new { DayOfWeek = g.Key, Count = g.Count() })
            .ToListAsync();

        // Статистика по часам
        var hourlyStats = await _context.Notes
            .Where(n => n.CreatedAt >= startDate)
            .GroupBy(n => n.CreatedAt.Hour)
            .Select(g => new { Hour = g.Key, Count = g.Count() })
            .ToListAsync();

        // Топ тегов
        var notesWithTags = await _context.Notes
            .Where(n => n.CreatedAt >= startDate && !string.IsNullOrEmpty(n.Tags))
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
            .Where(n => n.CreatedAt >= startDate && !string.IsNullOrEmpty(n.Activity))
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
        var moodTrends = await _context.Notes
            .Where(n => n.CreatedAt >= DateTime.Now.AddDays(-7))
            .GroupBy(n => n.CreatedAt.Date)
            .Select(g => new { 
                Date = g.Key, 
                AverageMood = g.Average(n => (int)n.Emotion),
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
        var daysBack = days ?? 30;
        var startDate = DateTime.Now.AddDays(-daysBack);

        var notes = await _context.Notes
            .Where(n => n.CreatedAt >= startDate)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        var goals = await _context.Goals
            .OrderByDescending(g => g.CreatedAt)
            .ToListAsync();

        // Создаем CSV данные
        var csvContent = "Дата,Время,Эмоция,Содержание,Теги,Активность,Закреплено,Поделиться с психологом\n";
        
        foreach (var note in notes)
        {
            csvContent += $"{note.CreatedAt:yyyy-MM-dd},{note.CreatedAt:HH:mm},{note.Emotion},\"{note.Content.Replace("\"", "\"\"")}\",{note.Tags ?? ""},{note.Activity ?? ""},{note.IsPinned},{note.ShareWithPsychologist}\n";
        }

        var fileName = $"sofia_export_{DateTime.Now:yyyy-MM-dd}.csv";
        var bytes = System.Text.Encoding.UTF8.GetBytes(csvContent);
        
        return File(bytes, "text/csv", fileName);
    }

    [HttpGet("insights")]
    public async Task<IActionResult> Insights()
    {
        var last30Days = DateTime.Now.AddDays(-30);
        
        // Анализ паттернов
        var insights = new List<string>();
        
        // Анализ эмоций
        var emotionAnalysis = await _context.Notes
            .Where(n => n.CreatedAt >= last30Days)
            .GroupBy(n => n.Emotion)
            .Select(g => new { Emotion = g.Key, Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .ToListAsync();

        if (emotionAnalysis.Any())
        {
            var mostFrequent = emotionAnalysis.First();
            var leastFrequent = emotionAnalysis.Last();
            
            insights.Add($"За последние 30 дней вы чаще всего чувствовали {GetEmotionName(mostFrequent.Emotion)} ({mostFrequent.Count} раз).");
            
            if (mostFrequent.Count > 5)
            {
                insights.Add($"Эмоция {GetEmotionName(mostFrequent.Emotion)} преобладает в ваших записях. Возможно, стоит обратить на это внимание.");
            }
        }

        // Анализ активности
        var activityAnalysis = await _context.Notes
            .Where(n => n.CreatedAt >= last30Days && !string.IsNullOrEmpty(n.Activity))
            .GroupBy(n => n.Activity)
            .Select(g => new { Activity = g.Key, Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .FirstOrDefaultAsync();

        if (activityAnalysis != null)
        {
            insights.Add($"Ваша самая частая активность: {activityAnalysis.Activity} ({activityAnalysis.Count} раз).");
        }

        // Анализ целей
        var goalProgress = await _context.Goals
            .Where(g => g.Status == GoalStatus.Active)
            .AverageAsync(g => g.Progress);

        if (goalProgress > 0)
        {
            insights.Add($"Средний прогресс по активным целям: {goalProgress:F0}%. Продолжайте в том же духе!");
        }

        ViewBag.Insights = insights;
        return View();
    }

    [HttpGet("report")]
    public async Task<IActionResult> GenerateReport(int? days, string format)
    {
        var daysBack = days ?? 30;
        var startDate = DateTime.Now.AddDays(-daysBack);
        var endDate = DateTime.Now;

        // Собираем данные для отчета
        var notes = await _context.Notes
            .Where(n => n.CreatedAt >= startDate && n.CreatedAt <= endDate)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        var goals = await _context.Goals
            .Where(g => g.CreatedAt >= startDate || g.Status == GoalStatus.Active)
            .ToListAsync();

        var practices = await _context.Practices
            .Where(p => p.IsActive)
            .ToListAsync();

        // Анализ эмоций
        var emotionStats = notes
            .GroupBy(n => n.Emotion)
            .Select(g => new { Emotion = g.Key, Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .ToList();

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
            AverageProgress = goals.Where(g => g.Status == GoalStatus.Active).Average(g => g.Progress)
        };

        // Тренды настроения
        var moodTrends = notes
            .GroupBy(n => n.CreatedAt.Date)
            .Select(g => new { 
                Date = g.Key, 
                AverageMood = g.Average(n => (int)n.Emotion),
                Count = g.Count()
            })
            .OrderBy(g => g.Date)
            .ToList();

        var reportData = new
        {
            Period = new { Start = startDate, End = endDate, Days = daysBack },
            Summary = new
            {
                TotalNotes = notes.Count,
                TotalGoals = goals.Count,
                ActiveGoals = goalStats.Active,
                CompletedGoals = goalStats.Completed,
                AverageMood = notes.Any() ? notes.Average(n => (int)n.Emotion) : 0,
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


