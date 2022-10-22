using SalesLiftPOC.Models;
using Microsoft.EntityFrameworkCore;
using SalesLiftPOC.Repository;
using SalesLiftPOC.Interface;
using SalesLiftPOC.Middleware;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);
var policy = new AuthorizationPolicyBuilder()
  .RequireAuthenticatedUser()
  .RequireRole("User")
  .Build();

// Add services to the container.

builder.Services.AddControllers();

//registering of depencency
builder.Services.AddScoped<IJobsRepository, JobsRepository>();

// get connection string to appsettings.json file. 
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

// registering a policy
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("ShouldBeEmployee", policy => policy.RequireRole("User"));
//});

/* To enable the authentication, we need to call AddSecurityDefinition and AddSecurityRequirement functions
by initiating OpenApiSecurityScheme and OpenApiSecurityRequirement classes respectively.*/
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Key Auth", Version = "v1" });
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "ApiKey must appear in header",
        Type = SecuritySchemeType.ApiKey,
        Name = "XApiKey",
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });
    var key = new OpenApiSecurityScheme()
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };
    var requirement = new OpenApiSecurityRequirement
                    {
                             { key, new List<string>() }
                    };
    c.AddSecurityRequirement(requirement);
   
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ApiKeyMiddleware>();
// checks if db is created or not. If not created creates one.
// If already exits then does nothing.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // use context
    dbContext.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
