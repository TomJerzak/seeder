using System.Collections.Generic;

namespace Seeder.Core
{
    public interface ISeedRepository
    {
        List<string> GetSeedsHistory();

        bool IsExistsSeedsHistory();

        void CreateSeedsHistory();

        void DeleteSeedsHistory();

        void AddToSeedsHistory(string seedId, string productVersion);

        void RunScript(string commandText);
    }
}
