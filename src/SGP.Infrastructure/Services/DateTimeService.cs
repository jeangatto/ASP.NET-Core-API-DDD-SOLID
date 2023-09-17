using System;
using SGP.Shared.Abstractions;

namespace SGP.Infrastructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.Now;
}
