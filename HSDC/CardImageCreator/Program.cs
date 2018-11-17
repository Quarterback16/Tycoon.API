using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;
using System.IO;
using System.Net;

namespace CardImageCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            int height = 40;

            List<string> images = new List<string>();

            XmlDocument xml = new XmlDocument();
            xml.Load("cards_en.xml");
            foreach (XmlNode node in xml.SelectNodes("//cards/card"))
            {
                images.Add(node.SelectSingleNode("image").InnerText);
            }
            
            foreach (string image in images)
            {

                // Create buffer
                Image bufferImg = new Bitmap(400, height);
                Graphics g = Graphics.FromImage(bufferImg);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                // Load background image
                if (!File.Exists("images/" + image + ".png"))
                {
                    WebClient wc = new WebClient();
                    wc.DownloadFile("http://wow.zamimg.com/images/hearthstone/cards/enus/original/" + image + ".png", "images/" + image + ".png");
                }
                Bitmap bitmap = new Bitmap("images/" + image + ".png");
                g.DrawImage(bitmap, new Rectangle(100, 0, 400, 40), new Rectangle(54, 135, 195, 20), GraphicsUnit.Pixel);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

                g.FillRectangle(new SolidBrush(Color.FromArgb(48, 48, 48)), new Rectangle(0, 0, 175, height));

                Rectangle gRect;
                LinearGradientBrush b;

                gRect = new Rectangle(174, 0, height * 2, height);
                b = new LinearGradientBrush(gRect, Color.FromArgb(255, 48, 48, 48), Color.FromArgb(0, 0, 0, 0), LinearGradientMode.Horizontal);
                g.FillRectangle(b, gRect);

                gRect = new Rectangle(174, 0, height, height);
                b = new LinearGradientBrush(gRect, Color.FromArgb(255, 48, 48, 48), Color.FromArgb(0, 0, 0, 0), LinearGradientMode.Horizontal);
                g.FillRectangle(b, gRect);

                g.Flush();

                bufferImg.Save("output/" + image + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

                g.Dispose();
                bufferImg.Dispose();
            }
            Console.WriteLine("Done.");
            Console.ReadLine();
        }
    }
}
