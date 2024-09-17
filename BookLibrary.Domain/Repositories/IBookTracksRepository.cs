using BookLibrary.Domain.Entities;

namespace BookLibrary.Domain.Repositories
{
    public interface IBookTracksRepository
    {
        Task<IEnumerable<BookTrack>> GetAll(Guid bookId, uint? count = 0);

        Task Create(BookTrack bookTrack);

        Task<bool> IsBookAvailableToUser(Guid bookId, string userLogin);
    }
}
