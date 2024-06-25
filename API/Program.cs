using API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddJsonFile("appsettings.json");

var connectionString = builder.Configuration.GetConnectionString("CRUD");

builder.Services.AddDbContext<CRUDbContext>(options =>
    options.UseSqlServer(connectionString));


var secretKey = builder.Configuration.GetSection("Setting").GetSection("Apitoken").Value;

if (string.IsNullOrEmpty(secretKey) || secretKey.Length < 32)
{
    throw new Exception("La clave del token API no está configurada correctamente o es demasiado corta. Debe tener al menos 32 caracteres.");
}

var keyBytes = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddCors(cors =>
{
    cors.AddPolicy("AllowAnyOrigin", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


//configuracion de la autenticacion JWT
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false,
        ValidateAudience = false
    };

    config.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            // Override the default behavior.
            context.HandleResponse();

            context.Response.StatusCode = 401;
            context.Response.Headers.Append("WWW-Authenticate", "Bearer");

            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAnyOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
