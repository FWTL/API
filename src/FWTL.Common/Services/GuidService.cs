using FWTL.Core.Services;
using System;

namespace FWTL.Common.Services
{
    public class GuidService : IGuidService
    {
        public Guid New => Guid.NewGuid();
    }
}