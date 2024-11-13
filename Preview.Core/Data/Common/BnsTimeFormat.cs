using System.Text;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Data.Common;
internal interface ITime
{
    internal ulong Ticks { get; }

    /// <summary>Returns the year part of this DateTime. The returned value is an integer between 1 and 9999.</summary>
    public int Year { get; }

    /// <summary>Returns the month part of this DateTime. The returned value is an integer between 1 and 12.</summary>
    public int Month { get; }

    /// <summary>Returns the day-of-month part of this DateTime. The returned value is an integer between 1 and 31.</summary>
    public int Day { get; }

    /// <summary>Returns the hour part of this DateTime. The returned value is an integer between 0 and 23.</summary>
    public sbyte Hour { get; }

    /// <summary>Returns the minute part of this DateTime. The returned value is an integer between 0 and 59.</summary>
    public int Minute { get; }

    /// <summary>Returns the second part of this DateTime. The returned value is an integer between 0 and 59.</summary>
    public int Second { get; }
}

internal static class BnsTimeFormat
{
    internal static string Format(ITime value, string format, IFormatProvider formatProvider)
    {
        if (string.IsNullOrEmpty(format))
        {
            return FormatC(value); // formatProvider ignored, as "c" is invariant
        }

        if (format.Length == 1)
        {
            char c = format[0];

            if (c == 'c' || (c | 0x20) == 't') // special-case to optimize the default TimeSpan format
            {
                return FormatC(value); // formatProvider ignored, as "c" is invariant
            }

            if (c == 'g')
            {
                return FormatG(value);
            }

            throw new FormatException("Format_InvalidString");
        }

        var vlb = new StringBuilder(256);
        FormatCustomized(value, format, ref vlb);
        return vlb.ToString();
    }

    internal static string FormatC(ITime value)
    {
        return $"{value.Year}/{value.Month}/{value.Day} {value.Hour}:{value.Minute:00}:{value.Second:00}";
    }

    internal static string FormatG(ITime value)
    {
        return null;
    }

    private static void FormatCustomized(ITime value, scoped ReadOnlySpan<char> format, ref StringBuilder result)
    {
        for (int i = 0; i < format.Length;)
        {
            char ch = format[i];
            int tokenLen;

            switch (ch)
            {
                case 'y':
                    i += tokenLen = ParseRepeatPattern(format, i, ch);
                    FormatDigits(ref result, value.Year, tokenLen, 4);
                    break;
                case 'M':
                    i += tokenLen = ParseRepeatPattern(format, i, ch);
                    FormatDigits(ref result, value.Month, tokenLen, 2);
                    break;
                case 'h':
                    i += tokenLen = ParseRepeatPattern(format, i, ch);
                    FormatDigits(ref result, value.Hour, tokenLen, 2);
                    break;
                case 'm':
                    i += tokenLen = ParseRepeatPattern(format, i, ch);
                    FormatDigits(ref result, value.Minute, tokenLen, 2);
                    break;
                case 's':
                    i += tokenLen = ParseRepeatPattern(format, i, ch);
                    FormatDigits(ref result, value.Second, tokenLen, 2);
                    break;
                case 'd':
                    i += tokenLen = ParseRepeatPattern(format, i, ch);
                    FormatDigits(ref result, value.Day, tokenLen, 8);
                    break;
                default:
                    i++;
                    result.Append(ch);
                    break;
            }
        }
    }

    internal static void FormatDigits(ref StringBuilder outputBuffer, int value, int length, int maximumLength)
    {
        if (length > maximumLength)
            throw new FormatException("Format_InvalidString");

        outputBuffer.Append(value.ToString(new string('0', length)));
    }

    internal static int ParseRepeatPattern(ReadOnlySpan<char> format, int pos, char patternChar)
    {
        int index = pos + 1;
        while ((uint)index < (uint)format.Length && format[index] == patternChar)
        {
            index++;
        }
        return index - pos;
    }

    internal static string AMPM(this sbyte value) => (value < 12 ? "Name.Time.Morning" : "Name.Time.Afternoon").GetText();
}