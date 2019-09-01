using System.Collections.Generic;

namespace RosterLib.Interfaces
{
    public interface IAdpMaster
    {
        Dictionary<string, string> Adp { get; set; }
        string DataSource { get; set; }

        string GetAdp(string playerName);
        int Load();
        int Load(string path);
    }
}