using System;
using System.Globalization;
using TipIt.Implementations;
using Xunit;
using Xunit.Abstractions;

namespace TipIt.Tests
{
    public class TflScheduleCreatorTests
    {
        private readonly ITestOutputHelper _output;
        public TflScheduleCreatorTests(
            ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData("2020-05-26 17:00", "5")]
        [InlineData("2020-05-26 13:00", "1")]
        [InlineData("2020-05-26 16:05", "4")]
        [InlineData("2020-05-26 16:25", "5")]
        [InlineData("2020-05-26 20:20", "8")]
        [InlineData("2020-05-26 22:15", "9")]
        [InlineData("2020-05-26 23:15", "9")]
        [InlineData("2020-05-26 11:05", "0")]
        public void Creator_KnowsHowToConvertDateToHour(
            string dateIn,
            string hourOut)
        {
            var cut = new TflScheduleCreator();
            var dt =
              DateTime.ParseExact(
                  dateIn, 
                  "yyyy-MM-dd HH:mm", 
                  CultureInfo.InvariantCulture);
            var result = cut.GameHour(
                dt);
            Assert.Equal(
                hourOut,
                result);
        }

        [Theory]
        [InlineData("2020-09-14 19:15", "2020")]
        [InlineData("2019-10-14 19:15", "2019")]
        [InlineData("2021-01-03 16:25", "2020")]
        [InlineData("2021-01-28 16:25", "2021")]
        public void Creator_KnowsHowToConvertDateToSeason(
            string dateIn,
            string seasonOut)
        {
            var cut = new TflScheduleCreator();
            var dt =
              DateTime.ParseExact(
                  dateIn,
                  "yyyy-MM-dd HH:mm",
                  CultureInfo.InvariantCulture);
            var result = cut.Season(
                dt);
            Assert.Equal(
                seasonOut,
                result);
        }

        [Theory]
        [InlineData(1, "A")]
        [InlineData(8, "H")]
        [InlineData(16, "P")]
        public void Creator_KnowsHowToConvertNumToGameNumber(
            int numberIn,
            string gameCodeOut)
        {
            var cut = new TflScheduleCreator();
            var result = cut.GameNumber(
                numberIn);
            Assert.Equal(
                gameCodeOut,
                result);
        }

        [Theory]
        [InlineData(1, "01")]
        [InlineData(8, "08")]
        [InlineData(16, "16")]
        public void Creator_KnowsHowToConvertWeekToTflWeek(
            int weekIn,
            string weekOut)
        {
            var cut = new TflScheduleCreator();
            var result = cut.TflWeek(
                weekIn);
            Assert.Equal(
                weekOut,
                result);
        }

        //  creator CANT insert 32 bit record from here (moved to Gerard)
    }
}
