using RiotSharp;
using RiotSharp.CurrentGameEndpoint.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ProcessStartEventTest
{

    using RiotSharp;

    class ApiFetcher
    {
        static int FOZRUK = 20377190;
        static int BAKER = 73687139;
        static int SMURF = 59807066;
        static int WOO = 58717498;


        static int CURRENT_SUMMONER = FOZRUK;
        public static RiotApi  api = RiotSharp.RiotApi.GetInstance("4091633c-fe9a-4549-85ed-8fab5089b6a8");

        public static int getData()
        {           
            RiotSharp.CurrentGameEndpoint.CurrentGame gameinfo;
            try
            {
                gameinfo = api.GetCurrentGame(Platform.EUW1, CURRENT_SUMMONER);
                GameQueueType qType = gameinfo.GameQueueType;

                if (isTeamRanked(qType))
                {
                    Console.WriteLine("Teamranked");
                    GameFetcher fetcher = new TeamRankedFetcher();
                    fetcher.getQueueData(gameinfo, CURRENT_SUMMONER);

                }
                else if (isSoloQueue(qType))
                {
                    Console.WriteLine("SoloQueue");
                    GameFetcher fetcher = new SoloQueueFetcher();
                    fetcher.getQueueData(gameinfo, CURRENT_SUMMONER);

                }
                else
                {
                    Console.WriteLine("Normal or Custom");
                    GameFetcher fetcher = new NormalCustomFetcher();
                    fetcher.getQueueData(gameinfo, CURRENT_SUMMONER);
                }
                return 0;
            } catch(RiotSharpException e)
            {
                Console.WriteLine(e);
                return -1;
            }              
        }

        public static void getData(int currenttry,int maxtries)
        {
           
            if(!(currenttry > maxtries))
            {
                Console.WriteLine("Fetch Data from api (Try {0} from {1})", currenttry, maxtries);
                FileWriter.WriteToFile("Loading...");
                if (getData() != 0)
                {
                    Console.WriteLine("Error");
                    Thread.Sleep(5000);
                    getData(++currenttry, maxtries);
                }   
            } else
            {
                Console.WriteLine("Max tries used, abort");
                FileWriter.WriteToFile("Not Ingame");
            }
            
        }

        #region - Apifetcher methods

        private static bool isSoloQueue(GameQueueType type)
        {
            return
                type == GameQueueType.RankedSolo5x5 || 
                type == GameQueueType.RankedPremade5x5 ||
                type == GameQueueType.TeamBuilderDraftRanked;
        }

        private static bool isTeamRanked(GameQueueType type)
        {
            return type ==GameQueueType.RankedTeam5x5 || type ==
                    GameQueueType.RankedTeam3x3;

        }

        #endregion
    }
}
