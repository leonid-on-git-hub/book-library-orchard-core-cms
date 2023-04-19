using BookLibrary.Domain.Dtos;
using BookLibrary.Domain.Entities;

namespace BookLibrary.Domain.Repositories
{
    public interface IBooksRepository
    {
        Task<IPagedCollection<Book>> GetAll(string? serachBookName = "", string? serachAuthor = "", uint? from = 0, uint? count = 10);

        Task<Book?> Get(Guid bookId);

        Task<Book> Create(Book book);

        Task Update(Book book);

        Task Delete(Guid bookId);

    }
}
