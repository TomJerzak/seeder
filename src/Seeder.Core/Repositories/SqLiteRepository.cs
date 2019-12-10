using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Seeder.Core.Repositories
{
    public class SqLiteRepository : ISeedRepository
    {
        private const string Table = "table";
        private const string SeedsHistory = "__SeedsHistory";
        private const string SeedId = "SeedId";
        private const string ProductVersion = "ProductVersion";

        private const string CreateSeedsHistoryTableQuery = "CREATE TABLE \"__SeedsHistory\" (\"SeedId\" TEXT NOT NULL, \"ProductVersion\" TEXT NOT NULL, PRIMARY KEY(\"SeedId\"));";
        private const string DropSeedsHistoryTableQuery = "DROP TABLE \"__SeedsHistory\"";

        private readonly string _connectionString;

        public SqLiteRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<string> GetSeedsHistory()
        {
            var seedsHistory = new List<string>();
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT \"SeedId\" FROM \"__SeedsHistory\"";

                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                            seedsHistory.Add(reader.GetString(0));
                }
            }

            return seedsHistory;
        }

        public bool IsExistsSeedsHistory()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT EXISTS (SELECT * FROM sqlite_master WHERE type='table' AND name=@table)";
                    command.Parameters.AddWithValue(Table, SeedsHistory);

                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                        {
                            return reader.GetBoolean(0);
                        }
                }
            }

            return false;
        }
        public void CreateSeedsHistory()
        {

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = CreateSeedsHistoryTableQuery;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteSeedsHistory()
        {

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand())
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

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO \"__SeedsHistory\" (\"SeedId\", \"ProductVersion\") VALUES(@SeedId, @ProductVersion)";
                    command.Parameters.AddWithValue(SeedId, seedId);
                    command.Parameters.AddWithValue(ProductVersion, productVersion);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void RunScript(string commandText)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqliteCommand())
                {
                    command.Connection = connection;
                    command.CommandText = commandText;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
