using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using HanafudaPoker.Network;

namespace HanafudaPoker.Bet
{
    static class Settlement
    {
        public static void Execute(int[] score)
        {
            int[] Bets = new int[score.Length];

            for(int seatID = 0; seatID < Bets.Length; seatID++)
            {
                Bets[seatID] = NetworkManager.GetBet(seatID);
            }
            
            int MainPot, SidePot1, SidePot2, ReturnPot;



        }

        private static void SetPot(int[] Bets)
        {
            
        }
    }
}