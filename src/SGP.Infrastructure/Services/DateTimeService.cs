using System;
using SGP.Shared.Interfaces;

namespace SGP.Infrastructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.Now;
}