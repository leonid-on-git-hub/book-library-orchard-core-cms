using BookLibrary.Domain.Repositories;
using BookLibrary.Domain.ValueObjects;
using BookLibrary.Storage.Records;
using SqlKata.Execution;
using System.Data;

namespace BookLibrary.Storage.Repositories
{
    public class BookTracksRepository : Repository, IBookTracksRepository
    {
        private const string BookTracksTableName = "BookTracks";

        public BookTracksRepository(IDbConnection connection) : base(connection) { }

        public async Task Create(BookTrack bookTrack)
        {
            var bookTrackRecord = BookTrackRecord.FromDomain(bookTrack);
            if (bookTrackRecord.Id == default)
            {
                bookTrackRecord.Id = Guid.NewGuid();
            }
            bookTrackRecord.ActionDate = DateTime.Now;

            await DataBase.Query(BookTracksTableName).InsertAsync(bookTrackRecord);
        }

        public async Task<IEnumerable<BookTrack>> GetAll(Guid bookId, uint? count = 0)
        {
            var query = DataBase.Query(BookTracksTableName).Where(nameof(BookTrackRecord.BookId), bookId);
            if (count > 0)
            {
                query.Take((int)count);
            }

            var bookTracksRecors = await query.GetAsync<BookTrackRecord>();
            var bookTracks = bookTracksRecors.Select(bookTrackRecord => bookTrackRecord.ToDomain());
            return bookTracks;
        }

        public async Task<bool> IsBookAvailableToUser(Guid bookId, string userLogin)
        {
            var bookTracks = await DataBase.Query(BookTracksTableName)
                .Where(nameof(BookTrackRecord.Login), userLogin)
                .Where(nameof(BookTrackRecord.BookId), bookId)
                .Where(query => query.Where(nameof(BookTrackRecord.Action), BookAction.Taken).OrWhere(nameof(BookTrackRecord.Action), BookAction.Put))
                .OrderBy(new string[] { nameof(BookTrackRecord.ActionDate) })
                .Take(1)
                .GetAsync<BookTrackRecord>();
            var bookTrack = bookTracks.FirstOrDefault();
            return bookTrack?.Action == BookAction.Taken.ToString();
        }
    }
}
