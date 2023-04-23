using BookLibrary.Domain.Dtos;
using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Repositories;
using BookLibrary.Storage.Dtos;
using BookLibrary.Storage.Records;
using SqlKata.Execution;
using System.Data;
using System.Net;

namespace BookLibrary.Storage.Repositories
{
    public class BooksRepository : Repository, IBooksRepository
    {
        private const string authorsTableName = "Authors";
        protected const string booksTableName = "Books";
        protected const string booksAuthorsTableName = "BooksAuthors";

        public BooksRepository(IDbConnection connection) : base(connection) { }

        public async Task<Book> Create(Book book)
        {
            var bookRecord = BookRecord.FromDomain(book);
            if (bookRecord.Id == default)
            {
                bookRecord.Id = Guid.NewGuid();
            }

            var persistedBook = await Get(bookRecord.Id);
            if (persistedBook != null)
            {
                throw new InvalidOperationException($"The book with Id: {bookRecord.Id} already exists.");
            }

            DataBase.Connection.Open();
            using var transaction = DataBase.Connection.BeginTransaction();
            try
            {
                await DataBase.Query(booksTableName).InsertAsync(bookRecord, transaction);
                await BindBookAndAuthors(bookRecord.Id, book.Authors.Distinct(), transaction);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally 
            { 
                DataBase.Connection.Close(); 
            }  

            var createdBook = await Get(bookRecord.Id);
            return createdBook == null ? throw new InvalidOperationException("Book creation exception. Book not created.") : book;
        }

        public virtual async Task Delete(Guid bookId)
        {
            await DataBase.Query(booksTableName).Where(nameof(BookRecord.Id), bookId).DeleteAsync();
        }

        public async Task<Book?> Get(Guid bookId)
        {
            var bookRecords = await DataBase.Query(booksTableName).Where(nameof(BookRecord.Id), bookId).GetAsync<BookRecord>();
            var bookRecord = bookRecords.FirstOrDefault();
            if (bookRecord == default)
            {
                return null;
            }

            var authors = new List<string>();
            var persistedBooksAuthors = await DataBase.Query(booksAuthorsTableName).Where(nameof(BookAuthorRecord.BookId), bookRecord.Id).GetAsync<BookAuthorRecord>();
            foreach (var persistedBookAuthor in persistedBooksAuthors)
            {
                var persistedAuthors = await DataBase.Query(authorsTableName).Where(nameof(AuthorRecord.Id), persistedBookAuthor.AuthorId).GetAsync<AuthorRecord>();
                var persistedAuthor = persistedAuthors.FirstOrDefault();
                if (persistedAuthor != default)
                {
                    authors.Add(persistedAuthor.Name);
                }
            }

            var book = Book.FromPersistence(bookRecord.Id, bookRecord.Name, authors, bookRecord.Year?.ToLocalTime(), bookRecord.Available);
            return book;
        }

        public async Task<IPagedCollection<Book>> GetAll(
            string? serachBookName = "", 
            string? serachAuthor = "", 
            DateTime? serachYear = null,
            bool searchOnlyAvailable = false,
            uint? from = 0, 
            uint? count = 10)
        {
            var query = DataBase.Query(booksTableName);
            if (!string.IsNullOrEmpty(serachBookName))
            {
                query = query.WhereContains(nameof(BookRecord.Name), serachBookName, false);
            }

            if (!string.IsNullOrEmpty(serachAuthor))
            {
                var persistedAuthors = await DataBase.Query(authorsTableName).WhereContains(nameof(AuthorRecord.Name), serachAuthor, false).GetAsync<AuthorRecord>();
                var persistedBooksAuthors = await DataBase.Query(booksAuthorsTableName)
                    .WhereIn(nameof(BookAuthorRecord.AuthorId), persistedAuthors.Select(author => author.Id))
                    .GetAsync<BookAuthorRecord>();
                if (persistedBooksAuthors.Any())
                {
                    query = query.WhereIn(nameof(BookRecord.Id), persistedBooksAuthors.Select(bookAuthor => bookAuthor.BookId));
                }
            }

            if (serachYear.HasValue)
            {
                query = query.WhereDatePart("year", nameof(BookRecord.Year), serachYear?.Year);
            }

            if (searchOnlyAvailable)
            {
                query = query.WhereTrue(nameof(BookRecord.Available));
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

            query = query.Take((int)(count ?? 0));
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
            var persistedBook = await Get(bookRecord.Id);
            if (persistedBook != null)
            {
                DataBase.Connection.Open();
                using var transaction = DataBase.Connection.BeginTransaction();
                try
                {
                    await DataBase.Query(booksTableName).Where(nameof(BookRecord.Id), bookRecord.Id).UpdateAsync(bookRecord, transaction);
                    await DataBase.Query(booksAuthorsTableName).Where(nameof(BookAuthorRecord.BookId), bookRecord.Id).DeleteAsync(transaction);
                    await BindBookAndAuthors(book.Id, book.Authors.Distinct(), transaction);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    DataBase.Connection.Close();
                }
            }
            else
            {
                await Create(book);
            }

        }

        #region Private

        private async Task BindBookAndAuthors(Guid bookId, IEnumerable<string> authors, IDbTransaction? transaction = null)
        {
            foreach (var authorName in authors)
            {
                var persistedAuthors = await DataBase.Query(authorsTableName).Where(nameof(AuthorRecord.Name), authorName).GetAsync<AuthorRecord>(transaction);
                var persistedAuthor = persistedAuthors.FirstOrDefault();
                var bookAuthorRecord = new BookAuthorRecord { BookId = bookId };
                if (persistedAuthor != default)
                {
                    bookAuthorRecord.AuthorId = persistedAuthor.Id;
                }
                else
                {
                    var authorId = Guid.NewGuid();
                    var newAuthor = new AuthorRecord { Id = authorId, Name = authorName };
                    await DataBase.Query(authorsTableName).InsertAsync(newAuthor, transaction);
                    bookAuthorRecord.AuthorId = authorId;
                }
                var persistedBookAuthor = await DataBase
                    .Query(booksAuthorsTableName)
                    .Where(nameof(BookAuthorRecord.AuthorId), bookAuthorRecord.AuthorId)
                    .Where(nameof(BookAuthorRecord.BookId), bookAuthorRecord.BookId)
                    .GetAsync<AuthorRecord>(transaction);
                if(persistedBookAuthor.FirstOrDefault() == default)
                {
                    await DataBase.Query(booksAuthorsTableName).InsertAsync(bookAuthorRecord, transaction);
                }
            }
        }

        #endregion
    }
}
