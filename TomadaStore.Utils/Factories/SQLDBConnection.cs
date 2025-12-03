using TomadaStore.Utils.Factories.Interfaces;

namespace TomadaStore.Utils.Factories
{
    public class SQLDBConnection : DBConnectionFactory
    {
        public override IDBConnection CreateDBConnection()
        {
            return new SQLDBConnectionImpl();
        }
    }
}
