using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using TvShowTracker.Data;
using TvShowTracker.Models;
using Microsoft.EntityFrameworkCore;
using TvShowTracker.Dtos;

namespace TvShowTracker.Services
{
    public class TvMazeBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly HttpClient _httpClient;

        public TvMazeBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _httpClient = new HttpClient();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TvShowTrackerContext>();

            int page = 0;
            //while (!stoppingToken.IsCancellationRequested)
            //{
            while (page < 1)
            {


                var allShows = await _httpClient.GetFromJsonAsync<List<TvMazeShowDto>>($"https://api.tvmaze.com/shows?page={page}");

                var shows = allShows?.Take(10).ToList();

                if (shows == null || shows.Count == 0) break;

                foreach (var showDto in shows)
                {

                    // Parse release date
                    DateTime releaseDate = DateTime.TryParse(showDto.premiered, out var date) ? date : DateTime.MinValue;

                    // Check if show with same Title and ReleaseDate exists
                    bool showExists = await db.Shows.AnyAsync(s => s.Title == showDto.name && s.ReleaseDate == releaseDate);

                    if (showExists) continue;

                    //if (await db.Shows.AnyAsync(s => s.Id == showDto.id)) continue;

                    var show = new Show
                    {
                        //Id = showDto.id,
                        Title = showDto.name,
                        Description = showDto.summary ?? "No description",
                        ReleaseDate = DateTime.TryParse(showDto.premiered, out var date2) ? date2 : DateTime.MinValue
                    };

                    // Add genres
                    foreach (var genreName in showDto.genres.Distinct())
                    {
                        var genre = await db.Genres.FirstOrDefaultAsync(g => g.Name == genreName)
                            ?? new Genre { Name = genreName };

                        show.Genres.Add(genre);
                    }

                    // Get episodes
                    var episodes = await _httpClient.GetFromJsonAsync<List<TvMazeEpisodeDto>>($"https://api.tvmaze.com/shows/{showDto.id}/episodes");
                    if (episodes != null)
                    {
                        foreach (var ep in episodes)
                        {
                            show.Episodes.Add(new Episode
                            {
                                //Id = ep.id,
                                Title = ep.name,
                                ReleaseDate = DateTime.TryParse(ep.airdate, out var epDate) ? epDate : DateTime.MinValue,
                                Season = ep.season,
                                EpisodeNumber = ep.number,
                                Show = show
                            });
                        }
                    }

                    // Get cast
                    var cast = await _httpClient.GetFromJsonAsync<List<TvMazeCastDto>>($"https://api.tvmaze.com/shows/{showDto.id}/cast");
                    if (cast != null)
                    {
                        foreach (var c in cast)
                        {
                            var name = c.person.name;
                            var actor = await db.Actors.FirstOrDefaultAsync(a => a.Name == name)
                                ?? new Actor { Name = name };

                            show.Actors.Add(actor);
                        }
                    }

                    db.Shows.Add(show);
                    await db.SaveChangesAsync();
                }
                page++;
            //}
            }
        }
    }
}
