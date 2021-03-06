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
            var fileName = "afl-2020-R14-18.csv";
            string path = Directory.GetCurrentDirectory();
            if (!File.Exists(fileName))
            {
                _output.WriteLine(
                    $"Could not find file {fileName} in {path}");
            }
            else
            {
                var sw = new StreamWriter(@"afl-schedule-2.json");
                var reader = new ChoCSVReader(fileName).WithFirstLineHeader();
                foreach (var x in reader)
                {
                    x.HomeTeam = ConvertAflTeam(x.HomeTeam);
                    x.AwayTeam = ConvertAflTeam(x.AwayTeam);
                    x.GameDate = ConvertDate(x.GameDate);
                    x.EventType = "schedule";
                    x.League = "AFL";
                    _output.WriteLine(x.DumpAsJson());
                    sw.WriteLine(x.DumpAsJson() + ",");
                }
                sw.Close();
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

        [Fact]
        public void TipIt_ConvertsNrlCsv2021ToJson_Ok()
        {
            //  uses ChinChoo ETL https://www.nuget.org/packages/ChoETL.JSON/
            //  use this test to generate output for pasting into schedule.json
            //  transform data downloaded from fixturedownload.com
            var fileName = "nrl-2021.csv";
            string path = Directory.GetCurrentDirectory();
            if (!File.Exists(fileName))
            {
                _output.WriteLine($"Could not find file {fileName} in {path}");
            }
            else
            {
                var sw = new StreamWriter(@"nrl-schedule-2021.json");

                var reader = new ChoCSVReader(fileName).WithFirstLineHeader();
                foreach (var x in reader)
                {
                    x.HomeTeam = ConvertNrlTeam(x.HomeTeam);
                    x.AwayTeam = ConvertNrlTeam(x.AwayTeam);
                    x.GameDate = ConvertDate(x.GameDate);
                    x.EventType = "schedule";
                    x.League = "NRL";
                    _output.WriteLine(x.DumpAsJson());
                    sw.WriteLine(x.DumpAsJson() + ",");
                }
                sw.Close();
            }
        }

        [Fact]
        public void TipIt_ConvertsNflCsvToJson_Ok()
        {
            var fileName = "nfl-2021.csv";
            string path = Directory.GetCurrentDirectory();
            if (!File.Exists(fileName))
            {
                _output.WriteLine($"Could not find file {fileName} in {path}");
            }
            else
            {
                var sw = new StreamWriter(@"nfl-schedule.json");
                var reader = new ChoCSVReader(fileName).WithFirstLineHeader();
                foreach (var x in reader)
                {
                    x.HomeTeam = ConvertNflTeam(x.HomeTeam);
                    x.AwayTeam = ConvertNflTeam(x.AwayTeam);
                    x.GameDate = ConvertDate(x.GameDate);
                    x.EventType = "schedule";
                    x.League = "NFL";
                    _output.WriteLine(x.DumpAsJson());
                    sw.WriteLine(x.DumpAsJson()+",");
                }
                sw.Close();
            }
        }
    }

	[ChoCSVFileHeader]
	[ChoCSVRecordObject(ObjectValidationMode = ChoObjectValidationMode.MemberLevel)]
	public class ScheduleRec
	{
		public int Round { get; set; }
		public string GameDate { get; set; }
		public string Location { get; set; }
		public string HomeTeam { get; set; }
		public string AwayTeam { get; set; }

		[ChoDefaultValue("schedule")]
		public string EventType { get; set; }
	}
}
