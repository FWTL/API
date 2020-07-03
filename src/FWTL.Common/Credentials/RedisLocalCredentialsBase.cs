using FWTL.Core.Credentials;
using Microsoft.Extensions.Configuration;

namespace FWTL.Common.Credentials
{
    public sealed class RedisLocalCredentialsBase : IConnectionString
    {
        private readonly bool _allowAdmin;

        public RedisLocalCredentialsBase(IConfiguration configuration)
        {
            _allowAdmin = true;
        }

        public string ConnectionString => $"localhost,abortConnect=False,allowAdmin={_allowAdmin}";
    }
}