using Microsoft.EntityFrameworkCore;
using TaskTrackerApi.Data;
using TaskTrackerApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default") ?? "Data Source=tasks.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Mini Task Tracker API", Version = "v1" });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();

var tasks = app.MapGroup("/api/tasks").WithTags("Tasks");

tasks.MapGet("/", async (AppDbContext db) =>
    await db.Tasks.OrderBy(t => t.DueDate).ToListAsync());

tasks.MapGet("/{id:int}", async (int id, AppDbContext db) =>
    await db.Tasks.FindAsync(id) is TaskItem task ? Results.Ok(task) : Results.NotFound());

tasks.MapPost("/", async (TaskItem input, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(input.Title))
        return Results.BadRequest(new { error = "Le titre est obligatoire." });

    var task = new TaskItem
    {
        Title = input.Title,
        Description = input.Description,
        DueDate = input.DueDate,
        IsDone = false
    };

    db.Tasks.Add(task);
    await db.SaveChangesAsync();

    return Results.Created($"/api/tasks/{task.Id}", task);
});

tasks.MapPut("/{id:int}", async (int id, TaskItem input, AppDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task is null) return Results.NotFound();

    task.Title = input.Title;
    task.Description = input.Description;
    task.DueDate = input.DueDate;
    task.IsDone = input.IsDone;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

tasks.MapDelete("/{id:int}", async (int id, AppDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task is null) return Results.NotFound();

    db.Tasks.Remove(task);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();