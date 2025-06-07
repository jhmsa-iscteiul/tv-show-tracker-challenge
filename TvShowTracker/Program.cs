using Microsoft.EntityFrameworkCore;
using TvShowTracker.Data;
using TvShowTracker.Services;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHostedService<TvMazeBackgroundService>();

// Register HttpClient for API calls
builder.Services.AddHttpClient();

builder.Services.AddControllers();

builder.Services.AddDbContext<TvShowTrackerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.WebHost.UseUrls("http://0.0.0.0:80");

var app = builder.Build();

app.UseRouting();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TvShowTrackerContext>();
    db.Database.Migrate();
}


app.Run();


