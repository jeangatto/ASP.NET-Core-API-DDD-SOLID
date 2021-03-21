using SGP.Shared.Interfaces;
using System;

namespace SGP.Infrastructure.Services
{
    public class UtcDateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.UtcNow;
    }
}