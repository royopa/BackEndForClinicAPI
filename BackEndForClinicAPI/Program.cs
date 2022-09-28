using BackEndForClinicAPI.Data;
using BackEndForClinicAPI.Helpers;
using BackEndForClinicAPI.Interfaces;
using BackEndForClinicAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMvc();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

//builder.Services.AddDbContext<BackEndForClinicAPIDBContext>(options => options.UseInMemoryDatabase("ClinicDB"));
builder.Services.AddDbContext<BackEndForClinicAPIDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BackEndForClinicAPI")));
builder.Services.AddSingleton<AuthenticationAndAuthorizationHelpers>();
builder.Services.AddSingleton<ConcurrentDictionary<string, string>>();

//dependency injections for controllers
builder.Services.AddScoped<IDoctorInterface, DoctorService>();
builder.Services.AddScoped<IPatientInterface, PatientService>();
builder.Services.AddScoped<IAppointmentInterface, AppointmentService>();
builder.Services.AddScoped<IAdminInterface, AdminService>();

builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapRazorPages();
});

app.MapControllers();

app.Run();
