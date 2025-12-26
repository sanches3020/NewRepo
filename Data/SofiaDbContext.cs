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
    // Tests / assessments
    public DbSet<Test> Tests { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<UserAnswer> UserAnswers { get; set; }
    public DbSet<TestResult> TestResults { get; set; }

    // Interpretation thresholds per test (percent-based)
    public DbSet<TestInterpretation> TestInterpretations { get; set; }

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

        // --- SEED: Built-in Tests (Depression, Anxiety, Stress) ---
        modelBuilder.Entity<Test>().HasData(
            new Test { Id = 1001, Name = "Шкала депрессии (PHQ-2)", Description = "Краткий скрининг симптомов депрессии", Type = Models.TestType.BuiltIn },
            new Test { Id = 1002, Name = "Шкала тревожности (GAD-2)", Description = "Краткий скрининг тревожности", Type = Models.TestType.BuiltIn },
            new Test { Id = 1003, Name = "Шкала стресса (PSS-4)", Description = "Краткая шкала восприятия стресса", Type = Models.TestType.BuiltIn }
        );

        // Questions for PHQ-2 (1001)
        modelBuilder.Entity<Question>().HasData(
            new Question { Id = 2001, TestId = 1001, Text = "За последние 2 недели: испытывали ли вы слабый интерес или удовольствие от занятий?" , Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2002, TestId = 1001, Text = "За последние 2 недели: чувствовали ли вы себя подавленным/унылым?" , Type = Models.AnswerType.SingleChoice}
        );

        // Questions for GAD-2 (1002)
        modelBuilder.Entity<Question>().HasData(
            new Question { Id = 2101, TestId = 1002, Text = "За последние 2 недели: сколько вы беспокоились и тревожились?" , Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2102, TestId = 1002, Text = "За последние 2 недели: было ли вам трудно перестать беспокоиться или контролировать беспокойство?" , Type = Models.AnswerType.SingleChoice}
        );

        // Questions for PSS-4 (1003)
        modelBuilder.Entity<Question>().HasData(
            new Question { Id = 2201, TestId = 1003, Text = "На прошлой неделе вы чувствовали, что не в состоянии контролировать важные вещи в жизни?" , Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2202, TestId = 1003, Text = "На прошлой неделе вы чувствовали напряжение и нервозность?" , Type = Models.AnswerType.SingleChoice},
            new Question { Id = 2203, TestId = 1003, Text = "На прошлой неделе вы чувствовали, что справляетесь с личными проблемами?" , Type = Models.AnswerType.SingleChoice}
        );

        // Answers (scale 0-3) for PHQ-2
        modelBuilder.Entity<Answer>().HasData(
            new Answer { Id = 3001, QuestionId = 2001, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 3002, QuestionId = 2001, Text = "Несколько дней", Value = 1, Order = 1 },
            new Answer { Id = 3003, QuestionId = 2001, Text = "Более половины дней", Value = 2, Order = 2 },
            new Answer { Id = 3004, QuestionId = 2001, Text = "Почти каждый день", Value = 3, Order = 3 },

            new Answer { Id = 3011, QuestionId = 2002, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 3012, QuestionId = 2002, Text = "Несколько дней", Value = 1, Order = 1 },
            new Answer { Id = 3013, QuestionId = 2002, Text = "Более половины дней", Value = 2, Order = 2 },
            new Answer { Id = 3014, QuestionId = 2002, Text = "Почти каждый день", Value = 3, Order = 3 }
        );

        // Answers for GAD-2
        modelBuilder.Entity<Answer>().HasData(
            new Answer { Id = 3101, QuestionId = 2101, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 3102, QuestionId = 2101, Text = "Несколько дней", Value = 1, Order = 1 },
            new Answer { Id = 3103, QuestionId = 2101, Text = "Более половины дней", Value = 2, Order = 2 },
            new Answer { Id = 3104, QuestionId = 2101, Text = "Почти каждый день", Value = 3, Order = 3 },

            new Answer { Id = 3111, QuestionId = 2102, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 3112, QuestionId = 2102, Text = "Несколько дней", Value = 1, Order = 1 },
            new Answer { Id = 3113, QuestionId = 2102, Text = "Более половины дней", Value = 2, Order = 2 },
            new Answer { Id = 3114, QuestionId = 2102, Text = "Почти каждый день", Value = 3, Order = 3 }
        );

        // Answers for PSS-4 (we'll use 0-4 and normalize in interpretation)
        modelBuilder.Entity<Answer>().HasData(
            new Answer { Id = 3201, QuestionId = 2201, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 3202, QuestionId = 2201, Text = "Почти никогда", Value = 1, Order = 1 },
            new Answer { Id = 3203, QuestionId = 2201, Text = "Иногда", Value = 2, Order = 2 },
            new Answer { Id = 3204, QuestionId = 2201, Text = "Часто", Value = 3, Order = 3 },
            new Answer { Id = 3205, QuestionId = 2201, Text = "Очень часто", Value = 4, Order = 4 },

            new Answer { Id = 3211, QuestionId = 2202, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 3212, QuestionId = 2202, Text = "Почти никогда", Value = 1, Order = 1 },
            new Answer { Id = 3213, QuestionId = 2202, Text = "Иногда", Value = 2, Order = 2 },
            new Answer { Id = 3214, QuestionId = 2202, Text = "Часто", Value = 3, Order = 3 },
            new Answer { Id = 3215, QuestionId = 2202, Text = "Очень часто", Value = 4, Order = 4 },

            new Answer { Id = 3221, QuestionId = 2203, Text = "Никогда", Value = 0, Order = 0 },
            new Answer { Id = 3222, QuestionId = 2203, Text = "Почти никогда", Value = 1, Order = 1 },
            new Answer { Id = 3223, QuestionId = 2203, Text = "Иногда", Value = 2, Order = 2 },
            new Answer { Id = 3224, QuestionId = 2203, Text = "Часто", Value = 3, Order = 3 },
            new Answer { Id = 3225, QuestionId = 2203, Text = "Очень часто", Value = 4, Order = 4 }
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
