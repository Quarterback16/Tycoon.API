namespace HearthCap.Core.GameCapture.HS.Events
{
    public class ArenaHeroDetected : GameEvent
    {
        public ArenaHeroDetected(string hero)
            : base("hero detected: " + hero)
        {
            Hero = hero;
        }

        public string Hero { get; protected set; }
    }
}
