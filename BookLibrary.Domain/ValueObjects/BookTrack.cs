using System.ComponentModel.DataAnnotations;

namespace BookLibrary.Domain.ValueObjects
{
    public class BookTrack
    {
        [Key]
        public Guid BookId { get; }
        public string BookName { get; }
        public string Login { get; }
        public DateTime ActionTime { get; }
        public string Action { get; }

        public BookTrack(Guid bookId, string bookName, string login, DateTime actionTime, string action)
        {
            BookId = bookId;
            BookName = bookName;
            Login = login;
            ActionTime = actionTime;
            Action = action;
        }

        public static BookTrack FromPersistence(Guid bookId, string bookName, string login, DateTime actionTime, string action)
        {
            return new BookTrack(bookId, bookName, login, actionTime, action);
        }
    }
}
