using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sofia.Web.Data;
using Sofia.Web.Models;

namespace Sofia.Web.Controllers;

[Route("companion")]
public class CompanionController : Controller
{
    private readonly SofiaDbContext _context;

    public CompanionController(SofiaDbContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        // Получаем последние эмоции пользователя
        var recentNotes = await _context.Notes
            .OrderByDescending(n => n.CreatedAt)
            .Take(5)
            .ToListAsync();

        var lastEmotion = recentNotes.FirstOrDefault()?.Emotion ?? EmotionType.Neutral;
        var petMood = GetPetMood(lastEmotion);
        var petMessage = GetPetMessage(lastEmotion, recentNotes.Count);

        ViewBag.PetMood = petMood;
        ViewBag.PetMessage = petMessage;
        ViewBag.LastEmotion = lastEmotion;
        ViewBag.RecentNotes = recentNotes;

        return View();
    }

    [HttpPost("feed")]
    public IActionResult Feed()
    {
        // Симуляция кормления питомца
        var happiness = Random.Shared.Next(70, 100);
        var message = GetFeedMessage(happiness);
        
        return Json(new { 
            success = true, 
            happiness = happiness,
            message = message,
            petMood = "happy"
        });
    }

    [HttpPost("play")]
    public IActionResult Play()
    {
        // Симуляция игры с питомцем
        var energy = Random.Shared.Next(60, 90);
        var message = GetPlayMessage(energy);
        
        return Json(new { 
            success = true, 
            energy = energy,
            message = message,
            petMood = "excited"
        });
    }

    [HttpPost("comfort")]
    public IActionResult Comfort()
    {
        // Утешение питомца
        var comfort = Random.Shared.Next(80, 100);
        var message = GetComfortMessage(comfort);
        
        return Json(new { 
            success = true, 
            comfort = comfort,
            message = message,
            petMood = "calm"
        });
    }

    [HttpGet("status")]
    public async Task<IActionResult> GetStatus()
    {
        var recentNotes = await _context.Notes
            .OrderByDescending(n => n.CreatedAt)
            .Take(3)
            .ToListAsync();

        var lastEmotion = recentNotes.FirstOrDefault()?.Emotion ?? EmotionType.Neutral;
        var petMood = GetPetMood(lastEmotion);
        var petMessage = GetPetMessage(lastEmotion, recentNotes.Count);

        return Json(new {
            petMood = petMood,
            petMessage = petMessage,
            lastEmotion = lastEmotion.ToString(),
            notesCount = recentNotes.Count
        });
    }

    private string GetPetMood(EmotionType emotion)
    {
        return emotion switch
        {
            EmotionType.VerySad => "sad",
            EmotionType.Sad => "concerned",
            EmotionType.Neutral => "neutral",
            EmotionType.Happy => "happy",
            EmotionType.VeryHappy => "excited",
            EmotionType.Anxious => "worried",
            EmotionType.Calm => "peaceful",
            EmotionType.Excited => "energetic",
            EmotionType.Frustrated => "confused",
            EmotionType.Grateful => "loving",
            _ => "neutral"
        };
    }

