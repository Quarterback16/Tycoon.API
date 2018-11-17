using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace HSDC
{
    public partial class FormMain : Form
    {
        public DeckCounter dc;
        public AutoCompleteStringCollection cardStrings;

        public FormMain()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false; 

            Form splash = new FormSplash();
            splash.Show();
            splash.Refresh();

                // Set up DeckCounter class
                dc = new DeckCounter(deck);
                // Set accessor values
                dc.LabelNumCards = labelNumCards;
                dc.LabelTopdeck = labelTopdeck;
                dc.FormMain = this;
                // Load last deck
                if (dc.Options.GetAsString("LastDeck") != "") dc.LoadDeck(dc.Options.GetAsString("LastDeck"));

                int w = Screen.PrimaryScreen.WorkingArea.Width;
                int h = Screen.PrimaryScreen.WorkingArea.Height;
                this.MaximumSize = new System.Drawing.Size(w, h);

            splash.Hide();
            splash.Dispose();

            if (dc.Options.GetAsInteger("WindowMainX") != 0)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(dc.Options.GetAsInteger("WindowMainX"), dc.Options.GetAsInteger("WindowMainY"));
            }

            dc.SetStayOnTop(dc.Options.GetAsBool("StayOnTop"));
        }

        private void buttonMenu_Click(object sender, EventArgs e)
        {
            Form FormMenu = new FormMenu(this.dc);
            FormMenu.Show();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            if (dc.Options.GetAsBool("ConfirmOnDeckReset"))
            {
                if (MessageBox.Show("Do you really want to reset the deck? This will reload the last default configuration of this deck.", "Really reset?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    dc.ResetDeck();
                }
            }
            else
            {
                dc.ResetDeck();
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            dc.Options.Set("WindowMainX", this.Location.X.ToString());
            dc.Options.Set("WindowMainY", this.Location.Y.ToString());

            if (dc.tooltip != null)
            {
                dc.Options.Set("WindowTooltipX", dc.tooltip.Location.X.ToString());
                dc.Options.Set("WindowTooltipY", dc.tooltip.Location.Y.ToString());
            }

        }

        private void buttonMemoryReader_Click(object sender, EventArgs e)
        {
            /*
            if (dc.MemoryReader.IsStarted)
            {
                dc.MemoryReader.Stop();
                buttonMemoryReader.Text = "MR = OFF";
            }
            else
            {
                dc.MemoryReader.Start();
                buttonMemoryReader.Text = "MR = ON";
            }
            */
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            /*
            int h = (this.deck.Height - dc.CardCountUnique() - 1) / dc.CardCountUnique();
            if (h < 20) h = 20;
            dc.SetCardHeight(h);

            int borderWidth = this.Width - this.ClientRectangle.Width;
            this.Width = (h * 10) + borderWidth;
            */

        }
        private void deck_Resize(object sender, EventArgs e)
        {
            //FormMain_Resize(sender, e);
        }

    }
}
