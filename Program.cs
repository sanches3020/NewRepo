using Microsoft.EntityFrameworkCore;
using Sofia.Web.Data;
using Sofia.Web.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Entity Framework with SQLite
builder.Services.AddDbContext<SofiaDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthorization();

// Ensure database is created and seed data is applied
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SofiaDbContext>();
    context.Database.EnsureCreated();
    
    // Ensure seed practices exist and are active
    if (!context.Practices.Any())
    {
        // If no practices exist, add seed data
        context.Practices.AddRange(
            new Practice { Id = 1, Name = "Дыхание 4-7-8", Description = "Техника успокоения через дыхание", Category = PracticeCategory.Breathing, DurationMinutes = 5, Instructions = "Вдох на 4 счета, задержка на 7, выдох на 8", IsActive = true },
            new Practice { Id = 2, Name = "Прогрессивная релаксация", Description = "Постепенное расслабление мышц", Category = PracticeCategory.Relaxation, DurationMinutes = 15, Instructions = "Напрягайте и расслабляйте каждую группу мышц", IsActive = true },
            new Practice { Id = 3, Name = "Визуализация безопасного места", Description = "Создание мысленного убежища", Category = PracticeCategory.Visualization, DurationMinutes = 10, Instructions = "Представьте место, где чувствуете себя в безопасности", IsActive = true },
            new Practice { Id = 4, Name = "КПТ: Работа с мыслями", Description = "Анализ и изменение негативных мыслей", Category = PracticeCategory.CBT, DurationMinutes = 20, Instructions = "Запишите мысль, оцените её реалистичность, найдите альтернативу", IsActive = true },
            new Practice { Id = 5, Name = "Медитация осознанности", Description = "Фокус на настоящем моменте", Category = PracticeCategory.Mindfulness, DurationMinutes = 10, Instructions = "Следите за дыханием, возвращайте внимание к настоящему", IsActive = true }
        );
        context.SaveChanges();
    }
    else
    {
        // Update existing seed practices to be active
        var practices = context.Practices.Where(p => p.Id >= 1 && p.Id <= 5).ToList();
        foreach (var practice in practices)
        {
            practice.IsActive = true;
        }
        if (practices.Any())
        {
            context.SaveChanges();
        }
    }
    
    // Ensure all psychologists have schedules and time slots
    var psychologists = context.Psychologists.Where(p => p.IsActive).ToList();
    var random = new Random();
    
    foreach (var psychologist in psychologists)
    {
        // Проверяем, есть ли у психолога расписание
        var hasSchedule = context.PsychologistSchedules.Any(s => s.PsychologistId == psychologist.Id && s.IsAvailable);
        
        if (!hasSchedule)
        {
            // Создаем базовое расписание для психолога (понедельник-пятница, 10:00-18:00)
            var schedules = new List<PsychologistSchedule>();
            for (int day = 1; day <= 5; day++) // Понедельник-Пятница
            {
                schedules.Add(new PsychologistSchedule
                {
                    PsychologistId = psychologist.Id,
                    DayOfWeek = (DayOfWeek)day,
                    StartTime = new TimeSpan(10, 0, 0),
                    EndTime = new TimeSpan(18, 0, 0),
                    IsAvailable = true,
                    CreatedAt = DateTime.Now
                });
            }
            context.PsychologistSchedules.AddRange(schedules);
            context.SaveChanges();
        }
        
        // Генерируем слоты для всех психологов (3-7 слотов на ближайшие 2 недели)
        var startDate = DateTime.Today.AddDays(1);
        var endDate = startDate.AddDays(14);
        
        var existingSlotsCount = context.PsychologistTimeSlots
            .Count(t => t.PsychologistId == psychologist.Id && 
                       t.Date.Date >= startDate.Date && 
                       t.Date.Date <= endDate.Date &&
                       t.IsAvailable && !t.IsBooked);
        
        // Если у психолога меньше 3 доступных слотов, создаем новые (минимум 3, максимум 7)
        if (existingSlotsCount < 3)
        {
            var schedules = context.PsychologistSchedules
                .Where(s => s.PsychologistId == psychologist.Id && s.IsAvailable)
                .ToList();
            
            if (schedules.Any())
            {
                // Определяем, сколько слотов нужно создать (минимум 3, максимум 7)
                var slotsToCreate = Math.Max(3, Math.Min(7 - existingSlotsCount, 7));
                var newSlots = new List<PsychologistTimeSlot>();
                var createdCount = 0;
                var attempts = 0;
                const int maxAttempts = 100; // Защита от бесконечного цикла
                
                // Собираем все возможные даты и времена
                var possibleSlots = new List<(DateTime date, TimeSpan time)>();
                for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                {
                    var daySchedule = schedules.FirstOrDefault(s => s.DayOfWeek == date.DayOfWeek);
                    if (daySchedule != null)
                    {
                        var currentTime = daySchedule.StartTime;
                        while (currentTime < daySchedule.EndTime)
                        {
                            possibleSlots.Add((date, currentTime));
                            currentTime = currentTime.Add(TimeSpan.FromHours(1));
                        }
                    }
                }
                
                // Перемешиваем список для случайного выбора
                possibleSlots = possibleSlots.OrderBy(x => random.Next()).ToList();
                
                // Создаем слоты
                foreach (var (date, time) in possibleSlots)
                {
                    if (createdCount >= slotsToCreate) break;
                    attempts++;
                    if (attempts > maxAttempts) break;
                    
                    // Проверяем, нет ли уже такого слота
                    var existingSlot = context.PsychologistTimeSlots
                        .Any(t => t.PsychologistId == psychologist.Id &&
                                 t.Date.Date == date &&
                                 t.StartTime == time);
                    
                    if (!existingSlot)
                    {
                        newSlots.Add(new PsychologistTimeSlot
                        {
                            PsychologistId = psychologist.Id,
                            Date = date,
                            StartTime = time,
                            EndTime = time.Add(TimeSpan.FromHours(1)),
                            IsAvailable = true,
                            IsBooked = false,
                            CreatedAt = DateTime.Now
                        });
                        createdCount++;
                    }
                }
                
                if (newSlots.Any())
                {
                    context.PsychologistTimeSlots.AddRange(newSlots);
                    context.SaveChanges();
                }
            }
        }
    }
}

app.MapStaticAssets();

// Root redirect to home
app.MapGet("/", () => Results.Redirect("/home"));

// explicit home shortcut
app.MapControllerRoute(
    name: "home",
    pattern: "home",
    defaults: new { controller = "Home", action = "Index" })
    .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
