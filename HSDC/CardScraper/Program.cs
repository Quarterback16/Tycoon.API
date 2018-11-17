using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Scrape("en", "www");
            Scrape("de", "de");
            Scrape("fr", "fr");
            Scrape("es", "es");
            Scrape("pt", "pt");
            Scrape("ru", "ru");
            Console.ReadLine();
        }

        static void Scrape(string language, string wwwpath)
        {
            WebClient wc = new WebClient();
            string result = wc.DownloadString("http://" + wwwpath + ".hearthhead.com/cards#text");
            int start = result.IndexOf("var hearthstoneCards") + 23;
            int end = result.IndexOf("}];", start);
            string json = result.Substring(start, end - start + 2);

            dynamic data = JsonConvert.DeserializeObject(json);
            StreamWriter sw = new StreamWriter("cards_" + language + ".sql");
            language = "\"" + language + "\"";

            int count = 0;
            foreach (dynamic card in data)
            {
                string id = Check(card.id, "integer");
                string image = Check(card.image, "string");
                string set = Check(card.set, "integer");
                string quality = Check(card.quality, "integer");
                string icon = Check(card.icon, "string");
                string type = Check(card.type, "integer");
                string cost = Check(card.cost, "integer");
                string health = Check(card.health, "integer");
                string attack = Check(card.attack, "integer");
                string faction = Check(card.faction, "integer");
                string classs = Check(card.classs, "integer");
                string elite = Check(card.elite, "integer");
                string collectible = Check(card.collectible, "integer");
                string name = Check(card.name, "string");
                string description = Check(card.description, "string");

                if (image.Contains("FP1"))
                {
                    sw.WriteLine("INSERT INTO cards VALUES (" + language + "," + id + "," + image + "," + set + "," + quality + "," + icon + "," + type + "," + cost + "," + health + "," + attack + "," + faction + "," + classs + "," + elite + "," + collectible + "," + name + "," + description + ");");
                    count++;
                }
            }
            sw.Close();
            Console.WriteLine("Successfully scraped " + language + ", " + count + " cards imported.");
        }

        static string Check(dynamic data, string type)
        {
            if (type == "string")
            {
                if (data == null)
                    return "\"\"";
                else
                {
                    string d = data.Value;
                    d = d.Replace("'", "\'");
                    d = d.Replace("\"", "\\\"");
                    return "\"" + d + "\"";
                }
            }
            else
            {
                if (data == null)
                    return "0";
                else
                    return data;
            }
        }
    }
}
