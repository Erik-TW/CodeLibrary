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
        public Task<Book> GetBook(string id);
        public Task<bool> PostBook(Book book);
        public Task<bool> PostBooks(List<Book> books);
        public Task<bool> PutBook(Book book);
    }
    public class BookService : IBookService
    {
        private CodeLibraryDbContext _context;
        public BookService(CodeLibraryDbContext context)
        {
            this._context = context;
        }

        public async Task<Book> GetBook(string id)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Book>> GetBooks()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<bool> PostBook(Book book)
        {
            if (book == null)
                return false;

            var pkExists = FindNextId(out string id);

            if (!pkExists)
                return false;

            book.Id = id;
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return true;
        }

        //I don't believe it is part of the assignment to have a POST endpoint for a list of books, but in order to speed up seeding the database i decided to make one.
        public async Task<bool> PostBooks(List<Book> books)
        {
            var pkExists = FindNextId(out string id);
            if (!pkExists)
                return false;
            if (!int.TryParse(id.Substring(1), out var index))
                return false;

            foreach (var book in books)
            {
                book.Id = "B" + index;
                _context.Books.Add(book);
                index++;
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PutBook(Book book)
        {
            if (await _context.Books.FirstOrDefaultAsync(b => b.Id == book.Id) == null)
                return false;

            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        private bool FindNextId(out string primaryKey)
        {
            primaryKey = "";
            if (!_context.Books.Any()) // Max can generate exceptions if there is no entries, so check if it's empty first.
            {
                primaryKey = "B1";
                return true;
            }
            else
            {
                var highestId = _context.Books.Max(b => b.Id);

                if (string.IsNullOrEmpty(highestId))
                    return false;


                if (int.TryParse(highestId.Substring(1), out var id))
                {
                    primaryKey = "B" + id;
                    return true;
                }
                return false;
            }
        }
    }
}
