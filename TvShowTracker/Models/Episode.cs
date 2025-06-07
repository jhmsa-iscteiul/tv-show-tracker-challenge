using System;

namespace TvShowTracker.Models
{
    public class Episode
    {
        public int Id { get; set; }
        public int ShowId { get; set; }
        public required Show Show { get; set; }

        public required string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Season { get; set; }
        public int EpisodeNumber { get; set; }
    }
}
