using Helpers.Interfaces;
using Helpers.Models;
using NLog;
using System.IO;

namespace Helpers
{
    public class TitleNormaliser : INormaliseTitles
    {
        public Logger Logger { get; set; }

        public TitleNormaliser()
        {
            Logger = LogManager.GetCurrentClassLogger();
        }

        public string NormaliseTitle(string title, string type, bool logIt = true)
        {
            //       Logger.Debug("Title IN:{0}", title);
            title = title.Replace("_", " ");
            title = title.Replace("\"", "'");
            title = title.Replace(".", " ");
            title = title.Replace(":", " ");
            title = title.Replace("|", " ");
            title = title.Replace("/", " ");
            title = title.Replace("\\", " ");
            title = title.Replace("*", " ");
            title = title.Replace("?", " ");
            title = title.Replace(">", " ");
            title = title.Replace("<", " ");  //  this may put a lot of spaces in the title
            var newTitle = string.Empty;
            for (int i = 0; i < title.Length; i++)
            {
                var aLetter = title.Substring(i, 1);
                if (i > 0)
                {
                    if (aLetter == " ")
                    {
                        if (title.Substring(i - 1, 1).Equals(" "))
                        {
                            continue;
                        }
                    }
                }
                newTitle += aLetter;
            }
            title = newTitle;
            if (type.Equals("TV"))
            {
                var tvtitle = TvTitle(title, logIt);
                if (!string.IsNullOrEmpty(tvtitle))
                    title = tvtitle;
            }
            else if (type.Equals("Movies"))
            {
                var movietitle = MovieTitle(title);
                if (!string.IsNullOrEmpty(movietitle))
                    title = movietitle;
            }
            return title.ToUpper();
        }

        private string TvTitle(string title, bool logIt = true)
        {
            if (logIt)
                Logger.Trace("  Extracting TV title from " + title);
            var mi = new MediaInfo(title);
            mi.Analyse(logIt);
            if (logIt)
                Logger.Trace("  TV title is " + mi.TvTitle);
            return mi.TvTitle;
        }

        private string MovieTitle(string title)
        {
            var theTitle = string.Empty;
            try
            {
                Logger.Trace("  Extracting Movie title from " + title);
                var mi = new MediaInfo(title);
                mi.Analyse();
                Logger.Trace("  Movie title is " + mi.Title);
                theTitle = mi.Title;
            }
            catch (PathTooLongException ex)
            {
                Logger.Error($"{ex.Message}:- Title has {title.Length} characters");
                theTitle = title.Substring(0, 100);
            }
            catch (System.Exception)
            {
                throw;
            }
            return theTitle;
        }
    }
}