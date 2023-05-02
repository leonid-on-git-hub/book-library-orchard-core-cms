using BookLibrary.Domain.Dtos;
using BookLibrary.Domain.Entities;
using BookLibrary.SqLiteStorage.Records;
using BookLibrary.Storage.Dtos;
using BookLibrary.Storage.Repositories;
using SqlKata.Execution;
using System.Data;

namespace BookLibrary.SqLiteStorage.Repositories
{
    public class SqLiteBooksRepository : BooksRepository
    {
        public SqLiteBooksRepository(IDbConnection connection) : base(connection) { }

        public override async Task<Book?> Get(Guid bookId)
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

            var book = Book.FromPersistence(new Guid(bookRecord.Id), bookRecord.Name, authors, bookRecord.Year?.ToLocalTime(), bookRecord.Available);
            return book;
        }

        public override async Task<IPagedCollection<Book>> GetAll(
            string? serachBookName = "",
            string? serachAuthor = "",
            DateTime? serachYear = null,
            bool searchOnlyAvailable = false,
            uint? from = 0,
            uint? count = 10)
        {
            var query = DataBase.Query(booksTableName).OrderBy(new string[] { nameof(BookRecord.Name) });
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
                query = query.Offset((int)from);
            }

            query = query.Limit((int)(count ?? 0));
            var bookRecords = await query.GetAsync<BookRecord>();
            var books = new List<Book>();
            foreach (var bookRecord in bookRecords)
            {
                var book = await Get(new Guid(bookRecord.Id));
                if (book != null)
                {
                    books.Add(book);
                }
            }

            return new PagedCollection<Book>(books, totalCount);
        }

        public override async Task Delete(Guid bookId)
        {
            DataBase.Connection.Open();
            using var transaction = DataBase.Connection.BeginTransaction();
            try
            {
                await DataBase.Query(BookTracksRepository.bookTracksTableName).Where(nameof(BookTrackRecord.BookId), bookId).DeleteAsync(transaction);
                await DataBase.Query(booksAuthorsTableName).Where(nameof(BookAuthorRecord.BookId), bookId).DeleteAsync(transaction);
                await DataBase.Query(booksTableName).Where(nameof(BookRecord.Id), bookId).DeleteAsync(transaction);
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

        #region Protected

        protected override async Task BindBookAndAuthors(Guid bookId, IEnumerable<string> authors, IDbTransaction? transaction = null)
        {
            foreach (var authorName in authors)
            {
                var persistedAuthors = await DataBase.Query(authorsTableName).Where(nameof(AuthorRecord.Name), authorName).GetAsync<AuthorRecord>(transaction);
                var persistedAuthor = persistedAuthors.FirstOrDefault();
                var bookAuthorRecord = new BookAuthorRecord { BookId = bookId.ToString().ToUpper() };
                if (persistedAuthor != default)
                {
                    bookAuthorRecord.AuthorId = persistedAuthor.Id;
                }
                else
                {
                    var authorId = Guid.NewGuid();
                    var newAuthor = new AuthorRecord { Id = authorId.ToString().ToUpper(), Name = authorName };
                    await DataBase.Query(authorsTableName).InsertAsync(newAuthor, transaction);
                    bookAuthorRecord.AuthorId = authorId.ToString().ToUpper();
                }
                var persistedBookAuthor = await DataBase
                    .Query(booksAuthorsTableName)
                    .Where(nameof(BookAuthorRecord.AuthorId), bookAuthorRecord.AuthorId)
                    .Where(nameof(BookAuthorRecord.BookId), bookAuthorRecord.BookId)
                    .GetAsync<BookAuthorRecord>(transaction);
                if (persistedBookAuthor.FirstOrDefault() == default)
                {
                    await DataBase.Query(booksAuthorsTableName).InsertAsync(bookAuthorRecord, transaction);
                }
            }
        }

        #endregion
    }
}
