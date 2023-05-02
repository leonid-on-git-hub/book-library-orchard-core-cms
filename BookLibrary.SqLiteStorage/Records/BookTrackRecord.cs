using BookLibrary.Domain.ValueObjects;

namespace BookLibrary.SqLiteStorage.Records
{
    public class BookTrackRecord
    {
        public string Id { get; set; } = string.Empty;
        public string BookId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime ActionDate { get; set; }
        public string Action { get; set; } = string.Empty;

        public static BookTrackRecord FromDomain(BookTrack bookTrack)
        {
            return new BookTrackRecord
            {
                BookId = bookTrack.BookId.ToString().ToUpper(),
                UserId = bookTrack.UserId,
                Action = bookTrack.Action.ToString(),
                ActionDate = bookTrack.ActionDate
            };
        }

        public BookTrack ToDomain()
        {
            if (!Enum.TryParse(typeof(BookAction), Action, true, out var action) || action is null)
            {
                throw new InvalidOperationException("BookTrackRecord, ToDomain method can't parse action to enum.");
            }
            return BookTrack.FromPersistence(new Guid(BookId), UserId, ActionDate, (BookAction)action);
        }
    }
}
