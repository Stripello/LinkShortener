using LinkShortener.Dal.Interfaces;
using LinkShortener.Dal.Models;
using Npgsql;
using Serilog;

namespace LinkShortener.Dal
{
    public class LinkRepository : ILinkRepository
    {
        private readonly string connectionString;
        public LinkRepository(LinkRepositorySettings settings)
        {
            connectionString = settings._connectionString;
        }

        public void RemoveAll(ushort year)
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                var querry = new NpgsqlCommand(@$"
                    DELETE FROM LinkShortenerTable
                    WHERE LastCallYear < $1;
                ", conn);
                querry.Parameters.AddWithValue((int)year);
                conn.Open();
                querry.ExecuteNonQuery();
                conn.Close();
            }
            catch (NpgsqlException)
            {
                var errorMessage = $"RemoveAll error, year = {year}";
                Log.Information(errorMessage);
                throw new Exception("Dal method RemoveAll() error.");
            }
        }

        public LinkDto ElementAt(int id)
        {
            var answer = new LinkDto();
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                var querry = new NpgsqlCommand(@$"
                    SELECT * FROM LinkShortenerTable
                    WHERE id=$1;
                ", conn);
                querry.Parameters.AddWithValue(id);
                conn.Open();
                NpgsqlDataReader rawData = querry.ExecuteReader();
                var linkDto = new LinkDto() { Id = id };
                while (rawData.Read())
                {
                    linkDto.LongLink = rawData[1].ToString() ?? string.Empty;
                    linkDto.HashOfLongLink = long.Parse(rawData[2].ToString() ?? "0");
                    linkDto.LastCallYear = ushort.Parse(rawData[3].ToString() ?? "0");
                }
                conn.Close();
                return linkDto;
            }
            catch (NpgsqlException)
            {
                var errorMessage = $"Trying get element by Id = {id} error in method DataServicePostgres.ElementAt().\n" +
                    $"Formed object: Id={answer.Id}, LongLink={answer.LongLink}, HashOfLongLink={answer.HashOfLongLink}, LastCallYear={answer.LastCallYear}.";
                Log.Information(errorMessage);
                throw new Exception("Dal method ElementAt() error.");
            }
        }

        public LinkDto Find(long hash, string fullName)
        {
            var answer = new LinkDto();
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                var querry = new NpgsqlCommand(@$"
                    SELECT * FROM LinkShortenerTable
                    WHERE HashOfLongLink=$1 AND LongLink=$2;
                ", conn);
                querry.Parameters.AddWithValue(hash);
                querry.Parameters.AddWithValue(fullName);
                conn.Open();
                NpgsqlDataReader rawData = querry.ExecuteReader();
                var linkDto = new LinkDto();
                while (rawData.Read())
                {
                    linkDto.Id = int.Parse(rawData[0].ToString() ?? "-1");
                    linkDto.LongLink = rawData[1].ToString() ?? string.Empty;
                    linkDto.HashOfLongLink = long.Parse(rawData[2].ToString() ?? "0");
                    linkDto.LastCallYear = ushort.Parse(rawData[3].ToString() ?? "0");
                }
                conn.Close();
                return linkDto;
            }
            catch (NpgsqlException)
            {
                var errorMessage = $"Trying get element by hash = {hash} and fullName = {fullName} error in method DataServicePostgres.Find().\n" +
                    $"Formed object: Id={answer.Id}, LongLink={answer.LongLink}, HashOfLongLink={answer.HashOfLongLink}, LastCallYear={answer.LastCallYear}.";
                Log.Information(errorMessage);
                throw new Exception("Dal method Find() error.");
            }
        }

        public int Add(LinkDto linkDto)
        {
            try
            {
                var conn = new NpgsqlConnection(connectionString);
                var querry = new NpgsqlCommand(@$"
                    INSERT INTO LinkShortenerTable (LongLink,HashOfLongLink,LastCallYear)
                    VALUES ($1,$2,$3)
                    RETURNING id;", conn);
                querry.Parameters.AddWithValue(linkDto.LongLink);
                querry.Parameters.AddWithValue(linkDto.HashOfLongLink);
                //ducttape
                querry.Parameters.AddWithValue((int)linkDto.LastCallYear);

                conn.Open();
                return (int)querry.ExecuteScalar()!;
            }
            catch (NpgsqlException)
            {
                var errorMessage = $"StoreData error. DtoObject:\nLongLink:{linkDto.LongLink}" +
                    $"\nHash:{linkDto.HashOfLongLink}\nLastCallYear:{linkDto.LastCallYear}";
                Log.Information(errorMessage);
                throw new Exception("Dal method Add() error.");
            }
        }
    }
}