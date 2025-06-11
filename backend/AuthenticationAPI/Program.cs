using AuthenticationAPI.Data;
using AuthenticationAPI.IRepository.IRepository;
using AuthenticationAPI.Models;
using AuthenticationAPI.Repository;
using AuthenticationAPI.Repository.IRepository;
using AuthenticationAPI.Service;
using AuthenticationAPI.util;
using Ecommerce.API.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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








//sql lite 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
   options.UseNpgsql(builder.Configuration.GetConnectionString("NeonConnection")));


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



// Add services and _repository  implementation
builder.Services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
builder.Services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ILeaveAllocationRepository, LeaveAllocationRepository>();
builder.Services.AddScoped<ILeaveAllocationService, CreateLeaveAllocationService>();

//service 
builder.Services.AddScoped<PayrollService>();
builder.Services.AddScoped<SeedSalaryForCategory>();
builder.Services.AddTransient<GenerateToken, TokenGenerator>();


//Generic _repository implementation 
builder.Services.AddScoped<IRepository<Department>, Repository<Department>>();
builder.Services.AddScoped<IRepository<Employee>, Repository<Employee>>();
builder.Services.AddScoped<IRepository<JobTitle>, Repository<JobTitle>>();
builder.Services.AddScoped<IRepository<LeaveType>, Repository<LeaveType>>();
builder.Services.AddScoped<IRepository<SalaryProgression>, Repository<SalaryProgression>>();
builder.Services.AddScoped<IRepository<CategoryGroup>, Repository<CategoryGroup>>();
builder.Services.AddScoped<IRepository<SalaryStep>, Repository<SalaryStep>>();
builder.Services.AddScoped<IRepository<Attendance>, Repository<Attendance>>();
builder.Services.AddScoped<IRepository<Allowance>, Repository<Allowance>>();
builder.Services.AddScoped<IRepository<Payrolls>, Repository<Payrolls>>();
builder.Services.AddScoped<IRepository<LeaveAllocation>, Repository<LeaveAllocation>>();
builder.Services.AddScoped<IRepository<Deduction>, Repository<Deduction>>();


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

//Middleware exceptions
app.UseExceptionHandlingMiddleware();
    
app.UseCors("AllowAll"); // Changed from "AllowFrontend" to match the policy name
//app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.Run();
