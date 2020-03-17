using ChoETL;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace TipIt.Tests
{
    public class ScheduleTests : BaseTests
    {
        private readonly ITestOutputHelper _output;

        public ScheduleTests(
            ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void TipIt_ConvertsAflCsvToJson_Ok()
        {
            //  uses ChinChoo ETL https://www.nuget.org/packages/ChoETL.JSON/
            //  use this test to generate output for pasting into schedule.json
            //  transform data downloaded from fixturedownload.com
            var fileName = "afl-schedule-2020.csv";
            string path = Directory.GetCurrentDirectory();
            if (!File.Exists(fileName))
            {
                _output.WriteLine($"Could not find file {fileName} in {path}");
            }
            else
            {
                var reader = new ChoCSVReader(fileName).WithFirstLineHeader();
                foreach (var x in reader)
                {
                    x.HomeTeam = ConvertAflTeam(x.HomeTeam);
                    x.AwayTeam = ConvertAflTeam(x.AwayTeam);
                    x.GameDate = ConvertDate(x.GameDate);
                    x.EventType = "schedule";
                    x.League = "AFL";
                    _output.WriteLine(x.DumpAsJson());
                }
            }
        }

        [Fact]
        public void TipIt_ConvertsNrlCsvToJson_Ok()
        {
            //  uses ChinChoo ETL https://www.nuget.org/packages/ChoETL.JSON/
            //  use this test to generate output for pasting into schedule.json
            //  transform data downloaded from fixturedownload.com
            var fileName = "nrl-schedule-2020.csv";
            string path = Directory.GetCurrentDirectory();
            if (!File.Exists(fileName))
            {
                _output.WriteLine($"Could not find file {fileName} in {path}");
            }
            else
            {
                var reader = new ChoCSVReader(fileName).WithFirstLineHeader();
                foreach (var x in reader)
                {
                    x.HomeTeam = ConvertNrlTeam(x.HomeTeam);
                    x.AwayTeam = ConvertNrlTeam(x.AwayTeam);
                    x.GameDate = ConvertDate(x.GameDate);
                    x.EventType = "schedule";
                    x.League = "NRL";
                    _output.WriteLine(x.DumpAsJson());
                }
            }
        }

    }

    //[ChoCSVFileHeader]
    //[ChoCSVRecordObject(ObjectValidationMode = ChoObjectValidationMode.MemberLevel)]
    //public class ScheduleRec
    //{
    //    public int Round { get; set; }
    //    public string GameDate { get; set; }
    //    public string Location { get; set; }
    //    public string HomeTeam { get; set; }
    //    public string AwayTeam { get; set; }

    //    [ChoDefaultValue("schedule")]
    //    public string EventType { get; set; }
    //}
}