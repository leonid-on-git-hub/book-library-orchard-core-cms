namespace BookLibrary.Domain.ValueObjects
{
    public class BookTrack
    {
        public Guid BookId { get; }
        public string Login { get; }
        public DateTime ActionDate { get; }
        public BookAction Action { get; }

        public BookTrack(Guid bookId, string login, DateTime actionDate, BookAction action)
        {
            BookId = bookId;
            Login = login;
            ActionDate = actionDate;
            Action = action;
        }

        public static BookTrack FromPersistence(Guid bookId, string login, DateTime actionDate, BookAction action)
        {
            return new BookTrack(bookId, login, actionDate, action);
        }
    }
}
