using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProcessStartEventTest
{
    class FileWriter
    {
		public static void WriteToFile(string message)
        {
            StreamWriter writer = new StreamWriter(@"E:\divisionranked");
            writer.Write(message);
            writer.Close();
        }

        public static void WriteToFile(string leaguename, string division, int points, int wins,int losses)
        {
            StreamWriter writer = new StreamWriter(@"E:\divisionranked");
            string message = String.Format(@"{0} {1} [{2}] - {3}/{4}",leaguename,division,points,wins,losses);
            writer.Write(message);
            writer.Close();
        }

        public static void WriteToFile(string teamname,string leaguename, string division, int points, int wins, int losses)
        {
            StreamWriter writer = new StreamWriter(@"E:\divisionranked");
            string message = String.Format(@"{0}\n{1} {2} [{3}] - {4}/{5}", teamname,leaguename, division, points, wins, losses);
            writer.Write(message);
            writer.Close();
        }
    }
}
