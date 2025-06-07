using System.Collections.Generic;

namespace TvShowTracker.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public ICollection<Show> Favorites { get; set; } = new List<Show>();
    }
}
