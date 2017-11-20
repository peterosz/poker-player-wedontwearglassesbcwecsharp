using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections;
using System;

namespace Nancy.Simple
{
	public static class PokerPlayer
	{
		public static readonly string VERSION = "Beat yo' ass #1.3";

		public static int BetRequest(JObject gameState)
		{
            try
            {

                int playerId = Int32.Parse(gameState.GetValue("in_action").Value<string>());
                var players = gameState.GetValue("players").Values().ToList();

                int currentBuyIn = Int32.Parse(gameState.GetValue("current_buy_in").Value<string>());
                int minRaise = Int32.Parse(gameState.GetValue("minumum_raise").Value<string>());

                JObject ourPlayer = players[playerId].Value<JObject>();

                var ourHand = ourPlayer.GetValue("hole_cards").Values().ToList();
                var firstCardRank = ourHand[0].Value<JObject>().GetValue("rank").Value<string>();
                var secondCardRank = ourHand[1].Value<JObject>().GetValue("rank").Value<string>();

                if (firstCardRank.Equals(secondCardRank))
                {
                    return currentBuyIn - Int32.Parse(ourPlayer.GetValue("bet").Value<string>()) + minRaise;
                }

                string[] highCards = new string[4] { "J", "K", "Q", "A" };
                if (highCards.Contains(firstCardRank) || highCards.Contains(secondCardRank))
                {
                    return currentBuyIn - Int32.Parse(ourPlayer.GetValue("bet").Value<string>()) + minRaise;
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

