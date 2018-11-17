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
using System.Drawing.Drawing2D;

namespace HSDC
{
    public partial class CardControl : Control
    {
        private DeckCounter dc;
        public Card Card;

        public CardControl(DeckCounter dc, Card card)
        {
            InitializeComponent();

            this.DoubleBuffered = true;

            this.Width = 250;
            this.Height = 25;
            this.Margin = new Padding(0, 0, 0, 1);

            this.dc = dc;
            this.Card = card;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            // Create buffer
            int width = 400, height = 40;
            Image bufferImg = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bufferImg);

            // Load background image
            if (dc.Options.GetAsBool("CardShowImage") == true)
            {
                Bitmap bitmap = new Bitmap("data/images/small/" + Card.Image + ".jpg");
                g.DrawImage(bitmap, 0, 0);
            }
            else
            {
                g.Clear(Color.DimGray);
            }

            // If user wants to have colored cards by type, color them ;-)
            if (dc.Options.GetAsBool("CardColorByType") == true)
            {
                if (dc.Options.GetAsBool("CardShowImage") == true)
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(64, Card.GetTypeColor())), new Rectangle(0, 0, width, height));
                }
                else
                {
                    g.Clear(Card.GetTypeColor());
                }
            }

            // Draw card name
            string name = Card.Name;
            if (dc.Options.GetAsBool("CardShowTD") == true)
            {
                float chance = 100f * Card.Count / dc.CardCount();
                name += " (" + Math.Round(chance, 0).ToString() + "%)";
            }
            Color c = (dc.Options.GetAsBool("CardColoredName") == true) ? Card.GetQualityColor() : Color.White;

            // Dim card background if this card is unavailable
            if (Card.Count == 0)
            {
                c = Color.Gray;
                g.FillRectangle(new SolidBrush(Color.FromArgb(dc.Options.GetAsInteger("CardDarkenValue"), 32, 32, 32)), new Rectangle(0, 0, width, height));
            }

            DrawText(g, new Rectangle(height + 2, 0, width, height), StringAlignment.Near, c, 18, name);


            // Show card count depending on number of cards remaining
            if (Card.Count > 1)
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(48, 48, 48)), new Rectangle(width - height, 0, height, height));
                g.DrawRectangle(new Pen(Color.FromArgb(255, 255, 255)), new Rectangle(width - height, 0, height, height));
                DrawText(g, new Rectangle(width - height, 0, height, height), StringAlignment.Center, Color.White, 22, Card.Count.ToString());
            }

            // Draw mana gem and cost
            if (dc.Options.GetAsBool("CardShowImage") == true)
            {
                Bitmap bitmap = new Bitmap("data/images/mana.png");
                g.DrawImage(bitmap, new Rectangle(0, 0, height, height), new Rectangle(0, 0, 25, 25), GraphicsUnit.Pixel);
            }
            else
            {
                g.FillRectangle(new SolidBrush( (Card.Count > 0) ? Color.RoyalBlue : Color.DarkBlue ), new Rectangle(0, 0, height, height));
                g.DrawRectangle(new Pen(Color.FromArgb(255, 255, 255)), new Rectangle(0, 0, height, height));
            }
            DrawText(g, new Rectangle(0, 0, height, height), StringAlignment.Center, Card.Count > 0 ? Color.White : Color.Gray, 22, Card.Cost.ToString());

            // Draw border
            g.DrawRectangle(new Pen(Color.Black), new Rectangle(0, 0, width, height));

            // Flip buffer to control
            pe.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            pe.Graphics.DrawImage(bufferImg, new Rectangle(0, 0, this.Width, this.Height), new Rectangle(0, 0, width, height), GraphicsUnit.Pixel);
        }
        void DrawText(Graphics g, Rectangle position, StringAlignment alignment, Color color, float fontSize, string text)
        {
            // Define alignment data
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = alignment;

            //Define font
            Font f = new Font("Impact", fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            Pen p = new Pen(Color.Black, 2);
            p.LineJoin = LineJoin.Round;

            // Draw text
            GraphicsPath gp = new GraphicsPath();
            gp.AddString(text, f.FontFamily, (int)f.Style, fontSize, position, sf);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawPath(p, gp);
            g.FillPath(new SolidBrush(color), gp);

            // Disposal
            gp.Dispose();
            sf.Dispose();
            f.Dispose();
            p.Dispose();
        }

        public void SetHeight(int height)
        {
            this.Height = height;
            this.Width = height * 10;
            this.Invalidate();
        }
      
        private void CardControl_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            if (me.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                dc.ShowTooltip(Card);          
                return;
            }

            DoClick(me.Button);

            if (dc.Options.GetAsBool("CardShowTD"))
            {
                dc.RefreshAllCards();
            }
            else
            {
                Invalidate();
            }

            dc.Refresh();
        }
        private void DoClick(System.Windows.Forms.MouseButtons button)
        {
            if (dc.Options.GetAsBool("SwapMouse"))
            {
                if (button == System.Windows.Forms.MouseButtons.Left)
                {
                    button = System.Windows.Forms.MouseButtons.Right;
                }
                else
                {
                    button = System.Windows.Forms.MouseButtons.Left;
                }
            }

            if (button == System.Windows.Forms.MouseButtons.Right)
            {
                if (Card.Count > 0) Card.Count--;
                if (Card.Count == 0 && dc.Options.GetAsBool("CardAutoSort")) dc.Sort();
            }
            if (button == System.Windows.Forms.MouseButtons.Left)
            {
                if (dc.CardCount() >= 30) return;
                if (Card.Count < 9) Card.Count++;
            }

        }
        private void CardControl_MouseEnter(object sender, EventArgs e)
        {
            dc.RefreshTDChance(Card.Count);
            if (dc.tooltip != null) dc.ShowTooltip(Card);
        }

    }
}
