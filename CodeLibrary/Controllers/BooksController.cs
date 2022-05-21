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