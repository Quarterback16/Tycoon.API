using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Argotic.Syndication;


namespace ArgoticRss
{
   class Program
   {
      static void Main(string[] args)
      {
         try
         {
            RssFeed feed = RssFeed.Create(new Uri("http://kat.ph/movies/?rss=1"));
            var nItems = feed.Channel.Items.Count();
            foreach (RssItem item in feed.Channel.Items)
            {
               Console.WriteLine("{0} {1}", item.Title, item.Description);
            }
            Console.ReadLine();

         }
         catch (Exception ex)
         {
            
            throw;
         }

      }
   }
}
