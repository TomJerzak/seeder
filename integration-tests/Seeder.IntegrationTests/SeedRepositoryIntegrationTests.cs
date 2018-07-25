﻿using FluentAssertions;
using Xunit;

namespace Seeder.IntegrationTests
{
    public class SeedRepositoryIntegrationTests
    {
        private string _connectionString = "User ID=seeder;Password=seeder;Host=127.0.0.1;Port=5432;Database=seeder;Pooling=true;";

        [Fact]
        public void seeds_history_table_not_exists()
        {
            ISeedRepository seedRepository = new SeedRepository(_connectionString);

            seedRepository.IsExistsSeedsHistory().Should().BeFalse();
        }

        [Fact]
        public void seeds_history_table_is_exists()
        {
            ISeedRepository seedRepository = new SeedRepository(_connectionString);
            seedRepository.CreateSeedsHistory();

            seedRepository.IsExistsSeedsHistory().Should().BeTrue();
            seedRepository.DeleteSeedsHistory();
        }

        [Fact]
        public void insert_with_creating_table()
        {
            ISeedRepository seedRepository = new SeedRepository(_connectionString);

            seedRepository.AddToSeedsHistory("Test", "1.0");

            seedRepository.IsExistsSeedsHistory().Should().BeTrue();
            seedRepository.DeleteSeedsHistory();
        }
    }
}