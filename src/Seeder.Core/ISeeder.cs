namespace Seeder.Core
{
    public interface ISeeder
    {
        void ExecuteChanges(ISeedRepository seedRepository);
    }
}
