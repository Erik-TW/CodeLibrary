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
        public Task<bool> PutBook(Book book, string id);
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

        public async Task<bool> PutBook(Book book, string id)
        {
            if (!_context.Books.Any(b => b.Id == id))
                return false;

            _context.Books.Update(book);
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
                var highestId = _context.Books.Where(b => !string.IsNullOrEmpty(b.Id)).ToList().Max(b => ParsePrimaryKeyToInt(b.Id)); //Have to cast it to a list to ensure client side evaluation or my method cant be found

                if (highestId <= 0)
                    return false;

                highestId++;
                primaryKey = "B" + highestId;
                return true;
            }
        }

        public int ParsePrimaryKeyToInt(string value) //Cannot use a tryParse in a LINQ query, so making a method that has the desireable effect and is usable in queries.
        {
            if (string.IsNullOrEmpty(value))
                return -1;

            var stringToParse = value.Substring(1);
            if (int.TryParse(stringToParse, out int id))
                return id;
            else
                return -1;
        }
    }
}
