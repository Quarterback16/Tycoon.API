using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Timers;


namespace HSDC
{
    public class MemoryReader
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        public struct MEMORY_BASIC_INFORMATION
        {
            public int BaseAddress;
            public int AllocationBase;
            public int AllocationProtect;
            public int RegionSize;
            public int State;
            public int Protect;
            public int lType;
        }
        public struct SYSTEM_INFO
        {
            public ushort processorArchitecture;
            ushort reserved;
            public uint pageSize;
            public IntPtr minimumApplicationAddress;
            public IntPtr maximumApplicationAddress;
            public IntPtr activeProcessorMask;
            public uint numberOfProcessors;
            public uint processorType;
            public uint allocationGranularity;
            public ushort processorLevel;
            public ushort processorRevision;
        }
        const int PROCESS_QUERY_INFORMATION = 0x0400;
        const int MEM_COMMIT = 0x00001000;
        const int PAGE_READWRITE = 0x04;
        const int PROCESS_WM_READ = 0x0010;

        private List<int> CardIdsAlreadyParsed;
        private Timer timer;
        private DeckCounter dc;

        public bool IsStarted;

        public MemoryReader(DeckCounter dc)
        {
            IsStarted = false;
            this.dc = dc;
            timer = null;
        }
        
        public void Start()
        {
            IsStarted = true;
            CardIdsAlreadyParsed = new List<int>();
            if (timer == null)
            {
                timer = new Timer(5000);
                timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            }
            timer.Start();
            timer_Elapsed(null, null);
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Work();
        }
        public void Stop()
        {
            IsStarted = false;
            timer.Stop();
        }

        public void Work()
        {
            List<string> cards = new List<string>();
            foreach (string s in ReadMemory())
            {
                int start = s.IndexOf("cardId=") + 7;
                string image = s.Substring(start, s.IndexOf(' ', start) - start);

                //System.Windows.Forms.MessageBox.Show(s);
                Card c = dc.FindCardByImage(image);
                if (c != null)
                {
                    int id = Convert.ToInt32(s.Substring(4, s.IndexOf(' ') - 4));
                    if (!CardIdsAlreadyParsed.Contains(id))
                    {
                        CardControl cc = dc.FindCardInDeck(c.Name);
                        if (cc != null)
                        {
                            if (cc.Card.Count > 0) cc.Card.Count--;
                            if (cc.Card.Count == 0 && dc.Options.GetAsBool("CardAutoSort")) dc.Sort();
                        }
                        CardIdsAlreadyParsed.Add(id);
                    }
                }
            }
            dc.Refresh();
            dc.RefreshAllCards();
        }

        private List<string> ReadMemory()
        {
            // getting minimum & maximum address

            SYSTEM_INFO sys_info = new SYSTEM_INFO();
            GetSystemInfo(out sys_info);

            IntPtr proc_min_address = sys_info.minimumApplicationAddress;
            IntPtr proc_max_address = sys_info.maximumApplicationAddress;

            // saving the values as long ints so I won't have to do a lot of casts later
            long proc_min_address_l = (long)proc_min_address;
            long proc_max_address_l = (long)proc_max_address;

            Process process = Process.GetProcessesByName("hearthstone")[0];

            // opening the process with desired access level
            IntPtr processHandle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_WM_READ, false, process.Id);

            // this will store any information we get from VirtualQueryEx()
            MEMORY_BASIC_INFORMATION mem_basic_info = new MEMORY_BASIC_INFORMATION();

            int bytesRead = 0;  // number of bytes read with ReadProcessMemory

            List<string> possibleMatches = new List<string>();

            while (proc_min_address_l < proc_max_address_l)
            {
                // 28 = sizeof(MEMORY_BASIC_INFORMATION)
                VirtualQueryEx(processHandle, proc_min_address, out mem_basic_info, 28);

                // if this memory chunk is accessible
                if (mem_basic_info.Protect == PAGE_READWRITE && mem_basic_info.State == MEM_COMMIT)
                {
                    // read everything in the buffer above
                    byte[] buffer = new byte[mem_basic_info.RegionSize];
                    ReadProcessMemory((int)processHandle, mem_basic_info.BaseAddress, buffer, mem_basic_info.RegionSize, ref bytesRead);

                    string s = System.Text.Encoding.ASCII.GetString(buffer);

                    //foreach (Match match in Regex.Matches(s, @"\[id=\d+ cardId=[A-Z0-9_]* zone=[A-Z]* zonePos=\d+\]"))
                    foreach (Match match in Regex.Matches(s, @"\[id=\d+ cardId=[A-Z0-9_]* zone=HAND zonePos=\d+\]"))
                    {
                        possibleMatches.Add(match.ToString());
                    }
                }
                // move to the next memory chunk
                proc_min_address_l += mem_basic_info.RegionSize;
                proc_min_address = new IntPtr(proc_min_address_l);
            }
            return possibleMatches;
        }

    }
}
