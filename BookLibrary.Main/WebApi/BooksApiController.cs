using BookLibrary.Domain.Dtos;
using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Main.WebApi
{
    [Route("api/books")]
    [ApiController]
    public class BooksApiController : ControllerBase
    {
        private readonly IBooksRepository booksRepository;

        public BooksApiController(IBooksRepository booksRepository)
        {
            this.booksRepository = booksRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Book book)
        {
            var createdBook = await booksRepository.Create(book);
            return Created($"api/books/{createdBook.Id}", createdBook);
        }

        [HttpGet]
        public async Task<ActionResult<IPagedCollection<Book>>> GetAll(string? serachBookName = "", string? serachAuthor = "", uint? from = 0, uint? count = 10)
        {
            var booksCollection = await booksRepository.GetAll(serachBookName, serachAuthor, from, count);
            return Ok(booksCollection);
        }

        [HttpGet("id:Guid")]
        public async Task<ActionResult<Book>> Get(Guid id)
        {
            var book = await booksRepository.Get(id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPut("id:Guid")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Book updatedBook)
        {
            if (id != updatedBook.Id)
            {
                return BadRequest();
            }

            await booksRepository.Update(updatedBook);
            return NoContent();
        }

        [HttpDelete("id:Guid")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await booksRepository.Delete(id);
            return Ok();
        }
    }
}
