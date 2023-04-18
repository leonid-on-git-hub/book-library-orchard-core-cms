using BookLibrary.Domain.Dtos;
using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Repositories;
using BookLibrary.Storage.Dtos;
using BookLibrary.Storage.Records;
using SqlKata.Execution;
using System.Data;

namespace BookLibrary.Storage.Repositories
{
    public class BooksRepository : Repository, IBooksRepository
    {
        private const string AuthorsTableName = "Authors";
        private const string BooksTableName = "Books";
        private const string BooksAuthorsTableName = "BooksAuthors";

        public async Task<Book> Create(Book book)
        {
            var bookId = Guid.NewGuid();
            var bookRecord = BookRecord.FromDomain(book);
            using var transaction = DataBase.Connection.BeginTransaction();
            try
            {
                await DataBase.Query(BooksTableName).InsertAsync(bookRecord, transaction);
                await BindBookAndAuthors(bookId, book.Authors, transaction);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            var createdBook = await Get(book.Id);
            return createdBook == null ? throw new InvalidOperationException("Book creation exception. Book not created.") : book;
        }

        public async Task Delete(Guid bookId)
        {
            await DataBase.Query(BooksTableName).Where(nameof(BookRecord.Id), bookId).DeleteAsync();
        }

        public async Task<Book?> Get(Guid bookId)
        {
            var bookRecords = await DataBase.Query(BooksTableName).Where(nameof(BookRecord.Id), bookId).GetAsync<BookRecord>();
            var bookRecord = bookRecords.FirstOrDefault();
            if (bookRecord == null)
            {
                return null;
            }
            var authors = new List<string>();
            var persistedBooksAuthors = await DataBase.Query(BooksAuthorsTableName).Where(nameof(BookAuthorRecord.BookId), bookRecord.Id).GetAsync<BookAuthorRecord>();
            foreach (var persistedBookAuthor in persistedBooksAuthors)
            {
                var persistedAuthors = await DataBase.Query(AuthorsTableName).Where(nameof(AuthorRecord.Id), persistedBookAuthor.AuthorId).GetAsync<AuthorRecord>();
                var persistedAuthor = persistedAuthors.FirstOrDefault();
                if (persistedAuthor != null)
                {
                    authors.Add(persistedAuthor.Name);
                }
            }
            var book = Book.FromPersistence(bookRecord.Id, bookRecord.Name, authors, bookRecord.Year, bookRecord.Available);
            return book;
        }

        public async Task<IPagedCollection<Book>> GetAll(string serachBookName = "", string serachAuthor = "", uint from = 0, uint count = 10)
        {
            var query = DataBase.Query(BooksTableName);
            if (!string.IsNullOrEmpty(serachBookName))
            {
                query = query.WhereLike(nameof(BookRecord.Name), serachBookName, false);
            }
            var persistedAuthors = await DataBase.Query(AuthorsTableName).WhereLike(nameof(AuthorRecord.Name), serachAuthor, false).GetAsync<AuthorRecord>();
            var persistedBooksAuthors = await DataBase.Query(BooksAuthorsTableName)
                .WhereIn(nameof(BookAuthorRecord.AuthorId), persistedAuthors.Select(author => author.Id))
                .GetAsync<BookAuthorRecord>();
            if (persistedBooksAuthors.Any())
            {
                query = query.WhereIn(nameof(BookRecord.Id), persistedBooksAuthors.Select(bookAuthor => bookAuthor.BookId));
            }
            var totalCount = await query.Clone().CountAsync<long>();
            if (totalCount == 0)
            {
                return new PagedCollection<Book>(Enumerable.Empty<Book>(), totalCount);
            }
            if (from > 0)
            {
                query = query.Skip((int)from);
            }
            query = query.Take((int)count);
            var bookRecords = await query.GetAsync<BookRecord>();
            var books = new List<Book>();
            foreach (var bookRecord in bookRecords)
            {
                var book = await Get(bookRecord.Id);
                if (book != null)
                {
                    books.Add(book);
                }
            }
            return new PagedCollection<Book>(books, totalCount);
        }

        public async Task Update(Book book)
        {
            var bookRecord = BookRecord.FromDomain(book);
            using var transaction = DataBase.Connection.BeginTransaction();
            try
            {
                await DataBase.Query(BooksTableName).Where(nameof(BookRecord.Id), bookRecord.Id).UpdateAsync(bookRecord, transaction);
                await DataBase.Query(BooksAuthorsTableName).Where(nameof(BookAuthorRecord.BookId), bookRecord.Id).DeleteAsync(transaction);
                await BindBookAndAuthors(book.Id, book.Authors, transaction);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        #region Private

        private async Task BindBookAndAuthors(Guid bookId, IEnumerable<string> authors, IDbTransaction? transaction = null)
        {
            foreach (var authorName in authors)
            {
                var persistedAuthors = await DataBase.Query(AuthorsTableName).Where(nameof(AuthorRecord.Name), authorName).GetAsync<AuthorRecord>();
                var persistedAuthor = persistedAuthors.FirstOrDefault();
                var bookAuthorRecord = new BookAuthorRecord { BookId = bookId };
                if (persistedAuthor != null)
                {
                    bookAuthorRecord.AuthorId = persistedAuthor.Id;
                }
                else
                {
                    var authorId = Guid.NewGuid();
                    var newAuthor = new AuthorRecord { Id = authorId, Name = authorName };
                    await DataBase.Query(AuthorsTableName).InsertAsync(newAuthor, transaction);
                    bookAuthorRecord.AuthorId = authorId;
                }
                await DataBase.Query(BooksAuthorsTableName).InsertAsync(bookAuthorRecord, transaction);
            }
        }

        #endregion
    }
}
