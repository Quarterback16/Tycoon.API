using System.Configuration;
using TFLLib;

namespace TflWebApi.Repositories
{

   public class TflRepository
   {
      private static readonly DataLibrarian instance = 
         new DataLibrarian(
            NflConnectionString(),
            TflConnectionString(), 
            CtlConnectionString()
            );

      public TflRepository()
      {
         
      }

      public DataLibrarian DataLibrarian
      {
         get
         {
            return instance;
         }
      }

      public static string NflConnectionString()
      {
         return ConnectionString( "NflConnectionString" );
      }

      public static string TflConnectionString()
      {
         return ConnectionString( "TflConnectionString" );
      }

      public static string CtlConnectionString()
      {
         return ConnectionString( "CtlConnectionString" );
      }

      private static string ConnectionString( string keyName )
      {
         var connections = ConfigurationManager.ConnectionStrings;
         var connStr = connections[ keyName ].ConnectionString;
         return connStr;
      }
   }
}