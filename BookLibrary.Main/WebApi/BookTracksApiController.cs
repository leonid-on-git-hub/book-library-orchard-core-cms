using BookLibrary.Domain.Repositories;
using BookLibrary.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Main.WebApi
{
    [Route("api/booktracks")]
    [ApiController]
    public class BookTracksApiController : ControllerBase
    {
        private readonly IBookTracksRepository bookTracksRepository;

        public BookTracksApiController(IBookTracksRepository bookTracksRepository)
        {
            this.bookTracksRepository = bookTracksRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookTrack bookTrack)
        {
            await bookTracksRepository.Create(bookTrack);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookTrack>>> GetAll(Guid bookId, uint? count = 0)
        {
            var bookTracks = await bookTracksRepository.GetAll(bookId, count);
            return Ok(bookTracks);
        }

        [HttpGet("{userLogin}/{id:Guid}")]
        public async Task<ActionResult<bool>> IsBookAvailableToUser(string userLogin, Guid bookId)
        {
            var result = await bookTracksRepository.IsBookAvailableToUser(bookId, userLogin);
            return Ok(result);
        }
    }
}
