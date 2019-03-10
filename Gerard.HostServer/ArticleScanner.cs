using Gerard.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using static Gerard.HostServer.Constants;

namespace Gerard.HostServer
{
	/// <summary>
	///   Examines news items to extract out significant events that may be auto updated
	/// </summary>
	public class ArticleExaminer
	{
		private readonly ITflService TflService;

		//public Logger Logger { get; set; }
		public ILog Log { get; set; }

		public int IgnoreCount { get; set; }

		public int BulkInputCount { get; set; }

		private readonly string[] Positions;

		public List<NewsArticleCommand> BulkArticles { get; set; }

		public List<ExaminationEvent> BulkEvents { get; set; }

		public ArticleExaminer(
			ITflService tflService,
			ILog logger)
		{
			Log = logger;
			TflService = tflService;
			BulkArticles = new List<NewsArticleCommand>();
			BulkEvents = new List<ExaminationEvent>();
			Positions = new[]
			{
				"FB/TE",
				"WR/KR",
				"LCB",
				"RCB",
				"OLB",
				"MLB",
				"G/T",
				"ILB",
				"QB",
				"RB",
				"WR",
				"LB",
				"FB",
				"CB",
				"TE",
				"PK",
				"DT",
				"DE",
				"SS",
				"LS",
				"FS",
				"DB",
				"OG",
				"OL",
				"LE",
				"RE",
				"KR",
				"OT",
				"NT",
				"RG",
				"LG",
				"S",
				"K",
				"G"
		 };
			if (TflService == null)
				LogError("No TFL service available!");
			else
				LogTrace("Article Examiner up and running");
		}

		private void LogInfo(string message)
		{
			Log.Info(message);
		}

		private void LogTrace(string message)
		{
			Log.Trace(message);
		}

		private void LogError(string message)
		{
			Log.Error(message);
		}

		public ExaminationEvent ExamineArticle(NewsArticleCommand article)
		{
			LogInfo("----------------------------------");
			LogInfo($"Examining {article}");

			var result = new ExaminationEvent
			{
				EventDate = article.ArticleDate,
				ExaminationDateTime = DateTime.Now,
				ArticleText = article.ArticleText
			};

			//  Offer sheet not official
			if (article.ArticleText.ToUpper().Contains("OFFER SHEET"))
			{
				LogResult(result);
				return result;
			}

			if (article.IsInDraftSeason())
			{
				//  Draft picks are handled by NFL.EXE
				if (article.ArticleText.ToUpper().Contains("OVERALL "))
				{
					LogResult(result);
					return result;
				}
				if (article.ArticleText.ToUpper().Contains("ROUND "))
				{
					//  sometime players are aquired in a trade for a late-round pick
					if (!article.ArticleText.ToUpper().Contains("LATE-ROUND "))
					{
						LogResult(result);
						return result;
					}
				}
			}

			result = PurifyTheText(article, result);
			LogTrace($"Purified {article}");

			//  Parse the article looking for a key word which triggers hunt 
			//  for a player and optionally team
			string[] words = SplitIntoWords(article);
			LogTrace($"article contains {words.Length} words");
			var previousWord = string.Empty;
			for (var i = 0; i < words.Length; i++)
			{
				var word = words[i];
				if (WordIsSigningWord(word))
				{
					ResultIsSigning(result, i, words);
				}
				if (WordIsWaiverWord(word))
				{
					ResultIsWaiver(result, i, words);
				}
				if (WordIsCutWord(word, previousWord))
				{
					ResultIsCut(result, i, words);
				}
				if (WordIsRetirementWord(word))
				{
					ResultIsRetirement(result, i, words);
				}
				previousWord = word;
			}

			//  Look for 2 words
			if (IsInjury(article.ArticleText))
			{
				LogTrace("Article is injury");
				result.RecommendedAction = "INJURY";
				result.IsInjury = true;
				result = FindPlayerNameByTeam(result, article.ArticleText);
			}

			if (result.IsRetirement)
			{
				LogResult(result);
				return result;
			}

			// Two words Free Agent
			if (article.ArticleText.ToUpper().Contains("FREE AGENT"))
			{
				LogTrace("Article is about Free agent");
				var fawords = article.ArticleText.Split(' ');
				for (var i = 0; i < fawords.Length; i++)
				{
					var word = fawords[i];
					if (word.ToUpper() == "AGENT")
					{
						ResultIsFreeAgent(result, i, fawords);
						break;
					}
				}
			}

			if (result.RecommendedAction.Equals(Result.Ignore))
			{
				LogResult(result);
				return result;
			}

			if (result.PlayerFirstName == null || result.PlayerLastName == null)
				result.RecommendedAction = Result.Ignore;
			else
			{
				if (!result.RecommendedAction.Equals("NEWBIE"))
				{
					LogTrace($"Asking service for: {result.PlayerFirstName} {result.PlayerLastName}");
					var p = TflService.GetNflPlayer(result.PlayerFirstName, result.PlayerLastName);
					if (p == null)
						result.RecommendedAction = Result.Ignore;
					else
						result.PlayerId = p.PlayerCode;
				}
			}
			LogResult(result);
			return result;
		}

