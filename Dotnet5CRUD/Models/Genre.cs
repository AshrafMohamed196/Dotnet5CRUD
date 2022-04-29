using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Dotnet5CRUD.Models
{
    public class Genre
    {
        [databasegenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        #region Relationships
        public List<Movie> movies { get; set; }
        #endregion Relationships
    }
}
