using System;

namespace SGP.Shared.Abstractions;

public interface IDateTimeService
{
    DateTime Now { get; }
}
