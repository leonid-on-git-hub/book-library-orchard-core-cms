using BookLibrary.Domain.Entities;

namespace BookLibrary.SqLiteStorage.Records
{
    public class BookRecord
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime? Year { get; set; }
        public bool Available { get; set; }

        public static BookRecord FromDomain(Book book)
        {
            return new BookRecord
            {
                Id = book.Id.ToString().ToUpper(),
                Name = book.Name,
                Year = book.Year ?? new DateTime(1753, 1, 1),
                Available = book.Available
            };
        }
    }
}
