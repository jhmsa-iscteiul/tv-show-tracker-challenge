using System.Collections.Generic;

namespace TvShowTracker.Models
{
    public class Show
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime ReleaseDate { get; set; }  

        public ICollection<Genre> Genres { get; set; } = new List<Genre>();
        public ICollection<Episode> Episodes { get; set; } = new List<Episode>();
        public ICollection<Actor> Actors { get; set; } = new List<Actor>();
    }
}
