using FluentMigrator;

namespace BookLibrary.SqLiteStorage.Migrations
{
    [Migration(1)]
    public class InitialMigration : Migration
    {
        public override void Up()
        {
            Create.Table("Authors")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("Name").AsString(64).NotNullable();

            Create.Table("Books")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("Name").AsString(1024).NotNullable()
                .WithColumn("Year").AsDateTime().Nullable()
                .WithColumn("Available").AsBoolean().NotNullable().WithDefaultValue(true);

            Create.Table("BooksAuthors")
                .WithColumn("BookId").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("AuthorId").AsGuid().NotNullable().PrimaryKey();

            Create.Table("BookTracks")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("BookId").AsGuid().NotNullable()
                .WithColumn("UserId").AsString(100).NotNullable()
                .WithColumn("ActionDate").AsDateTime().NotNullable()
                .WithColumn("Action").AsString(256).NotNullable();
        }

        public override void Down()
        {
            Delete.Table("BookTracks");

            Delete.Table("BooksAuthors");

            Delete.Table("Books");

            Delete.Table("Authors");
        }
    }
}
