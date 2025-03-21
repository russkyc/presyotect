using Presyotect.Core.Types;

namespace Presyotect.Utilities;

public static class DateTimeExtensions
{
    public static string AsIdentifier(this DateTime date)
    {
        return date.ToString("yyyyMMdd");
    }
    
    public static DateTime NextOfWeekday(this DateTime date)
    {
        int daysToAdd = ((int)date.DayOfWeek - (int)date.DayOfWeek + 7) % 7;
        if (daysToAdd == 0)
        {
            daysToAdd = 7;
        }
        return date.AddDays(daysToAdd);
    }
    
    public static DateTime EndOfHour(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, date.Hour, 59, 59, 999);
    }

    public static DateTime EndOfDay(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
    }

    public static DateTime EndOfWeek(this DateTime date)
    {
        var culture = Thread.CurrentThread.CurrentCulture;
        var firstDayOfWeek = culture.DateTimeFormat.FirstDayOfWeek;
        int offset = (int)firstDayOfWeek - (int)date.DayOfWeek;
        if (offset > 0)
            offset -= 7;
        DateTime endOfWeek = date.AddDays(offset + 6);

        return new DateTime(endOfWeek.Year, endOfWeek.Month, endOfWeek.Day, 23, 59, 59, 999);
    }

    public static DateTime EndOfMonth(this DateTime date)
    {
        DateTime firstDayOfNextMonth = new DateTime(date.Year, date.Month, 1).AddMonths(1);
        DateTime lastDayOfMonth = firstDayOfNextMonth.AddDays(-1);

        return new DateTime(lastDayOfMonth.Year, lastDayOfMonth.Month, lastDayOfMonth.Day, 23, 59, 59, 999);
    }

    public static DateTime EndOfYear(this DateTime date)
    {
        DateTime lastDayOfYear = new DateTime(date.Year, 12, 31);

        return new DateTime(lastDayOfYear.Year, lastDayOfYear.Month, lastDayOfYear.Day, 23, 59, 59, 999);
    }

    public static DateTime StartOfHour(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0, 0);
    }

    public static DateTime StartOfDay(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
    }

    public static DateTime StartOfWeek(this DateTime date, bool treatMondayAsFirstDay = false)
    {
        var firstDayOfWeek = treatMondayAsFirstDay ? DayOfWeek.Monday : Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
        int offset = (int)date.DayOfWeek - (int)firstDayOfWeek;

        if (offset < 0)
        {
            offset += 7;
        }
        
        DateTime startOfWeek = date.AddDays(-offset);

        if (treatMondayAsFirstDay && startOfWeek.DayOfWeek != DayOfWeek.Monday)
        {
            startOfWeek = startOfWeek.AddDays(-((int)startOfWeek.DayOfWeek - (int)DayOfWeek.Monday));
        }

        return new DateTime(startOfWeek.Year, startOfWeek.Month, startOfWeek.Day, 0, 0, 0, 0);
    }

    public static DateTime StartOfMonth(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, 1, 0, 0, 0, 0);
    }

    public static DateTime StartOfYear(this DateTime date)
    {
        return new DateTime(date.Year, 1, 1, 0, 0, 0, 0);
    }

    public static DateTime AddInterval(this DateTime date, MonitoringInterval interval, int count)
    {
        return interval switch
        {
            MonitoringInterval.Hourly => date.AddHours(count),
            MonitoringInterval.Every3Hours => date.AddHours(3 * count),
            MonitoringInterval.Every4Hours => date.AddHours(4 * count),
            MonitoringInterval.Every6Hours => date.AddHours(6 * count),
            MonitoringInterval.Every8Hours => date.AddHours(8 * count),
            MonitoringInterval.Every12Hours => date.AddHours(12 * count),
            MonitoringInterval.Daily => date.AddDays(count),
            MonitoringInterval.Weekly => date.AddDays(7 * count),
            MonitoringInterval.Every2Weeks => date.AddDays(14 * count),
            MonitoringInterval.Monthly => date.AddMonths(count),
            MonitoringInterval.Every2Months => date.AddMonths(2 * count),
            MonitoringInterval.Quarterly => date.AddMonths(3 * count),
            MonitoringInterval.BiAnnually => date.AddMonths(6 * count),
            MonitoringInterval.Annually => date.AddYears(count),
            _ => date
        };
    }

    public static DateTime NextEndOfInterval(this DateTime date, MonitoringInterval interval)
    {
        return interval switch
        {
            MonitoringInterval.Hourly => date.EndOfHour().AddHours(1),
            MonitoringInterval.Every3Hours => date.AddHours(3 - (date.Hour % 3)).EndOfHour(),
            MonitoringInterval.Every4Hours => date.AddHours(4 - (date.Hour % 4)).EndOfHour(),
            MonitoringInterval.Every6Hours => date.AddHours(6 - (date.Hour % 6)).EndOfHour(),
            MonitoringInterval.Every8Hours => date.AddHours(8 - (date.Hour % 8)).EndOfHour(),
            MonitoringInterval.Every12Hours => date.AddHours(12 - (date.Hour % 12)).EndOfHour(),
            MonitoringInterval.Daily => date.EndOfDay().AddDays(1),
            MonitoringInterval.Weekly => date.EndOfWeek().AddDays(7),
            MonitoringInterval.Every2Weeks => date.EndOfWeek().AddDays(14),
            MonitoringInterval.Monthly => date.EndOfMonth().AddMonths(1),
            MonitoringInterval.Every2Months => date.EndOfMonth().AddMonths(2),
            MonitoringInterval.Quarterly => date.EndOfMonth().AddMonths(3),
            MonitoringInterval.BiAnnually => date.EndOfMonth().AddMonths(6),
            MonitoringInterval.Annually => date.EndOfYear().AddYears(1),
            _ => date
        };
    }

    public static DateTime NextStartOfInterval(this DateTime date, MonitoringInterval interval)
    {
        return interval switch
        {
            MonitoringInterval.Hourly => date.StartOfHour().AddHours(1),
            MonitoringInterval.Every3Hours => date.AddHours(3 - (date.Hour % 3)).StartOfHour(),
            MonitoringInterval.Every4Hours => date.AddHours(4 - (date.Hour % 4)).StartOfHour(),
            MonitoringInterval.Every6Hours => date.AddHours(6 - (date.Hour % 6)).StartOfHour(),
            MonitoringInterval.Every8Hours => date.AddHours(8 - (date.Hour % 8)).StartOfHour(),
            MonitoringInterval.Every12Hours => date.AddHours(12 - (date.Hour % 12)).StartOfHour(),
            MonitoringInterval.Daily => date.StartOfDay().AddDays(1),
            MonitoringInterval.Weekly => date.StartOfWeek().AddDays(7),
            MonitoringInterval.Every2Weeks => date.StartOfWeek().AddDays(14),
            MonitoringInterval.Monthly => date.StartOfMonth().AddMonths(1),
            MonitoringInterval.Every2Months => date.StartOfMonth().AddMonths(2),
            MonitoringInterval.Quarterly => date.StartOfMonth().AddMonths(3),
            MonitoringInterval.BiAnnually => date.StartOfMonth().AddMonths(6),
            MonitoringInterval.Annually => date.StartOfYear().AddYears(1),
            _ => date
        };
    }

    public static DateTime NextInterval(this DateTime date, MonitoringInterval interval)
    {
        return interval switch
        {
            MonitoringInterval.Hourly => date.AddHours(1),
            MonitoringInterval.Every3Hours => date.AddHours(3),
            MonitoringInterval.Every4Hours => date.AddHours(4),
            MonitoringInterval.Every6Hours => date.AddHours(6),
            MonitoringInterval.Every8Hours => date.AddHours(8),
            MonitoringInterval.Every12Hours => date.AddHours(12),
            MonitoringInterval.Daily => date.AddDays(1),
            MonitoringInterval.Weekly => date.AddDays(7),
            MonitoringInterval.Every2Weeks => date.AddDays(14),
            MonitoringInterval.Monthly => date.AddMonths(1),
            MonitoringInterval.Every2Months => date.AddMonths(2),
            MonitoringInterval.Quarterly => date.AddMonths(3),
            MonitoringInterval.BiAnnually => date.AddMonths(6),
            MonitoringInterval.Annually => date.AddYears(1),
            _ => date
        };
    }
}