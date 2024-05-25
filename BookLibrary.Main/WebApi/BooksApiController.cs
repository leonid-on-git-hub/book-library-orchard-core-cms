using BookLibrary.Domain.Dtos;
using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Repositories;
using BookLibrary.Main.Dtos;
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
        public async Task<IActionResult> Create([FromBody] BookDto bookDto)
        {
            var book = bookDto.ToDomain();
            var createdBook = await booksRepository.Create(book);
            return Created($"api/books/{createdBook.Id}", createdBook);
        }

        [HttpGet]
        public async Task<ActionResult<IPagedCollection<Book>>> GetAll(
            string? serachBookName = "",
            string? serachAuthor = "",
            DateTime? serachYear = null,
            bool searchOnlyAvailable = false,
            uint? from = 0,
            uint? count = 10)
        {
            var booksCollection = await booksRepository.GetAll(serachBookName, serachAuthor, serachYear, searchOnlyAvailable, from, count);
            return Ok(booksCollection);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> Get(Guid id)
        {
            var book = await booksRepository.Get(id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] BookDto updatedBookDto)
        {
            if (id != updatedBookDto.Id)
            {
                return BadRequest();
            }

            var updatedBook = updatedBookDto.ToDomain();
            await booksRepository.Update(updatedBook);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await booksRepository.Delete(id);
            return Ok();
        }
    }
}
