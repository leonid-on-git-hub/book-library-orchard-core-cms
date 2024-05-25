namespace BookLibrary.Domain.Entities
{
    public class Book
    {
        public Guid Id { get; }
        public string Name { get; }
        public IEnumerable<string> Authors { get; }
        public DateTime? Year { get; }
        public bool Available { get; }

        public Book(string name, IEnumerable<string> authors, DateTime? year, bool available)
            : this(Guid.NewGuid(), name, authors, year, available) { }

        public static Book FromPersistence(Guid id, string name, IEnumerable<string> authors, DateTime? year, bool available)
        {
            return new Book(id, name, authors, year, available);
        }

        private Book(Guid id, string name, IEnumerable<string> authors, DateTime? year, bool available)
        {
            Id = id;
            Name = name;
            Authors = authors;
            Year = year;
            Available = available;
        }
    }
}
