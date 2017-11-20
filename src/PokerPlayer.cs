﻿using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections;
using System;

namespace Nancy.Simple
{
	public static class PokerPlayer
	{
		public static readonly string VERSION = "Beat yo' ass #1.2";

		public static int BetRequest(JObject gameState)
		{
            try
            {

                int playerId = gameState.GetValue("in_action").Value<int>();
                var players = gameState.GetValue("players").Values().ToList();

                int currentBuyIn = gameState.GetValue("current_buy_in").Value<int>();
                int minRaise = gameState.GetValue("minumum_raise").Value<int>();

                JObject ourPlayer = players[playerId].Value<JObject>();

                var ourHand = ourPlayer.GetValue("hole_cards").Values().ToList();
                var firstCardRank = ourHand[0].Value<JObject>().GetValue("rank").Value<string>();
                var secondCardRank = ourHand[1].Value<JObject>().GetValue("rank").Value<string>();

                if (firstCardRank.Equals(secondCardRank))
                {
                    return currentBuyIn - ourPlayer.GetValue("bet").Value<int>() + minRaise;
                }

                string[] highCards = new string[4] { "J", "K", "Q", "A" };
                if (highCards.Contains(firstCardRank) || highCards.Contains(secondCardRank))
                {
                    return currentBuyIn - ourPlayer.GetValue("bet").Value<int>() + minRaise;
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

