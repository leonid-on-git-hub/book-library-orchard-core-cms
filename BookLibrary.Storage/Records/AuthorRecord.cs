namespace BookLibrary.Storage.Records
{
    internal class AuthorRecord
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public AuthorRecord()
        {
            Name = string.Empty;
        }
    }
}
