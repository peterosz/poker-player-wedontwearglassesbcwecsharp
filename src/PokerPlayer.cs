using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace Nancy.Simple
{
	public static class PokerPlayer
	{
		public static readonly string VERSION = "Beat yo' ass #3.0";
        private static bool weRaised;
        private static int commCount = 0;

		public static int BetRequest(JObject gameState)
		{
            try
            {
                int playerId = gameState["in_action"].ToObject<int>();
                List<JToken> commCards = gameState["community_cards"].Values().Where(s => s.Type.Equals(JTokenType.Object)).ToList();
                List<JToken> handCards = gameState.SelectToken("players[" + playerId + "].hole_cards").Values().Where(s => s.Type.Equals(JTokenType.Object)).ToList();
                List<JToken> allCards = new List<JToken>(commCards);
                allCards.AddRange(handCards);
                string hand = JsonConvert.SerializeObject(handCards);
                string all = JsonConvert.SerializeObject(allCards);
                string urlbase = "http://rainman.leanpoker.org/rank?cards=";

                string urlhand = urlbase + hand;
                JObject respHand = null;
                HttpWebRequest requestHand = (HttpWebRequest)WebRequest.Create(urlhand);
                using (HttpWebResponse responseHand = (HttpWebResponse)requestHand.GetResponse())
                using (Stream resStreamHand = responseHand.GetResponseStream())
                using (StreamReader readerHand = new StreamReader(resStreamHand))
                    respHand = JObject.Parse(readerHand.ReadToEnd());

                string urlall = urlbase + all;
                JObject respAll = null;
                HttpWebRequest requestAll = (HttpWebRequest)WebRequest.Create(urlhand);
                using (HttpWebResponse responseAll = (HttpWebResponse)requestAll.GetResponse())
                using (Stream resStreamAll = responseAll.GetResponseStream())
                using (StreamReader readerAll = new StreamReader(resStreamAll))
                    respAll = JObject.Parse(readerAll.ReadToEnd());

                int currentBuyIn = gameState["current_buy_in"].ToObject<int>();
                int currentBet = gameState.SelectToken("players[" + playerId + "].bet").ToObject<int>();
                int minRaise = gameState["minimum_raise"].ToObject<int>();
                int handRank = respHand["rank"].ToObject<int>();
                int allRank = respAll["rank"].ToObject<int>();
                
                var firstCardRank = gameState.SelectToken("players[" + playerId + "].hole_cards[0].rank").ToObject<string>();
                var secondCardRank = gameState.SelectToken("players[" + playerId + "].hole_cards[1].rank").ToObject<string>();
                var firstCardSuit = gameState.SelectToken("players[" + playerId + "].hole_cards[0].suit").ToObject<string>();
                var secondCardSuit = gameState.SelectToken("players[" + playerId + "].hole_cards[1].suit").ToObject<string>();
                
                if (commCount != commCards.Count)
                {
                    commCount = commCards.Count;
                    weRaised = false;
                }

                if(allRank >= 4)
                {
                    return gameState.SelectToken("players[" + playerId + "].stack").ToObject<int>();
                }

                if(allRank >= 2)
                {
                    weRaised = true;
                    return currentBuyIn - currentBet + minRaise;
                }else if(currentBuyIn-currentBet != 0)
                {
                    return 0;
                }

                // call if already raised
                if(currentBuyIn > 0 && weRaised)
                {
                    return currentBuyIn - currentBet;
                }

                // raise if same rank/suite high card
                string[] highCards = new string[4] { "J", "K", "Q", "A" };
                if (firstCardRank.Equals(secondCardRank)||
                    firstCardSuit.Equals(secondCardSuit) ||
                    highCards.Contains(firstCardRank) || 
                    highCards.Contains(secondCardRank))
                {
                    weRaised = true;
                    return currentBuyIn - currentBet + minRaise;
                }


            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.StackTrace);
            }
        
            return 0;
		}

		public static void ShowDown(JObject gameState)
		{
			//TODO: Use this method to showdown
		}
	}
}

