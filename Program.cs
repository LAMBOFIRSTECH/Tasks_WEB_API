using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.OpenApi.Models;
using Tasks_WEB_API.Models;
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
            Email= "lamboartur94@gmail.com"
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

builder.Services.AddControllers();
builder.Services.AddDbContext<DailyTasksContext>(opt =>
     opt.UseInMemoryDatabase("Managed Tasks Database")); //Contexte de base de données
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(con =>
    {
        con.SwaggerEndpoint("/swagger/1.0/swagger.json", "Daily Tasks Management API");
        //con.SwaggerEndpoint("/swagger/1.1/swagger.json", "Tasks API");
        con.RoutePrefix = string.Empty;
    });
}
//app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();