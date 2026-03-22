using System.Globalization;

namespace PomoTimer.Domain
{
    public class Time
    {
        public TimeOnly TimeOnly { get; protected set; }
        public static Time MinValue = new Time(TimeOnly.MinValue);
        public Time(TimeOnly timeOnly)
        {
            TimeOnly = timeOnly;
        }
        public static Time Create(int hours, int minutes, int seconds) =>
            new Time(new TimeOnly(hours, minutes, seconds, 0));
        public static Time Create(int hours, int minutes, int seconds, int milliseconds) =>
            new Time(new TimeOnly(hours, minutes, seconds, milliseconds));
        public static Time FromString(string timeStr)
        {
            string[] formats = { "HH:mm", "H:mm" };
            var culture = CultureInfo.GetCultureInfo("ja-JP"); // 日本
            var styles = DateTimeStyles.AllowWhiteSpaces;
            if (TimeOnly.TryParseExact(timeStr, formats, culture, styles, out TimeOnly timeOnly))
                return new Time(timeOnly);
            else
                return new Time(TimeOnly.MinValue);
        }
        public Time Add(int hours, int minutes, int seconds)
        {
            TimeOnly value = TimeOnly;
            value = (hours != 0) ? value.Add(TimeSpan.FromHours(hours)) : value;
            value = (minutes != 0) ? value.Add(TimeSpan.FromMinutes(minutes)) : value;
            value = (seconds != 0) ? value.Add(TimeSpan.FromSeconds(seconds)) : value;
            return new Time(value);
        }
        public Time Subtract(int hours, int minutes, int seconds) =>
            Add(hours * -1, minutes * -1, seconds * -1);
        public override bool Equals(object? obj)
        {
            if((obj == null)|| (this.GetType() != obj.GetType()))
                return false;
            Time time = (Time) obj;
            return this.Equals(time);
        }
        public bool Equals(Time time) => this.TimeOnly.Equals(time.TimeOnly);
        public override int GetHashCode() => this.TimeOnly.GetHashCode();
        public static bool operator ==(Time a, Time b) => a.Equals(b);
        public static bool operator !=(Time a, Time b) => !a.Equals(b);
        public static bool operator >(Time a, Time b) => (a.TimeOnly > b.TimeOnly);
        public static bool operator <(Time a, Time b) => (a.TimeOnly < b.TimeOnly);
        public static bool operator >=(Time a, Time b) => (a.TimeOnly >= b.TimeOnly);
        public static bool operator <=(Time a, Time b) => (a.TimeOnly <= b.TimeOnly);
        public static Time operator +(Time a, Time b)
        {
            return a.Add(b.TimeOnly.Hour, b.TimeOnly.Minute, b.TimeOnly.Second);
        }
        public static Time operator -(Time a, Time b)
        {
            return a.Subtract(b.TimeOnly.Hour, b.TimeOnly.Minute, b.TimeOnly.Second);
        }
    }
}
