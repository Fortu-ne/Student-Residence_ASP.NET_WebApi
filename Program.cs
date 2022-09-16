using JulyProject.Repositroy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using WepApiWithToken;
using WepApiWithToken.Authentication;
using WepApiWithToken.Helper;
using WepApiWithToken.Interface;
using WepApiWithToken.Model;
using WepApiWithToken.Repository;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped(typeof(IAuthentication<>), typeof(AuthRep<>));



builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<IServiceManager, ServiceRep>();
builder.Services.AddScoped<IManager, ManagerRep>();
builder.Services.AddScoped<IStudent, StudentRep>();

builder.Services.AddTransient<Seed>();

builder.Services.AddAutoMapper(typeof(MappingProfiles));


builder.Services.AddScoped<IAddress, AddressRep>();
builder.Services.AddScoped<ICourse, CourseRep>();
builder.Services.AddScoped<IGender, GenderRep>();
builder.Services.AddScoped<IMaintenance, MaintenanceRep>();
builder.Services.AddScoped<IRooms, RoomRep>();
builder.Services.AddScoped<IMaintenancetype, MaintenanceTypeRep>();
builder.Services.AddScoped<IStatus, StatusRep>();
builder.Services.AddScoped<ICleaning, CleaningRep>();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
    {
        Description = "Standard Authorizattion header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey

    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    }
    );

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<Seed>();
        service.SeedDataContext();
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy => policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
