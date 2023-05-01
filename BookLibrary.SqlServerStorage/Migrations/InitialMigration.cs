using FluentMigrator;

namespace BookLibrary.SqlServerStorage.Migrations
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
            Create.ForeignKey("FK_BooksAuthorsAuthorId")
                .FromTable("BooksAuthors").ForeignColumn("AuthorId")
                .ToTable("Authors").PrimaryColumn("Id");
            Create.ForeignKey("FK_BooksAuthorsBookId")
                .FromTable("BooksAuthors").ForeignColumn("BookId")
                .ToTable("Books").PrimaryColumn("Id");

            Create.Table("BookTracks")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("BookId").AsGuid().NotNullable()
                .WithColumn("UserId").AsString(100).NotNullable()
                .WithColumn("ActionDate").AsDateTime().NotNullable()
                .WithColumn("Action").AsString(256).NotNullable();
            Create.ForeignKey("FK_BookTracksBookId")
                .FromTable("BookTracks").ForeignColumn("BookId")
                .ToTable("Books").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_BookTracksBookId");
            Delete.Table("BookTracks");

            Delete.ForeignKey("FK_BooksAuthorsBookId");
            Delete.ForeignKey("FK_BooksAuthorsAuthorId");
            Delete.Table("BooksAuthors");

            Delete.Table("Books");

            Delete.Table("Authors");
        }
    }
}
