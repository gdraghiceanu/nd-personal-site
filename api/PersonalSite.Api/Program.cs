using PersonalSite.Api.Endpoints;
using PersonalSite.Api.Services;
using SendGrid.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// ── CORS ───────────────────────────────────────────────────────────────────────
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? [];

builder.Services.AddCors(opt =>
    opt.AddDefaultPolicy(policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()));

// ── Email ──────────────────────────────────────────────────────────────────────
builder.Services.AddSendGrid(opt =>
    opt.ApiKey = builder.Configuration["SendGrid:ApiKey"]
        ?? throw new InvalidOperationException("SendGrid:ApiKey is not configured."));

builder.Services.AddScoped<IEmailService, SendGridEmailService>();

// ── Swagger / OpenAPI ──────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new()
    {
        Title   = "PersonalSite API",
        Version = "v1",
        Description = "Backend API for Natalia Draghiceanu personal site"
    });
});

// ── Validation ─────────────────────────────────────────────────────────────────
builder.Services.AddProblemDetails();

var app = builder.Build();

// ── Middleware ─────────────────────────────────────────────────────────────────
app.UseExceptionHandler();
app.UseCors();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/swagger/v1/swagger.json", "PersonalSite API v1"));
}

// ── Health ─────────────────────────────────────────────────────────────────────
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
   .WithTags("Health")
   .WithName("HealthCheck")
   .ExcludeFromDescription();

// ── Endpoints ──────────────────────────────────────────────────────────────────
app.MapContactEndpoints();

app.Run();
