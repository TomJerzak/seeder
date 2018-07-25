namespace Seeder
{
    public interface ISeedRepository
    {
        bool IsExistsSeedsHistory();

        void CreateSeedsHistory();

        void DeleteSeedsHistory();

        void AddToSeedsHistory(string seedId, string productVersion);

        void RunScript(string seedId);
    }
}
