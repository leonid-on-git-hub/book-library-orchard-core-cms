namespace BookLibrary.Storage.Records
{
    internal class BookTrackRecord
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public string Login { get; set; }
        public DateTime ActionDate { get; set; }
        public string Action { get; set; }

        public BookTrackRecord()
        {
            Login = string.Empty;
            Action = string.Empty;
        }
    }
}
