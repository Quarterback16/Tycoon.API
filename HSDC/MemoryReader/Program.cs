using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace NetSniffer
{
    public static class Program
    {
        static byte[] data;
        static Socket sck;
        static List<string> cardNames = null;

        static void Main(string[] args)
        {
            string cards = "CS1_042|CS1_069|CS1_112|CS1_113|CS1_129|CS1_130|CS2_003|CS2_004|CS2_005|CS2_007|CS2_008|CS2_009|CS2_011|CS2_012|CS2_013|CS2_022|CS2_023|CS2_024|CS2_025|CS2_026|CS2_027|CS2_028|CS2_029|CS2_031|CS2_032|CS2_033|CS2_037|CS2_038|CS2_039|CS2_041|CS2_042|CS2_045|CS2_046|CS2_053|CS2_057|CS2_059|CS2_061|CS2_062|CS2_063|CS2_064|CS2_065|CS2_072|CS2_073|CS2_074|CS2_075|CS2_076|CS2_077|CS2_080|CS2_084|CS2_087|CS2_088|CS2_089|CS2_091|CS2_092|CS2_093|CS2_094|CS2_097|CS2_103|CS2_104|CS2_105|CS2_106|CS2_108|CS2_112|CS2_114|CS2_117|CS2_118|CS2_119|CS2_120|CS2_121|CS2_122|CS2_124|CS2_125|CS2_127|CS2_131|CS2_141|CS2_142|CS2_146|CS2_147|CS2_150|CS2_151|CS2_155|CS2_161|CS2_162|CS2_168|CS2_169|CS2_171|CS2_172|CS2_173|CS2_179|CS2_181|CS2_182|CS2_186|CS2_187|CS2_188|CS2_189|CS2_196|CS2_197|CS2_200|CS2_201|CS2_203|CS2_213|CS2_221|CS2_222|CS2_226|CS2_227|CS2_231|CS2_232|CS2_233|CS2_234|CS2_235|CS2_236|CS2_237|DS1_055|DS1_070|DS1_175|DS1_178|DS1_183|DS1_184|DS1_185|DS1_188|DS1_233|EX1_001|EX1_002|EX1_004|EX1_005|EX1_006|EX1_007|EX1_008|EX1_009|EX1_010|EX1_011|EX1_012|EX1_014|EX1_015|EX1_016|EX1_017|EX1_019|EX1_020|EX1_021|EX1_023|EX1_025|EX1_028|EX1_029|EX1_032|EX1_033|EX1_043|EX1_044|EX1_045|EX1_046|EX1_048|EX1_049|EX1_050|EX1_055|EX1_057|EX1_058|EX1_059|EX1_062|EX1_066|EX1_067|EX1_076|EX1_080|EX1_082|EX1_083|EX1_084|EX1_085|EX1_089|EX1_091|EX1_093|EX1_095|EX1_096|EX1_097|EX1_100|EX1_102|EX1_103|EX1_105|EX1_110|EX1_112|EX1_116|EX1_124|EX1_126|EX1_128|EX1_129|EX1_130|EX1_131|EX1_132|EX1_133|EX1_134|EX1_136|EX1_137|EX1_144|EX1_145|EX1_154|EX1_155|EX1_158|EX1_160|EX1_161|EX1_162|EX1_164|EX1_165|EX1_166|EX1_169|EX1_170|EX1_173|EX1_178|EX1_238|EX1_241|EX1_243|EX1_244|EX1_245|EX1_246|EX1_247|EX1_248|EX1_249|EX1_250|EX1_251|EX1_258|EX1_259|EX1_274|EX1_275|EX1_277|EX1_278|EX1_279|EX1_283|EX1_284|EX1_287|EX1_289|EX1_294|EX1_295|EX1_298|EX1_301|EX1_302|EX1_303|EX1_304|EX1_306|EX1_308|EX1_309|EX1_310|EX1_312|EX1_313|EX1_315|EX1_316|EX1_317|EX1_319|EX1_320|EX1_323|EX1_332|EX1_334|EX1_335|EX1_339|EX1_341|EX1_345|EX1_349|EX1_350|EX1_354|EX1_355|EX1_360|EX1_362|EX1_363|EX1_365|EX1_366|EX1_371|EX1_379|EX1_382|EX1_383|EX1_384|EX1_390|EX1_391|EX1_392|EX1_393|EX1_396|EX1_398|EX1_399|EX1_400|EX1_402|EX1_405|EX1_407|EX1_408|EX1_409|EX1_410|EX1_411|EX1_412|EX1_414|EX1_506|EX1_507|EX1_508|EX1_509|EX1_522|EX1_531|EX1_533|EX1_534|EX1_536|EX1_537|EX1_538|EX1_539|EX1_543|EX1_544|EX1_549|EX1_554|EX1_556|EX1_557|EX1_558|EX1_559|EX1_560|EX1_561|EX1_562|EX1_563|EX1_564|EX1_565|EX1_567|EX1_570|EX1_571|EX1_572|EX1_573|EX1_575|EX1_577|EX1_578|EX1_581|EX1_582|EX1_583|EX1_584|EX1_586|EX1_587|EX1_590|EX1_591|EX1_593|EX1_594|EX1_595|EX1_596|EX1_597|EX1_603|EX1_604|EX1_606|EX1_607|EX1_608|EX1_609|EX1_610|EX1_611|EX1_612|EX1_613|EX1_614|EX1_616|EX1_617|EX1_619|EX1_620|EX1_621|EX1_622|EX1_623|EX1_624|EX1_625|EX1_626|NEW1_003|NEW1_004|NEW1_005|NEW1_007|NEW1_008|NEW1_010|NEW1_011|NEW1_012|NEW1_014|NEW1_016|NEW1_017|NEW1_018|NEW1_019|NEW1_020|NEW1_021|NEW1_022|NEW1_023|NEW1_024|NEW1_025|NEW1_026|NEW1_027|NEW1_029|NEW1_030|NEW1_031|NEW1_036|NEW1_037|NEW1_038|NEW1_040|NEW1_041|PRO_001|tt_004|tt_010";
            cardNames = new List<string>();
            cardNames.AddRange(cards.Split('|'));

            // we are only listening to IPv4 interfaces
            var IPv4Addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(al => al.AddressFamily == AddressFamily.InterNetwork).AsEnumerable();

            // start a sniffer for each interface
            foreach (IPAddress ip in IPv4Addresses)
            {
                if (ip.ToString() == "192.168.1.100") Sniff(ip);
            }
  
            // wait until a key is pressed
            Console.Read();
        }

        static void Sniff(IPAddress ip)
        {
            // setup the socket to listen on, we are listening just to IPv4 IPAddresses
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
            sck.Bind(new IPEndPoint(ip, 0));
            sck.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);
            sck.IOControl(IOControlCode.ReceiveAll, new byte[4] { 1, 0, 0, 0 }, null);
  
            byte[] data = new byte[4096];
            sck.BeginReceive(data, 0, data.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
        }

        private static void OnReceive(IAsyncResult ar)
        {
            int nReceived = sck.EndReceive(ar);
            IPHeader ipHeader = new IPHeader(data, nReceived);
            if (ipHeader.ProtocolType == Protocol.TCP)
            {
                TCPHeader tcpHeader = new TCPHeader(ipHeader.Data, ipHeader.MessageLength);
                
                //Console.WriteLine(tcpHeader.SourcePort + "->" + tcpHeader.DestinationPort);

                if (Convert.ToInt32(tcpHeader.SourcePort) == 1119 || Convert.ToInt32(tcpHeader.DestinationPort) == 1119)
                //if ( Convert.ToInt32(tcpHeader.SourcePort) > 80)
                {
                    if (tcpHeader.MessageLength > 0)
                    {
                        byte[] payload = new byte[tcpHeader.MessageLength];
                        Buffer.BlockCopy(tcpHeader.Data, 0, payload, 0, tcpHeader.MessageLength);

                        Console.WriteLine("Found Hearthstone traffic! IP: {0}, TCP: {1}", ipHeader.MessageLength, tcpHeader.MessageLength);
                        Console.WriteLine("ASC: " + System.Text.Encoding.ASCII.GetString(payload));
                        Console.WriteLine("UNI: " + System.Text.Encoding.Unicode.GetString(payload));
                        Console.WriteLine("HEX: " + BitConverter.ToString(payload));

                        string pay = System.Text.Encoding.ASCII.GetString(payload);

                        foreach (string name in cardNames)
                        {
                            if (pay.Contains(name))
                            {
                                Console.WriteLine("Found card: " + name + "! Data: " + pay);
                            }
                        }

                    }
                }
            }

            /*
            int size = (data[2] * 256) + data[3];
            int porto = (data[20] * 256) + data[21];
            int porti = (data[22] * 256) + data[23];
            //Console.WriteLine("IP Version: {0}, Packet Size: {1} bytes, Id: {2}\n", (data[0] >> 4), size, (data[4] * 256) + data[5]);
            Console.WriteLine("Source: {0}.{1}.{2}.{3}, Destination: {4}.{5}.{6}.{7}, Size: {8}", data[12], data[13], data[14], data[15], data[16], data[17], data[18], data[19], size);

            byte[] payload = new byte[size];
            Buffer.BlockCopy(data, 40, payload, 0, size);
            string pay = System.Text.Encoding.ASCII.GetString(payload);

            if (porti == 3724 || porto == 3724)
            {
                Console.WriteLine("Found Hearthstone traffic!" + size);
                foreach (string name in cardNames)
                {
                    //Console.WriteLine(pay);
                    if (pay.Contains(name))
                    {
                        Console.WriteLine("Found card: " + name + "! Data: " + pay);
                    }
                }
            }
            */

            data = new byte[4096]; 
            sck.BeginReceive(data, 0, data.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
        }
  

    }

    public enum Protocol
    {
        TCP = 6,
        UDP = 17,
        Unknown = -1
    };

}
