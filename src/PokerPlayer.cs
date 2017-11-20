using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Nancy.Simple
{
	public static class PokerPlayer
	{
		public static readonly string VERSION = "Beat yo' ass #2.1.5";
        private static bool weRaised;
        private static int commCount = 0;

		public static int BetRequest(JObject gameState)
		{
            try
            {

                int playerId = gameState["in_action"].ToObject<int>();
                List<JToken> commCards = gameState["community_cards"].Values().ToList();

                int currentBuyIn = gameState["current_buy_in"].ToObject<int>();
                int minRaise = gameState["minimum_raise"].ToObject<int>();
                
                var firstCardRank = gameState.SelectToken("players[" + playerId + "].hole_cards[0].rank").ToObject<string>();
                var secondCardRank = gameState.SelectToken("players[" + playerId + "].hole_cards[1].rank").ToObject<string>();
                var firstCardSuit = gameState.SelectToken("players[" + playerId + "].hole_cards[0].suit").ToObject<string>();
                var secondCardSuit = gameState.SelectToken("players[" + playerId + "].hole_cards[1].suit").ToObject<string>();
                
                if (commCount != commCards.Count)
                {
                    commCount = commCards.Count;
                    weRaised = false;
                }

                if(currentBuyIn > 0 && weRaised)
                {
                    return currentBuyIn - gameState.SelectToken("players[" + playerId + "].bet").ToObject<int>();
                }

                if (firstCardRank.Equals(secondCardRank))
                {
                    weRaised = true;
                    return currentBuyIn - gameState.SelectToken("players[" + playerId + "].bet").ToObject<int>() + minRaise;
                }

                if (firstCardSuit.Equals(secondCardSuit))
                {
                    weRaised = true;
                    return currentBuyIn - gameState.SelectToken("players[" + playerId + "].bet").ToObject<int>() + minRaise;
                }

                string[] highCards = new string[4] { "J", "K", "Q", "A" };
                if (highCards.Contains(firstCardRank) || highCards.Contains(secondCardRank))
                {
                    weRaised = true;
                    return currentBuyIn - gameState.SelectToken("players[" + playerId + "].bet").ToObject<int>() + minRaise;
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