		private static string[] SplitIntoWords(NewsArticleCommand article)
		{
			var actualWords = new List<string>();
			var rawWords = article.ArticleText.Split(' ');
			for (var i = 0; i < rawWords.Length; i++)
			{
				var word = rawWords[i];
				if (WordHas2FullStops(word))
				{
					var endOfFirst = word.LastIndexOf('.');
					var firstName = word.Substring(0, endOfFirst + 1);
					var lastName = word.Substring(endOfFirst + 1, word.Length - endOfFirst - 1);
					actualWords.Add(firstName);
					actualWords.Add(lastName);
				}
				else
					actualWords.Add(word);
			}
			return actualWords.ToArray();
		}

		private static bool WordHas2FullStops(string word)
		{
			int countFullStops = word.Count(c => c == '.');
			return (countFullStops.Equals(2));
		}

		private void LogResult(ExaminationEvent result)
		{
			LogInfo($"Result: {result.RecommendedAction}");
		}

		private ExaminationEvent FindPlayerNameByTeam(
			ExaminationEvent result,
			string articleText)
		{
			var words = articleText.Split(' ');
			for (var i = 0; i < words.Length; i++)
			{
				var word = words[i];
				if (WordIsTeamWord(word))
				{
					SetPlayer(result, words);
					SetTeam(result, i, words, 0);
				}
			}
			return result;
		}

		private static bool WordIsTeamWord(string word)
		{
			var team = GetTeamCodeFor(word);
			return team != "??";
		}

		private static bool IsInjury(string articleText)
		{
			var isInjury = false;
			articleText = articleText.ToUpper();
			if (articleText.Contains("TORE HIS ACL"))
				isInjury = true;
			else if (articleText.Contains("MISS"))
			{
				if (articleText.Contains(" SEASON"))
					isInjury = true;
			}
			else
			{
				if (!articleText.Contains("INJURED")) return false;
				if (articleText.Contains("RESERVE"))
					isInjury = true;
			}
			return isInjury;
		}

		private ExaminationEvent PurifyTheText(
			NewsArticleCommand article,
			ExaminationEvent result)
		{
			//  get rid of the full stop at the end
			if (article.ArticleText.Substring(Math.Max(0, article.ArticleText.Length - 1)) == ".")
				article.ArticleText = article.ArticleText.Substring(0, article.ArticleText.Length - 1);
			//  get rid of commas
			article.ArticleText = article.ArticleText.Replace(",", string.Empty);
			//  get rid of the term exclusive rights free agent
			article.ArticleText = article.ArticleText.Replace("exclusive rights free agent ", string.Empty);
			//  get rid of the term restricted free agent
			article.ArticleText = article.ArticleText.Replace("restricted free agent ", string.Empty);
			//  Treat "agreed to terms with" as "signed"
			article.ArticleText = article.ArticleText.Replace("agreed to terms with", "signed");
			//  Treat "has agreed to a pay cut signing" as "signed"
			article.ArticleText = article.ArticleText.Replace("has agreed to a pay cut signing", "is signing");
			//  Treat "re-signed" as "signed"
			article.ArticleText = article.ArticleText.Replace("re-signed", "signed");
			//  Treat "claimed" as "signed"
			article.ArticleText = article.ArticleText.Replace("claimed", "signed");
			//  Treat "has retired" as "is retiring"
			article.ArticleText = article.ArticleText.Replace("has retired", "is retiring");
			//  Treat "waived/failed physical" as "waived"
			article.ArticleText = article.ArticleText.Replace("waived/failed physical", "waived");
			//  Treat "waived/released" as "waived"
			article.ArticleText = article.ArticleText.Replace("waived/released", "waived");
			//  Get rid of the positions
			article.ArticleText = StripOutPositions(article.ArticleText, result);
			//  remove possessives
			article.ArticleText = article.ArticleText.Replace("'s", "");
			return result;
		}

