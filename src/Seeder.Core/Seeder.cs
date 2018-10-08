using System;
using System.Collections.Generic;
using System.IO;

namespace Seeder.Core
{
    public class Seeder : ISeeder
    {
        public void ExecuteChanges(ISeedRepository seedRepository)
        {            
            var listOfChanges = Engine.ListOfChanges(GetSeedsHistory(seedRepository), GetSeedsFilesHistory());

            Console.WriteLine("\nList of changes:");
            foreach (var seedId in listOfChanges)
            {
                Console.WriteLine(seedId);

                var fileStream = new FileStream($"{Constants.StorageName}/{seedId}{Constants.SqlExtension}", FileMode.Open);
                using (var streamReader = new StreamReader(fileStream))
                    seedRepository.RunScript(streamReader.ReadToEnd());

                seedRepository.AddToSeedsHistory(seedId.Replace(Constants.SqlExtension, string.Empty), Engine.GetProductVersion());
            }
        }

        private static List<string> GetSeedsFilesHistory()
        {
            var seedsFilesHistory = new List<string>();

            Console.WriteLine("\nFiles:");

            var files = new DirectoryInfo(Constants.StorageName).GetFiles();
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
