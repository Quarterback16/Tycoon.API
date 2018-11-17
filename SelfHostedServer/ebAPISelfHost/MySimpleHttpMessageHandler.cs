using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ebAPISelfHost
{
   public class MySimpleHttpMessageHandler : HttpMessageHandler
   {
      protected override Task<HttpResponseMessage>
		 SendAsync(
            HttpRequestMessage request, 
            System.Threading.CancellationToken cancellationToken)
      {
         Console.WriteLine("Received a http message");
         var task = new Task<HttpResponseMessage>(() =>
         {
			 var msg = new HttpResponseMessage()
			 {
				 Content = new StringContent("Hello SelfHosting")
			 };
			 Console.WriteLine("Sent a http message");
            return msg;
         });
         task.Start();
         return task;
      }
   }
}