    private string GetPetMessage(EmotionType emotion, int notesCount)
    {
        var messages = emotion switch
        {
            EmotionType.VerySad => new[] { 
                "😢 Я вижу, что тебе очень грустно... Хочешь обнять меня?", 
                "💙 Ты не одинок, я здесь с тобой. Расскажи мне, что случилось?",
                "🤗 Давай вместе переживем это трудное время. Я поддержу тебя!"
            },
            EmotionType.Sad => new[] { 
                "😔 Похоже, у тебя грустный день. Хочешь поговорить об этом?", 
                "💙 Я чувствую твою грусть. Давай найдем что-то хорошее в этом дне?",
                "🤗 Иногда грусть - это нормально. Я рядом, чтобы поддержать тебя!"
            },
            EmotionType.Neutral => new[] { 
                "😊 Привет! Как дела? Хочешь поиграть со мной?", 
                "🐾 Я здесь! Расскажи мне, как прошел твой день?",
                "💫 Давай проведем время вместе! Что бы ты хотел сделать?"
            },
            EmotionType.Happy => new[] { 
                "😄 Ура! Я вижу, что ты в хорошем настроении! Давай повеселимся!", 
                "🎉 Твоя радость заразительна! Хочешь поиграть?",
                "✨ Когда ты счастлив, я тоже счастлив! Давай отпразднуем!"
            },
            EmotionType.VeryHappy => new[] { 
                "🤩 Вау! Ты просто сияешь от счастья! Это прекрасно!", 
                "🎊 Твоя радость просто невероятна! Давай поделимся этим настроением!",
                "🌟 Ты делаешь мир лучше своей улыбкой! Я так горжусь тобой!"
            },
            EmotionType.Anxious => new[] { 
                "😰 Я чувствую твою тревогу... Давай сделаем глубокий вдох вместе?", 
                "🤲 Ты в безопасности. Я здесь, чтобы помочь тебе успокоиться.",
                "💆‍♀️ Давай попробуем расслабиться. Я буду рядом с тобой."
            },
            EmotionType.Calm => new[] { 
                "😌 Какая прекрасная тишина... Я чувствую твой покой.", 
                "🧘‍♀️ Ты выглядишь таким спокойным. Это очень красиво.",
                "🌿 Твоя внутренняя гармония вдохновляет меня!"
            },
            EmotionType.Excited => new[] { 
                "🤩 Ты полон энергии! Давай направим ее в игру!", 
                "⚡ Твоя энергия заразительна! Хочешь поиграть в активную игру?",
                "🎯 Я чувствую твой энтузиазм! Давай сделаем что-то крутое!"
            },
            EmotionType.Frustrated => new[] { 
                "😤 Понимаю твое раздражение... Давай попробуем успокоиться?", 
                "🤝 Иногда все идет не так, как хочется. Я помогу тебе справиться.",
                "💪 Ты сильнее своих проблем. Давай найдем решение вместе!"
            },
            EmotionType.Grateful => new[] { 
                "🙏 Твоя благодарность согревает мое сердце!", 
                "💝 Я тоже благодарен за то, что ты есть в моей жизни!",
                "✨ Твоя благодарность делает мир лучше!"
            },
            _ => new[] { "🐾 Привет! Как дела?" }
        };

        return messages[Random.Shared.Next(messages.Length)];
    }

    private string GetFeedMessage(int happiness)
    {
        var messages = new[] {
            "🍎 Спасибо за вкусную еду! Я чувствую себя отлично!",
            "🥕 Ммм, как вкусно! Ты лучший хозяин!",
            "🍌 Эта еда дала мне много энергии! Готов играть!",
            "🥗 Спасибо за заботу! Я так счастлив!"
        };
        return messages[Random.Shared.Next(messages.Length)];
    }

    private string GetPlayMessage(int energy)
    {
        var messages = new[] {
            "🎾 Ура! Игра была потрясающей! Я полон энергии!",
            "🏃‍♂️ Это было так весело! Хочешь еще поиграть?",
            "🎯 Отличная игра! Ты лучший партнер по играм!",
            "⚽ Я так счастлив, что мы играем вместе!"
        };
        return messages[Random.Shared.Next(messages.Length)];
    }

    private string GetComfortMessage(int comfort)
    {
        var messages = new[] {
            "🤗 Твои объятия такие теплые... Я чувствую себя в безопасности.",
            "💙 Спасибо, что утешаешь меня. Ты самый лучший друг!",
            "😌 Твоя забота успокаивает меня. Я так счастлив рядом с тобой!",
            "🌟 Ты делаешь меня счастливым просто тем, что ты есть!"
        };
        return messages[Random.Shared.Next(messages.Length)];
    }
}


