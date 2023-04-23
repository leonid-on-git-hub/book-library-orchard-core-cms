using BookLibrary.Domain.Entities;

namespace BookLibrary.Storage.Records
{
    public class BookRecord
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? Year { get; set; }
        public bool Available { get; set; }

        public static BookRecord FromDomain(Book book)
        {
            return new BookRecord
            {
                Id = book.Id,
                Name = book.Name,
                Year = book.Year,
                Available = book.Available
            };
        }
    }
}
