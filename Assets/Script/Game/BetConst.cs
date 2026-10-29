using UnityEngine;

namespace HanafudaPoker.Bet
{
    public static class BetConst
    {
        public const float INSURANCE_COST = 0.1f; // 保険にかかる金額比（ベット額に対して）(例；100 Bet -> Insurance 100 * 0.1 = 10)
        public const float INSURANCE_EFFECT = 0.5f; // 減額を0.5倍にする
        public const int FIRST_BET_MAXIMUM = 100;
        public const int SECOND_BET_MAXIMUM = 200;
        public const int THIRD_BET_MAXIMUM = 500;
        public const int KOIKOI_BET_MAMIMUM = int.MaxValue;
        public const int ANTE = 20; // 参加料として最初に払うベットの事
        public const int KOIKOI_MULTIPLIER = 2; // こいこい後の勝負でもらえる金額の乗算係数

    }
}