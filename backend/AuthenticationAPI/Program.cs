using AuthenticationAPI.Data;
using AuthenticationAPI.IRepository.IRepository;
using AuthenticationAPI.Models;
using AuthenticationAPI.Repository;
using AuthenticationAPI.Repository.IRepository;
using AuthenticationAPI.Service;
using AuthenticationAPI.util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Payroll.Model;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("Authorization");
    });
});

var JWTSetting = builder.Configuration.GetSection("JWTSetting");

// Add services to the container.

//add database 
//sql server database
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//sql lite 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
      options.UseSqlite("Data Source = Hrdummy.db"));

//add identity role in DI container
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add authentication in DI container
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWTSetting:ValidAudience"],
        ValidIssuer = builder.Configuration["JWTSetting:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSetting:securityKey"]!))
    };
}).AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
});



// Add services and repository  implementation
builder.Services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
builder.Services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ILeaveAllocationRepository, LeaveAllocationRepository>();
builder.Services.AddScoped<ICreateLeaveAllocationService, CreateLeaveAllocationService>();

//service 
builder.Services.AddScoped<PayrollService>();
builder.Services.AddScoped<SeedSalaryForCategory>();
builder.Services.AddTransient<GenerateToken, TokenGenerator>();

//Generic repository implementation 
builder.Services.AddScoped<IRepository<Department>, Repository<Department>>();
builder.Services.AddScoped<IRepository<Employee>, Repository<Employee>>();
builder.Services.AddScoped<IRepository<JobTitle>, Repository<JobTitle>>();
builder.Services.AddScoped<IRepository<LeaveType>, Repository<LeaveType>>();
builder.Services.AddScoped<IRepository<SalaryProgression>, Repository<SalaryProgression>>();
builder.Services.AddScoped<IRepository<CategoryGroup>, Repository<CategoryGroup>>();
builder.Services.AddScoped<IRepository<SalaryStep>, Repository<SalaryStep>>();
builder.Services.AddScoped<IRepository<Attendance>, Repository<Attendance>>();

builder.Services.AddOpenApi();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(u =>
{
    u.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"Jwt Authorization Example: 'bearer [token]'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    u.AddSecurityRequirement(new OpenApiSecurityRequirement() {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"

                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
         .WithDarkMode(true)
        .WithDefaultHttpClient(ScalarTarget.Node, ScalarClient.HttpClient)
        .WithDarkModeToggle(true)
        .WithPreferredScheme("Bearer")
        .WithHttpBearerAuthentication(bearer =>
        {
            bearer.Token = "Bearer [token]";
        });
        options.Authentication = new ScalarAuthenticationOptions
        {
            PreferredSecurityScheme = "Bearer"
        };
    });
    // Remove HTTPS redirection in development
    //  app.UseHttpsRedirection();
}

// Use CORS before other middleware
app.UseCors("AllowAll"); // Changed from "AllowFrontend" to match the policy name
//app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// synchronize database with latest migration
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
      
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();
