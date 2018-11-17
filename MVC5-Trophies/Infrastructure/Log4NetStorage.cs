using log4net;
using log4net.Config;
using StackExchange.Profiling;
using StackExchange.Profiling.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC5_Trophies.Infrastructure
{
   /// <summary>
   ///     User Log4Net as storage.
   /// </summary>
   internal class Log4NetStorage : IStorage
   {
      /// <summary>
      ///     The logger by Log4Net.
      /// </summary>
      private static readonly ILog Log4NetLogger;

      /// <summary>
      ///     Initializes static members of the <see cref="Log4NetStorage" /> class.
      /// </summary>
      static Log4NetStorage()
      {
         XmlConfigurator.Configure();
         Log4NetLogger = LogManager.GetLogger(typeof(Log4NetStorage));
      }

      /// <summary>
      /// Returns a list of <see cref="P:StackExchange.Profiling.MiniProfiler.Id"/>s that haven't been seen by
      ///     <paramref name="user"/>.
      /// </summary>
      /// <param name="user">
      /// User identified by the current <c>MiniProfiler.Settings.UserProvider</c>
      /// </param>
      /// <returns>
      /// the list of key values.
      /// </returns>
      public List<Guid> GetUnviewedIds(string user)
      {
         throw new NotSupportedException("This method should never run");
      }


      /// <summary>
      /// list the result keys.
      /// </summary>
      /// <param name="maxResults">
      /// The max results.
      /// </param>
      /// <param name="start">
      /// The start.
      /// </param>
      /// <param name="finish">
      /// The finish.
      /// </param>
      /// <param name="orderBy">
      /// order by.
      /// </param>
      /// <returns>
      /// the list of keys in the result.
      /// </returns>
      public IEnumerable<Guid> List(int maxResults, DateTime? start = null, DateTime? finish = null,
            ListResultsOrder orderBy = ListResultsOrder.Descending)
      {
         throw new NotSupportedException("This method should never run");
      }


      /// <summary>
      /// Returns a <see cref="T:StackExchange.Profiling.MiniProfiler"/> from storage based on <paramref name="id"/>, which
      ///     should map to <see cref="P:StackExchange.Profiling.MiniProfiler.Id"/>.
      /// </summary>
      /// <param name="id">
      /// The id.
      /// </param>
      /// <remarks>
      /// Should also update that the resulting profiler has been marked as viewed by its profiling
      ///     <see cref="P:StackExchange.Profiling.MiniProfiler.User"/>.
      /// </remarks>
      /// <returns>
      /// The <see cref="T:StackExchange.Profiling.MiniProfiler"/>.
      /// </returns>
      public MiniProfiler Load(Guid id)
      {
         throw new NotSupportedException("This method should never run");
      }


      /// <summary>
      /// Stores <paramref name="profiler"/> under its <see cref="P:StackExchange.Profiling.MiniProfiler.Id"/>.
      /// </summary>
      /// <param name="profiler">
      /// The results of a profiling session.
      /// </param>
      /// <remarks>
      /// Should also ensure the profiler is stored as being un-viewed by its profiling
      ///     <see cref="P:StackExchange.Profiling.MiniProfiler.User"/>.
      /// </remarks>
      public void Save(MiniProfiler profiler)
      {
         if (profiler == null || Log4NetLogger == null) return;
         Log4NetLogger.Info(string.Format("User<{0}>: {1}", profiler.User, profiler ) );
      }


      /// <summary>
      /// Sets a particular profiler session so it is considered "un-viewed"
      /// </summary>
      /// <param name="user">
      /// The user.
      /// </param>
      /// <param name="id">
      /// The id.
      /// </param>
      public void SetUnviewed(string user, Guid id)
      {
         // do nothing
      }


      /// <summary>
      /// Sets a particular profiler session to "viewed"
      /// </summary>
      /// <param name="user">
      /// The user.
      /// </param>
      /// <param name="id">
      /// The id.
      /// </param>
      public void SetViewed(string user, Guid id)
      {
         throw new NotSupportedException("This method should never run");
      }
   }

}
