using BookLibrary.Domain.Repositories;
using BookLibrary.Domain.ValueObjects;
using BookLibrary.Storage.Records;
using SqlKata.Execution;
using System.Data;

namespace BookLibrary.Storage.Repositories
{
    public class BookTracksRepository(IDbConnection connection) : Repository(connection), IBookTracksRepository
    {
        public static readonly string bookTracksTableName = "BookTracks";

        public async Task Create(BookTrack bookTrack)
        {
            var bookTrackRecord = BookTrackRecord.FromDomain(bookTrack);
            if (bookTrackRecord.Id == default)
            {
                bookTrackRecord.Id = Guid.NewGuid();
            }
            bookTrackRecord.ActionDate = DateTime.UtcNow;

            await DataBase.Query(bookTracksTableName).InsertAsync(bookTrackRecord);
        }

        public virtual async Task<IEnumerable<BookTrack>> GetAll(Guid bookId, uint? count = 0)
        {
            var query = DataBase.Query(bookTracksTableName).Where(nameof(BookTrackRecord.BookId), bookId).OrderByDesc([nameof(BookTrackRecord.ActionDate)]);
            if (count > 0)
            {
                query.Take((int)count);
            }

            var bookTracksRecords = await query.GetAsync<BookTrackRecord>();
            var bookTracks = bookTracksRecords.Select(bookTrackRecord => bookTrackRecord.ToDomain());
            return bookTracks;
        }

        public virtual async Task<bool> IsBookAvailableToUser(Guid bookId, string userId)
        {
            var userBookTracks = await DataBase.Query(bookTracksTableName)
                .Where(nameof(BookTrackRecord.UserId), userId)
                .Where(nameof(BookTrackRecord.BookId), bookId)
                .Where(query => query.Where(nameof(BookTrackRecord.Action), BookAction.Taken.ToString()).OrWhere(nameof(BookTrackRecord.Action), BookAction.Put.ToString()))
                .OrderByDesc([nameof(BookTrackRecord.ActionDate)])
                .Take(1)
                .GetAsync<BookTrackRecord>();
            var userBookTrack = userBookTracks.FirstOrDefault();
            var allBookTracks = await DataBase.Query(bookTracksTableName)
                .Where(nameof(BookTrackRecord.BookId), bookId)
                .Where(query => query.Where(nameof(BookTrackRecord.Action), BookAction.Taken.ToString()).OrWhere(nameof(BookTrackRecord.Action), BookAction.Put.ToString()))
                .OrderByDesc([nameof(BookTrackRecord.ActionDate)])
                .Take(1)
                .GetAsync<BookTrackRecord>();
            var bookTrack = allBookTracks.FirstOrDefault();
            return bookTrack?.Action == default || userBookTrack?.Action == BookAction.Taken.ToString() || bookTrack?.Action == BookAction.Put.ToString();
        }
    }
}
