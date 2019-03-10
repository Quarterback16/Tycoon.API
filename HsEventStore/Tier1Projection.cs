using System.Collections.Generic;

namespace HsEventStore
{
    public class Tier1Projection : Handles<MetaChangeEvent>
    {
        public List<string> Tier1Decks { get; set; }

        public Tier1Projection()
        {
            Tier1Decks = new List<string>();
            //  get events from the event store
            //  handle them
            //  output the list
        }

        public void Handle(MetaChangeEvent e)
        {
            
        }
    }
}
