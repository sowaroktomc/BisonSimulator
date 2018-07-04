using System;

namespace Sowalabs.Bison.Common.DateTimeProvider
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}
