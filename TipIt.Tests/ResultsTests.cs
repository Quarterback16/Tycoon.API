using System.IO;
using Xunit;
using Xunit.Abstractions;
using ChoETL;
using System;
using System.Runtime.Serialization;

namespace TipIt.Tests
{
    public class ResultsTests : BaseTests
    {
        private readonly ITestOutputHelper _output;

        public ResultsTests(
            ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void TipIt_ConvertsAflResultsCsvToJson_Ok()
        {
            //  uses ChinChoo ETL https://www.nuget.org/packages/ChoETL.JSON/
            //  use this test to generate output for pasting into schedule.json
            //  transform data downloaded from fixturedownload.com
            var fileName = "afl-2019-results.csv";
            string path = Directory.GetCurrentDirectory();
            if (!File.Exists(fileName))
            {
                _output.WriteLine($"Could not find file {fileName} in {path}");
            }
            else
            {
                var reader = new ChoCSVReader<ResultRec>(fileName)
                    .WithFirstLineHeader();
                foreach (var x in reader)
                {
                    x.HomeTeam = ConvertAflTeam(x.HomeTeam);
                    x.AwayTeam = ConvertAflTeam(x.AwayTeam);
                    x.GameDate = ConvertDate(x.GameDate);
                    var jo = new ResultJson(x, "AFL");
                    _output.WriteLine(jo.DumpAsJson());
                }
            }
        }

        [Fact]
        public void TipIt_ConvertsNrlResultsCsvToJson_Ok()
        {
            var fileName = "nrl-2021.csv";
            string path = Directory.GetCurrentDirectory();
            if (!File.Exists(fileName))
            {
                _output.WriteLine($"Could not find file {fileName} in {path}");
            }
            else
            {
                var reader = new ChoCSVReader<ResultRec>(fileName)
                    .WithFirstLineHeader();
                foreach (var x in reader)
                {
                    x.HomeTeam = ConvertNrlTeam(x.HomeTeam);
                    x.AwayTeam = ConvertNrlTeam(x.AwayTeam);
                    x.GameDate = ConvertDate(x.GameDate);
                    var jo = new ResultJson(x, "NRL");
                    _output.WriteLine(jo.DumpAsJson());
                }
            }
        }

        [Fact]
        public void ResultJson_KnowsScoresFromResult()
        {
            var resultRec = new ResultRec
            {
                Result = "67 - 105"
            };
            var cut = new ResultJson(resultRec, "AFL");
            Assert.True(cut.HomeScore == 67);
            Assert.True(cut.AwayScore == 105);
        }

        [ChoCSVFileHeader]
        [ChoCSVRecordObject(ObjectValidationMode = ChoObjectValidationMode.MemberLevel)]
        public class ResultRec
        {
            public int Round { get; set; }
            public string GameDate { get; set; }
            public string Location { get; set; }
            public string HomeTeam { get; set; }
            public string AwayTeam { get; set; }
            public string Result { get; set; }
        }

        [Serializable]
        [DataContract]
        public class ResultJson
        {
            public ResultJson(
                ResultRec x, 
                string league)
            {
                Round = x.Round;
                GameDate = x.GameDate;
                Location = x.Location;
                League = league;
                EventType = "result";
                HomeTeam = x.HomeTeam;
                AwayTeam = x.AwayTeam;
                HomeScore = HomeScoreFromResult(x.Result);
                AwayScore = AwayScoreFromResult(x.Result);
            }

            public int HomeScoreFromResult(string result)
            {
                var dashSpot = result.IndexOf('-');
                var awayScore = result.Substring(
                    startIndex: 0,
                    length: dashSpot -1);
                return Int32.Parse(awayScore);

            }
            public int AwayScoreFromResult(string result)
            {
                var dashSpot = result.IndexOf('-');
                var awayScore = result.Substring(
                    startIndex: dashSpot + 2,
                    length: result.Length - dashSpot - 2);
                return Int32.Parse(awayScore);
            }

            [DataMember]
            public int Round { get; set; }
            [DataMember]
            public string GameDate { get; set; }
            [DataMember]
            public string Location { get; set; }
            [DataMember]
            public string HomeTeam { get; set; }
            [DataMember]
            public string AwayTeam { get; set; }
            [DataMember]
            public int HomeScore { get; set; }
            [DataMember]
            public int AwayScore { get; set; }
            [DataMember]
            public string League { get; set; }
            [DataMember]
            public string EventType { get; set; }
        }
    }
}
