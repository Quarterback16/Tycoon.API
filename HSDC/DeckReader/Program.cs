using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace DeckReader
{
    class Program
    {
        static void Main(string[] args)
        {
            WebClient wc = new WebClient();
            wc.DownloadFile("http://www.hearthhead.com/deck=27761/murlock-semi-control-strike!", "deck.html");
        }
    }
}
