using Helpers.Interfaces;
using Helpers.Models;
using NLog;
using System.IO;
using System.Xml;

namespace Helpers
{
    public class TorrentMaster : XmlCache
    {
        private readonly INormaliseTitles TitleNormaliser;

        #region Constructors

        public TorrentMaster(
			string name, 
			string dirName, 
			string fileName,
			INormaliseTitles titleNormailser)
           : base(name)
        {
            Logger = LogManager.GetCurrentClassLogger();
            TitleNormaliser = titleNormailser;
            Filename = dirName + fileName;
            try
            {
                //  load HT from the xml
                XmlDoc = new XmlDocument();
                XmlDoc.Load(Filename);
                var listNode = XmlDoc.ChildNodes[2];
                foreach (XmlNode node in listNode.ChildNodes)
                    AddXmlMedia(node);

                //  Create the XML navigation objects to allow xpath queries
                EpXmlDoc = new System.Xml.XPath.XPathDocument(Filename);
                // bad implementation - does not throw an exeception if XML is invalid
                Utility.Announce(string.Format("{0} loaded OK!", Filename));
                Nav = EpXmlDoc.CreateNavigator();

                //DumpHt();
                //DumpMedia();
                Utility.Announce($"Master constructed : {name}-{Filename} {TheHt.Count} items");
            }
            catch (IOException e)
            {
                Utility.Announce(
					$"Unable to open {Filename} xmlfile - {e.Message}");
            }
        }

        #endregion Constructors

        #region Reading

        public string NormaliseTitle(string title, string type)
        {
            return TitleNormaliser.NormaliseTitle(title, type);
        }

        public TorrentItem GetItem(TorrentItem item)
        {
            TorrentItem m;
            var theTitle = TitleNormaliser.NormaliseTitle(item.Title, item.Type);
            if (TheHt.ContainsKey(theTitle))
            {
                m = (TorrentItem)TheHt[item.Title.ToUpper()];
                CacheHits++;
            }
            else
            {
                //  new it up
                m = new TorrentItem
                {
                    Filename = item.Filename,
                    Title = TitleNormaliser.NormaliseTitle(item.Title, item.Type),
                    LibraryDate = item.LibraryDate,
                    Type = item.Type
                };
                PutItem(m);
                CacheMisses++;
            }
            return m;
        }

        public bool HaveItem(string title, string type)
        {
            var ntitle = TitleNormaliser.NormaliseTitle(title, type);
            var haveIt = TheHt.ContainsKey(ntitle);
            if (!haveIt)
                Logger.Trace($"   ...Raw title is >{title}<");
            return haveIt;
        }

        #endregion Reading

        #region Writing

        public void PutItem(TorrentItem m)
        {
            var title = m.Title;
            if (!TheHt.ContainsKey(title))
            {
                TheHt.Add(title, m);
                IsDirty = true;
                Logger.Trace($"    {title} added to HT");
            }
            else
                Logger.Trace($"    {title} already in HT");
        }

        private void AddXmlMedia(XmlNode node)
        {
            AddTorrentItem(new TorrentItem(node));
        }

        public void AddTorrentItem(TorrentItem m)
        {
            PutItem(m);
        }

        #endregion Writing

        #region Persistence

        /// <summary>
        ///   Converts the memory hash table to XML
        /// </summary>
        public void Dump2Xml()
        {
            if ((TheHt.Count > 0) && IsDirty)
            {
                var writer = new XmlTextWriter(string.Format("{0}", Filename), null);

                writer.WriteStartDocument();
                writer.WriteComment("Comments: " + Name);
                writer.WriteStartElement("torrent-list");

                var myEnumerator = TheHt.GetEnumerator();
                while (myEnumerator.MoveNext())
                {
                    var m = (TorrentItem)myEnumerator.Value;
                    WriteMediaNode(writer, m);
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
                Utility.Announce(string.Format("{0} created.", Filename));
            }
            else
                Utility.Announce(string.Format("No changes to {0}.", Filename));
        }

        private void WriteMediaNode(XmlTextWriter writer, TorrentItem m)
        {
            writer.WriteStartElement("media-item");
            WriteElement(writer, "filename", m.Filename);
            WriteElement(writer, "type", m.Type);
            WriteElement(
                writer,
                "title", TitleNormaliser.NormaliseTitle(
                   m.Title,
                   m.Type,
                   false));
            WriteElement(writer, "libdate", m.LibraryDate.ToShortDateString());
            writer.WriteEndElement();
        }

        #endregion Persistence

        #region Logging

        public void DumpMedia()
        {
            var myEnumerator = TheHt.GetEnumerator();
            while (myEnumerator.MoveNext())
            {
                var s = (TorrentItem)myEnumerator.Value;
                Utility.Announce($"Season {myEnumerator.Key}:- ");
            }
        }

        #endregion Logging
    }
}