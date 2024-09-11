using BookLibrary.Domain.Entities;

namespace BookLibrary.Main.Dtos
{
    public class BookDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public IEnumerable<string> Authors { get; set; } = [];
        public DateTime? Year { get; set; }
        public bool Available { get; set; }

        public Book ToDomain()
        {
            if (Id is null)
            {
                return new Book(Name, Authors, Year, Available);
            }
            else
            {
                return Book.FromPersistence((Guid)Id, Name, Authors, Year, Available);
            }
        }
    }
}
