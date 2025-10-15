using Microsoft.EntityFrameworkCore;
using Sofia.Web.Models;

namespace Sofia.Web.Data;

public class SofiaDbContext : DbContext
{
    public SofiaDbContext(DbContextOptions<SofiaDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<Practice> Practices { get; set; }
    public DbSet<Goal> Goals { get; set; }
    public DbSet<Psychologist> Psychologists { get; set; }
    public DbSet<PsychologistReview> PsychologistReviews { get; set; }
    public DbSet<PsychologistAppointment> PsychologistAppointments { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<NotificationSettings> NotificationSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Настройка связей User
        modelBuilder.Entity<User>()
            .HasOne(u => u.PsychologistProfile)
            .WithOne(p => p.User)
            .HasForeignKey<Psychologist>(p => p.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Note>()
            .HasOne(n => n.User)
            .WithMany(u => u.Notes)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Goal>()
            .HasOne(g => g.User)
            .WithMany(u => u.Goals)
            .HasForeignKey(g => g.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PsychologistAppointment>()
            .HasOne(a => a.User)
            .WithMany(u => u.Appointments)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed practices
        modelBuilder.Entity<Practice>().HasData(
            new Practice { Id = 1, Name = "Дыхание 4-7-8", Description = "Техника успокоения через дыхание", Category = PracticeCategory.Breathing, DurationMinutes = 5, Instructions = "Вдох на 4 счета, задержка на 7, выдох на 8" },
            new Practice { Id = 2, Name = "Прогрессивная релаксация", Description = "Постепенное расслабление мышц", Category = PracticeCategory.Relaxation, DurationMinutes = 15, Instructions = "Напрягайте и расслабляйте каждую группу мышц" },
            new Practice { Id = 3, Name = "Визуализация безопасного места", Description = "Создание мысленного убежища", Category = PracticeCategory.Visualization, DurationMinutes = 10, Instructions = "Представьте место, где чувствуете себя в безопасности" },
            new Practice { Id = 4, Name = "КПТ: Работа с мыслями", Description = "Анализ и изменение негативных мыслей", Category = PracticeCategory.CBT, DurationMinutes = 20, Instructions = "Запишите мысль, оцените её реалистичность, найдите альтернативу" },
            new Practice { Id = 5, Name = "Медитация осознанности", Description = "Фокус на настоящем моменте", Category = PracticeCategory.Mindfulness, DurationMinutes = 10, Instructions = "Следите за дыханием, возвращайте внимание к настоящему" }
        );

        // Seed sample goals
        modelBuilder.Entity<Goal>().HasData(
            new Goal { Id = 1, Title = "Ежедневные практики", Description = "Выполнять хотя бы одну практику в день", Type = GoalType.Wellness, Progress = 30 },
            new Goal { Id = 2, Title = "Ведение дневника", Description = "Записывать мысли и эмоции каждый день", Type = GoalType.Personal, Progress = 60 },
            new Goal { Id = 3, Title = "Работа с тревогой", Description = "Применять техники КПТ при тревоге", Type = GoalType.Therapy, IsFromPsychologist = true, Progress = 25 }
        );

        // Seed Psychologists
        modelBuilder.Entity<Psychologist>().HasData(
            new Psychologist 
            { 
                Id = 1, 
                Name = "Анна Петрова", 
                Specialization = "Когнитивно-поведенческая терапия, тревожные расстройства", 
                Description = "Опытный психолог с 8-летним стажем работы. Специализируется на работе с тревожными расстройствами и депрессией. Использует методы КПТ и техники осознанности.",
                Education = "МГУ, факультет психологии, магистр",
                Experience = "8 лет практики",
                Languages = "Русский, английский",
                Methods = "КПТ, техники осознанности, арт-терапия",
                PricePerHour = 3000,
                ContactPhone = "+7 (495) 123-45-67",
                ContactEmail = "anna.petrova@psychology.ru",
                IsActive = true,
                CreatedAt = DateTime.Now
            },
            new Psychologist 
            { 
                Id = 2, 
                Name = "Михаил Соколов", 
                Specialization = "Семейная терапия, работа с подростками", 
                Description = "Семейный психолог с 12-летним опытом. Помогает решать семейные конфликты и проблемы в отношениях. Работает с подростками и их родителями.",
                Education = "СПбГУ, факультет психологии, кандидат наук",
                Experience = "12 лет практики",
                Languages = "Русский, французский",
                Methods = "Семейная терапия, системный подход, гештальт-терапия",
                PricePerHour = 4000,
                ContactPhone = "+7 (812) 234-56-78",
                ContactEmail = "mikhail.sokolov@family-psych.ru",
                IsActive = true,
                CreatedAt = DateTime.Now
            },
            new Psychologist 
            { 
                Id = 3, 
                Name = "Елена Волкова", 
                Specialization = "Работа с травмами, EMDR терапия", 
                Description = "Сертифицированный EMDR терапевт с 10-летним стажем. Специализируется на работе с травматическими переживаниями и ПТСР.",
                Education = "МГПУ, факультет психологии, магистр",
                Experience = "10 лет практики",
                Languages = "Русский, немецкий",
                Methods = "EMDR, соматическая терапия, работа с травмой",
                PricePerHour = 5000,
                ContactPhone = "+7 (495) 345-67-89",
                ContactEmail = "elena.volkova@trauma-therapy.ru",
                IsActive = true,
                CreatedAt = DateTime.Now
            }
        );

        // Seed Reviews
        modelBuilder.Entity<PsychologistReview>().HasData(
            new PsychologistReview { Id = 1, PsychologistId = 1, Rating = 5, Comment = "Отличный специалист! Помогла справиться с тревожностью. Очень рекомендую.", CreatedAt = DateTime.Now.AddDays(-5) },
            new PsychologistReview { Id = 2, PsychologistId = 1, Rating = 4, Comment = "Профессиональный подход, чувствуется опыт. Результат есть.", CreatedAt = DateTime.Now.AddDays(-10) },
            new PsychologistReview { Id = 3, PsychologistId = 2, Rating = 5, Comment = "Помог решить семейные проблемы. Очень благодарны!", CreatedAt = DateTime.Now.AddDays(-3) },
            new PsychologistReview { Id = 4, PsychologistId = 3, Rating = 5, Comment = "EMDR терапия действительно работает. Спасибо за помощь!", CreatedAt = DateTime.Now.AddDays(-7) }
        );
    }
}
