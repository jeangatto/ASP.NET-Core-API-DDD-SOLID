using SGP.Shared.Interfaces;
using System;

namespace SGP.Infrastructure.Services
{
    public class LocalDateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
    }
}