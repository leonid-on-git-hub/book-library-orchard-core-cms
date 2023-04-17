using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Repositories;

namespace BookLibrary.Repository.Repositories
{
    public class BooksRepository : Repository, IBooksRepository
    {
        public Task Create(Book book)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Guid bookId)
        {
            throw new NotImplementedException();
        }

        public Task<Book> Get(Guid bookId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Book>> GetAll(string serachString = "", uint from = 0, uint count = 10)
        {
            throw new NotImplementedException();
        }

        public Task Update(Book book)
        {
            throw new NotImplementedException();
        }
    }
}
