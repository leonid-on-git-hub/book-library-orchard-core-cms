using BookLibrary.Domain.ValueObjects;

namespace BookLibrary.Domain.Entities
{
    public class BookTrack(Guid bookId, string userId, DateTime actionDate, BookAction action)
    {
        public Guid BookId { get; } = bookId;
        public string UserId { get; } = userId;
        public DateTime ActionDate { get; } = actionDate;
        public BookAction Action { get; } = action;

        public static BookTrack FromPersistence(Guid bookId, string userId, DateTime actionDate, BookAction action)
        {
            return new BookTrack(bookId, userId, actionDate, action);
        }
    }
}
