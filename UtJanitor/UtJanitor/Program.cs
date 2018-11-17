using NLog;
using System;
using UtJanitor.Logging;

namespace UtJanitor
{
	class Program
	{

		static void Main( string[] args )
		{
			//  3 args
			//  1) %F  file name
			//  2) %D  output Directory
			//  3) %M  status message
            var myLogger = new NLogger();
            var a = 0;

            foreach (var arg in args)
            {
                a++;
                var msg = string.Format("{0}:{1}", a, arg);
                myLogger.Info( msg );
                Console.WriteLine(msg);
                Console.ReadLine();
            }
      
		}
	}
}
