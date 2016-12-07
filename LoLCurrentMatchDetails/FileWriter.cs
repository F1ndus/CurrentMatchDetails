using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LoLCurrentMatchDetails
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
            string message = String.Format(@"{0} {1}{2}({3}LP) {4}W / {5}L",leaguename,division,Environment.NewLine,points,wins,losses);
            writer.Write(message);
            writer.Close();
        }

        
    }
}