		private string StripOutPositions(string articleText, ExaminationEvent result)
		{
			NewMethodSetPosition(articleText, result);
			return Positions.Aggregate(articleText, (current, pos) =>
				current.Replace(pos + " ", string.Empty));
		}

		private void NewMethodSetPosition(string articleText, ExaminationEvent result)
		{
			for (int i = 0; i < Positions.Length; i++)
			{
				var pos = Positions[i] + " ";
				if (articleText.Contains(pos))
				{
					result.Position = Positions[i];
				}
			}
		}

		private void ResultIsSigning(
			ExaminationEvent result,
			int wordIndex,
			IList<string> words)
		{
			result.IsSigning = true;
			result.IsWaiver = false;
			result.IsRetirement = false;
			result.RecommendedAction
				= words[wordIndex].ToUpper() == "SIGNED"
				|| words[wordIndex].ToUpper() == "SIGNING" ? "SIGN" : "TRADE";

			if (words[wordIndex].ToUpper() == "SIGNED")
			{
				SetPlayer(result, words);
				SetTeam(result, wordIndex, words);
			}
			else
			{
				if (result.RecommendedAction == "TRADE")
				{
					SetPlayer1(result, words);
					SetTeam(result, wordIndex, words);
				}
				else
				{
					SetPlayer1(result, words);
					SetTeam(result, wordIndex, words, 4);
				}
			}
			if (result.TeamId.Equals("??"))
			{
				result.IsSigning = false;
				result.RecommendedAction = Result.Ignore;
			}
		}

		private void SetPlayer1(
			ExaminationEvent result,
			IList<string> words)
		{
			LogTrace("SetPlayer1");
			for (int i = 0; i < words.Count - 1; i++)
			{
				var playerFirstName = words[i];
				if (NotAName(playerFirstName)) continue;
				var playerLastName = words[i + 1];
				if (NotAName(playerLastName)) continue;
				var playerName = $"{playerFirstName} {playerLastName}";
				LogTrace($"Calling Service to find {playerName}");
				var p = TflService.GetNflPlayer(
					playerFirstName,
					playerLastName);
				if (p != null)
				{
					result.PlayerId = p.PlayerCode;
					result.PlayerFirstName = playerFirstName;
					result.PlayerLastName = playerLastName;
					result.PlayerName = playerName;
					break;
				}
			}
		}

		private void SetPlayer(
			ExaminationEvent result,
			IList<string> words)
		{
			var realFirstName = string.Empty;
			var realLastName = string.Empty;
			LogTrace("SetPlayer");
			for (int i = 0; i < words.Count - 1; i++)
			{
				var playerFirstName = words[i];
				if (NotAName(playerFirstName)) continue;
				var playerLastName = words[i + 1];
				if (NotAName(playerLastName)) continue;
				//  attempt a code lookup
				var playerName = $"{playerFirstName} {playerLastName}";
				LogTrace($"Calling Service to find {playerName}");
				result.PlayerFirstName = playerFirstName;
				result.PlayerLastName = playerLastName;
				var p = TflService.GetNflPlayer(
					playerFirstName,
					playerLastName);
				if (p != null)
				{
					if (string.IsNullOrEmpty(result.PlayerId))
					{
						result.PlayerId = p.PlayerCode;
						result.PlayerFirstName = playerFirstName;
						result.PlayerLastName = playerLastName;
						result.PlayerName = playerName;
						realFirstName = playerFirstName;
						realLastName = playerLastName;
					}
					else
					{
						// 2 players found, we can only handle 1 at a time
						result.PlayerId = string.Empty;
						result.PlayerFirstName = string.Empty;
						result.PlayerLastName = string.Empty;
						result.PlayerName = string.Empty;
						break;
					}
				}
			}
			if (string.IsNullOrEmpty(result.PlayerId)
				&& !string.IsNullOrEmpty(result.PlayerFirstName)
				&& !string.IsNullOrEmpty(result.PlayerLastName)
				)
			{
				if (!result.RecommendedAction.Equals("WAIVER"))
				{
					// potential Newbie??
					Log.Info($"{result.PlayerFirstName} {result.PlayerLastName} is a potential newbie");
					result.RecommendedAction = "NEWBIE";
				}
			}
			else
			{
				//  we have identified, get rid of newbie names
				result.PlayerFirstName = realFirstName;
				result.PlayerLastName = realLastName;
			}
		}

