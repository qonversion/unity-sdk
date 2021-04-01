using System;

internal static class Utils
{
    internal static DateTime FormatDate(long time)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(time);

        return dateTimeOffset.DateTime.ToLocalTime();
    }
}