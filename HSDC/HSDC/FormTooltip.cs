using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace HSDC
{
    public partial class FormTooltip : Form
    {
        private DeckCounter dc;
        private Card Card;

        public FormTooltip(DeckCounter dc, Card card)
        {
            InitializeComponent();
            this.dc = dc;
            this.Card = card;

            if (dc.Options.GetAsInteger("WindowTooltipX") != 0)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(dc.Options.GetAsInteger("WindowTooltipX"), dc.Options.GetAsInteger("WindowTooltipY"));
            }

        }

        public void SetCard(Card c)
        {
            this.Card = c;
            FormTooltip_Load(null, null);
        }

        private void FormTooltip_Load(object sender, EventArgs e)
        {
            // Load image from local filesystem (if exists), if not from webserver
            string language = dc.Options.GetAsString("Language");
            if (!File.Exists("data/images/" + language + "/" + Card.Image + ".png"))
            {
                string lang = "";
                switch (language)
                {
                    case "en": lang = "enus"; break;
                    case "es": lang = "eses"; break;
                    case "de": lang = "dede"; break;
                    case "fr": lang = "frfr"; break;
                    case "pt": lang = "ptbr"; break;
                    case "ru": lang = "ruru"; break;
                    default: lang = "enus"; break;
                }
                WebClient wc = new WebClient();
                wc.DownloadFile("http://wow.zamimg.com/images/hearthstone/cards/" + lang + "/original/" + Card.Image + ".png", "data/images/" + language + "/" + Card.Image + ".png");
            }

            // Load and display image
            pictureBox.Load("data/images/" + language + "/" + Card.Image + ".png");
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            dc.tooltip = null;
            dc.Options.Set("WindowTooltipX", this.Location.X.ToString());
            dc.Options.Set("WindowTooltipY", this.Location.Y.ToString());
            this.Close();
        }

        private void FormTooltip_FormClosing(object sender, FormClosingEventArgs e)
        {
            dc.tooltip = null;
            dc.Options.Set("WindowTooltipX", this.Location.X.ToString());
            dc.Options.Set("WindowTooltipY", this.Location.Y.ToString());
        }
    }
}
