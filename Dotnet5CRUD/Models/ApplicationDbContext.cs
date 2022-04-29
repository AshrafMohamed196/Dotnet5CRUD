using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dotnet5CRUD.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }


        #region Add Domain Classes to map to database
        public DbSet<Genre> genres { get; set; }
        public DbSet<Movie> movies { get; set; }
        #endregion 
    }
}
