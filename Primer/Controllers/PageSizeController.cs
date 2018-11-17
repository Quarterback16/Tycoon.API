using Primer.Infrastructure;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Primer.Controllers
{
   public class PageSizeController : ApiController, ICustomController
   {
      //private static string TargetUrl = "http://apress.com";  //  blocked by firewall
      private static string TargetUrl = "http://wde308498/openwiki";

      public async Task<long> GetPageSize( CancellationToken cToken )
      {
         WebClient wc = new WebClient();
         Stopwatch sw = Stopwatch.StartNew();
         byte[] apressData = await wc.DownloadDataTaskAsync( TargetUrl );
         Debug.WriteLine( "Elapsed ms: {0}", sw.ElapsedMilliseconds );
         return apressData.LongLength;
      }

      public Task PostUrl( string newUrl, CancellationToken cToken )
      {
         TargetUrl = newUrl;
         return Task.FromResult<object>( null );
      }

   }
}