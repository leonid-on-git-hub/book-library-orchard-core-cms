using BookLibrary.Storage.Records;
using BookLibrary.Storage.Repositories;
using SqlKata.Execution;
using System.Data;

namespace BookLibrary.SqLiteStorage.Repositories
{
    public class SqLiteBooksRepository : BooksRepository
    {
        public SqLiteBooksRepository(IDbConnection connection) : base(connection) { }

        public override async Task Delete(Guid bookId)
        {
            using var transaction = DataBase.Connection.BeginTransaction();
            try
            {
                await DataBase.Query(BookTracksRepository.bookTracksTableName).Where(nameof(BookTrackRecord.BookId), bookId).DeleteAsync(transaction);
                await DataBase.Query(booksAuthorsTableName).Where(nameof(BookAuthorRecord.BookId), bookId).DeleteAsync(transaction);
                await DataBase.Query(booksTableName).Where(nameof(BookRecord.Id), bookId).DeleteAsync(transaction);
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }

        }
    }
}
