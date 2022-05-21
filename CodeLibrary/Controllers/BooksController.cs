using CodeLibrary.Data.Models;
using CodeLibrary.Data.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CodeLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }


        [HttpGet]
        public async Task<IEnumerable<Book>> GetBooks()
        {
            return await _bookService.GetBooks();
        }

        [HttpGet("id")]
        public async Task<IEnumerable<Book>> GetBooksSortedById()
        {
            return await _bookService.GetBooksSortedById();
        }

        [HttpGet("id/{id}")]
        public async Task<IEnumerable<Book>> GetBooksSortedById(string id)
        {
            return await _bookService.GetBooksFilteredById(id);
        }

        [HttpGet("author")]
        public async Task<IEnumerable<Book>> GetBooksSortedByAuthor()
        {
            return await _bookService.GetBooksSortedByAuthor();
        }

        [HttpGet("author/{value}")]
        public async Task<IEnumerable<Book>> GetBooksSortedByAuthor(string value)
        {
            return await _bookService.GetBooksFilteredByAuthor(value);
        }

        [HttpGet("title")]
        public async Task<IEnumerable<Book>> GetBooksSortedByTitle()
        {
            return await _bookService.GetBooksSortedByTitle();
        }

        [HttpGet("title/{value}")]
        public async Task<IEnumerable<Book>> GetBooksSortedByTitle(string value)
        {
            return await _bookService.GetBooksFilteredByTitle(value);
        }

        [HttpGet("genre")]
        public async Task<IEnumerable<Book>> GetBooksSortedByGenre()
        {
            return await _bookService.GetBooksSortedByGenre();
        }

        [HttpGet("genre/{value}")]
        public async Task<IEnumerable<Book>> GetBooksSortedByGenre(string value)
        {
            return await _bookService.GetBooksFilteredByGenre(value);
        }
        [HttpGet("description")]
        public async Task<IEnumerable<Book>> GetBooksSortedByDescription()
        {
            return await _bookService.GetBooksSortedByDescription();
        }

        [HttpGet("description/{value}")]
        public async Task<IEnumerable<Book>> GetBooksSortedByDescription(string value)
        {
            return await _bookService.GetBooksFilteredByDescription(value);
        }

        [HttpGet("published")]
        public async Task<IEnumerable<Book>> GetBooksSortedByPublishedDate()
        {
            return await _bookService.GetBooksSortedByPublishedDate();
        }

        [HttpGet("published/{year}")]
        public async Task<IEnumerable<Book>> GetBooksSortedByPublishedYear(int year)
        {
            return await _bookService.GetBooksFilteredByPublishedDate(year, 1, 1);
        }

        [HttpGet("published/{year}/{month}")]
        public async Task<IEnumerable<Book>> GetBooksSortedByPublishedYearMonth(int year, int month)
        {
            return await _bookService.GetBooksFilteredByPublishedDate(year, month, 1);
        }

        [HttpGet("published/{year}/{month}/{day}")]
        public async Task<IEnumerable<Book>> GetBooksSortedByPublishedYearMonthDay(int year, int month, int day)
        {
            return await _bookService.GetBooksFilteredByPublishedDate(year, month, day);
        }

        [HttpGet("price")]
        public async Task<IEnumerable<Book>> GetBooksSortedByPrice()
        {
            return await _bookService.GetBooksSortedByPrice();
        }


        [HttpGet("price/{price}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksFilteredByPrice(string price)
        {
            if(string.IsNullOrEmpty(price))
                return Ok(await _bookService.GetBooksSortedByPrice());

            if(price.Contains("&"))
            {
                var prices = price.Split("&", StringSplitOptions.RemoveEmptyEntries);

                if (prices.Length != 2)
                    return BadRequest();

                prices[0] = prices[0].Replace(',', '.');
                prices[1] = prices[1].Replace(',', '.');
                if (double.TryParse(prices[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var minPrice) && double.TryParse(prices[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var maxPrice))
                    return Ok(await _bookService.GetBooksWithinPriceRange(minPrice, maxPrice));
                else
                    return BadRequest();
            }
            else
            {
                price = price.Replace(',', '.');
                if (double.TryParse(price, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedPrice))
                    return Ok(await _bookService.GetBooksFilteredByPrice(parsedPrice));
                else
                    return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostBook(Book book)
        {
             var isSuccess = await _bookService.PostBook(book);

            if (isSuccess)
                return Created("", book);
            else
                return BadRequest();
        }

        [HttpPost("multiple")]
        public async Task<ActionResult> PostBooks(List<Book> books)
        {
            var isSuccess = await _bookService.PostBooks(books);

            if (isSuccess)
                return Created("", books);
            else
                return BadRequest();
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> PutBook(Book book, string id)
            {
            book.Id = id;
            var isSuccess = await _bookService.PutBook(book, id);

            if (isSuccess)
               return NoContent();
            else
                return NotFound();
        }

    }
}