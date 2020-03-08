using CommandLine;
using System.Collections.Generic;


namespace TipIt
{
    public class Options
    {
        [Option('l')]
        public string League { get; set; }

        [Option('r')]
        public int Round { get; set; }

        [Option('o')]
        public string Output { get; set; }
    }
}
