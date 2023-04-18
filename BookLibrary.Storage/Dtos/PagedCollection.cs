using BookLibrary.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
