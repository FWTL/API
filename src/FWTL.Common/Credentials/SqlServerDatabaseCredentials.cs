using FWTL.Common.Extensions;
using FWTL.Core.Credentials;
using Microsoft.Extensions.Configuration;

namespace FWTL.Common.Credentials
{
    public sealed class SqlServerDatabaseCredentials : IConnectionString
    {
        private readonly string _catalog;

        private readonly string _password;

        private readonly int _port;

        private readonly string _url;

        private readonly string _user;

        public SqlServerDatabaseCredentials(IConfiguration configuration, string prefix)
        {
            _url = configuration[$"{prefix}:Url"];
            _port = configuration[$"{prefix}:Port"].To<int>();
            _catalog = configuration[$"{prefix}:Catalog"];
            _user = configuration[$"{prefix}:User"];
            _password = configuration[$"{prefix}:Password"];
        }

        public string ConnectionString =>
            $"Server=tcp:{_url},{_port};Initial Catalog={_catalog};Persist Security Info=False;User ID={_user};Password={_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
    }
}