using CodeLibrary.Data.Models;
using CodeLibrary.Data.Services;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("author/{id}")]
        public async Task<IEnumerable<Book>> GetBooksSortedByAuthor(string id)
        {
            return await _bookService.GetBooksFilteredByAuthor(id);
        }

        [HttpGet("title")]
        public async Task<IEnumerable<Book>> GetBooksSortedByTitle()
        {
            return await _bookService.GetBooksSortedByTitle();
        }

        [HttpGet("title/{id}")]
        public async Task<IEnumerable<Book>> GetBooksSortedByTitle(string id)
        {
            return await _bookService.GetBooksFilteredByTitle(id);
        }

        [HttpGet("genre")]
        public async Task<IEnumerable<Book>> GetBooksSortedByGenre()
        {
            return await _bookService.GetBooksSortedByGenre();
        }

        [HttpGet("genre/{id}")]
        public async Task<IEnumerable<Book>> GetBooksSortedByGenre(string id)
        {
            return await _bookService.GetBooksFilteredByGenre(id);
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

                if (double.TryParse(prices[0], out var minPrice) && double.TryParse(prices[1], out var maxPrice))
                    return Ok(await _bookService.GetBooksWithinPriceRange(minPrice, maxPrice));
                else
                    return BadRequest();
            }
            else
            {
                if(double.TryParse(price, out var parsedPrice))
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