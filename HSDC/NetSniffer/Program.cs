using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using System.IO;

namespace NetSniffer
{
    class Program
    {
        static List<Card> Cards;
        static Dictionary<string, string> Heroes;
        static StreamWriter sw;

        static void Main(string[] args)
        {
            sw = new StreamWriter("E:\\data.txt", false);

            Heroes = new Dictionary<string, string>();
            Heroes.Add("HERO_01", "Garrosh");
            Heroes.Add("HERO_02", "Thrall");
            Heroes.Add("HERO_03", "Valeera");
            Heroes.Add("HERO_04", "Uther");
            Heroes.Add("HERO_05", "Rexxar");
            Heroes.Add("HERO_06", "Malfurion");
            Heroes.Add("HERO_07", "Gul'dan");
            Heroes.Add("HERO_08", "Jaina");
            Heroes.Add("HERO_09", "Anduin");

            LoadCards();

            // Retrieve the device list from the local machine
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;
            /*

            if (allDevices.Count == 0)
            {
                Console.WriteLine("No interfaces found! Make sure WinPcap is installed.");
                return;
            }
            // Print the list
            for (int i = 0; i != allDevices.Count; ++i)
            {
                LivePacketDevice device = allDevices[i];
                Console.Write((i + 1) + ". " + device.Name);
                if (device.Description != null)
                    Console.WriteLine(" (" + device.Description + ")");
                else
                    Console.WriteLine(" (No description available)");
            }
            */
            int deviceIndex = 3;
            /*do
            {
                Console.WriteLine("Enter the interface number (1-" + allDevices.Count + "):");
                string deviceIndexString = Console.ReadLine();
                if (!int.TryParse(deviceIndexString, out deviceIndex) ||
                    deviceIndex < 1 || deviceIndex > allDevices.Count)
                {
                    deviceIndex = 0;
                }
            } while (deviceIndex == 0);
            */
            // Take the selected adapter
            PacketDevice selectedDevice = allDevices[deviceIndex - 1];


            // Open the device
            using (PacketCommunicator communicator = selectedDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                //Console.WriteLine("Listening on " + selectedDevice.Description + "...");
                using (BerkeleyPacketFilter filter = communicator.CreateFilter("port 1119 or port 3724"))
                {
                    communicator.SetFilter(filter);
                }
                // start the capture
                communicator.ReceivePackets(0, PacketHandler);
            }

        }

        static public void LoadCards()
        {
            Cards = new List<Card>();
            XmlDocument xml = new XmlDocument();
            xml.Load("cards_en.xml");
            foreach (XmlNode node in xml.SelectNodes("//cards/card"))
            {
                Card c = new Card();
                c.Id = Convert.ToInt32(node.SelectSingleNode("id").InnerText);
                c.Quality = Convert.ToInt32(node.SelectSingleNode("quality").InnerText);
                c.Name = node.SelectSingleNode("name").InnerText;
                c.Cost = Convert.ToInt32(node.SelectSingleNode("cost").InnerText);
                switch (Convert.ToInt32(node.SelectSingleNode("type").InnerText))
                {
                    case 4: c.Type = "minion"; break;
                    case 5: c.Type = "spell"; break;
                    case 7: c.Type = "weapon"; break;
                }
                c.Image = node.SelectSingleNode("image").InnerText;
                Cards.Add(c);
            }
        }

        public Card FindCardByImage(string image)
        {
            return Cards.Find(c => c.Image == image);
        }

        private static string HexAsciiConvert(string hex)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= hex.Length - 2; i += 2)
            {
                sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hex.Substring(i, 2),
                System.Globalization.NumberStyles.HexNumber))));
            }
            return sb.ToString();
        }

        // Callback function invoked by Pcap.Net for every incoming packet
        private static void PacketHandler(Packet packet)
        {

            Datagram payload = packet.Ethernet.IpV4.Payload;

            if (payload.Length == 20 || payload.Length == 31) return;

            sw.WriteLine(payload.Length + "; " + packet.Ethernet.IpV4.Payload.ToHexadecimalString());
            sw.Flush();

            //Console.WriteLine(packet.Ethernet.IpV4.Payload.ToString());
            Console.WriteLine(payload.Length + ": " + packet.Ethernet.IpV4.Payload.ToHexadecimalString());

            if (payload.ToHexadecimalString().StartsWith("07"))
            {
                Console.Write("Recv own hero data: ");
                foreach (string name in Heroes.Keys)
                {
                    //if (payload.Contains(name)) Console.WriteLine(Heroes[name]);
                }
            }
            if (payload.ToHexadecimalString().StartsWith("10"))
            {
                Console.WriteLine("Recv enemy hero data: ");
                foreach (string name in Heroes.Keys)
                {
                    //if (payload.Contains(name)) Console.WriteLine(Heroes[name]);
                }
            }

            // Mulligan choice
            //if (data[0] == 0x03)
            {
            }

            if (payload.ToHexadecimalString().StartsWith("0e"))
            {
                string x = HexAsciiConvert(payload.ToHexadecimalString());
                foreach (Card c in Cards)
                {
                    if (x.Contains(c.Image))
                    {

                        Console.WriteLine("Card drawn or played: " + c.Name + "!");
                    }
                }
            }


            Console.WriteLine("---");

        }

    }
}
