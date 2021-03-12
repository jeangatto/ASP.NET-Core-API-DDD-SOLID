using SGP.Shared.Interfaces;
using System;

namespace SGP.Shared
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
    }
}