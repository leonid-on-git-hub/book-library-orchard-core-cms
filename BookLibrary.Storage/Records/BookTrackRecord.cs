namespace BookLibrary.Storage.Records
{
    internal class BookTrackRecord
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public string Login { get; set; } = string.Empty;
        public DateTime ActionDate { get; set; }
        public string Action { get; set; } = string.Empty;
    }
}
