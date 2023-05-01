namespace BookLibrary.Domain.ValueObjects
{
    public class BookTrack
    {
        public Guid BookId { get; }
        public string UserId { get; }
        public DateTime ActionDate { get; }
        public BookAction Action { get; }

        public BookTrack(Guid bookId, string userId, DateTime actionDate, BookAction action)
        {
            BookId = bookId;
            UserId = userId;
            ActionDate = actionDate;
            Action = action;
        }

        public static BookTrack FromPersistence(Guid bookId, string userId, DateTime actionDate, BookAction action)
        {
            return new BookTrack(bookId, userId, actionDate, action);
        }
    }
}
