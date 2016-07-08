using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Diagnostics;
using System.Threading;

namespace ProcessStartEventTest
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>

        public static System.Management.ManagementEventWatcher mgmtWtch;
        public static System.Management.ManagementEventWatcher stopWatch;

        static void Main(string[] args)
        {
            // Display the number of command line arguments:
            System.Console.WriteLine(args.Length);

            mgmtWtch = new System.Management.ManagementEventWatcher("Select * From Win32_ProcessStartTrace");
            mgmtWtch.EventArrived += new System.Management.EventArrivedEventHandler(mgmtWtch_EventArrived);
            mgmtWtch.Start();


            stopWatch = new System.Management.ManagementEventWatcher(" Select * From Win32_ProcessStopTrace");
            stopWatch.EventArrived += new System.Management.EventArrivedEventHandler(mgmtWtch_EventArrived);
            stopWatch.Start();

           var  t = new Thread(() => ApiFetcher.getData(1, 1));
            t.Start();
            Process.GetCurrentProcess().WaitForExit();

        }

        static void mgmtWtch_EventArrived(object sender, System.Management.EventArrivedEventArgs e)
        {

            PropertyData processName = e.NewEvent.Properties["ProcessName"];
            string test = processName.Value.ToString();
            Console.WriteLine(test);
            if (test.Contains("League of Lege"))
            {
                if (e.NewEvent.ClassPath.ToString().Equals("Win32_ProcessStartTrace"))
                {
                    Console.WriteLine("League of Legends launched");
                    ApiFetcher.getData(0, 3);
                }
                else
                {
                    Console.WriteLine("League of Legends stopped");
                    FileWriter.WriteToFile("Not Ingame");
                }
            }
        }

        private static void startjar()
        {
            string jar = @"E:\philipp\Programmieren\GetQueue\build\libs\GetQueue-1.0-SNAPSHOT.jar";
            string args = "start";

            System.Diagnostics.Process clientProcess = new Process();
            clientProcess.StartInfo.FileName = "java";
            clientProcess.StartInfo.Arguments = @"-jar " + jar + " " + args;
            clientProcess.StartInfo.RedirectStandardOutput = true;
            clientProcess.StartInfo.RedirectStandardError = true;
            clientProcess.StartInfo.UseShellExecute = false;
            clientProcess.StartInfo.CreateNoWindow = true;
            clientProcess.Start();
            clientProcess.BeginOutputReadLine();
            clientProcess.BeginErrorReadLine();
            clientProcess.OutputDataReceived += new DataReceivedEventHandler((s, es) =>
            {
                Console.WriteLine(es.Data);
            });
            clientProcess.ErrorDataReceived += new DataReceivedEventHandler((s, es) => { Console.WriteLine(es.Data); });
        }

    }
}

