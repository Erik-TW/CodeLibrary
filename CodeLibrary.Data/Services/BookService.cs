using CodeLibrary.Data.Context;
using CodeLibrary.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLibrary.Data.Services
{

    public interface IBookService
    {
        public Task<IEnumerable<Book>> GetBooks();
    }
    public class BookService : IBookService
    {
        private CodeLibraryDbContext _context;
        public BookService(CodeLibraryDbContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Book>> GetBooks()
        {
            return await _context.Books.ToListAsync();
        }
    }
}
