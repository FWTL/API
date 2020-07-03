using Microsoft.AspNetCore.Identity;
using System;

namespace FWTL.Domain.Users
{
    public class User : IdentityUser<Guid>
    {
        public string TimeZoneId { get; set; }
    }
}