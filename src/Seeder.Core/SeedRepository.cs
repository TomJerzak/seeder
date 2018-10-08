using System.Collections.Generic;
using Npgsql;
using Seeder.Core;

namespace Seeder
{
    public class SeedRepository : ISeedRepository
    {
        private const string Schema = "schema";
        private const string Table = "table";
        private const string Public = "public";
        private const string SeedsHistory = "__SeedsHistory";
        private const string SeedId = "SeedId";
        private const string ProductVersion = "ProductVersion";

        private const string CreateSeedsHistoryTableQuery = "CREATE TABLE public.\"__SeedsHistory\" (\"SeedId\" varchar(150) NOT NULL, \"ProductVersion\" varchar(32) NOT NULL, CONSTRAINT \"PK___SeedsHistory\" PRIMARY KEY(\"SeedId\")) WITH(OIDS = FALSE);";
        private const string DropSeedsHistoryTableQuery = "DROP TABLE public.\"__SeedsHistory\"";

        private readonly string _connectionString;

        public SeedRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<string> GetSeedsHistory()
        {
            var seedsHistory = new List<string>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT \"SeedId\" FROM public.\"__SeedsHistory\"";

                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                            seedsHistory.Add(reader.GetString(0));
                }
            }

            return seedsHistory;
        }

        public bool IsExistsSeedsHistory()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_schema = @schema AND table_name = @table)";
                    command.Parameters.AddWithValue(Schema, Public);
                    command.Parameters.AddWithValue(Table, SeedsHistory);

                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                            return reader.GetBoolean(0);
                }
            }

            return false;
        }

        public void CreateSeedsHistory()
        {

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = CreateSeedsHistoryTableQuery;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteSeedsHistory()
        {

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = DropSeedsHistoryTableQuery;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddToSeedsHistory(string seedId, string productVersion)
        {
            if (!IsExistsSeedsHistory())
                CreateSeedsHistory();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO public.\"__SeedsHistory\" (\"SeedId\", \"ProductVersion\") VALUES(@seedId, @productVersion)";
                    command.Parameters.AddWithValue(SeedId, seedId);
                    command.Parameters.AddWithValue(ProductVersion, productVersion);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void RunScript(string commandText)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = commandText;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
