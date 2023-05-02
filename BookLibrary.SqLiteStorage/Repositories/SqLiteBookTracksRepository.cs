using BookLibrary.Domain.ValueObjects;
using BookLibrary.SqLiteStorage.Records;
using BookLibrary.Storage.Repositories;
using SqlKata.Execution;
using System.Data;

namespace BookLibrary.SqLiteStorage.Repositories
{
    public class SqLiteBookTracksRepository : BookTracksRepository
    {
        public SqLiteBookTracksRepository(IDbConnection connection) : base(connection) { }

        public override async Task<IEnumerable<BookTrack>> GetAll(Guid bookId, uint? count = 0)
        {
            var query = DataBase.Query(bookTracksTableName).Where(nameof(BookTrackRecord.BookId), bookId).OrderByDesc(new string[] { nameof(BookTrackRecord.ActionDate) });
            if (count > 0)
            {
                query.Take((int)count);
            }

            var bookTracksRecors = await query.GetAsync<BookTrackRecord>();
            var bookTracks = bookTracksRecors.Select(bookTrackRecord => bookTrackRecord.ToDomain());
            return bookTracks;
        }

        public override async Task<bool> IsBookAvailableToUser(Guid bookId, string userId)
        {
            var userBookTracks = await DataBase.Query(bookTracksTableName)
                .Where(nameof(BookTrackRecord.UserId), userId)
                .Where(nameof(BookTrackRecord.BookId), bookId)
                .Where(query => query.Where(nameof(BookTrackRecord.Action), BookAction.Taken.ToString()).OrWhere(nameof(BookTrackRecord.Action), BookAction.Put.ToString()))
                .OrderByDesc(new string[] { nameof(BookTrackRecord.ActionDate) })
                .Take(1)
                .GetAsync<BookTrackRecord>();
            var userBookTrack = userBookTracks.FirstOrDefault();
            var allBookTracks = await DataBase.Query(bookTracksTableName)
                .Where(nameof(BookTrackRecord.BookId), bookId)
                .Where(query => query.Where(nameof(BookTrackRecord.Action), BookAction.Taken.ToString()).OrWhere(nameof(BookTrackRecord.Action), BookAction.Put.ToString()))
                .OrderByDesc(new string[] { nameof(BookTrackRecord.ActionDate) })
                .Take(1)
                .GetAsync<BookTrackRecord>();
            var bookTrack = allBookTracks.FirstOrDefault();
            return bookTrack?.Action == default || userBookTrack?.Action == BookAction.Taken.ToString() || bookTrack?.Action == BookAction.Put.ToString();
        }
    }
}
