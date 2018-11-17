using System;
using Microsoft.Owin.Hosting;

namespace SelfHost
{
   class Program
   {
      static void Main( string[] args )
      {
         WebApp.Start<Startup>( "http://localhost:5000/" );
         Console.ReadLine();
      }
   }
}
