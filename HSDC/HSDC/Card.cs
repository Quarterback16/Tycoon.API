using System;
using System.Drawing;

namespace HSDC
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
            return MemberwiseClone();
        }

        public Color GetQualityColor()
        {
            switch (Quality)
            {
                case 0: return Color.FromArgb(255, 255, 255);
                case 1: return Color.FromArgb(255, 255, 255);
                case 2: return Color.Gray;
                case 3: return Color.FromArgb(0, 112, 255);
                case 4: return Color.FromArgb(163, 53, 238);
                case 5: return Color.FromArgb(255, 128, 0);
                default: return Color.Gray;
            }
        }

        public Color GetTypeColor()
        {
            switch (Type)
            {
                case "minion": return Color.Maroon;
                case "spell": return Color.ForestGreen;
                case "weapon": return Color.DimGray;
            }
            return Color.DimGray;
        }
    }
}
