using RosterLib.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace RosterLib.Helpers
{
	public class AdpMaster : IAdpMaster
    {
        public Dictionary<string, string> Adp { get; set; }
        public string DataSource { get; set; }

        public AdpMaster()
        {
            Adp = new Dictionary<string, string>();
        }

        public AdpMaster(
            string dataSource) : this()
        {
			//  data source assumes 12 team league
			//  adp would change for differnt sized leagues
            DataSource = dataSource;
        }

        public int Load(
            string path)
        {
            return LoadCsv(path);
        }

        public int Load()
        {
            return LoadCsv(DataSource);
        }

        public string GetAdp(string playerName)
        {
            var adp = string.Empty;
            if (Adp.Count == 0)
                Load();

            if (CsvHas(ref playerName))
                adp = Adp[playerName];
            return adp;
        }

		private bool CsvHas(
            ref string playerName)
		{
			if (Adp.ContainsKey(playerName))
				return true;

			//  look harder eg Patrick Mahomes is Pat Mahomes
			playerName = TakeOutNoise(playerName);
			var name = playerName.Split(' ');
			var surname = name[name.Length-1];
			foreach (KeyValuePair<string, string> pair in Adp)
			{
				var keyName = TakeOutNoise(pair.Key);
				if (keyName.Contains(surname))
				{
					playerName = pair.Key;
					return true;
				}
			};
			//Console.WriteLine($"{playerName} not found");
			return false;
		}

		private string TakeOutNoise(string playerName)
		{
			var name = playerName.Replace("Jr.", string.Empty);
			name = name.Replace("Jr", string.Empty);
			name = name.Replace("II", string.Empty);
			name = name.Replace("III", string.Empty);
			return name;
		}

		private int LoadCsv(
            string path)
        {
            var lineCount = 0;
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    // there is no header line
                    //if (lineCount > 0)
                    //{
                        var playerName = values[2];
                        var adp = values[1];

//                        Console.WriteLine(
//                            $"{playerName} {adp}");
                        Adp.Add(playerName, adp);
                    //}
                    lineCount++;
                }
            }
            return Adp.Count;
        }
    }
}
