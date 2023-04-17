namespace BookLibrary.Domain.Entities
{
    public class BookTracks
    {
        public Guid BookId { get; }
        //public string BookName { get; }
        public bool? BookAvailable { get; }
        public bool BookAvailableToUser { get; }
        //public IEnumerable<BookTrack> Tracks { get; }

        //public BookTracks(Guid bookId, string bookName, bool? bookAvailable, bool availableInUser, IEnumerable<BookTrack> bookTracks)
        //{
        //    BookId = bookId;
        //    BookName = bookName;
        //    BookAvailable = bookAvailable;
        //    BookAvailableInUser = availableInUser;
        //    Tracks = bookTracks;
        //}
    }
}
