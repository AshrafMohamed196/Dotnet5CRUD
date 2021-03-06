using Dotnet5CRUD.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dotnet5CRUD.ViewModels
{
    public class MovieFormViewModel
    {
        public int Id { get; set; }
        [Required, StringLength(250)]
        public string Title { get; set; }

        public int Year { get; set; }

        [Range(1,10)]
        public double Rate { get; set; }

        [Required, StringLength(2500)]
        public string StoryLine { get; set; }
        public byte[] Poster { get; set; }

        #region Relationships
        // Remember to make it Restricked instead of cascade
        public byte GenreId { get; set; }
        public IEnumerable<Genre> genres { get; set; }
        #endregion

    }
}
