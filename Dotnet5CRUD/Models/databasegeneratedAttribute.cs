using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dotnet5CRUD.Models
{
    internal class databasegeneratedAttribute : Attribute
    {
        private DatabaseGeneratedOption identity;

        public databasegeneratedAttribute(DatabaseGeneratedOption identity)
        {
            this.identity = identity;
        }
    }
}