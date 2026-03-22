
/*
namespace Test
{
    using AddTimeOnlyParams = (TimeOnly Time, int Hours, int Minutes, int Seconds);
    using GetRestTimeStrParams = (TimeOnly Time, string EndTimeStr);
    using GetRestTimeStrParamsWithFormat = (TimeOnly Time, TimeOnly EndTime, string Format);
    public sealed record TestCase<TArgs, TOutput>(TArgs Args, TOutput CompareValue);

    [Collection("Global test collection")]
    public class DateTimeServiceTest
    {
        private readonly TimeService _timeService;
        public DateTimeServiceTest()
        {
            _timeService = Ioc.Default.GetRequiredService<TimeService>();
        }
        [Fact]
        public void TimeOnly型から文字列型に変換するGetTimeStrメソッドのテスト()
        {
            var cases = new[]
            {
                new TestCase<TimeOnly, string>(new TimeOnly(0, 10), "00:10"),
                new TestCase<TimeOnly, string>(new TimeOnly(8, 20), "08:20"),
                new TestCase<TimeOnly, string>(new TimeOnly(15, 30), "15:30"),
            };
            foreach (var c in cases)
            {
                Assert.Equal(_timeService.GetTimeStr(c.Args), c.CompareValue);
            }
        }
        [Fact]
        public void Convertのテスト_stringToTimeOnly()
        {
            var cases = new[]
            {
                new TestCase<string, TimeOnly>("00:10", new TimeOnly(0, 10)),
                new TestCase<string, TimeOnly>("08:20", new TimeOnly(8, 20)),
                new TestCase<string, TimeOnly>("15:30", new TimeOnly(15, 30)),
            };
            foreach (var c in cases)
            {
                Assert.Equal(_dateTimeService.Convert(c.Args), c.CompareValue);
            }
        }
        [Fact]
        public void Convertのテスト_TimeOnlyToDateTime()
        {
            var cases = new[]
            {
                new TestCase<TimeOnly, DateTime>(new TimeOnly(0, 10, 0), new DateTime(2026, 03, 11, 0, 10, 0)),
                new TestCase<TimeOnly, DateTime>(new TimeOnly(0, 20, 1), new DateTime(2026, 03, 11, 0, 20, 1)),
            };
            foreach (var c in cases)
            {
                Assert.Equal(_dateTimeService.Convert(c.Args), c.CompareValue);
            }
        }
        [Fact]
        public void Convertのテスト_DateTimeToDateTimeOffset()
        {
            var cases = new[]
            {
                new TestCase<DateTime, DateTimeOffset>(
                    new DateTime(2026, 03, 11, 0, 10, 0), new DateTimeOffset(2026, 03, 11, 0, 10, 0, TimeSpan.FromHours(9))),
                new TestCase<DateTime, DateTimeOffset>(
                    new DateTime(2026, 03, 11, 0, 20, 1), new DateTimeOffset(2026, 03, 11, 0, 20, 1, TimeSpan.FromHours(9))),
            };
            foreach (var c in cases)
            {
                Assert.Equal(_dateTimeService.Convert(c.Args), c.CompareValue);
            }
        }
        [Fact]
        public void 指定した時間を加算するAddTimeOnlyメソッドのテスト()
        {
            var cases = new[]
            {
                new TestCase<AddTimeOnlyParams, TimeOnly>((new TimeOnly(0, 10), 1, 2, 0), new TimeOnly(1, 12)),
                new TestCase<AddTimeOnlyParams, TimeOnly>((new TimeOnly(1, 20), -1, -2, 0), new TimeOnly(0, 18)),
                new TestCase<AddTimeOnlyParams, TimeOnly>((new TimeOnly(2, 30), 5, -5, 0), new TimeOnly(7, 25)),
            };
            foreach (var c in cases) {
                Assert.Equal(_dateTimeService.AddTimeOnly(c.Args.Time, c.Args.Hours, c.Args.Minutes, c.Args.Seconds),
                    c.CompareValue);
            }
        }
        [Fact]
        public void GetRestTimeStrByhhmmのテスト_string()
        {
            var cases = new[]
            {
                new TestCase<GetRestTimeStrParams, string>((new TimeOnly(0, 10, 0), "00:20"), "00:10"),
                new TestCase<GetRestTimeStrParams, string>((new TimeOnly(1, 20, 1), "01:40"), "00 19"),
                new TestCase<GetRestTimeStrParams, string>((new TimeOnly(15, 30, 0), "16:30"), "01:00"),
            };
            foreach (var c in cases)
            {
                Assert.Equal(_dateTimeService.GetRestTimeStrByhhmm(c.Args.Time, c.Args.EndTimeStr), c.CompareValue);
            }
        }
        [Fact]
        public void GetRestTimeStrByhhmmのテスト_TimeOnly()
        {
            Assert.Equal("01:02", _dateTimeService.GetRestTimeStrByhhmm(new TimeOnly(0, 0, 0), new TimeOnly(1, 2, 0)));
        }
        [Fact]
        public void GetRestTimeStrBymmssのテスト_string()
        {
            var cases = new[]
            {
                new TestCase<GetRestTimeStrParams, string>((new TimeOnly(0, 10, 0), "00:20"), "10:00"),
                new TestCase<GetRestTimeStrParams, string>((new TimeOnly(0, 20, 1), "00:40"), "19 59"),
            };
            foreach (var c in cases)
            {
                Assert.Equal(_dateTimeService.GetRestTimeStrBymmss(c.Args.Time, c.Args.EndTimeStr), c.CompareValue);
            }
        }
        [Fact]
        public void GetRestTimeStrBymmssのテスト_TimeOnly()
        {
            Assert.Equal("01 02", _dateTimeService.GetRestTimeStrByhhmm(new TimeOnly(0, 0, 0), new TimeOnly(1, 2, 3)));
        }
        [Fact]
        public void GetRestTimeStrのテスト()
        {
            var cases = new[]
            {
                new TestCase<GetRestTimeStrParamsWithFormat, string>(
                    (new TimeOnly(0, 10, 0), new TimeOnly(0, 20), @"hh\:mm"), "00:10"),
                new TestCase<GetRestTimeStrParamsWithFormat, string>(
                    (new TimeOnly(0, 20, 1), new TimeOnly(0, 40), @"hh\:mm"), "00 19"),
                new TestCase<GetRestTimeStrParamsWithFormat, string>(
                    (new TimeOnly(0, 10, 0), new TimeOnly(0, 20), @"mm\:ss"), "10:00"),
                new TestCase<GetRestTimeStrParamsWithFormat, string>(
                    (new TimeOnly(0, 20, 1), new TimeOnly(0, 40), @"mm\:ss"), "19 59"),
            };
            foreach (var c in cases)
            {

                Assert.Equal(_dateTimeService.GetRestTimeStr(c.Args.Time, c.Args.EndTime, c.Args.Format), c.CompareValue);
            }
        }
        [Fact]
        public void GetNowStrByHHmmのテスト()
        {
            var cases = new[]
            {
                new TestCase<TimeOnly, string>(new TimeOnly(0, 10), "00:10"),
                new TestCase<TimeOnly, string>(new TimeOnly(0, 20, 1), "00 20"),
            };
            foreach (var c in cases)
            {
                Assert.Equal(_dateTimeService.GetNowStrByHHmm(c.Args), c.CompareValue);
            }
        }
        [Fact]
        public void ChangeTimeSeparateCharのテスト()
        {
            Assert.Equal("01:02:02", _dateTimeService.ChangeTimeSeparateChar(new TimeOnly(1, 2, 2), "01:02:02"));
            Assert.Equal("01 02 03", _dateTimeService.ChangeTimeSeparateChar(new TimeOnly(1, 2, 3), "01:02:03"));
            Assert.Equal("01:02", _dateTimeService.ChangeTimeSeparateChar(new TimeSpan(1, 2, 2), @"hh\:mm"));
            Assert.Equal("01 02", _dateTimeService.ChangeTimeSeparateChar(new TimeSpan(1, 2, 3), @"hh\:mm"));
            Assert.Equal("02:02", _dateTimeService.ChangeTimeSeparateChar(new TimeSpan(1, 2, 2), @"mm\:ss"));
            Assert.Equal("02 03", _dateTimeService.ChangeTimeSeparateChar(new TimeSpan(1, 2, 3), @"mm\:ss"));
        }
        [Fact]
        public void CreateTimeのテスト()
        {
            Assert.Equal(_dateTimeService.CreateTime(1, 2, 3), new TimeOnly(1, 2, 3, 0));
            Assert.Equal(_dateTimeService.CreateTime(4, 3, 2, 1), new TimeOnly(4, 3, 2, 1));
            Assert.Equal(_dateTimeService.CreateTime(new TimeOnly(10, 20, 30, 40)), new TimeOnly(10, 20, 30));
        }
    }
}
*/