		private bool NotAName(string word)
		{
			word = word.ToUpper();
			if (word == "THE"
				|| IsTeamWord(word)
				|| word == "A"
				|| word == "AND"
				|| word == "ACQUIRED"
				|| word == "BELIEVES"
				|| word == "CONTRACT"
				|| word == "CUT"
				|| word == "EXERCISE"
				|| word == "EXCHANGE"
				|| word == "FIRST-ROUND"
				|| word == "FRIDAY"
				|| word == "FORMALY"
				|| word == "FROM"
				|| word == "FREE"
				|| word == "GAME"
				|| word == "HIS"
				|| word == "HOSTED"
				|| word == "FOR"
				|| word == "IN"
				|| word == "IS"
				|| word == "KNOW"
				|| word == "MONDAY"
				|| word == "NOT"
				|| word == "OF"
				|| word == "ON"
				|| word == "OFFER"
				|| word == "ONE-YEAR"
				|| word == "OPTION"
				|| word == "OFF"
				|| word == "PAY"
				|| word == "PASSING"
				|| word == "RE-SIGNED"
				|| word == "RETIRED"
				|| word == "SAID"
				|| word == "SATURDAY"
				|| word == "SIGNED"
				|| word == "SUNDAY"
				|| word == "TUESDAY"
				|| word == "THE"
				|| word == "THURSDAY"
				|| word == "VISITED"
				|| word == "WAIVERS"
				|| word == "WEDNESDAY"
				|| word == "WHEN")
				return true;
			return false;
		}

		private bool IsTeamWord(string word)
		{
			var teamWord = GetTeamCodeFor(word);
			if (teamWord.Equals("??"))
				return false;
			return true;
		}

		private void ResultIsWaiver(
			ExaminationEvent result,
			int wordIndex,
			IList<string> words)
		{
			result.IsWaiver = true;
			result.IsSigning = false;
			result.IsRetirement = false;
			result.RecommendedAction = "WAIVER";

			SetPlayer(result, words);
			SetTeam(result, wordIndex, words);
		}

		private void ResultIsCut(
			ExaminationEvent result,
			int wordIndex,
			IList<string> words)
		{
			result.IsCut = true;
			result.IsWaiver = false;
			result.IsSigning = false;
			result.IsRetirement = false;
			SetPlayer(result, words);
			SetTeam(result, wordIndex, words);
			if (string.IsNullOrEmpty(result.PlayerId))
				result.RecommendedAction = Result.Ignore;
			else
				result.RecommendedAction = Result.Cut;
		}

		private void ResultIsFreeAgent(
			ExaminationEvent result,
			int wordIndex,
			IList<string> words)
		{
			result.IsCut = true;
			result.IsWaiver = false;
			result.IsSigning = false;
			result.IsRetirement = false;
			result.RecommendedAction = Result.Cut;
			SetPlayer(result, words);
		}

		private void ResultIsRetirement(
			ExaminationEvent result,
			int wordIndex,
			IList<string> words)
		{
			result.IsCut = false;
			result.IsWaiver = false;
			result.IsSigning = false;
			result.IsRetirement = true;
			result.RecommendedAction = Result.Retired;
			SetPlayer(result, words);
		}

