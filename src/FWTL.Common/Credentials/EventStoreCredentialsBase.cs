using FWTL.Common.Extensions;
using FWTL.Core.Credentials;
using Microsoft.Extensions.Configuration;

namespace FWTL.Common.Credentials
{
    public sealed class EventStoreCredentialsBase : IConnectionString
    {
        public EventStoreCredentialsBase(IConfiguration configuration)
        {
            User = configuration["EventStore:User"];
            Password = configuration["EventStore:Password"];
            Ip = configuration["EventStore:Ip"];
            Port = configuration["EventStore:Port"].To<int>();
        }

        public string Ip { get; }

        public string Password { get; }

        public int Port { get; }

        public string User { get; }

        public string ConnectionString => $"tcp://{User}:{Password}@{Ip}:{Port}";
    }
}