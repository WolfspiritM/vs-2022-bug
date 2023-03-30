using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(x => x.AddDefaultPolicy(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

var app = builder.Build();

app.UseCors();
app.MapGet("/", () => Results.Ok(typeof(Program).Assembly.FullName));


var logger = app.Services.GetService<ILogger<Program>>();
for (var i = 0; i < 100; i++)
{
	logger.LogInformation("---------------------------------------------------------------------------------------------------------");
}


app.Run();