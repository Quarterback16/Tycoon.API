using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

namespace NetSniffer
{
    public class Card : ICloneable
    {
        public int Id = 0;
        public int Quality = 0;
        public string Name = "{ card name }";
        public int Cost = 0;
        public string Type = "minion";
        public string Image = "";
        public int Count = 1;

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
