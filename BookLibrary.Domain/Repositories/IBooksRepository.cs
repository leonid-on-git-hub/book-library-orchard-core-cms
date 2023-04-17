using BookLibrary.Domain.Entities;

namespace BookLibrary.Domain.Repositories
{
    public interface IBooksRepository
    {
        Task<IEnumerable<Book>> GetAll(string serachString = "", uint from = 0, uint count = 10);

        Task<Book> Get(Guid bookId);

        Task Create(Book book);

        Task Update(Book book);

        Task Delete(Guid bookId);

    }
}
