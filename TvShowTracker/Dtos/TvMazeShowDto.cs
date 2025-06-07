namespace TvShowTracker.Dtos
{
    public class TvMazeShowDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string? summary { get; set; }
        public string? premiered { get; set; }
        public List<string> genres { get; set; } = new();
    }

    public class TvMazeEpisodeDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string airdate { get; set; }
        public int season { get; set; }
        public int number { get; set; }
    }

    public class TvMazeCastDto
    {
        public TvMazePersonDto person { get; set; }
    }

    public class TvMazePersonDto
    {
        public string name { get; set; }
    }
}
