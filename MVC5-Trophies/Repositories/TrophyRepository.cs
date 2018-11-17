using MVC5_Trophies.Interfaces;
using System;
using System.Xml.Linq;

namespace MVC5_Trophies.Repositories
{
   public class TrophyRepository
   {
      public const string K_XmlFile = "trophy.xml";
      public const string K_XmlPath = "xml-path";

      public XDocument XmlDoc { get; set; }

      public string XmlFileName { get; set; }

      public TrophyRepository( IConfigurationManager cm )
      {
         XmlFileName = string.Format( "{0}{1}", cm.AppSettings[K_XmlPath], K_XmlFile );
      }

      public void Load()
      {
         if ( XmlFileName != null )
            XmlDoc = XDocument.Load(XmlFileName);
      }
   }
}