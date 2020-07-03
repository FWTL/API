using FWTL.Common.Credentials;

namespace FWTL.Api.Database
{
    public class ApiDatabaseCredentials
    {
        public ApiDatabaseCredentials(LocalDatabaseCredentials credentials)
        {
            ConnectionString = credentials.ConnectionString;
        }

        public ApiDatabaseCredentials(SqlServerDatabaseCredentials credentials)
        {
            ConnectionString = credentials.ConnectionString;
        }

        public string ConnectionString { get; }
    }
}