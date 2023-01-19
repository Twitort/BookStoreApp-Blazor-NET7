using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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

app.UseHttpsRedirection();

// Apply the AllowAll policy defined above:
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
