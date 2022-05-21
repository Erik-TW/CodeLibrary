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
        public Task<IEnumerable<Book>> GetBooksSortedById();
        public Task<IEnumerable<Book>> GetBooksFilteredById(string searchValue);
        public Task<IEnumerable<Book>> GetBooksSortedByAuthor();
        public Task<IEnumerable<Book>> GetBooksFilteredByAuthor(string searchValue);
        public Task<IEnumerable<Book>> GetBooksSortedByTitle();
        public Task<IEnumerable<Book>> GetBooksFilteredByTitle(string searchValue);
        public Task<IEnumerable<Book>> GetBooksSortedByGenre();
        public Task<IEnumerable<Book>> GetBooksFilteredByGenre(string searchValue);
        public Task<IEnumerable<Book>> GetBooksSortedByDescription();
        public Task<IEnumerable<Book>> GetBooksFilteredByDescription(string searchValue);
        public Task<IEnumerable<Book>> GetBooksSortedByPublishedDate();
        public Task<IEnumerable<Book>> GetBooksFilteredByPublishedDate(int year, int month, int day);
        public Task<IEnumerable<Book>> GetBooksSortedByPrice();
        public Task<IEnumerable<Book>> GetBooksFilteredByPrice(double price);
        public Task<IEnumerable<Book>> GetBooksWithinPriceRange(double minPrice, double maxPrice);
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

        public async Task<IEnumerable<Book>> GetBooksSortedById()
        {
            var books = await _context.Books.ToListAsync(); //cast to list so the expression can be evaluated client side and Parse method can be run
            return books.OrderBy(b => ParsePrimaryKeyToInt(b.Id));
        }

        public async Task<IEnumerable<Book>> GetBooksFilteredById(string searchValue)
        {
            
            var books = await _context.Books.Where(b => !string.IsNullOrEmpty(b.Id) && b.Id.ToLower().Contains(searchValue.ToLower())).ToListAsync(); //cast to list so the expression can be evaluated client side and Parse method can be run
            return books.OrderBy(b => ParsePrimaryKeyToInt(b.Id));
        }

        public async Task<IEnumerable<Book>> GetBooksSortedByAuthor()
        {
            return await _context.Books.OrderBy(b => b.Author).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksFilteredByAuthor(string searchValue)
        {
            return await _context.Books.Where(b => !string.IsNullOrEmpty(b.Author) && b.Author.ToLower().Contains(searchValue.ToLower())).OrderBy(b => b.Author).ToListAsync();
        }
        public async Task<IEnumerable<Book>> GetBooksSortedByTitle()
        {
            return await _context.Books.OrderBy(b => b.Title).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksFilteredByTitle(string searchValue)
        {
            return await _context.Books.Where(b => !string.IsNullOrEmpty(b.Title) && b.Title.ToLower().Contains(searchValue.ToLower())).OrderBy(b => b.Title).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksSortedByGenre()
        {
            return await _context.Books.OrderBy(b => b.Genre).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksFilteredByGenre(string searchValue)
        {
            return await _context.Books.Where(b => !string.IsNullOrEmpty(b.Genre) && b.Genre.ToLower().Contains(searchValue.ToLower())).OrderBy(b => b.Genre).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksSortedByDescription()
        {
            return await _context.Books.OrderBy(b => b.Description).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksFilteredByDescription(string searchValue)
        {
            return await _context.Books.Where(b => !string.IsNullOrEmpty(b.Description) && b.Description.ToLower().Contains(searchValue.ToLower())).OrderBy(b => b.Description).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksSortedByPublishedDate()
        {
            return await _context.Books.OrderBy(b => b.Publish_Date).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksFilteredByPublishedDate(int year, int month, int day)
        {
            var date = new DateTime(year, month, day);
            return await _context.Books.Where(b => b.Publish_Date.HasValue && b.Publish_Date.Value >= date).OrderBy(b => b.Publish_Date).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksSortedByPrice()
        {
            return await _context.Books.OrderBy(b => b.Price).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksFilteredByPrice(double price)
        {
            return await _context.Books.Where(b => b.Price == price).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksWithinPriceRange(double minPrice, double maxPrice)
        {
            return await _context.Books.Where(b => b.Price >= minPrice && b.Price <= maxPrice).OrderBy(b => b.Price).ToListAsync();
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
