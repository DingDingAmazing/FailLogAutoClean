using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FailLogAutoClean
{
    public partial class Service1 : ServiceBase
    {
        System.Timers.Timer timer1;
        public Service1()
        {
            InitializeComponent();
            if(!System.Diagnostics.EventLog.SourceExists("2.0FailLogClean"))
            {
                System.Diagnostics.EventLog.CreateEventSource("2.0FailLogClean", "DailyCleanLog");
            }
            eventLog1.Source = "2.0FailLogClean";
            eventLog1.Log = "DailyCleanLog";
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("Sevice has started!");
            timer1 = new System.Timers.Timer();
            timer1.Interval = 60*60*30;//30minutes to elapse
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Elapsed);
            timer1.AutoReset = true;
            timer1.Enabled = true;
            eventLog1.WriteEntry("Real-time-clean has been running!");

            writestr("Service starts!");
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("Sevice has stoped");
            timer1.Enabled = false;

            writestr("Service stops!");
        }

        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            eventLog1.WriteEntry("Start real-time clean");
            writestr("Autoclean 04 Script Execute Report");
            try
            {
                Path01Clean();
                Path02Clean();
            }
            catch(Exception err)
            {
                writestr(err.Message);
            }
            writestr_01("\r\n");
        }

        public void Path01Clean()
        {
            try
            {
                //folder 01 path
                string strFolderPath01 = @"D:\IT&V Automation Test\ICI 2.0 Script\02 K226\04 Script Execute Report\Fail Image Folder";
                DirectoryInfo dyInfo01 = new DirectoryInfo(strFolderPath01);
                //get all files in the path
                foreach (FileInfo feInfo01 in dyInfo01.GetFiles())
                {
                    //delete all files 2 days before today
                    if(feInfo01.CreationTime.Year < DateTime.Today.Year)
                    {
                        feInfo01.Delete();
                        writestr_01("Delete" + feInfo01.Name + feInfo01.Extension);
                    }
                    else
                    {
                        if (feInfo01.CreationTime.DayOfYear < DateTime.Today.DayOfYear - 2)
                        {
                            feInfo01.Delete();
                            writestr_01("Delete" + feInfo01.Name + feInfo01.Extension);
                        } 
                    }
                }
             }
            catch(Exception err)
            {
                writestr(err.Message);
            }
        }

        public void Path02Clean()
        {
            try
            {
                //folder 02 path
                string strFolderPath02 = @"D:\IT&V Automation Test\ICI 2.0 Script\02 K226\04 Script Execute Report";
                DirectoryInfo dyInfo02 = new DirectoryInfo(strFolderPath02);
                foreach (FileInfo feInfo02 in dyInfo02.GetFiles())
                {
                    if (feInfo02.CreationTime.Year < DateTime.Today.Year)
                    {
                        feInfo02.Delete();
                        writestr_01("Delete" + feInfo02.Name + feInfo02.Extension);
                    }
                    else
                    {
                        if (feInfo02.CreationTime.DayOfYear < DateTime.Today.DayOfYear - 2)
                        {
                            feInfo02.Delete();
                            writestr_01("Delete" + feInfo02.Name + feInfo02.Extension);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                writestr(err.Message);
            }
        }

        public void writestr(string readme)
        {
            StreamWriter dout = new StreamWriter(@"D:/CustomService/" + System.DateTime.Today.ToString("yyyyMMdd") + ".txt", true);
            dout.Write("\r\n" + System.DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") + "    " + readme);
            dout.Close();
        }

        public void writestr_01(string readme)
        {
            StreamWriter dout = new StreamWriter(@"D:/CustomService/" + System.DateTime.Today.ToString("yyyyMMdd") + ".txt", true);
            dout.Write("\r\n" + readme);
            dout.Close();
        }
    }
}
