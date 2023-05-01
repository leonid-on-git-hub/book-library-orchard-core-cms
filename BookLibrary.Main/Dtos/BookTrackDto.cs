using BookLibrary.Domain.ValueObjects;

namespace BookLibrary.Main.Dtos
{
    public class BookTrackDto
    {
        public Guid BookId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public DateTime ActionDate { get; set; }
        public string Action { get; set; } = string.Empty;
    }
}
