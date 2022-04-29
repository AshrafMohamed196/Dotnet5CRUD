using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dotnet5CRUD.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required, MaxLength(250)]
        public string Title { get; set; }

        public int Year { get; set; }
    
        public double Rate { get; set; }

        [Required, MaxLength(2500)]
        public string StoryLine { get; set; }

        [Required,Display(Name ="Select Poster...")]
        public byte[] Poster { get; set; }

        #region Relationships
        // Remember to make it Restricked instead of cascade
        public byte GenreId { get; set; }
        public Genre Genre { get; set; }

        #endregion Relationships






    }
}
