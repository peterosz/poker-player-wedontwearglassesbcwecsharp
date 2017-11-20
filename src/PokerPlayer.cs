using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Nancy.Simple
{
	public static class PokerPlayer
	{
		public static readonly string VERSION = "Beat yo' ass #1.9.8";

		public static int BetRequest(JObject gameState)
		{
            try
            {

                int playerId = gameState["in_action"].ToObject<int>();
                Console.Error.WriteLine("playerId ok");
                List<JToken> players = gameState["players"].Values().ToList();
                Console.Error.WriteLine("players ok");

                int currentBuyIn = gameState["current_buy_in"].ToObject<int>();
                Console.Error.WriteLine("currentBuyIn ok");
                int minRaise = gameState["minimum_raise"].ToObject<int>();
                Console.Error.WriteLine("minRaise ok");

                JToken ourPlayer = players[playerId];
                Console.Error.WriteLine("ourPlayer ok");

                var ourHand = ourPlayer.SelectToken("hole_cards");
                Console.Error.WriteLine("ourHand ok");
                var firstCardRank = ourPlayer.SelectToken("hole_cards[0].rank").ToObject<string>();
                Console.Error.WriteLine("firstcard ok");
                var secondCardRank = ourPlayer.SelectToken("hole_cards[1].rank").ToObject<string>();
                Console.Error.WriteLine("secondcard ok");

                Console.Error.WriteLine(firstCardRank);
                Console.Error.WriteLine(secondCardRank);

                if (firstCardRank.Equals(secondCardRank))
                {
                    return currentBuyIn - ourPlayer["bet"].ToObject<int>() + minRaise;
                }

                string[] highCards = new string[4] { "J", "K", "Q", "A" };
                if (highCards.Contains(firstCardRank) || highCards.Contains(secondCardRank))
                {
                    return currentBuyIn - ourPlayer["bet"].ToObject<int>() + minRaise;
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

