// <copyright file="ApiFetcherTest.cs">Copyright ©  2016</copyright>
using System;
using LoLCurrentMatchDetails;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using RiotSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoLCurrentMatchDetails.Tests
{
    /// <summary>Diese Klasse enthält parametrisierte Komponententests für ApiFetcher.</summary>
    [PexClass(typeof(ApiFetcher))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class ApiFetcherTest
    {
        /// <summary>Test-Stub für getData()</summary>
        [TestMethod]
        public void getDataTest()
        {
            Console.WriteLine("test");
            RiotSharp.FeaturedGamesEndpoint.FeaturedGames testgame = ApiFetcher.api.GetFeaturedGames(Region.euw);
            string sname = testgame.GameList[0].Participants[0].SummonerName;
            ApiFetcher.CURRENT_SUMMONER = ApiFetcher.api.GetSummoner(Region.euw, sname).Id;
            //ApiFetcher.CURRENT_SUMMONER = 60077041;
            int result = ApiFetcher.getData();
            ApiFetcher.getData(1, 1);
            Assert.IsTrue(true);
            // TODO: Assertionen zu Methode ApiFetcherTest.getDataTest() hinzufügen
        }
    }
}
