using Microsoft.EntityFrameworkCore;
using Sofia.Web.Models;

namespace Sofia.Web.Data;

public class SofiaDbContext : DbContext
{
    public SofiaDbContext(DbContextOptions<SofiaDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<Practice> Practices { get; set; }
    public DbSet<Goal> Goals { get; set; }
    public DbSet<Psychologist> Psychologists { get; set; }
    public DbSet<PsychologistReview> PsychologistReviews { get; set; }
    public DbSet<PsychologistAppointment> PsychologistAppointments { get; set; }
    public DbSet<PsychologistSchedule> PsychologistSchedules { get; set; }
    public DbSet<PsychologistTimeSlot> PsychologistTimeSlots { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<NotificationSettings> NotificationSettings { get; set; }
    public DbSet<EmotionEntry> EmotionEntries { get; set; }
    public DbSet<UserStatistics> UserStatistics { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Конвертация enum → string
        modelBuilder.Entity<Practice>()
            .Property(p => p.Category)
            .HasConversion<string>();

        modelBuilder.Entity<Goal>()
            .Property(g => g.Type)
            .HasConversion<string>();

        // Конвертация EmotionType → integer (храним как int в БД)
        modelBuilder.Entity<Note>()
            .Property(n => n.Emotion)
            .HasConversion<int>();

        modelBuilder.Entity<EmotionEntry>()
            .Property(e => e.Emotion)
            .HasConversion<int>();

        // Связи
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

        modelBuilder.Entity<EmotionEntry>()
            .HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserStatistics>()
            .HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PsychologistSchedule>()
            .HasOne(s => s.Psychologist)
            .WithMany(p => p.Schedules)
            .HasForeignKey(s => s.PsychologistId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PsychologistTimeSlot>()
            .HasOne(t => t.Psychologist)
            .WithMany(p => p.TimeSlots)
            .HasForeignKey(t => t.PsychologistId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PsychologistTimeSlot>()
            .HasOne(t => t.BookedByUser)
            .WithMany()
            .HasForeignKey(t => t.BookedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<PsychologistReview>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        // --- SEEDING ---

        var createdDate = DateTime.SpecifyKind(DateTime.Parse("2024-01-01T00:00:00"), DateTimeKind.Utc);

        // Seed: Practices
        modelBuilder.Entity<Practice>().HasData(
            new Practice { Id = 1, Name = "Дыхание 4-7-8", Description = "Техника успокоения через дыхание", Category = PracticeCategory.Breathing, DurationMinutes = 5, Instructions = "Вдох на 4 счета, задержка на 7, выдох на 8", IsActive = true },
            new Practice { Id = 2, Name = "Прогрессивная релаксация", Description = "Постепенное расслабление мышц", Category = PracticeCategory.Relaxation, DurationMinutes = 15, Instructions = "Напрягайте и расслабляйте каждую группу мышц", IsActive = true },
            new Practice { Id = 3, Name = "Визуализация безопасного места", Description = "Создание мысленного убежища", Category = PracticeCategory.Visualization, DurationMinutes = 10, Instructions = "Представьте место, где чувствуете себя в безопасности", IsActive = true },
            new Practice { Id = 4, Name = "КПТ: Работа с мыслями", Description = "Анализ и изменение негативных мыслей", Category = PracticeCategory.CBT, DurationMinutes = 20, Instructions = "Запишите мысль, оцените её реалистичность, найдите альтернативу", IsActive = true },
            new Practice { Id = 5, Name = "Медитация осознанности", Description = "Фокус на настоящем моменте", Category = PracticeCategory.Mindfulness, DurationMinutes = 10, Instructions = "Следите за дыханием, возвращайте внимание к настоящему", IsActive = true }
        );

        // Seed: Goals
        modelBuilder.Entity<Goal>().HasData(
            new Goal { Id = 1, Title = "Ежедневные практики", Description = "Выполнять хотя бы одну практику в день", Type = GoalType.Wellness, Progress = 30 },
            new Goal { Id = 2, Title = "Ведение дневника", Description = "Записывать мысли и эмоции каждый день", Type = GoalType.Personal, Progress = 60 },
            new Goal { Id = 3, Title = "Работа с тревогой", Description = "Применять техники КПТ при тревоге", Type = GoalType.Therapy, IsFromPsychologist = true, Progress = 25 }
        );

        //  Seed: Psychologists
        modelBuilder.Entity<Psychologist>().HasData(
            new Psychologist
            {
                Id = 1,
                Name = "Ирина Смирнова",
                IsActive = true,
                CreatedAt = createdDate
            },
            new Psychologist
            {
                Id = 2,
                Name = "Алексей Иванов",
                IsActive = true,
                CreatedAt = createdDate
            },
            new Psychologist
            {
                Id = 3,
                Name = "Мария Коваль",
                IsActive = true,
                CreatedAt = createdDate
            }
        );

        //  Seed: Reviews (фиксированные даты)
        modelBuilder.Entity<PsychologistReview>().HasData(
            new PsychologistReview
            {
                Id = 1,
                PsychologistId = 1,
                Rating = 5,
                Comment = "Отличный специалист!",
                CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2024-01-10T00:00:00"), DateTimeKind.Utc),
                IsVisible = true,
                IsApproved = true
            },
            new PsychologistReview
            {
                Id = 2,
                PsychologistId = 1,
                Rating = 4,
                Comment = "Профессиональный подход",
                CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2024-01-05T00:00:00"), DateTimeKind.Utc),
                IsVisible = true,
                IsApproved = true
            },
            new PsychologistReview
            {
                Id = 3,
                PsychologistId = 2,
                Rating = 5,
                Comment = "Помог решить семейные проблемы",
                CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2024-01-15T00:00:00"), DateTimeKind.Utc),
                IsVisible = true,
                IsApproved = true
            },
            new PsychologistReview
            {
                Id = 4,
                PsychologistId = 3,
                Rating = 5,
                Comment = "EMDR терапия действительно работает",
                CreatedAt = DateTime.SpecifyKind(DateTime.Parse("2024-01-20T00:00:00"), DateTimeKind.Utc),
                IsVisible = true,
                IsApproved = true
            }
        );
    }
}
