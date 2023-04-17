using BookLibrary.Domain.Repositories;
using BookLibrary.Domain.ValueObjects;

namespace BookLibrary.Storage.Repositories
{
    public class BookTracksRepository : Repository, IBookTracksRepository
    {
        public Task Create(BookTrack bookTrack)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BookTrack>> GetAll(Guid bookId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsBookAvailableToUser(Guid bookId, string userLogin)
        {
            throw new NotImplementedException();
        }
    }
}
