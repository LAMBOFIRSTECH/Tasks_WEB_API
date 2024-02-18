using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Tasks_WEB_API.Interfaces;
using Tasks_WEB_API.Models;
using Tasks_WEB_API.Repositories;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(con =>
{
    con.SwaggerDoc("1.0", new OpenApiInfo
    {
        Title = "DailyTasks | Api",
        Description = "An ASP.NET Core Web API for managing Tasks App",
        Version = "1.0",
        Contact = new OpenApiContact
        {
            Name = "Artur Lambo",
            Email = "lamboartur94@gmail.com"
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
    var xmlPath = Path.Combine(AppContext.BaseDirectory, "Tasks_WEB_API.xml");
    con.IncludeXmlComments(xmlPath);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5163",
                                              "https://localhost:7082");
                      });
});

builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddDbContext<DailyTasksMigrationsContext>(opt =>
{
    string conStrings = builder.Configuration.GetConnectionString("DefaultConnection");

    opt.UseInMemoryDatabase(conStrings);
});

builder.Services.AddScoped<IUtilisateurRepository,UtilisateurRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(con =>
    {
        con.SwaggerEndpoint("/swagger/1.0/swagger.json", "Daily Tasks Management API");

        con.RoutePrefix = string.Empty;
    });
}

app.UseCors(MyAllowSpecificOrigins);
var rewriteOptions = new RewriteOptions()
    .AddRewrite(  @"^www\.taskmoniroting/Taskmanagement",   "https://localhost:7082/index.html",true);
      

app.UseRewriter(rewriteOptions);
app.UseAuthorization();

app.MapControllers(); 
app.UseRouting();
app.Run();