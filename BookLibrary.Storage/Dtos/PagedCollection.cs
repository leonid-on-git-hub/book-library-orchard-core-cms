using BookLibrary.Domain.Dtos;

namespace BookLibrary.Storage.Dtos
{
    public class PagedCollection<ItemType>(IEnumerable<ItemType> items, long totalCount) : IPagedCollection<ItemType> where ItemType : class
    {
        public IEnumerable<ItemType> Items { get; } = items;

        public long TotalCount { get; } = totalCount;
    }
}
