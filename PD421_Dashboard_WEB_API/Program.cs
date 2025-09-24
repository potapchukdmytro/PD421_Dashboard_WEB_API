using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PD421_Dashboard_WEB_API.BLL.Services.Auth;
using PD421_Dashboard_WEB_API.BLL.Services.Genre;
using PD421_Dashboard_WEB_API.BLL.Settings;
using PD421_Dashboard_WEB_API.DAL;
using PD421_Dashboard_WEB_API.DAL.Entitites.Identity;
using PD421_Dashboard_WEB_API.DAL.Initializer;
using PD421_Dashboard_WEB_API.DAL.Repositories.Game;
using PD421_Dashboard_WEB_API.DAL.Repositories.Genre;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add dbcontext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultDb"));
});

// Add identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add automapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxNzg5NTE2ODAwIiwiaWF0IjoiMTc1ODAwNDY5MiIsImFjY291bnRfaWQiOiIwMTk5NTEzZTdlYmY3YjYwOGI4Y2I3NTI3YTE3ZTI5MyIsImN1c3RvbWVyX2lkIjoiY3RtXzAxazU4a3hoZXN2ZWI3aDZncms2MHBrYXJrIiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.OMUeI0YxSQYUSUYehr5O6yevTWgsGamrSrCFSZ7Sd3fNsl01WU-pr6M6wusxNSxoQ6w8-lqrjOk6gj8KShQQhmvz91wRuRm_rObvAaDQEBRDit7iSUe6J7EH8lDmpqlUuJQ8zN0lCTgIDwaHDaI9h4FcSVy6qmi68oETGI876KCUf5ifCCwDSpZjirIws5XvO6IpQEkCp8FWd2UkTWvrHaaJWFbxOWfKbx_j5AeHPE1o5Piiz7qF6QKX8MzOj44f0yRExRKMCeQSauqRBgO33CooOm0mxbU2-Mx5tb3PPHdaFe7YxPKdRYSJ1TsRn3DELSrxnKsPE11X4eIXYuJh6w";
}, AppDomain.CurrentDomain.GetAssemblies());

// Add repositories
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();

// Add services
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Add settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Seed();

app.Run();
