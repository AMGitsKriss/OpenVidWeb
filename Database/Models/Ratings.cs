using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Database.Models
{
    public partial class Ratings
    {
        public Ratings()
        {
            Video = new HashSet<Video>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Video> Video { get; set; }
    }
}
