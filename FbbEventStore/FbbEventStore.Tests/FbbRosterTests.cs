using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FbbEventStore.Tests
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	public class FbbRosterTests
	{
		private FbbRosters _sut;

		[TestInitialize]
		public void Setup()
		{
			_sut = new FbbRosters(
				new FbbEventStore());
		}

		[TestMethod]
		public void FbbEventStore_ConstructOk()
		{
			Assert.IsNotNull(_sut);
		}

		[TestMethod]
		public void FbbEventStore_KnowsRosterForCa()
		{
			var result = _sut.GetRoster("CA");
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Count > 0);
			_sut.DumpRoster("CA");
		}

		[TestMethod]
		public void FbbEventStore_KnowsMlbTeamForJoshBell()
		{
			var result = _sut.GetMlbTeam("Josh Bell");
			Assert.AreEqual("Pit", result);
		}


		[TestMethod]
		public void FbbEventStore_KnowsRosterForTc()
		{
			var result = _sut.GetRoster("TC");
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Count > 0);
			_sut.DumpRoster("TC");
		}

		[TestMethod]
		public void FbbEventStore_KnowsWhoOwnsYelich()
		{
			var result = _sut.GetOwnerOf("Christian Yelich");
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Equals("CA"));
		}

		[TestMethod]
		public void FbbEventStore_KnowsWhoOwnsPlayer()
		{
			var plyr = "Kirby Yates";
			var result = _sut.GetOwnerOf(plyr);
			Assert.IsNotNull(result);
			System.Console.WriteLine($"{result} owns {plyr}");
			Assert.IsTrue(result.Equals("BBJ"));
		}

		[TestMethod]
		public void TheClosers()
		{
			var plyrs = new List<Closer>
			{
				new Closer("Greg Holland",       "AD", "strong"),
				new Closer( "A.J. Minter",       "AB", "weak" ),
				new Closer( "Mychal Givens",     "BO", "committee" ),
				new Closer( "Ryan Brasier",      "BRS",  "medium" ),
				new Closer( "Steve Cishek",       "CHC", "medium" ),
				new Closer( "Raisel Iglesias",   "CR", "strong" ),
				new Closer( "Brad Hand",         "CI", "strong" ),
				new Closer( "Wade Davis",        "COL", "strong" ),
				new Closer( "Alex Colome",       "CWS", "strong" ),
				new Closer( "Shane Greene",       "DT", "strong" ),
				new Closer( "Roberto Osuna",     "HA", "strong" ),
				new Closer( "Ian Kennedy",       "KC", "committee" ),
				new Closer( "Hansel Robles",     "LAA", "medium" ),
				new Closer( "Kenley Jansen",     "LAD", "strong" ),
				new Closer( "Sergio Romo",       "MIA", "weak" ),
				new Closer( "Josh Hader",        "MB", "weak" ),
				new Closer( "Blake Parker",      "MT", "weak" ),
				new Closer( "Edwin Diaz",        "NYM", "strong" ),
				new Closer( "Aroldis Chapman",   "NYY", "strong" ),
				new Closer( "Blake Trienen",     "OA", "strong" ),
				new Closer( "Hector Neris",      "PHP", "weak" ),
				new Closer( "Felipe Vazquez",    "PIT", "strong" ),
				new Closer( "Kirby Yates",       "SD", "strong" ),
				new Closer( "Anthony Swarzak",   "SM", "committee" ),
				new Closer( "Will Smith",        "SF", "strong" ),
				new Closer( "Jordan Hicks",      "SLC", "medium" ),
				new Closer( "Jose Alvarado",     "Tam", "committee" ),
				new Closer( "Jose Leclerc",       "TR", "strong" ),
				new Closer( "Ken Giles",         "TB", "strong" ),
				new Closer( "Sean Doolittle",    "Wsh", "strong" )
			};

			foreach (var p in plyrs)
			{
				var owner = _sut.GetOwnerOf(p.Name);
				Assert.IsNotNull(owner);
				System.Console.WriteLine( 
					$"{owner} owns {p}");				
			}
		}

		[TestMethod]
		public void FbbEventStore_KnowsWhoOwnsTheBatters()
		{
			string[] plyr =
				{
					"Anthony Rendon",     // WSH
					"DJ LeMahieu",       //  NYY
					"David Peralta",     // ARI
					"Cody Bellinger",     //  LAD
					"Kolten Wong",         //  STL
					"Freddie Freeman",       // ATL
					"Carlos Santana",         // CLE
					"Tim Beckham",       //  SEA
					"Wilson Ramos",      //  NYM
					"Adam Jones",     //  ARI
					"Roberto Osuna",     //  HA
					"Ozzie Albies",        //  ATL
				};
			var result = _sut.GetOwnersOf(plyr);
			Assert.IsNotNull(result);
			foreach (var item in result)
			{
				var lastWord = item.Split(' ').Last();
				//if (lastWord.Equals("FA"))
					System.Console.WriteLine(item);
			}
		}

		[TestMethod]
		public void FbbEventStore_KnowsWhoOwnsTheMariners()
		{
			string[] plyr =
				{
					"Mallex Smith",     // CF
					"Mitch Haniger",       //  RF
					"Edwin Encarnacion",   // DH
					"Jay Bruce",     //  LF
					"Ryan Healy",         //  1B
					"Kyle Seager",       // 3B
					"Omar Narvaez",         // C
					"Dee Gordon",       //  2B
					"J.P. Crawford",      //  SS
					"Domingo Santana",
					"Tim Beckham",

				};
			var result = _sut.GetOwnersOf(plyr);
			Assert.IsNotNull(result);
			foreach (var item in result)
			{
				var lastWord = item.Split(' ').Last();
				//if ( lastWord.Equals("FA"))
				System.Console.WriteLine(item);
			}
		}

		[TestMethod]
		public void FbbEventStore_KnowsWhoOwnsTheTop10()
		{
			string[] plyr =
				{
					"Tim Anderson",     // CWS
					"DJ LeMahieu",       //  NYY
					"Anthony Rendon",   // WSH
					"Carlos Santana",     //  CLE
					"Cody Bellinger",         //  LAD
					"Mike Trout",       // LAA
					"Elvis Andrus",         // TEX
					"Freddie Galvis",       //  TOR
					"Jorge Polanco",      //  MIN
					"Pete Alonso"         //  NYM
				};
			var result = _sut.GetOwnersOf(plyr);
			Assert.IsNotNull(result);
			foreach (var item in result)
			{
				var lastWord = item.Split(' ').Last();
				//if ( lastWord.Equals("FA"))
				System.Console.WriteLine(item);
			}
		}

		[TestMethod]
		public void FbbEventStore_KnowsWhoOwnsTheFirstBasemen()
		{
			string[] plyr =
				{
					"Carlos Santana",     // CLE
					"Cody Bellinger",         //  LAD
					"Pete Alonso",         //  NYM
					"Freddie Freeman",       //  ATL
					"Mitch Moreland",   // BOS
					"Josh Bell",     //  PIT
					"Ryon Healy",       // SEA
					"Yuli Gurriel",         // HOU
					"Ji-Man Choi",       //  TB
					"Miguel Cabrera"      //  DET
				};
			var result = _sut.GetOwnersOf(plyr);
			Assert.IsNotNull(result);
			foreach (var item in result)
			{
				var lastWord = item.Split(' ').Last();
				//if ( lastWord.Equals("FA"))
				System.Console.WriteLine(item);
			}
		}

		[TestMethod]
		public void TheCatchers()
		{
			string[] plyr =
				{
					"Wilson Ramos",
					"Willson Contreras",  
					"Omar Narvaez",
					"Jonathon Lucroy",
					"Gary Sanchez",
					"J.T. Realmuto", 
					"Francisco Cervelli", 
					"Yadier Molina", 
					"Buster Posey",
					"Martin Maldonado"
				};
			var result = _sut.GetOwnersOf(plyr);
			Assert.IsNotNull(result);
			foreach (var item in result)
			{
				var lastWord = item.Split(' ').Last();
				//if ( lastWord.Equals("FA"))
				System.Console.WriteLine(item);
			}
		}

		[TestMethod]
		public void ThePitchers()
		{
			string[] plyr =
				{
					"Marco Gonzales",
					"Brad Keller",
					"Mike Fiers",
					"Yusei Kikuchi",
					"Jose Berriors",
					"Marco Estrada",
					"Jake Arrieta",
					"Trevor Bauer",
					"Matt Shoemaker",
					"Luis Castillo",
					"Jon Gray",
					"Cole Hamels",
					"Max Scherzer",
					"Madison Bumgarner",
					"Patrick Corbin",
					"Noah Syndergaard",
					"Gerrit Cole",
					"Blake Snell",
					"David Hess",
					"Mike Minor"
				};
			var result = _sut.GetOwnersOf(plyr);
			Assert.IsNotNull(result);
			foreach (var item in result)
			{
				var lastWord = item.Split(' ').Last();
				//if ( lastWord.Equals("FA"))
				System.Console.WriteLine(item);
			}
		}

		[TestMethod]
		public void WhoOwnsThisPlayer()
		{
			var plyr = "Niki Goodrum";
			var result = _sut.GetOwnerOf(plyr);
			Assert.IsNotNull(result);
			System.Console.WriteLine($"{result} owns {plyr}");
		}

		[TestMethod]
		public void PotentialAcesWhoMayHaveBeenDropped()
		{
			string[] plyr =
				{
					"Freddy Peralta",
					"Nathan Eovaldi",
					"Michael Pineda",
					"Matt Strahm",
					"Nick Pivetta",
					"Luke Weaver"
				};
			var result = _sut.GetOwnersOf(plyr);
			Assert.IsNotNull(result);
			foreach (var item in result)
			{
				var lastWord = item.Split(' ').Last();
				//if ( lastWord.Equals("FA"))
				System.Console.WriteLine(item);
			}
		}

		[TestMethod]
		public void InjuredPlayersWhoMayHaveBeenDropped()
		{
			string[] plyr =
				{
					"Rich Hill",
					"Scooter Gennett",
					"Matt Olson",
					"Shohei Ohtani",
					"Andrew Heaney",
					"Corey Dickerson",
					"Migeul Sano",
					"Carlos Martinez",
					"Ryan McMahon",
					"Juston Upton",
					"Hyun-Jin Ryu",
					"Aaron Hicks",
					"Daniel Murphy"
				};
			var result = _sut.GetOwnersOf(plyr);
			Assert.IsNotNull(result);
			foreach (var item in result)
			{
				var lastWord = item.Split(' ').Last();
				//if ( lastWord.Equals("FA"))
				System.Console.WriteLine(item);
			}
		}

		[TestMethod]
		public void Prospects()
		{
			string[] plyr =
				{
					"Franmill Reyes",
					"Ji-Man Choi",
					"Nick Anderson"
				};
			var result = _sut.GetOwnersOf(plyr);
			Assert.IsNotNull(result);
			foreach (var item in result)
			{
				var lastWord = item.Split(' ').Last();
				//if ( lastWord.Equals("FA"))
				System.Console.WriteLine(item);
			}
		}
	}
}
