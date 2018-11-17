using System;
using System.Linq;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using StackExchange.Profiling;

namespace Employment.Web.Mvc.Infrastructure.Services.Profiled
{
    /// <summary>
    /// Defines a client message inspector for profiling the time taken in sending and receiving the message.
    /// </summary>
    public class ProfilerMessageInspector : IClientMessageInspector
    {
        /// <summary>
        /// Executes before sending a request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="channel">The channel.</param>
        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel)
        {
            var service = channel.GetType().Name;
            var method = !string.IsNullOrEmpty(request.Headers.Action) && request.Headers.Action.Contains('/') ? request.Headers.Action.Split('/').Last() : request.Headers.Action;

            // Start the profiler step just before sending the request
            return MiniProfiler.Current.Step(string.Format("WCF: {0}.{1}", service, method));
        }

        /// <summary>
        /// Executes after receiving a reply.
        /// </summary>
        /// <param name="reply">The reply.</param>
        /// <param name="correlationState">The correlation state.</param>
        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            var profiler = correlationState as IDisposable;

            if (profiler != null)
            {
                try
                {
                    // Dispose of the profiler after receiving the request
                    profiler.Dispose();
                }
                catch (NullReferenceException e)
                {
                    // Dispose will throw an exception if Profiler is null in the Dispose method of StackExchange.Profiling.Timing
                    Elmah.ErrorSignal.FromCurrentContext().Raise(e);
                }
            }
        }
    }
}
