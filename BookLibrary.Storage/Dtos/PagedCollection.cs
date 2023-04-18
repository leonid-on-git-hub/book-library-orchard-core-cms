using BookLibrary.Domain.Dtos;

namespace BookLibrary.Storage.Dtos
{
    public class PagedCollection<ItemType> : IPagedCollection<ItemType> where ItemType : class
    {
        public IEnumerable<ItemType> Items { get; }

        public long TotalCount { get; }

        public PagedCollection(IEnumerable<ItemType> items, long totalCount)
        {
            Items = items;
            TotalCount = totalCount;
        }
    }
}
