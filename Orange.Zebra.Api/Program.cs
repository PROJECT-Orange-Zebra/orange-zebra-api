using Orange.Zebra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Orange.Zebra.Api.Security;

var builder = WebApplication.CreateBuilder(args);

string authority = builder.Configuration["Auth0:Authority"] ??
		throw new ArgumentNullException("Auth0:Authority");
	
	string audience = builder.Configuration["Auth0:Audience"] ??
		throw new ArgumentNullException("Auth0:Audience");

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAuthentication(options =>
	{
		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	})
	.AddJwtBearer(options =>
	{
		options.Authority = authority;
		options.Audience = audience;
    });

builder.Services.AddAuthentication(options =>
	{
		options.AddPolicy("delete:catalog", policy =>
			policy.RequireAuthenticatedUser().RequiredClaim("scope", "delete:catalog"));
	});

builder.Services.AddDbContext<StoreContext>(options => options.UseSqlite("Data Source = ../Registrar.sqlite"
, b => b.MigrationsAssembly("Orange.Zebra.Api")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddCors(options =>
	{
		options.AddDefautPolicy(builder =>
		{
			builder.WithOrigins("https://localhost:3000")
				.AllowAnyHeader()
				.AllowAnyMethod();
		});
	});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
