﻿using RiotSharp;
using RiotSharp.CurrentGameEndpoint.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using LoLCurrentMatchDetails.Properties;

namespace LoLCurrentMatchDetails
{

    using RiotSharp;
    using System.IO;
    class ApiFetcher
    {
        // static int CURRENT_SUMMONER = LoLCurrentMatchDetails.Properties.Settings.Default.SummonerID;
        public static long CURRENT_SUMMONER = 0;
        static string api_key = null;
        static ApiEvents eventtrigger;
        public static List<long> favlist = new List<long>();
        // The folder for the roaming current user 


        static string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        static string appFolder = Path.Combine(folder, "LoLCurrentMatchDetails");
        public static RiotApi api = null;

        public static void registerEventHandler(ApiEvents eventhandler)
        {
            eventtrigger = eventhandler;
        }

      
        static ApiFetcher() {
            // Check if folder exists and if not, create it
            if (!Directory.Exists(appFolder))
            {
                Directory.CreateDirectory(appFolder);
                File.Create(appFolder + "/settings.properties");
            }

            string[] settings  = File.ReadAllLines(appFolder + "/settings.properties");
            Array.ForEach<string>(settings, str =>
            {
                string[] kv = str.Split('=');
                if (kv[0].Equals("key"))
                    api_key = kv[1];
                else if (kv[0].Equals("summonerid"))
                    CURRENT_SUMMONER = Convert.ToInt32(kv[1]);
                else if(kv[0].Contains("fav"))
                {
                    favlist.Add(Convert.ToInt32(kv[1]));
                }
            });
              api = RiotSharp.RiotApi.GetInstance(api_key);
        }

        

        public static int getData()
        {
            eventtrigger.onStartedUpdate();
            Properties.Settings.Default.Save();
            RiotSharp.CurrentGameEndpoint.CurrentGame gameinfo;
            string retval = "error";
            try
            {
                gameinfo = api.GetCurrentGame(Platform.EUW1, CURRENT_SUMMONER);
                GameQueueType qType = gameinfo.GameQueueType;

                if (isTeamRanked(qType))
                {
                    Console.WriteLine("Teamranked");
                    GameFetcher fetcher = new TeamRankedFetcher();
                   retval = fetcher.getQueueData(gameinfo, CURRENT_SUMMONER);

                }
                else if (isSoloQueue(qType))
                {
                    Console.WriteLine("SoloQueue");
                    GameFetcher fetcher = new SoloQueueFetcher();
                   retval = fetcher.getQueueData(gameinfo, CURRENT_SUMMONER);

                }
                else
                {
                    Console.WriteLine("Normal or Custom");
                    GameFetcher fetcher = new NormalCustomFetcher();
                    retval = fetcher.getQueueData(gameinfo, CURRENT_SUMMONER);
                }
                eventtrigger.onFinishedUpdate(retval);
                return 0;
            } catch(RiotSharpException e)
            {
                Console.WriteLine(e);
                FileWriter.WriteToFile("Not Ingame");
                eventtrigger.onFinishedUpdate("Not Ingame");
                return -1;
            } catch(JsonSerializationException e)
            {
                Console.WriteLine(e);
                eventtrigger.onFinishedUpdate("Unknown Gamemode");
                FileWriter.WriteToFile("Unknown Gamemode");
                return -2;
            }            
        }

        public static long getSummonerID(string name)
        {
            return ApiFetcher.api.GetSummoner(Region.euw, name).Id;
        }

        public static string getSummonerName(long id)
        {
            return ApiFetcher.api.GetSummonerName(Region.euw, id).Name;
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
                
            }
            
        }

        #region - Apifetcher methods

        private static bool isSoloQueue(GameQueueType type)
        {
            return
                type == GameQueueType.RankedSolo5x5 ||
                type == GameQueueType.RankedPremade5x5 ||
                type == GameQueueType.RankedFlexSR ||
                type == GameQueueType.RankedFlexTT ||
                type == GameQueueType.TeamBuilderDraftRanked5x5 ||
                type == GameQueueType.TeamBuilderRankedSolo;
        }

        private static bool isTeamRanked(GameQueueType type)
        {
            return type ==GameQueueType.RankedTeam5x5 || type ==
                    GameQueueType.RankedTeam3x3;

        }

        #endregion
    }
}
