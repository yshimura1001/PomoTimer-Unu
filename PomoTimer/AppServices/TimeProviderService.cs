using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using PomoTimer.Domain;

namespace PomoTimer.AppServices
{
    public abstract class TimeProviderServiceBase
    {
        public bool IsTest { get; protected set; } = false;
        protected readonly FakeTimeProvider _fakeTimeProvider = Ioc.Default.GetRequiredService<FakeTimeProvider>();
        public TimeProviderServiceBase(bool isTest) => IsTest = isTest;
        protected TimeProvider GetTimeProvider() => (IsTest) ? _fakeTimeProvider : TimeProvider.System;
        public void SetLocalTimeZone()
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            _fakeTimeProvider.SetLocalTimeZone(timeZoneInfo);
        }
        public void SetTime(DateTimeOffset value) => _fakeTimeProvider.SetUtcNow(value.UtcDateTime);
        public void AdvanceTime(TimeSpan delta) => _fakeTimeProvider.Advance(delta);
        protected string ChangeTimeSeparateChar(TimeOnly now, string timeStr) =>
            (now.Second % 2 == 0) ? timeStr : timeStr.Replace(":", " ");
        protected string ChangeTimeSeparateChar(TimeSpan span, string format)
        {
            string timeStr = span.ToString(format);
            return (span.Seconds % 2 == 0) ? timeStr : timeStr.Replace(":", " ");
        }
        public DateTime ToDateTime(Time time)
        {
            TimeOnly timeOnly = time.TimeOnly;
            DateTimeOffset now = GetTimeProvider().GetLocalNow();
            return new DateTime(now.Year, now.Month, now.Day, timeOnly.Hour, timeOnly.Minute, timeOnly.Second, 0);
        }
        public DateTimeOffset ToDateTimeOffset(Time time)
        {
            TimeOnly timeOnly = time.TimeOnly;
            DateTimeOffset now = GetTimeProvider().GetLocalNow();
            return new DateTimeOffset(
                now.Year, now.Month, now.Day, timeOnly.Hour, timeOnly.Minute, timeOnly.Second, 0, TimeSpan.FromHours(9));
        }
        public string ToString(Time time, string format) => ChangeTimeSeparateChar(time.TimeOnly, time.TimeOnly.ToString(format));
        public string ToString(TimeSpan timeSpan, string format)
        {
            format = (format == "HH:mm") ? @"hh\:mm" : @"mm\:ss";
            return ChangeTimeSeparateChar(timeSpan, format);
        }
    }

    public class TimeProviderService : TimeProviderServiceBase
    {
        public const string HHMM = "HH:mm";
        public const string MMSS = "mm:ss";
        public NowTimeProviderService  Now  { get; protected set; }
        public RestTimeProviderService Rest { get; protected set; }
        public TimeProviderService(bool isTest) : base(isTest)
        {
            Now = (isTest) ? DIContainer.GetNowTimeProviderService(true) : DIContainer.GetNowTimeProviderService(false);
            Rest = (isTest) ? DIContainer.GetRestTimeProviderService(true) : DIContainer.GetRestTimeProviderService(false);
        }
    }
    public class NowTimeProviderService : TimeProviderServiceBase
    {
        public NowTimeProviderService(bool isTest) : base(isTest) { }
        protected TimeOnly GetNowTime()
        {
            DateTimeOffset now = GetTimeProvider().GetLocalNow();
            return new TimeOnly(now.Hour, now.Minute, now.Second, 0);
        }
        public Time ToTime() => new Time(GetNowTime());
    }
    public class RestTimeProviderService : TimeProviderServiceBase
    {
        public const string HHMM = "HH:mm";
        public const string MMSS = "mm:ss";
        public RestTimeProviderService(bool isTest) : base(isTest) { }
        public TimeSpan ToTimeSpan(Time a, Time b) => (b.TimeOnly - a.TimeOnly);
    }
}