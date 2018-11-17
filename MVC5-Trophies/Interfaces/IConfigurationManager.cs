using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC5_Trophies.Interfaces
{
   /// <summary>
   /// Defines the methods and properties that are required for a Configuration Manager.
   /// </summary>
   public interface IConfigurationManager
   {
      /// <summary>
      /// Name value collection of Application settings.
      /// </summary>
      NameValueCollection AppSettings { get; }

      /// <summary>
      /// Connection strings.
      /// </summary>
      /// <param name="name">Unique name of the connection string.</param>
      /// <returns>The connection string.</returns>
      string ConnectionStrings(string name);

      /// <summary>
      /// Get a section from the Web Config.
      /// </summary>
      /// <typeparam name="T">Type of section.</typeparam>
      /// <param name="sectionName">Name of section.</param>
      /// <returns>The section as type.</returns>
      T GetSection<T>(string sectionName);
   }
}
