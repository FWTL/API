using FWTL.Core.Services;
using NodaTime.TimeZones;
using System.Collections.Generic;
using System.Linq;

namespace FWTL.Common.Services
{
    public class TimeZonesService : ITimeZonesService
    {
        public IEnumerable<string> GetAll()
        {
            return TzdbDateTimeZoneSource.Default.ZoneLocations.Select(zone => zone.ZoneId).OrderBy(zone => zone).ToList();
        }

        public bool Exist(string zoneId)
        {
            return TzdbDateTimeZoneSource.Default.ZoneLocations.Any(zone => zone.ZoneId == zoneId);
        }
    }
}