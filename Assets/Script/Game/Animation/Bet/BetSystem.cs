using System.Collections.Generic;
using UnityEngine;

using HanafudaPoker.Network;
using HanafudaPoker.UIs;
using HanafudaPoker.Games;

// ho6:betSystem‚ÍAƒlƒbƒgƒ[ƒN‚©‚çæ“¾‚µ‚½Œ»İ‚Ì‚¿‹à‚É‰‚¶‚½•ª‚Ì‹àŠz‚ğ¶¬
namespace HanafudaPoker.Bet
{
    public static class BetSystem
    {
        // task:Šebetƒ^[ƒ“‚ÅA“q‚¯‚ê‚éÅ‘å‹àŠz‚ğ§ŒÀ‚·‚é
        public static bool TryBet(int betAmount)
        {
            // Œ»İ‚Ì‚¿‹à
            int money = NetworkManager.GetMoney();    // ‚±‚ê‚ÍŠ‹à‚ğæ“¾‚·‚éŠÖ”‚ª—~‚µ‚¢
            int allBetValue = NetworkManager.GetBet(); 

            // BetŠz‚ªğŒ‚ğ–‚½‚µ‚Ä‚È‚¢(•s³Eƒ}ƒCƒiƒX‚É‚È‚é)
            if (betAmount + allBetValue > 100 /*‚±‚Ì100‚ÍGameConst‚ÅŒˆ‚ß‚ç‚ê‚Ä‚é*/|| betAmount > money)
            {
                return false;
            }

            // BetŠz‚ğİ’è
            NetworkManager.SetBet(betAmount);       // Š|‚¯‚½‹àŠz
            NetworkManager.AddBet(betAmount);       // ‚±‚ê‚Ü‚ÅŠ|‚¯‚½‹àŠz
            NetworkManager.AddMoney(-betAmount);    // Š‹à

            return true;
        }

    }
}