using System.Collections.Generic;

namespace FWTL.Core.Services
{
    public interface ITimeZonesService
    {
        IEnumerable<string> GetAll();

        bool Exist(string zoneId);
    }
}