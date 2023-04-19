using BookLibrary.Domain.ValueObjects;

namespace BookLibrary.Storage.Records
{
    public class BookTrackRecord
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public string Login { get; set; } = string.Empty;
        public DateTime ActionDate { get; set; }
        public string Action { get; set; } = string.Empty;

        public static BookTrackRecord FromDomain(BookTrack bookTrack)
        {
            return new BookTrackRecord
            {
                BookId = bookTrack.BookId,
                Login = bookTrack.Login,
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
            return BookTrack.FromPersistence(BookId, Login, ActionDate, (BookAction)action);
        }
    }
}
