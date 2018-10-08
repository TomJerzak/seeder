using System;
using System.Collections.Generic;
using System.IO;

namespace Seeder.Core
{
    public class Seeder : ISeeder
    {
        public void ExecuteChanges(ISeedRepository seedRepository, string pathToSeedsFilesHistory = "")
        {
            Console.WriteLine("\nList of changes:");
            var listOfChanges = Seed.ListOfChanges(GetSeedsHistory(seedRepository), GetSeedsFilesHistory(pathToSeedsFilesHistory));
            foreach (var seedId in listOfChanges)
            {
                Console.WriteLine(seedId);

                FileStream fileStream;
                if (string.IsNullOrEmpty(pathToSeedsFilesHistory))
                    fileStream = new FileStream($"{Constants.StorageName}/{seedId}.sql", FileMode.Open);
                else
                    fileStream = new FileStream($"{pathToSeedsFilesHistory}\\{Constants.StorageName}/{seedId}.sql", FileMode.Open);

                using (var streamReader = new StreamReader(fileStream))
                    seedRepository.RunScript(streamReader.ReadToEnd());

                seedRepository.AddToSeedsHistory(seedId.Replace(".sql", string.Empty), Seed.GetProductVersion());
            }
        }

        private static List<string> GetSeedsFilesHistory(string pathToSeedsFilesHistory)
        {
            var seedsFilesHistory = new List<string>();

            Console.WriteLine("\nFiles:");
            FileInfo[] files;
            if (string.IsNullOrEmpty(pathToSeedsFilesHistory))
                files = new DirectoryInfo(Constants.StorageName).GetFiles();
            else
                files = new DirectoryInfo($"{pathToSeedsFilesHistory}\\{Constants.StorageName}").GetFiles();

            foreach (var file in files)
            {
                if (file.Extension.Equals(Constants.SqlExtension))
                {
                    seedsFilesHistory.Add(file.Name.Replace(Constants.SqlExtension, string.Empty));
                    Console.WriteLine(file.Name);
                }
            }

            return seedsFilesHistory;
        }

        private List<string> GetSeedsHistory(ISeedRepository seedRepository)
        {
            var seedsHistory = new List<string>();

            Console.WriteLine("\nSeeds:");
            if (seedRepository.IsExistsSeedsHistory())
            {
                seedsHistory = seedRepository.GetSeedsHistory();
                foreach (var seed in seedsHistory)
                    Console.WriteLine(seed);
            }

            return seedsHistory;
        }
    }
}
