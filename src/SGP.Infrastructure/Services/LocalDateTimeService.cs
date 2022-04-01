using System;
using SGP.Shared.Interfaces;

namespace SGP.Infrastructure.Services;

public class LocalDateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}