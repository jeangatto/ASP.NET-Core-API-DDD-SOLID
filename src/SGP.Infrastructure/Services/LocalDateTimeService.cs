using SGP.Shared.Interfaces;
using System;

namespace SGP.Infrastructure.Services
{
    public class LocalDateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}