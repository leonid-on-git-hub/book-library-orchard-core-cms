using BookLibrary.Domain.Entities;

namespace BookLibrary.Main.Dtos
{
    public class BookDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public IEnumerable<string> Authors { get; set; } = Enumerable.Empty<string>();
        public DateTime? Year { get; set; }
        public bool Available { get; set; }

        public Book ToDomain()
        {
            return new Book(Id ?? Guid.NewGuid(), Name, Authors, Year, Available);
        }
    }
}
