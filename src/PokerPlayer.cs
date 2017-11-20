using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Nancy.Simple
{
	public static class PokerPlayer
	{
		public static readonly string VERSION = "Beat yo' ass #1.7";

		public static int BetRequest(JObject gameState)
		{
            try
            {

                int playerId = gameState.GetValue("in_action").ToObject<int>();
                Console.Error.WriteLine("playerId ok");
                List<JToken> players = gameState.GetValue("players").Values().ToList();
                Console.Error.WriteLine("players ok");

                int currentBuyIn = gameState.GetValue("current_buy_in").ToObject<int>();
                int minRaise = gameState.GetValue("minumum_raise").ToObject<int>();

                JObject ourPlayer = players[playerId].ToObject<JObject>();

                var ourHand = ourPlayer.GetValue("hole_cards").Values().ToList();
                var firstCardRank = ourHand[0].ToObject<JObject>().GetValue("rank").ToObject<string>();
                var secondCardRank = ourHand[1].ToObject<JObject>().GetValue("rank").ToObject<string>();

                Console.Error.WriteLine(firstCardRank);
                Console.Error.WriteLine(secondCardRank);

                if (firstCardRank.Equals(secondCardRank))
                {
                    return currentBuyIn - ourPlayer.GetValue("bet").ToObject<int>() + minRaise;
                }

                string[] highCards = new string[4] { "J", "K", "Q", "A" };
                if (highCards.Contains(firstCardRank) || highCards.Contains(secondCardRank))
                {
                    return currentBuyIn - ourPlayer.GetValue("bet").ToObject<int>() + minRaise;
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

