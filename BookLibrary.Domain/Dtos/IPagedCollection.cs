namespace BookLibrary.Domain.Dtos
{
    public interface IPagedCollection<ItemType> where ItemType : class
    {
        IEnumerable<ItemType> Items { get; }
        long TotalCount { get; }
    }
}
