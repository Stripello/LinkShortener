using LinkShortener.Dal;
using LinkShortener.Dal.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DalAndBdTests
{
    public class LinkRepositoryTests
    {
        const string connectionString =
            "Server=localhost;Port=5432;User Id=postgres;Password=somepasswordforpostgres;Database=TestLinkShortenerPostgresDb;";

        [Fact]
        public void Add_Succed()
        {
            //Arrange
            var conn = new NpgsqlConnection(connectionString);
            var query = new NpgsqlCommand(@"CREATE TABLE IF NOT EXISTS public.LinkShortenerTable
            (
                Id SERIAL,
                LongLink VARCHAR(30000) NOT NULL,
                HashOfLongLink BIGINT NOT NULL,
                LastCallYear SMALLINT NOT NULL
            );", conn);
            conn.Open();
            query.ExecuteNonQuery();
            conn.Close();
            var testObject = new LinkRepository(new LinkRepositorySettings(connectionString));

            //Act
            var expected = Enumerable.Range(1, 10).ToList();
            var actual = new List<int>() { };
            for (int i = 0; i < 10; i++)
            {
                actual.Add(testObject.Add(new LinkDto { LongLink = "firstLink", HashOfLongLink = 0, LastCallYear = 0 }));
            }

            //Cleanup resources
            conn.Open();
            query.CommandText = ("DROP TABLE public.LinkShortenerTable;");
            query.ExecuteNonQuery();
            conn.Close();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Add_Fail()
        {
            //Arrange
            var conn = new NpgsqlConnection(connectionString);
            var query = new NpgsqlCommand(@"CREATE TABLE IF NOT EXISTS public.LinkShortenerTable
            (
                Id SERIAL,
                LongLink VARCHAR(30000) NOT NULL,
                HashOfLongLink BIGINT NOT NULL,
                LastCallYear SMALLINT NOT NULL
            );", conn);
            conn.Open();
            query.ExecuteNonQuery();
            conn.Close();
            var testObject = new LinkRepository(new LinkRepositorySettings(connectionString));

            //Act
            testObject.ElementAt(42);
            Assert.Throws<NullReferenceException>(() => testObject.Add(null!));

            //Cleanup resources
            conn.Open();
            query.CommandText = ("DROP TABLE public.LinkShortenerTable;");
            query.ExecuteNonQuery();
            conn.Close();
        }

        [Fact]
        public void ElementAt_Succed()
        {
            //Arrange
            var conn = new NpgsqlConnection(connectionString);
            var expected = new LinkDto { Id = 42, LongLink = "www.google.com", HashOfLongLink = 0, LastCallYear = 0 };
            var query = new NpgsqlCommand(@$"CREATE TABLE IF NOT EXISTS public.LinkShortenerTable
            (
                Id SERIAL,
                LongLink VARCHAR(30000) NOT NULL,
                HashOfLongLink BIGINT NOT NULL,
                LastCallYear SMALLINT NOT NULL
            );
                INSERT INTO public.LinkShortenerTable (Id,LongLink,HashOfLongLink,LastCallYear)
                VALUES ({expected.Id},'{expected.LongLink}',{expected.HashOfLongLink},{expected.LastCallYear});", conn);
            conn.Open();
            query.ExecuteNonQuery();
            conn.Close();
            var testObject = new LinkRepository(new LinkRepositorySettings(connectionString));

            //Act
            var actual = testObject.ElementAt(expected.Id);

            //Asert
            Assert.True(expected.Id == actual.Id &&
                expected.LastCallYear == actual.LastCallYear &&
                expected.LongLink == actual.LongLink &&
                expected.HashOfLongLink == actual.HashOfLongLink);

            //Cleanup resources
            conn.Open();
            query.CommandText = ("DROP TABLE public.LinkShortenerTable;");
            query.ExecuteNonQuery();
            conn.Close();
        }

        [Fact]
        public void ElementAt_NotExistingId()
        {
            //Arrange
            var conn = new NpgsqlConnection(connectionString);
            const int randomEmptyId = 1000;
            var query = new NpgsqlCommand(@$"CREATE TABLE IF NOT EXISTS public.LinkShortenerTable
            (
                Id SERIAL,
                LongLink VARCHAR(30000) NOT NULL,
                HashOfLongLink BIGINT NOT NULL,
                LastCallYear SMALLINT NOT NULL
            );", conn);
            conn.Open();
            query.ExecuteNonQuery();
            conn.Close();
            var testObject = new LinkRepository(new LinkRepositorySettings(connectionString));

            //Act
            var actual = testObject.ElementAt(randomEmptyId);

            //Assert
            Assert.Null(actual.LongLink);

            //Cleanup resources
            conn.Open();
            query.CommandText = ("DROP TABLE public.LinkShortenerTable;");
            query.ExecuteNonQuery();
            conn.Close();
        }

        [Fact]
        public void Find_Succed()
        {
            //Arrange
            var conn = new NpgsqlConnection(connectionString);
            var expected = new LinkDto { Id = 42, LongLink = "www.google.com", HashOfLongLink = 15, LastCallYear = 0 };
            var query = new NpgsqlCommand(@$"CREATE TABLE IF NOT EXISTS public.LinkShortenerTable
            (
                Id SERIAL,
                LongLink VARCHAR(30000) NOT NULL,
                HashOfLongLink BIGINT NOT NULL,
                LastCallYear SMALLINT NOT NULL
            );
                INSERT INTO public.LinkShortenerTable (Id,LongLink,HashOfLongLink,LastCallYear)
                VALUES ({expected.Id},'{expected.LongLink}',{expected.HashOfLongLink},{expected.LastCallYear}),
                       ({expected.Id - 1},'{expected.LongLink}',{expected.HashOfLongLink - 1},{expected.LastCallYear}),
                       ({expected.Id - 2},'{expected.LongLink}textSalt',{expected.HashOfLongLink},{expected.LastCallYear});", conn);
            conn.Open();
            query.ExecuteNonQuery();
            conn.Close();
            var testObject = new LinkRepository(new LinkRepositorySettings(connectionString));

            //Act
            var actual = testObject.Find(expected.HashOfLongLink, expected.LongLink);

            //Asert
            Assert.True(expected.Id == actual.Id &&
                expected.LastCallYear == actual.LastCallYear &&
                expected.LongLink == actual.LongLink &&
                expected.HashOfLongLink == actual.HashOfLongLink);

            //Cleanup resources
            conn.Open();
            query.CommandText = ("DROP TABLE public.LinkShortenerTable;");
            query.ExecuteNonQuery();
            conn.Close();
        }

        [Fact]
        public void Find_NotFound()
        {
            //Arrange
            var conn = new NpgsqlConnection(connectionString);
            var correctElement = new LinkDto { Id = 42, LongLink = "www.google.com", HashOfLongLink = 15, LastCallYear = 0 };
            var query = new NpgsqlCommand(@$"CREATE TABLE IF NOT EXISTS public.LinkShortenerTable
            (
                Id SERIAL,
                LongLink VARCHAR(30000) NOT NULL,
                HashOfLongLink BIGINT NOT NULL,
                LastCallYear SMALLINT NOT NULL
            );
                INSERT INTO public.LinkShortenerTable (Id,LongLink,HashOfLongLink,LastCallYear)
                VALUES ({correctElement.Id - 1},'{correctElement.LongLink}',{correctElement.HashOfLongLink - 1},{correctElement.LastCallYear}),
                       ({correctElement.Id - 2},'{correctElement.LongLink}textSalt',{correctElement.HashOfLongLink},{correctElement.LastCallYear});", conn);
            conn.Open();
            query.ExecuteNonQuery();
            conn.Close();
            var testObject = new LinkRepository(new LinkRepositorySettings(connectionString));

            //Act
            var actual = testObject.Find(correctElement.HashOfLongLink, correctElement.LongLink);

            //Asert
            Assert.Null(actual.LongLink);

            //Cleanup resources
            conn.Open();
            query.CommandText = ("DROP TABLE public.LinkShortenerTable;");
            query.ExecuteNonQuery();
            conn.Close();
        }

        [Fact]
        public void RemoveAll_Succed()
        {
            //Arrange
            var conn = new NpgsqlConnection(connectionString);
            const int someYear = 21;
            var query = new NpgsqlCommand(@$"CREATE TABLE IF NOT EXISTS public.LinkShortenerTable
            (
                Id SERIAL,
                LongLink VARCHAR(30000) NOT NULL,
                HashOfLongLink BIGINT NOT NULL,
                LastCallYear SMALLINT NOT NULL
            );
                INSERT INTO public.LinkShortenerTable (LongLink,HashOfLongLink,LastCallYear)
                VALUES ('',0,{someYear}),
                       ('',0,{someYear + 1}),
                       ('',0,{someYear - 1});", conn);
            conn.Open();
            query.ExecuteNonQuery();
            conn.Close();
            var testObject = new LinkRepository(new LinkRepositorySettings(connectionString));

            //Act
            testObject.RemoveAll(someYear);
            query.CommandText = $@"SELECT COUNT(id) FROM public.LinkShortenerTable;";
            conn.Open();
            var actual = (long)query.ExecuteScalar()!;
            conn.Close();
            var expected = 2;

            //Asert
            Assert.Equal(actual, expected);

            //Cleanup resources
            conn.Open();
            query.CommandText = ("DROP TABLE public.LinkShortenerTable;");
            query.ExecuteNonQuery();
            conn.Close();
        }
    }
}