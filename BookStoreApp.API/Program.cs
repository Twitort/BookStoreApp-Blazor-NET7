using BookStoreApp.API.Configurations;
using BookStoreApp.API.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

var useless = new DateTime();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connString = builder.Configuration.GetConnectionString("BookStoreAppDbConnection");
builder.Services.AddDbContext<BookStoreDbContext>(options => options.UseSqlServer(connString));

builder.Services.AddAutoMapper(typeof(MapperConfig));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure logging using Serilog:
builder.Host.UseSerilog((ctx, lc) => 
	lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

// Define a CORS policy to allow requests from other origins (sites):
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll",
		b => b.AllowAnyMethod().
		AllowAnyHeader().
		AllowAnyOrigin());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// A simple change to test the branch switching.

app.UseHttpsRedirection();

// Apply the AllowAll policy defined above:
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
