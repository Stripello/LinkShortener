using FluentMigrator;

namespace LinkShortener.Migrator.Migrations;

[Migration(202208121836)]
public class CreateTable : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
			CREATE TABLE public.LinkShortenerTable
			(
				Id SERIAL,
				LongLink VARCHAR(30000) NOT NULL,
				HashOfLongLink BIGINT NOT NULL,
				LastCallYear SMALLINT NOT NULL
			);
			");
    }

    public override void Down()
    {
        Execute.Sql(@"DROP TABLE public.LinkShortenerTable");
    }
} 