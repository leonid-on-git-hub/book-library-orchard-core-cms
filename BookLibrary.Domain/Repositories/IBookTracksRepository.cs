using BookLibrary.Domain.ValueObjects;

namespace BookLibrary.Domain.Repositories
{
    public interface IBookTracksRepository
    {
        Task<IEnumerable<BookTrack>> GetAll(Guid bookId);

        Task<BookTrack> Create(BookTrack bookTrack);

        Task<bool> IsBookAvailableToUser(Guid bookId, string userLogin);
    }
}
