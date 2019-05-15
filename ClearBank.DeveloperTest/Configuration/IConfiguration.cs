using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Configuration
{
    public interface IConfiguration
    {
        DataStoreType DataStoreType { get; }
    }
}