
using Microsoft.OpenApi.Models;
using OrderSystem.API.Filters;
using OrderSystem.Application.DependencyInjection;
using OrderSystem.Domain.Services;
using OrderSystem.Infrastructure;
using OrderSystem.Infrastructure.Data;


var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<BadRequestFilter>();
    options.Filters.Add<ValidationExceptionFilter>();
    options.Filters.Add<ConflictFilter>();
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("MinhaAppNextJS", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // URL do seu Next.js
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// --- INÍCIO DO BLOCO DE SEED ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var passwordService = services.GetRequiredService<IPasswordService>();

        // Chama o método estático que criamos
        DbInitializer.Seed(context, passwordService);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro ao popular o banco de dados.");
    }
}
// --- FIM DO BLOCO DE SEED ---


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// --- 2. Habilitar o CORS ---
// Importante: UseCors deve vir antes de UseAuthorization e depois de UseRouting
app.UseCors("MinhaAppNextJS");


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseStaticFiles(); // Isso libera o acesso à pasta wwwroot

app.Run();

