using System.Reflection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Tasks_WEB_API;
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

	var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	con.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

});

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: MyAllowSpecificOrigins,
					  policy =>
					  {
						  policy.WithOrigins("https://localhost:7082");
					  });
});

builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddDbContext<DailyTasksMigrationsContext>(opt =>
{
	string conStrings = builder.Configuration.GetConnectionString("DefaultConnection");

	opt.UseInMemoryDatabase(conStrings);
});

builder.Services.AddScoped<IReadUsersMethods, UtilisateurRepository>();
builder.Services.AddScoped<IWriteUsersMethods, UtilisateurRepository>();
builder.Services.AddScoped<IReadTasksMethods, TacheRepository>();
builder.Services.AddScoped<IWriteTasksMethods, TacheRepository>();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication("BasicAuthentication")
	.AddScheme<AuthenticationSchemeOptions, AuthentificationBasic>("BasicAuthentication", options => { });


builder.Services.AddAuthorization(options =>
 {
	 // Politique d'autorisation pour les administrateurs
	 options.AddPolicy("AdminPolicy", policy =>
		 policy.RequireRole(nameof(Utilisateur.Privilege.Admin))
			   .RequireAuthenticatedUser()
			   .AddAuthenticationSchemes("BasicAuthentication"));


	 // Politique d'autorisation pour les utilisateurs non-administrateurs
	 options.AddPolicy("UserPolicy", policy =>
		 policy.RequireRole(nameof(Utilisateur.Privilege.UserX))
			   .RequireAuthenticatedUser()  // L'utilisateur doit être authentifié
			   .AddAuthenticationSchemes("BasicAuthentication"));
 });

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
else if (app.Environment.IsProduction())
{

	// Gérer les erreurs dans un environnement de production
	app.UseExceptionHandler("/Error");
	app.UseHsts();

}

app.UseCors(MyAllowSpecificOrigins);
var rewriteOptions = new RewriteOptions()
	 .AddRewrite(@"^www\.taskmoniroting/Taskmanagement", "https://localhost:7082/index.html", true);
app.UseHttpsRedirection();
app.UseRewriter(rewriteOptions);
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
 {
	 endpoints.MapControllers();
 });
app.Run();