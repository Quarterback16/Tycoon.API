using DataDynamics.ActiveReports;
using ProgramAssuranceTool.Interfaces;
using ProgramAssuranceTool.Models;
using ProgramAssuranceTool.ViewModels.Review;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using Filter = MvcJqGrid.Filter;
using GridSettings = MvcJqGrid.GridSettings;
using Rule = MvcJqGrid.Rule;

namespace ProgramAssuranceTool.Helpers
{
	public static class AppHelper
	{
		private static bool? _isLocalWorkstation;
		public static bool IsLocalWorkstation
		{
			get
			{
				if (_isLocalWorkstation != null)
				{
					return _isLocalWorkstation.Value;
				}
				if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["Environment"]))
				{
					_isLocalWorkstation = ConfigurationManager.AppSettings["Environment"].Equals("local",
																														  StringComparison.OrdinalIgnoreCase);
					return _isLocalWorkstation.Value;
				}
				return false;
			}
		}

		/// <summary>
		///   The location of the Content folder
		/// </summary>
		public static string ContentRoot
		{
			get
			{
				var rootUrl = ConfigurationManager.AppSettings["RootUrl"];
				const string contentVirtualRoot = "/Content";
				return String.Format("{0}{1}", rootUrl, contentVirtualRoot);
			}
		}

		public static string ScriptRoot
		{
			get
			{
				var rootUrl = ConfigurationManager.AppSettings["RootUrl"];
				const string contentVirtualRoot = "/Scripts";
				return String.Format("{0}{1}", rootUrl, contentVirtualRoot);
			}
		}

		public static string ImageRoot
		{
			get { return String.Format("{0}/{1}", ContentRoot, "Images"); }
		}

		public static string CssRoot
		{
			get { return String.Format("{0}/{1}", ContentRoot, "CSS"); }
		}

		private static int? _batchJobFrequencyInSeconds;
		public static int BatchJobFrequencyInSeconds
		{
			get
			{
				if (_batchJobFrequencyInSeconds != null)
				{
					return _batchJobFrequencyInSeconds.Value;
				}

				_batchJobFrequencyInSeconds = 36000; // default to 10 hours

				// try to get its value from config file
				if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["BatchJobFrequencyInSeconds"]))
				{
					var stringValue = ConfigurationManager.AppSettings["BatchJobFrequencyInSeconds"];

					int intValue;
					int.TryParse(stringValue, out intValue);

					// ensure that the minimum frequency is within a reasonable time to complete the batch job e.g. 1/2 hour
					if (intValue >= 1800)
					{
						_batchJobFrequencyInSeconds = intValue;
					}

					return _batchJobFrequencyInSeconds.Value;
				}

				return _batchJobFrequencyInSeconds.Value;
			}
		}

		public static string ImageUrl(string imageFile)
		{
			return String.Format("{0}/{1}", ImageRoot, imageFile);
		}

		public static string Image(string imageFile)
		{
			var imageUrl = ImageUrl(imageFile);
			//<img src="css/blueprint/plugins/buttons/icons/tick.png" alt=""/>
			return String.Format("<img src=\"{0}\" alt=\"\" />", imageUrl);
		}

		public static string Image(string imageFile, string altText)
		{
			var imageUrl = ImageUrl(imageFile);
			//<img src="css/blueprint/plugins/buttons/icons/tick.png" alt=""/>
			return String.Format("<img src=\"{0}\" alt=\"{1}\" />", imageUrl, altText);
		}

		public static string Css(string cssFile)
		{
			return String.Format("<link href=\"{0}/{1}\" rel=\"stylesheet\" type=\"text/css\" />", CssRoot, cssFile);
		}

		public static string CssUrl(string cssFile)
		{
			return String.Format("{0}/{1}", CssRoot, cssFile);
		}

		public static string JScriptUrl(string jsFile)
		{
#if _DEBUG
			return string.Format("{0}/{1}?{2}", ScriptRoot, jsFile, DateTime.Now.Ticks);
#else
			return String.Format("{0}/{1}?{2}", ScriptRoot, jsFile, 1);
#endif
		}

		#region  Activity List

		public static void SetSessionQueryUserId(HttpSessionStateBase session, string userId)
		{
			if (String.IsNullOrEmpty(userId)) userId = String.Empty;
			session[CommonConstants.SessionQueryUserId] = userId;
		}

		public static string GetSessionQueryUserId(HttpSessionStateBase session)
		{
			return session[CommonConstants.SessionQueryUserId] == null
						 ? String.Empty
						 : session[CommonConstants.SessionQueryUserId].ToString();
		}

		public static string GetSessionQueryFromDate(HttpSessionStateBase session)
		{
			return session[CommonConstants.SessionQueryFromDate] == null
						 ? String.Empty
						 : session[CommonConstants.SessionQueryFromDate].ToString();
		}

		public static void SetSessionQueryFromDate(HttpSessionStateBase session, string fromDate)
		{
			if (String.IsNullOrEmpty(fromDate)) fromDate = String.Empty;
			session[CommonConstants.SessionQueryFromDate] = fromDate;
		}

		public static void SetSessionQueryToDate(HttpSessionStateBase session, string toDate)
		{
			if (String.IsNullOrEmpty(toDate)) toDate = String.Empty;
			session[CommonConstants.SessionQueryToDate] = toDate;
		}

		public static string GetSessionQueryToDate(HttpSessionStateBase session)
		{
			return session[CommonConstants.SessionQueryToDate] == null
						 ? String.Empty
						 : session[CommonConstants.SessionQueryToDate].ToString();
		}

		public static string LongDateTime(DateTime? theDate)
		{
			if ((theDate != null) && (theDate != new DateTime(1, 1, 1)))
			{
				var aDate = (DateTime)theDate;
				return String.Format("{0: ddd dd MMM yyyy @ hh:mm tt}", aDate);
			}
			return String.Empty;
		}

		#endregion

		/// <summary>
		/// To return how many pages in total based on the total number of records and  page size
		/// </summary>
		/// <param name="totRecs">Total number of records</param>
		/// <param name="pageSize">Size of the page/ records per page</param>
		/// <returns></returns>
		public static int PagesInTotal(int totRecs, int pageSize)
		{
			if (totRecs.Equals(0)) return 0;
			var numPages = (totRecs / pageSize) + 1;
			var remainder = totRecs % pageSize;
			if (remainder == 0) numPages--;
			return numPages;
		}

		public static GridSettings DefaultGridSettings()
		{
			var gs = new GridSettings { IsSearch = false, PageIndex = 1, PageSize = 9999999, SortOrder = "asc" };
			return gs;
		}

		public static string SessionIdentifier(HttpRequestBase request)
		{
			var httpCookie = request.Cookies[FormsAuthentication.FormsCookieName];
			if (httpCookie != null)
			{
				var sessionIdentifier = String.Join("", MD5.Create().ComputeHash(
					Encoding.ASCII.GetBytes(
						httpCookie.Value)).Select(s => s.ToString("x2")));
				return sessionIdentifier;
			}
			return string.Empty;
		}

		public static string ShortDate(DateTime? theDate)
		{
			if (theDate != null)
			{
				var aDate = (DateTime)theDate;
				return aDate.ToShortDateString() + "   ";
			}
			return String.Empty;
		}

		public static string ShortDateAndTime(DateTime? theDate)
		{
			return theDate != null ? string.Format("{0:f}", theDate) : String.Empty;
		}

		public static string VersionNumber()
		{
			var vn = string.Format("3.02.01-{0}", GetCV()); //  Major Version, Minor Version, Build Number
			return vn;
		}

		public static string GetCV()
		{
			const string ds = "????";
			var str = GetConnectionString();
			return GetCVcode(str, ds);
		}

		public static string GetCVcode(string str, string ds)
		{
			var nds = str.IndexOf("=", StringComparison.Ordinal);
			if (nds > -1)
			{
				var dds = str.IndexOf(".", StringComparison.Ordinal);
				if (dds > -1) ds = str.Substring(nds + 1, dds - nds - 1);
			}
			var size = 4;
			if (ds.Length < size)
				size = ds.Length;
			return ds.Substring(ds.Length - size, size);
		}

		internal static string GetConnectionString()
		{
			var connectionString = ConfigurationManager.ConnectionStrings[CommonConstants.ConnectionString];

			if (connectionString == null)
				return "There is no " + CommonConstants.ConnectionString + " connection string";

			return string.IsNullOrEmpty(connectionString.ToString())
						 ? "Connection string has not been set" : connectionString.ConnectionString;
		}

		#region Session stuff

		public static void SetSessionProjectId(HttpSessionStateBase session, int projectId)
		{
			session[CommonConstants.SessionProjectId] = projectId;
		}

		public static void SetSessionProjectName(HttpSessionStateBase session, string projectName)
		{
			session[CommonConstants.SessionProjectName] = projectName;
		}

		public static void SetSessionUploadId(HttpSessionStateBase session, int uploadId)
		{
			session[CommonConstants.SessionUploadId] = uploadId;
		}

		public static void SetSessionUploadName(HttpSessionStateBase session, string uploadName)
		{
			session[CommonConstants.SessionUploadName] = uploadName;
		}

		public static int GetSessionProjectId(HttpSessionStateBase session)
		{
			return session[CommonConstants.SessionProjectId] == null
						 ? 0
						 : Int32.Parse(session[CommonConstants.SessionProjectId].ToString());
		}

		public static string GetSessionProjectName(HttpSessionStateBase session)
		{
			return session[CommonConstants.SessionProjectName] == null
						 ? string.Empty
						 : session[CommonConstants.SessionProjectName].ToString();
		}

		public static int GetSessionUploadId(HttpSessionStateBase session)
		{
			return session[CommonConstants.SessionUploadId] == null
						 ? 0
						 : Int32.Parse(session[CommonConstants.SessionUploadId].ToString());
		}

		public static string GetSessionUploadName(HttpSessionStateBase session)
		{
			return session[CommonConstants.SessionUploadName] == null
						 ? string.Empty
						 : session[CommonConstants.SessionUploadName].ToString();
		}

		public static void SetSessionReviewDetails(HttpSessionStateBase session, string html)
		{
			if (String.IsNullOrEmpty(html)) html = String.Empty;
			session[CommonConstants.SessionReviewDetails] = html;
		}

		public static string GetSessionReviewDetails(HttpSessionStateBase session)
		{
			return session[CommonConstants.SessionReviewDetails] == null
						 ? string.Empty
						 : session[CommonConstants.SessionReviewDetails].ToString();
		}

		public static void SetSessionRelatedData(HttpSessionStateBase session, string html)
		{
			if (String.IsNullOrEmpty(html)) html = String.Empty;
			session[CommonConstants.SessionRelatedData] = html;
		}

		public static string GetSessionRelatedData(HttpSessionStateBase session)
		{
			return session[CommonConstants.SessionRelatedData] == null
						 ? string.Empty
						 : session[CommonConstants.SessionRelatedData].ToString();
		}

		public static void SetSessionSampleData(HttpSessionStateBase session, string html)
		{
			if (String.IsNullOrEmpty(html)) html = String.Empty;
			session[CommonConstants.SessionSampleData] = html;
		}

		public static string GetSessionSampleData(HttpSessionStateBase session)
		{
			return session[CommonConstants.SessionSampleData] == null
						 ? string.Empty
						 : session[CommonConstants.SessionSampleData].ToString();
		}

		public static void SetSessionAdminStatus(HttpSessionStateBase session, string adminStatus)
		{
			session[CommonConstants.SessionAdminStatus] = adminStatus;
		}

		public static string GetSessionAdminStatus(HttpSessionStateBase session)
		{
			return session[CommonConstants.SessionAdminStatus] == null
						 ? string.Empty
						 : session[CommonConstants.SessionAdminStatus].ToString();
		}

		public static void SetSessionUploadedFrom(HttpSessionStateBase session, string uploadedFrom)
		{
			session[CommonConstants.SessionUploadedFrom] = uploadedFrom;
		}

		public static string GetSessionUploadedFrom(HttpSessionStateBase session)
		{
			return session[CommonConstants.SessionUploadedFrom] == null
						 ? string.Empty
						 : session[CommonConstants.SessionUploadedFrom].ToString();
		}

		public static void SetSessionUploadedTo(HttpSessionStateBase session, string uploadedTo)
		{
			session[CommonConstants.SessionUploadedTo] = uploadedTo;
		}

		public static string GetSessionUploadedTo(HttpSessionStateBase session)
		{
			return session[CommonConstants.SessionUploadedTo] == null
						 ? string.Empty
						 : session[CommonConstants.SessionUploadedTo].ToString();
		}

		public static void SetSessionProjectGrid(HttpSessionStateBase session, string html)
		{
			session[CommonConstants.SessionProjectGrid] = html;
		}

		public static string GetSessionProjectGrid(HttpSessionStateBase session)
		{
			return session[CommonConstants.SessionProjectGrid] == null
						 ? string.Empty
						 : session[CommonConstants.SessionProjectGrid].ToString();
		}

		/// <summary>
		/// Clears the session check lists.
		/// </summary>
		/// <param name="session">The session.</param>
		public static void ClearSessionCheckLists(HttpSessionStateBase session)
		{
			session[CommonConstants.SessionCheckListsOriginal] = null;
			session[CommonConstants.SessionCheckListsUserModified] = null;
			session[CommonConstants.SessionCheckListUserModified] = null;
		}

		/// <summary>
		/// Sets the session check lists.
		/// </summary>
		/// <param name="session">The session.</param>
		/// <param name="setOriginalData">if set to <c>true</c> [set original data], otherwise set user modified data</param>
		/// <param name="checkList">The check list.</param>
		public static void SetSessionCheckLists(HttpSessionStateBase session, bool setOriginalData, CheckList checkList)
		{
			var sessionKey = setOriginalData ? CommonConstants.SessionCheckListsOriginal : CommonConstants.SessionCheckListsUserModified;

			var checkLists = session[sessionKey] as List<CheckList> ?? new List<CheckList>();

			// remove existing data to avoid duplication
			if (checkLists.Any())
			{
				var index = checkLists.FindIndex(0, c => c.ReviewID == checkList.ReviewID);
				if (index > -1)
				{
					checkLists.RemoveAt(index);
				}
			}

			if (setOriginalData)
			{
				// add it back always
				checkLists.Add(checkList);
			}
			else
			{
				// *** only add into the session if user-modified data is difference with original data ***

				var originalCheckList = session[CommonConstants.SessionCheckListsOriginal] as List<CheckList> ?? new List<CheckList>();
				var original = originalCheckList.FirstOrDefault(o => o.ReviewID == checkList.ReviewID) ?? new CheckList();

				var diff = 
					(original.IsClaimDuplicateOverlapping ?? string.Empty) != (checkList.IsClaimDuplicateOverlapping ?? string.Empty) ||
					(original.IsClaimIncludedInDeedNonPayableOutcomeList ?? string.Empty) != (checkList.IsClaimIncludedInDeedNonPayableOutcomeList ?? string.Empty) ||
					(original.DoesDocEvidenceMeetGuidelineRequirement ?? string.Empty) != (checkList.DoesDocEvidenceMeetGuidelineRequirement ?? string.Empty) ||
					(original.IsDocEvidenceConsistentWithESS ?? string.Empty) != (checkList.IsDocEvidenceConsistentWithESS ?? string.Empty) ||
					(original.IsDocEvidenceSufficientToSupportPaymentType ?? string.Empty) != (checkList.IsDocEvidenceSufficientToSupportPaymentType ?? string.Empty) ||
					(original.Comment ?? string.Empty) != (checkList.Comment ?? string.Empty)
					;

				if (diff)
				{
					// add it back if different
					checkLists.Add(checkList);
				}
			}

			session[sessionKey] = checkLists;
		}

		/// <summary>
		/// Gets the session check lists.
		/// </summary>
		/// <param name="session">The session.</param>
		/// <param name="getOriginalData">if set to <c>true</c> [get original data], otherwise get user modified data</param>
		/// <returns></returns>
		public static List<CheckList> GetSessionCheckLists(HttpSessionStateBase session, bool getOriginalData)
		{
			var sessionKey = getOriginalData ? CommonConstants.SessionCheckListsOriginal : CommonConstants.SessionCheckListsUserModified;

			var checkLists = session[sessionKey] as List<CheckList> ?? new List<CheckList>();
			return checkLists;
		}

		public static void SetSessionCheckList(HttpSessionStateBase session, CheckList checkList)
		{
			session[CommonConstants.SessionCheckListUserModified] = checkList;
		}

		public static CheckList GetSessionCheckList(HttpSessionStateBase session)
		{
			return session[CommonConstants.SessionCheckListUserModified] as CheckList;
		}

		#endregion

		public static string RemoveDomain(this string userName)
		{
			if (String.IsNullOrEmpty(userName)) return userName;
			var val = userName.Split('\\');
			if (val.Length == 2) return val[1];
			val = userName.Split('@');
			return val.Length == 2 ? val[1] : userName;
		}

		public static string GetCode(this string searchTerm)
		{
			var code = string.Empty;
			if (!string.IsNullOrWhiteSpace(searchTerm))
			{
				code = searchTerm.Split(new[] { ',', ' ', '-' }, StringSplitOptions.RemoveEmptyEntries)[0];
			}
			return code;
		}

		public static void Announce(string rpt, int indent = 3)
		{
			var theLength = rpt.Length + indent;
			rpt = rpt.PadLeft(theLength, ' ');
			Console.WriteLine(rpt); //  not viewable when in Unit Test mode
			//WriteLog( rpt );
#if DEBUG
			Debug.WriteLine(rpt);
#endif
		}

		public static decimal Average(decimal score, int total)
		{
			return total == 0 ? 0.0M : (score / total);
		}

		public static decimal Percent(decimal quotient, decimal divisor)
		{
			if (divisor == 0.0M) return divisor;
			return (quotient / divisor);
		}

		/// <summary>
		///   Diagnostic method for timing processes - Stops the watch.
		/// </summary>
		/// <param name="stopwatch">The stopwatch.</param>
		/// <param name="logger">The logger.</param>
		/// <param name="message">The message saying how long the process being timed took.</param>
		public static void StopTheWatch(Stopwatch stopwatch, ILog logger, string message)
		{
			stopwatch.Stop();
			var ts = stopwatch.Elapsed;
			var elapsedTime = FormatElapsedTime(ts);
			logger.Info(string.Format("{0} took {1}", message, elapsedTime));
		}

		/// <summary>
		///   Diagnostic method for timing processes - Stops the watch.
		/// </summary>
		/// <param name="stopwatch">The stopwatch.</param>
		/// <returns>the elapsed time</returns>
		public static string StopTheWatch(Stopwatch stopwatch)
		{
			stopwatch.Stop();
			var ts = stopwatch.Elapsed;
			var elapsedTime = FormatElapsedTime(ts);
			return elapsedTime;
		}

		/// <summary>
		///   Formats the elapsed time for better readibility.
		/// </summary>
		/// <param name="ts">The time span.</param>
		/// <returns></returns>
		public static string FormatElapsedTime(TimeSpan ts)
		{
			var elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
													  ts.Hours, ts.Minutes, ts.Seconds,
													  ts.Milliseconds);
			return elapsedTime;
		}

		public static string QuarterFor(DateTime theDate)
		{
			var theYear = theDate.Year;
			var theMonth = theDate.Month;

			return String.Format("{0}{1:#}", theYear, (int)GetQuarter((Month)theMonth));
		}

		public static Quarter GetQuarter(Month month)
		{
			if (month <= Month.March)
				// 1st Quarter = January 1 to March 31
				return Quarter.First;
			if ((month >= Month.April) && (month <= Month.June))
				// 2nd Quarter = April 1 to June 30
				return Quarter.Second;
			if ((month >= Month.July) && (month <= Month.September))
				// 3rd Quarter = July 1 to September 30
				return Quarter.Third;
			return Quarter.Fourth;
		}

		public enum Quarter
		{
			First = 1,
			Second = 2,
			Third = 3,
			Fourth = 4
		}

		public enum Month
		{
			January = 1,
			February = 2,
			March = 3,
			April = 4,
			May = 5,
			June = 6,
			July = 7,
			August = 8,
			September = 9,
			October = 10,
			November = 11,
			December = 12
		}

		public static TResult With<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator)
			where TResult : class
			where TInput : class
		{
			return o != null ? evaluator(o) : null;
		}

		public static TResult Return<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator, TResult failureValue)
			where TResult : class
			where TInput : class
		{
			return o != null ? evaluator(o) : failureValue;
		}

		public static string ProjectDetailsLink(int projectId, string projectName)
		{
			return string.Format("<a href='/Project/Details/{0}?tabNo=2'>{1}</a>", projectId, projectName);
		}

		internal static string NullableDollarAmount(decimal? amt)
		{
			var output = string.Empty;
			if (amt != null)
				if (amt > 0)
					output = string.Format("{0:C}", amt);
			return output;
		}

		internal static string NullableAmount(long? amt)
		{
			var output = string.Empty;
			if (amt != null)
				if (amt > 0)
					output = amt.ToString();
			return output;
		}

		public static void DetermineOrgEsaAndSite(List<Review> reviewList, out string orgCode, out string esaCode,
																out string siteCode)
		{
			orgCode = String.Empty;
			esaCode = String.Empty;
			siteCode = String.Empty;
			if (reviewList == null) return;

			//  need primary org, primary ESA and primary Site
			var orgDict = new Dictionary<string, int>();
			var esaDict = new Dictionary<string, int>();
			var siteDict = new Dictionary<string, int>();
			foreach (var reviewItem in reviewList)
			{
				if (reviewItem.OrgCode != null)
					AddToDict(reviewItem.OrgCode, orgDict);
				if (reviewItem.ESACode != null)
					AddToDict(reviewItem.ESACode, esaDict);
				if (reviewItem.SiteCode != null)
					AddToDict(reviewItem.SiteCode, siteDict);
			}
			orgCode = HighestCode(orgDict);
			esaCode = HighestCode(esaDict);
			siteCode = HighestCode(siteDict);
		}

		public static void AddToDict(string code, Dictionary<string, int> dict)
		{
			if (dict.ContainsKey(code))
				dict[code]++;
			else
				dict.Add(code, 1);
		}

		private static string HighestCode(Dictionary<string, int> dict)
		{
			var code = "????";
			int[] highest = { 0 };
			foreach (var pair in dict.Where(pair => pair.Value > highest[0]))
			{
				code = pair.Key;
				highest[0] = pair.Value;
			}
			return code;
		}

		public static string ListFor(IEnumerable<string> list)
		{
			var sb = new StringBuilder();
			sb.Append("<ul class='list-group'>");
			foreach (var i in list)
				sb.Append(string.Format("<li class='{1}'>{0}</li>", i, "list-group-item"));

			sb.Append("</ul>");
			return sb.ToString();
		}

		public static string ReviewEditLink(int reviewId)
		{
			return string.Format("<a href='/Review/Edit/{0}'>{1}</a>", reviewId, reviewId);
		}

		public static string FlagOut(bool isTrue)
		{
			return isTrue ? "Yes" : "No";
		}

		public static string FlagOut( string mfFlag )
		{
			return mfFlag.Equals( "Y" ) ? "Yes" : "No";
		}

		public static string FormatPercentage(decimal number)
		{
			return string.Format("{0}%   ", number * 100);
		}

		public static string FormatDecimal(decimal number)
		{
			return string.Format("{0:0.000}   ", number);
		}

		public static string FormatInteger(int number)
		{
			return string.Format("{0}   ", number);
		}

		public static string FormatCurrency(decimal amount)
		{
			return string.Format("{0}   ", NullableDollarAmount(amount));
		}

		public static string FormatLong(long number)
		{
			return string.Format("{0}   ", number);
		}

		public static string TableFromArray(string[] label, string[] data, string header, bool showEmpty)
		{
			var sb = new StringBuilder();
			sb.Append("<div class='table-responsive'>");
			sb.Append(string.Format(
				"<table caption='{0}' class='table table-striped table-bordered table-hover table-condensed'>", header));
			sb.Append(string.Format("<tr><th colspan='2'>{0}</th></tr>", header));
			var i = 0;
			foreach (var l in label)
			{
				if (showEmpty || IsFull(data[i]))
					sb.Append(string.Format("<tr><td>{0}</td><td>{1}</td></tr>", l, data[i]));
				i++;
			}
			sb.Append("</table>");
			sb.Append("</div>");
			return sb.ToString();
		}

		private static bool IsFull(string s)
		{
			var isFull = true;
			if (String.IsNullOrEmpty(s))
				isFull = false;
			else
			{
				s = s.Trim();
				if (s == "0")
					isFull = false;
				else
				{
					if (s.Equals("01/01/0001") || s.Equals("1/01/0001"))
						isFull = false;
				}
			}
			return isFull;
		}

		public static string AdminOrNot(string loginId, HttpSessionStateBase session)
		{
			var adminStatus = GetSessionAdminStatus(session);
			if (string.IsNullOrEmpty(adminStatus))
			{
				var user = new PatUser(loginId);
				adminStatus = user.IsAdministrator() ? "ADMIN" : "USER";
				SetSessionAdminStatus(session, adminStatus);
			}
			return adminStatus.Equals("ADMIN") ? "*" : string.Empty;
		}


		public static bool IsAdministrator(IIdentity userIdentity)
		{
			var user = new PatUser(userIdentity);
			return user.IsAdministrator();
		}

		/// <summary>
		///    Get previous item in the array
		/// </summary>
		/// <param name="value"></param>
		/// <param name="items"></param>
		/// <returns>if not found it return -1 or if over the limit of the array then it will stay at the current item. If everything fine then it return the previous item</returns>
		public static int GetPreviousItem(int value, int[] items)
		{
			int pos;
			return GetPreviousOrNextItem(value, items, CommonConstants.ButtonPrevious, out pos);
		}

		/// <summary>
		///    Get next item in the array
		/// </summary>
		/// <param name="value"></param>
		/// <param name="items"></param>
		/// <returns>if not found it return -1 or if over the limit of the array then it will stay at the current item. If everything fine then it return the next item</returns>
		public static int GetNextItem(int value, int[] items)
		{
			int pos;
			return GetPreviousOrNextItem(value, items, CommonConstants.ButtonNext, out pos);
		}

		/// <summary>
		///    Get the item current position in the array
		/// </summary>
		/// <param name="value"></param>
		/// <param name="items"></param>
		/// <returns>if not found it return -1 or if over the limit of the array then it will stay at the current item. If everything fine then it return the next item</returns>
		public static int GetCurrentPosition(int value, int[] items)
		{
			int pos;
			GetPreviousOrNextItem(value, items, CommonConstants.ButtonNext, out pos);
			return pos + 1;
		}

		private static int GetPreviousOrNextItem(int value, int[] items, string direction, out int pos)
		{
			var item = -1;

			// initialise
			pos = -1;

			if (items != null)
			{
				// search current item first
				pos = Array.IndexOf(items, value);
				var found = pos > -1;
				if (found)
				{
					// default to current item
					item = items[pos];

					if (direction == CommonConstants.ButtonPrevious)
					{
						var prevIndex = pos - 1;
						if (prevIndex > -1)
						{
							// move to previous item
							item = items[prevIndex];
						}
					}

					if (direction == CommonConstants.ButtonNext)
					{
						var nextIndex = pos + 1;

						if (nextIndex < items.Length)
						{
							// move to next item
							item = items[nextIndex];
						}
					}
				}
			}
			return item;
		}

		/// <summary>
		/// Determines whether Previous Button is enabled or not
		/// </summary>
		/// <param name="currentPos">The current position.</param>
		/// <param name="length">The length.</param>
		/// <returns></returns>
		public static bool IsPreviousButtonEnabled(int currentPos, int length)
		{
			return length > 1 && currentPos != 1;
		}

		/// <summary>
		/// Determines whether Next Button is enabled or not
		/// </summary>
		/// <param name="currentPos">The current position.</param>
		/// <param name="length">The length.</param>
		/// <returns></returns>
		public static bool IsNextButtonEnabled(int currentPos, int length)
		{
			return length > 1 && currentPos != length;
		}


		public static GridSettings GridSettingsFromReviewCriteria(ReviewListCriteriaViewModel criteria)
		{
			var gs = new GridSettings
				{
					IsSearch = true,
					PageIndex = 1,
					PageSize = 999999999,
					SortColumn = "ReviewId",
					SortOrder = "asc",
					Where = new Filter()
				};
			var ruleList = new List<Rule>();
			AddRuleFor(criteria.ReviewId, "ReviewId", ruleList);
			AddRuleFor(criteria.UploadId, "UploadId", ruleList);
			AddRuleFor(criteria.ClaimId, "ClaimId", ruleList);
			AddRuleFor(criteria.JobseekerId, "JobseekerId", ruleList);
			AddRuleFor(criteria.ActivityId, "ActivityId", ruleList);
			AddRuleFor(criteria.AssessmentCode, "AssessmentCode", ruleList);
			AddRuleFor(criteria.OutcomeCode, "OutcomeCode", ruleList);
			AddRuleFor(criteria.AssessmentAction, "AssessmentAction", ruleList);
			AddRuleFor(criteria.RecoveryReason, "RecoveryReason", ruleList);
			AddRuleFor(criteria.SiteCode, "SiteCode", ruleList);
			AddRuleFor(criteria.EsaCode, "EsaCode", ruleList);
			AddRuleFor(criteria.StateCode, "StateCode", ruleList);
			AddRuleFor(criteria.OrgCode, "OrgCode", ruleList);
			gs.Where.groupOp = "AND";
			gs.Where.rules = ruleList.ToArray();
			return gs;
		}

		private static void AddRuleFor(string data, string fieldName, ICollection<Rule> rulez)
		{
			if (string.IsNullOrEmpty(data)) return;
			var rule = new Rule { field = fieldName, op = "bw", data = data };
			rulez.Add(rule);
		}

		public static byte[] ToByteArray(this Stream stream)
		{
			stream.Position = 0;
			var buffer = new byte[stream.Length];
			for (var totalBytesCopied = 0; totalBytesCopied < stream.Length; )
				totalBytesCopied += stream.Read(buffer, totalBytesCopied, Convert.ToInt32(stream.Length) - totalBytesCopied);
			return buffer;
		}

		public static string ToJson<T>(this T sourceObject)
		{
			var serializer = new JavaScriptSerializer();
			var json = serializer.Serialize(sourceObject);
			return json;
		}

		public static T ToObject<T>(this string jsonString) where T : GridSettings
		{
			var serializer = new JavaScriptSerializer();
			var myObject = serializer.Deserialize<T>(jsonString);
			return myObject;
		}

		public static bool IsNotAnExcelCsv(HttpPostedFileBase hpf)
		{
			return !hpf.ContentType.Equals("application/vnd.ms-excel"); //  a CSV file created by Excel
		}

		internal static void DeleteFile(string fileName)
		{
			if (File.Exists(fileName))
				File.Delete(fileName);
		}

		public static string SearchCriteria(GridSettings gridSettings)
		{
			var criteriaString = string.Format("{0}:{1}-{2}", gridSettings.PageIndex, gridSettings.PageSize,
														  CriteriaOut(gridSettings.Where));
			return criteriaString;
		}

		private static string CriteriaOut(Filter whereFilter)
		{
			var sb = new StringBuilder();
			if (whereFilter != null && whereFilter.rules != null)
			{
				foreach (var rule in whereFilter.rules)
					sb.Append(string.Format("{0}:{1}:{2},", rule.field, rule.op, rule.data));
			}
			return sb.ToString();
		}

		public static string Environment()
		{
			var env = "TEST";
			if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["Environment"]))
				env = ConfigurationManager.AppSettings["Environment"];
			return env.ToUpper();
		}

		public static bool IsTestEnvironment()
		{
			var env = Environment();
			var isTest = env.Equals("TEST") || env.Equals("TESTFIX");
			return isTest;
		}

		public static bool IsDevEnvironment()
		{
			var env = Environment();
			var isDev = env.Equals("DEV") || env.Equals("DEVFIX");
			return isDev;
		}

		public static bool IsPreProdEnvironment()
		{
			var env = Environment();
			var isPreProd = env.Equals("PRE") || env.Equals("PREPROD");
			return isPreProd;
		}

		public static string WcagFixGrid(IHtmlString g, string gridId, string gridTitle)
		{
			var initGrid = g.ToHtmlString();
			var fixedGrid = initGrid.Replace(string.Format("<table id=\"{0}\">", gridId),
														string.Format("<table id=\"{0}\" title=\"{1}\"><tbody/>", gridId, gridTitle));
			return fixedGrid;
		}

		public static void AddDoubleQuoteToText(ControlCollection controls)
		{
			//padded all the text with double quote so if there's a comma in it, 
			//it will not be a problem when exported as csv file
			foreach (var control in controls)
			{
				if (control is TextBox)
				{
					var text = (control as TextBox).Text;
					(control as TextBox).Text = string.Format("\"{0}\"", text);
				}
				else if (control is Label)
				{
					var text = (control as Label).Text;
					(control as Label).Text = string.Format("\"{0}\"", text);
				}

			}
		}

		public static decimal ToDecimal(object objectValue)
		{
			decimal result;
			var textValue = string.Format("{0}", objectValue);
			decimal.TryParse(textValue, out result);
			return result;
		}

		public static int ToInt(object objectValue)
		{
			int result;
			var textValue = string.Format("{0}", objectValue);
			int.TryParse(textValue, out result);
			return result;
		}

		public static long ToLong(object objectValue)
		{
			long result;
			var textValue = string.Format("{0}", objectValue);
			long.TryParse(textValue, out result);
			return result;
		}

		public static bool ToDbDateTime(string dateString, out DateTime dateValue)
		{
			var sqlMinDate = new DateTime(1753, 1, 1);

			var ok = DateTime.TryParse(dateString, out dateValue);

			if (!ok || dateValue < sqlMinDate)
			{
				dateValue = sqlMinDate;
			}

			return ok;
		}

		public static DateTime ToDbDateTime(string dateString)
		{
			DateTime dateValue;
			var sqlMinDate = new DateTime(1753, 1, 1);

			var ok = DateTime.TryParse(dateString, out dateValue);

			if (!ok || dateValue < sqlMinDate)
			{
				dateValue = sqlMinDate;
			}

			return dateValue;
		}

		public static DateTime? ToNullDateTime(string dateString)
		{
			DateTime dateValue;
			var ok = DateTime.TryParse(dateString, out dateValue);

			if (!ok)
			{
				return null;
			}

			return dateValue;
		}

		public static string ToAdwBoolean(object nullableBool)
		{
			// initial value must be null NOT empty string.
			// this function works together with the other function: ToNullableBool (see below)
			string result = null;	

			var boolean = nullableBool as bool?;

			if (boolean != null)
			{
				result = boolean.Value ? DataConstants.YES : DataConstants.NO;
			}

			return result;
		}

		public static bool? ToNullableBool(string boolString)
		{
			bool? result = null;

			if (!string.IsNullOrWhiteSpace(boolString))
			{
				string[] trueList = { "YES", "Y", "1" };
				result = trueList.Any(x => x.Equals(boolString, StringComparison.OrdinalIgnoreCase));
			}

			return result;
		}

	}

	/// <summary>
	///    For development helper only
	/// </summary>
	public static class DebugHelper
	{
		/// <summary>
		///    It will return true on this condition: DateTime.Now.Minute % 5 != 0 and if environment is 'local'
		/// </summary>
		public static bool IsTemporaryAdmin
		{
			get
			{
				var isAdmin = DateTime.Now.Minute % 5 != 0;
				// it will be an admin for the first 4 minute gap, the 5th will be not an admin
				return AppHelper.IsLocalWorkstation && isAdmin;
			}
		}

		/// <summary>
		///    This is used as for development tool only
		///    It will return true for even number and if environment is 'local', otherwise false
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public static bool TrueOnEvenNumber(int number)
		{
			// it will work only on local and even Number
			return AppHelper.IsLocalWorkstation && number % 2 == 0;
		}

	}
}