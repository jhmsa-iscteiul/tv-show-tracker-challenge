using System.Collections.Generic;

namespace TvShowTracker.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public ICollection<Show> Shows { get; set; } = new List<Show>();
    }
}
