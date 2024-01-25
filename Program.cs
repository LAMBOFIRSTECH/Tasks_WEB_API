using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(con =>{
    con.SwaggerDoc("1.0",new OpenApiInfo{
        Title="TasksManagement Api",
        Description=" Description : APi de gestion de tache",
        Version="1.0",
        Contact= new OpenApiContact{
            Name="Artur Lambo",
            Email="lamboartur94@gmail.com"
        }
    });
    var xmlPath=Path.Combine(AppContext.BaseDirectory,"Tasks_WEB_API.xml");
    con.IncludeXmlComments(xmlPath);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("http://localhost:5163",
                                              "https://localhost:7082");
                      });
});

builder.Services.AddControllers();
// builder.Services.AddDbContext<TodoContext>(opt =>
//     opt.UseInMemoryDatabase("TodoList")); Contexte de base de données
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(con => {
        con.SwaggerEndpoint("/swagger/1.0/swagger.json","Tasks Management API");
        con.SwaggerEndpoint("/swagger/1.1/swagger.json","Tasks API");
        con.RoutePrefix=string.Empty;
    });
}

//app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();

