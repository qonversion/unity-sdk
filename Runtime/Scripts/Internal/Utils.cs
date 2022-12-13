using System;
using System.Collections.Generic;

internal static class Utils
{
    internal static DateTime FormatDate(long time)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(time);

        return dateTimeOffset.DateTime.ToLocalTime();
    }

    internal static string PrintObjectList<T>(List<T> objectsToPrint)
    {
        if (objectsToPrint == null) return "";

        string result = string.Empty;
        foreach (T val in objectsToPrint)
        {
            if (val != null) result += val.ToString();
        }

        return result;
    }
}