		private void SetTeam(
			ExaminationEvent result,
			int wordIndex,
			IList<string> words)
		{
			LogTrace("SetTeam");
			try
			{
				var possibleTeamName = words[wordIndex - 1];
				if (possibleTeamName.Equals("have"))
				{
					wordIndex--;
					possibleTeamName = words[wordIndex - 1];
				}
				result.TeamName = possibleTeamName;
				result.TeamId = GetTeamCodeFor(words[wordIndex - 1]);
			}
			catch (ArgumentOutOfRangeException)
			{
				result.TeamName = "???";
				result.TeamId = "??";
			}
			LogTrace($"SetTeam to {result.TeamName}");
		}

		private void SetTeam(
			ExaminationEvent result,
			int wordIndex,
			IList<string> words,
			int offset)
		{
			LogTrace("SetTeam");
			try
			{
				result.TeamName = words[wordIndex - offset];
				result.TeamId = GetTeamCodeFor(words[wordIndex - offset]);
			}
			catch (ArgumentOutOfRangeException)
			{
				result.TeamName = "???";
				result.TeamId = "??";
			}
			LogTrace($"SetTeam to {result.TeamName}");
		}

		private static bool WordIsSigningWord(string word)
		{
			return word.ToUpper().Equals("SIGNED")
				|| word.ToUpper().Equals("ACQUIRED")
				|| word.ToUpper().Equals("SIGNING");
		}

		private static bool WordIsWaiverWord(string word)
		{
			return word.ToUpper().Equals("WAIVED");
		}

		private static bool WordIsCutWord(string word, string previousWord)
		{
			if (word.ToUpper().Equals("RELEASED"))
			{
				return true;
			}
			if (word.ToUpper().Equals("CUT"))
			{
				if (previousWord.ToUpper().Equals("PAY"))
					return false;
				return true;
			}
			return false;
		}

		private static bool WordIsRetirementWord(string word)
		{
			return word.ToUpper().Equals("RETIREMENT")
				|| word.ToUpper().Equals("RETIRING")
				|| word.ToUpper().Equals("RETIRED");
		}

		private static string GetTeamCodeFor(string teamName)
		{
			var teamCode = "??";
			// just do this in memory for now
			switch (teamName.ToUpperInvariant())
			{
				case "BEARS":
					teamCode = "CH";
					break;

				case "FALCONS":
					teamCode = "AF";
					break;

				case "COWBOYS":
					teamCode = "DC";
					break;

				case "CARDINALS":
					teamCode = "AC";
					break;

				case "LIONS":
					teamCode = "DL";
					break;

				case "PANTHERS":
					teamCode = "CP";
					break;

				case "GIANTS":
					teamCode = "NG";
					break;

				case "49ERS":
					teamCode = "SF";
					break;

				case "PACKERS":
					teamCode = "GB";
					break;

				case "SAINTS":
					teamCode = "NO";
					break;

				case "EAGLES":
					teamCode = "PE";
					break;

				case "RAMS":
					teamCode = "LR";
					break;

				case "VIKINGS":
					teamCode = "MV";
					break;

				case "BUCCANEERS":
					teamCode = "TB";
					break;

				case "BUCS":
					teamCode = "TB";
					break;

				case "REDSKINS":
					teamCode = "WR";
					break;

				case "SEAHAWKS":
					teamCode = "SS";
					break;

				case "RAVENS":
					teamCode = "BR";
					break;

				case "TEXANS":
					teamCode = "HT";
					break;

				case "BILLS":
					teamCode = "BB";
					break;

				case "BRONCOS":
					teamCode = "DB";
					break;

				case "BENGALS":
					teamCode = "CI";
					break;

				case "COLTS":
					teamCode = "IC";
					break;

				case "DOLPHINS":
					teamCode = "MD";
					break;

				case "CHIEFS":
					teamCode = "KC";
					break;

				case "BROWNS":
					teamCode = "CL";
					break;

				case "JAGUARS":
					teamCode = "JJ";
					break;

				case "PATRIOTS":
					teamCode = "NE";
					break;

				case "RAIDERS":
					teamCode = "OR";
					break;

				case "STEELERS":
					teamCode = "PS";
					break;

				case "TITANS":
					teamCode = "TT";
					break;

				case "JETS":
					teamCode = "NJ";
					break;

				case "CHARGERS":
					teamCode = "LC";
					break;
			}
			return teamCode;
		}
	}
}
