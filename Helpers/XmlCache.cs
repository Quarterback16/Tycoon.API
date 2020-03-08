using NLog;
using System.Collections;
using System.Xml;
using System.Xml.XPath;

namespace Helpers
{
    public class XmlCache : ICache
    {
        public Logger Logger { get; set; }

        protected XPathDocument EpXmlDoc;
        protected XPathNavigator Nav;

        public XmlCache(string entityName)
        {
            Logger = LogManager.GetCurrentClassLogger();

            CacheMisses = 0;
            CacheHits = 0;
            IsDirty = false;

            Logger.Trace($"XmlCache.Init Constructing {entityName} master");

            Name = entityName;
            TheHt = new Hashtable();
        }

        public int CacheHits { get; set; }

        public int CacheMisses { get; set; }

        public bool IsDirty { get; set; }

        public XmlDocument XmlDoc { get; set; }

        public Hashtable TheHt { get; set; }

        public string Filename { get; set; }

        public string Name { get; set; }

        public virtual decimal GetStat(string theKey)
        {
            return 0.0M;
        }

        public string StatsMessage()
        {
            return $"{Name} Cache hits {CacheHits} misses {CacheMisses}";
        }

        public static void WriteElement(
            XmlTextWriter writer, 
            string name, 
            string text)
        {
            writer.WriteStartElement(name);
            writer.WriteString(text);
            writer.WriteEndElement();
        }

        public void DumpHt()
        {
            var myEnumerator = TheHt.GetEnumerator();
            var i = 0;
            Logger.Trace("\t-INDEX-\t-KEY-\t-VALUE-");
            while (myEnumerator.MoveNext())
                Logger.Trace($"\t[{i++}]:\t{myEnumerator.Key}\t{myEnumerator.Value}");
        }
    }
}