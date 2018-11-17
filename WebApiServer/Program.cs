using Microsoft.Owin.Hosting;
using System;

namespace WebApiServer
{
   internal class Program
   {
      private static void Main( string[] args )
      {
         WebApp.Start<Startup>( "http://localhost:5000/" );
         Console.ReadLine();
      }
   }
}