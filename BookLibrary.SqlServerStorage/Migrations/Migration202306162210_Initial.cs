using FluentMigrator;

namespace BookLibrary.SqlServerStorage.Migrations
{
    [TimestampedMigration(year: 2023, month: 06, day: 16, hour: 22, minute: 10)]
    public class Migration202306162210_Initial : Migration
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
                .ToTable("Authors").PrimaryColumn("Id").OnDeleteOrUpdate(System.Data.Rule.Cascade);
            Create.ForeignKey("FK_BooksAuthorsBookId")
                .FromTable("BooksAuthors").ForeignColumn("BookId")
                .ToTable("Books").PrimaryColumn("Id").OnDeleteOrUpdate(System.Data.Rule.Cascade);

            Create.Table("BookTracks")
                .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
                .WithColumn("BookId").AsGuid().NotNullable()
                .WithColumn("UserId").AsString(100).NotNullable()
                .WithColumn("ActionDate").AsDateTime().NotNullable()
                .WithColumn("Action").AsString(256).NotNullable();
            Create.ForeignKey("FK_BookTracksBookId")
                .FromTable("BookTracks").ForeignColumn("BookId")
                .ToTable("Books").PrimaryColumn("Id").OnDeleteOrUpdate(System.Data.Rule.Cascade);
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
