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

        [Fact]
        public void Creator_KnowsHowToConvertDateToHour()
        {
            var cut = new TflScheduleCreator();
            var dt =
              DateTime.ParseExact(
                  "2020-05-26 17:00", 
                  "yyyy-MM-dd HH:mm", 
                  CultureInfo.InvariantCulture);
            var result = cut.GameHour(
                dt);
            Assert.True(
                result == "4",
                $"Hour calculated = {result}");
        }
    }
}
