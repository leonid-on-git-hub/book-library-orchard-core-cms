using BookLibrary.Domain.Repositories;
using BookLibrary.Domain.ValueObjects;
using BookLibrary.Main.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Users;
using OrchardCore.Users.Models;

namespace BookLibrary.Main.WebApi
{
    [Route("api/booktracks")]
    [ApiController]
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public class BookTracksApiController : ControllerBase
    {
        private readonly IBookTracksRepository bookTracksRepository;
        private readonly UserManager<IUser> userManager;

        public BookTracksApiController(IBookTracksRepository bookTracksRepository, UserManager<IUser> userManager)
        {
            this.bookTracksRepository = bookTracksRepository;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookTrackDto bookTrackDto)
        {
            var user = (User?)await userManager.FindByNameAsync(bookTrackDto.UserName) ?? (User?)await userManager.FindByEmailAsync(bookTrackDto.UserEmail);
            if (user == null || !Enum.TryParse(bookTrackDto.Action, out BookAction bookAction))
            {
                return BadRequest();
            }
            var bookTrack = new BookTrack(bookTrackDto.BookId, user.UserId, bookTrackDto.ActionDate, bookAction);
            await bookTracksRepository.Create(bookTrack);
            return Ok();
        }

        [HttpGet("{count}/{bookId}")]
        public async Task<ActionResult<IEnumerable<BookTrackDto>>> GetAll(Guid bookId, uint? count = 0)
        {
            var bookTracksDtos = new List<BookTrackDto>();
            var usersCache = new Dictionary<string, User?>();
            var bookTracks = await bookTracksRepository.GetAll(bookId, count);
            foreach (var bookTrack in bookTracks)
            {
                var bookTrackDto = new BookTrackDto
                {
                    BookId = bookTrack.BookId,
                    Action = bookTrack.Action.ToString(),
                    ActionDate = bookTrack.ActionDate.ToLocalTime(),
                };
                var usersCacheContainsUser = usersCache.ContainsKey(bookTrack.UserId);
                var user = usersCacheContainsUser ? usersCache[bookTrack.UserId] : (User?)await userManager.FindByIdAsync(bookTrack.UserId);
                if (user != null)
                {
                    if (!usersCacheContainsUser)
                    {
                        usersCache.Add(user.UserId, user);
                    }
                    bookTrackDto.UserName = user.UserName;
                    bookTrackDto.UserEmail = user.Email;
                }
                else
                {
                    bookTrackDto.UserName = "User not found";
                    bookTrackDto.UserEmail = string.Empty;
                }
                bookTracksDtos.Add(bookTrackDto);
            }
            return Ok(bookTracksDtos);
        }

        [HttpGet("user/{userLogin}/{bookId}")]
        public async Task<ActionResult<bool>> IsBookAvailableToUser(string userLogin, Guid bookId)
        {
            var user = (User?)await userManager.FindByNameAsync(userLogin);
            if (user == null)
            {
                return Ok(false);
            }
            var result = await bookTracksRepository.IsBookAvailableToUser(bookId, user.UserId);
            return Ok(result);
        }
    }
}
