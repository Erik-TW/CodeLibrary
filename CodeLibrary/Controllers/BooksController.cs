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
    }
}