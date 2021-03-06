using CommandLine;
using System;
using TipIt.Implementations;
using TipIt.TippingStrategies;

namespace TipIt
{
    class Program
    {
        /// <summary>
        ///    -l AFL -r 2
        ///    -l AFL -o pl  //  previous ladder
        ///    -l AFL -o ps  //  stats
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var context = new TippingContext();
			//TODO: some validation could go here
			Console.WriteLine($"There are {context.LeagueCount()} leagues");
			Console.WriteLine(
				$"League NRL has {context.ScheduledRoundCount("NRL")} rounds scheduled");
			Console.WriteLine(
				$"League AFL has {context.ScheduledRoundCount("AFL")} rounds scheduled");
			context.DumpResults("NRL");
			var options = new Options();
            var result = Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o => options.League = o.League)
                .WithParsed(o => options.Output = o.Output)
                .WithParsed(o => options.Round = o.Round);

            if (!string.IsNullOrEmpty(options.Output) 
                && options.Output.Equals("ps"))
            {
                PastLeagueStats(options.League, context);
                return;
            }
            var round = 1;
//          var tipster = new TableTipster(context);
            var tipster = new NibbleTipster(context);
            if (string.IsNullOrEmpty(options.League))
            {
                Console.WriteLine("=== NRL ==================================================");
                tipster.ShowTips(
                    "NRL", 
                    context.NextRound("NRL"));
                Console.WriteLine("=== AFL ==================================================");
                tipster.ShowTips(
                    "AFL",
                    context.NextRound("AFL"));
            }
            else
            {
                if (options.Round == 0)
                    round = context.NextRound(options.League);
                else
                    round = options.Round;
                Console.WriteLine($"Command is TIP {options.League} Round {round}");
                tipster.ShowTips(
                    options.League, 
                    round);
            }
        }

        private static void PastLeagueStats(
            string league, 
            TippingContext context)
        {
            if (string.IsNullOrEmpty(league))
            {
                Console.WriteLine(context.LastYearsStats("AFL"));
                Console.WriteLine(context.LastYearsStats("NRL"));
                return;
            }
            Console.WriteLine(context.LastYearsStats(league));
        }
    }
}
