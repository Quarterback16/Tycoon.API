using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TrafficSniffer
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        //[STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm AppForm = new MainForm();
            System.Windows.Forms.Application.Run(AppForm);
        }


    }
}
