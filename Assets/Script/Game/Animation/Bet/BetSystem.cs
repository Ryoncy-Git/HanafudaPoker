using System.Collections.Generic;
using UnityEngine;

using HanafudaPoker.Network;
using HanafudaPoker.UIs;
using HanafudaPoker.Games;

// ho6:betSystemは、ネットワークから取得した現在の持ち金に応じた分の金額を生成
namespace HanafudaPoker.Bet
{
    public static class BetSystem
    {
        private static void Start()
        {
            // NetworkManager.SetBet(100);
        }

        public static bool TryBet(int betAmount)
        {
            // 現在の持ち金
            int money = NetworkManager.GetBet();    // これは所持金を取得する関数が欲しい

            // Bet額が条件を満たしてない(不正・マイナスになる)
            if (betAmount <= 0 || betAmount > money)
            {
                return false;
            }

            // Bet額を設定
            NetworkManager.SetBet(betAmount);

            money = money - betAmount;

            return true;
        }

    }
}