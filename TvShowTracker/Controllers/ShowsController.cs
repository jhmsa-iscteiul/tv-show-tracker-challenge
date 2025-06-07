using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TvShowTracker.Data;
using TvShowTracker.Dtos;

[ApiController]
[Route("api/[controller]")]
public class ShowsController : ControllerBase
{
    private readonly TvShowTrackerContext _context;

    public ShowsController(TvShowTrackerContext context)
    {
        _context = context;
    }

    // GET: api/shows?page=1&pageSize=10&sortBy=Name&sortOrder=asc
    [HttpGet]
    public async Task<IActionResult> GetAll(
        int page = 1,
        int pageSize = 10,
        string sortBy = "title",
        string sortOrder = "asc")
    {
        var query = _context.Shows
            .Include(s => s.Genres)
            .Include(s => s.Episodes)
            .Include(s => s.Actors)
            .AsQueryable();

        // Sorting
        query = (sortBy.ToLower(), sortOrder.ToLower()) switch
        {
            ("title", "asc") => query.OrderBy(s => s.Title),
            ("title", "desc") => query.OrderByDescending(s => s.Title),
            ("releasedate", "asc") => query.OrderBy(s => s.ReleaseDate),
            ("releasedate", "desc") => query.OrderByDescending(s => s.ReleaseDate),
            _ => query.OrderBy(s => s.Title)
        };

        var totalItems = await query.CountAsync();

        var shows = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new TvMazeShowDto
            {
                id = s.Id,
                name = s.Title,
                summary = s.Description,
                premiered = s.ReleaseDate.ToString("yyyy-MM-dd"),
                genres = s.Genres.Select(g => g.Name).ToList(),
                
            })
            .ToListAsync();

        return Ok(new
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            Data = shows
        });
    }



   [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var show = await _context.Shows
            .Include(s => s.Episodes)
            .Include(s => s.Genres)
            .Include(s => s.Actors)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (show == null)
            return NotFound();

        var showDto = new TvMazeShowDto
        {
            id = show.Id,
            name = show.Title,
            summary = show.Description,
            premiered = show.ReleaseDate.ToString("yyyy-MM-dd"),
            genres = show.Genres.Select(g => g.Name).ToList(),
        };

        // Add episodes
        var episodeDtos = show.Episodes.Select(e => new TvMazeEpisodeDto
        {
            id = e.Id,
            name = e.Title,
            airdate = e.ReleaseDate.ToString("yyyy-MM-dd") ?? "",  // Assuming AirDate is nullable DateTime?
            season = e.Season,
            number = e.EpisodeNumber
        }).ToList();

        // Add actors
        var castDtos = show.Actors.Select(a => new TvMazeCastDto
        {
            person = new TvMazePersonDto
            {
                name = a.Name
            }
        }).ToList();

        // Return combined data as an anonymous object
        return Ok(new
        {
            show = showDto,
            episodes = episodeDtos,
            cast = castDtos
        });
    }

    // GET: api/shows/bygenre/comedy?page=1&pageSize=10
    [HttpGet("bygenre/{genreName}")]
    public async Task<IActionResult> GetByGenre(string genreName, int page = 1, int pageSize = 10)
    {
        var query = _context.Shows
            .Where(s => s.Genres.Any(g => g.Name == genreName));

        var totalItems = await query.CountAsync();

        var shows = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new
            {
                s.Id,
                s.Title,
                s.Description,
                s.ReleaseDate
                // any other Show properties you want to return,
                //Genres = s.Genres.Select(g => new { g.Id, g.Name }) // minimal genre info only
            })
            .ToListAsync();

        return Ok(new
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            Data = shows
        });
    }


    // GET: api/shows/bytype/drama?page=1&pageSize=10
    // [HttpGet("bytype/{type}")]
    // public async Task<IActionResult> GetByType(string type, int page = 1, int pageSize = 10)
    // {
    //     var query = _context.Shows.Where(s => s.Type == type);

    //     var totalItems = await query.CountAsync();

    //     var shows = await query
    //         .Skip((page - 1) * pageSize)
    //         .Take(pageSize)
    //         .ToListAsync();

    //     return Ok(new {
    //         Page = page,
    //         PageSize = pageSize,
    //         TotalItems = totalItems,
    //         Data = shows
    //     });
    // }

    // GET: api/shows/5/actors
    [HttpGet("{id}/actors")]
    public async Task<IActionResult> GetActors(int id)
    {
        var showExists = await _context.Shows.AnyAsync(s => s.Id == id);
        if (!showExists)
            return NotFound();

        var actors = await _context.Actors
            .Where(a => a.Shows.Any(s => s.Id == id))
            .Select(a => new
            {
                a.Id,
                a.Name,
                // include only properties you want to expose, no navigation props!
            })
            .ToListAsync();

        return Ok(actors);
    }
}
