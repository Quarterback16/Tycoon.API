namespace Helpers.Interfaces
{
    public interface INormaliseTitles
    {
        string NormaliseTitle(string title, string type, bool logIt = true);
    }
}