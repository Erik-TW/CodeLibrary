using CodeLibrary.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLibrary.Data.Context
{
    public class CodeLibraryDbContext : DbContext
    {

        public CodeLibraryDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Book> Books { get; set; }
     
    }
}
