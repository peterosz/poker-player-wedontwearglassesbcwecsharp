using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Nancy.Simple
{
	public static class PokerPlayer
	{
		public static readonly string VERSION = "Beat yo' ass #2.1.1";
        private static bool weRaised;

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

                if(currentBuyIn > 0 && weRaised)
                {
                    Console.Error.WriteLine(currentBuyIn - gameState.SelectToken("players[" + playerId + "].bet").ToObject<int>());
                    return currentBuyIn - gameState.SelectToken("players[" + playerId + "].bet").ToObject<int>();
                }

                if (firstCardRank.Equals(secondCardRank))
                {
                    return currentBuyIn - gameState.SelectToken("players[" + playerId + "].bet").ToObject<int>() + minRaise;
                    weRaised = true;
                }

                if (firstCardSuit.Equals(secondCardSuit))
                {
                    return currentBuyIn - gameState.SelectToken("players[" + playerId + "].bet").ToObject<int>() + minRaise;
                    weRaised = true;
                }

                string[] highCards = new string[4] { "J", "K", "Q", "A" };
                if (highCards.Contains(firstCardRank) || highCards.Contains(secondCardRank))
                {
                    return currentBuyIn - gameState.SelectToken("players[" + playerId + "].bet").ToObject<int>() + minRaise;
                    weRaised = true;
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

