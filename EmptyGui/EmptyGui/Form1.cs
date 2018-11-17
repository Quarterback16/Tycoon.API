using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using EmptyGui;

namespace GerardGui
{
   public partial class Form1 : Form
   {
      public string LastMessage { get; set; }

      public Form1()
      {
         InitializeComponent();
         backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
         backgroundWorker1.DoWork += backgroundWorker1_DoWork;
         var versionInfo = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
         var startDate = DateTime.Now;
         var diffDays = versionInfo.Build;
         var computedDate = startDate.AddDays( diffDays );

         //TODO:  Give the app a Name
         Text = string.Format( "<insert GUI name here> {0} g built {1}", versionInfo.ToString(), computedDate.ToLongDateString() );
         PreFlightTests();
      }

      private void PreFlightTests()
      {
         InsertMessage( "Pre Flight test complete." );
      }

      private void CheckSetting( string setting )
      {
         var theConfigSetting = ConfigurationManager.AppSettings.Get( setting );
         InsertMessage( ! string.IsNullOrEmpty( theConfigSetting )
                           ? string.Format( "{0} set to {1}", setting, theConfigSetting )
                           : string.Format( "{0} does not exist", setting ) );
      }

      private void CheckDirExists(string setting, string dir)
      {
         InsertMessage(System.IO.Directory.Exists(dir)
                           ? string.Format("{0} set to {1}", setting, dir)
                           : string.Format("{0} does not exist", dir));
      }

      private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
      {
         try
         {
            var helperBW = sender as BackgroundWorker;

            //TODO:  instantiate the real worker and tell it to start
            var realWorker = new FakeWorker();

            var gw = new GuiWorker( Text, realWorker )
               {
                  Pollinterval = Int32.Parse(ConfigurationManager.AppSettings["PollInterval"]),
                  Verbose = ConfigurationManager.AppSettings["Verbose"] == "true",
               };

            gw.Go( helperBW, e );
         }
         catch (Exception ex)
         {
            textBox1.Text += ex.StackTrace;
            throw;
         }
      }

      private void button1_Click(object sender, EventArgs e)
      {
         try
         {
            label1.Text = string.Empty;
            startDateLabel.Text += DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();

            if (backgroundWorker1.IsBusy != true)
            {
               backgroundWorker1.RunWorkerAsync();
            }
         }
         catch (Exception ex)
         {
            textBox1.Text += ex.StackTrace;
            throw;
         }
      }

      /// <summary>
      ///   The progress report message
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
      {
         var msg = e.UserState.ToString();
         if (msg == LastMessage) return;

         label1.Text = msg;
         if (e.ProgressPercentage.Equals(99))
            InsertMessage(msg); //  into the textarea as well

         LastMessage = msg;
      }

      private void InsertMessage(string msg)
      {
         textBox1.Text = string.Format( "{0} {1}", DateTime.Now.ToString("hh:mm"), msg ) + Environment.NewLine + textBox1.Text;
      }

      private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
         if (e.Cancelled)
         {
            label1.Text = "The task has been cancelled";
         }
         else if (e.Error != null)
         {
            label1.Text = "Error. Details: " + (e.Error as Exception).ToString();
         }
         else
         {
            label1.Text = "The task has been completed. Results: " + e.Result.ToString();
         }
      }
   }
}