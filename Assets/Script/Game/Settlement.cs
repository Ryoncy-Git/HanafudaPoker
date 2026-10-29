using UnityEngine;
using System;
using System.Collections.Generic;

using HanafudaPoker.Network;

namespace HanafudaPoker.Bet
{
    public static class Settlement
    {
        public static void Execute(int[] score)
        {
            int[] Bets = new int[score.Length];

            for (int seatID = 0; seatID < Bets.Length; seatID++)
            {
                Bets[seatID] = NetworkManager.GetBet(seatID);
            }

            // 各プレイヤーの受取額を計算
            int[] payouts = CalculatePayouts(Bets, score);

            // 清算処理
            for (int seatID = 0; seatID < Bets.Length; seatID++)
            {
                int profit = payouts[seatID] - Bets[seatID];

                

                // 利益を加算

                if(NetworkManager.GetInsurace() > 0)
                {
                    // もし保険に入っていて、利益がマイナスなら
                    if(profit < 0)
                    {
                        // 損失を半分に
                        profit /= 2;
                    }
                }

                Debug.Log(
                    $"Player {seatID} : " +
                    $"Bet = {Bets[seatID]}, " +
                    $"Insurance = {NetworkManager.GetInsurace() > 0}, " +
                    $"Score = {score[seatID]}, " +
                    $"Payout = {payouts[seatID]}, " +
                    $"Profit = {profit}"
                );


                NetworkManager.AddMoney(payouts[seatID], seatID);
            }
        }

        private static int[] CalculatePayouts(int[] bets, int[] score)
        {
            if (bets == null || score == null ||
                bets.Length != score.Length)
            {
                throw new ArgumentException("掛金またはスコアが不正です");
            }

            int playerCount = bets.Length;
            int[] payouts = new int[playerCount];

            // 掛金の境界を昇順で取得
            List<int> betLevels = new List<int>();

            foreach (int bet in bets)
            {
                if (bet < 0)
                    throw new ArgumentException("掛金が負の値です");

                if (bet > 0 && !betLevels.Contains(bet))
                {
                    betLevels.Add(bet);
                }
            }

            betLevels.Sort();

            int previousLevel = 0;

            foreach (int level in betLevels)
            {
                // このポットに参加できるプレイヤー
                List<int> participants = new List<int>();

                for (int seatID = 0; seatID < playerCount; seatID++)
                {
                    if (bets[seatID] >= level)
                    {
                        participants.Add(seatID);
                    }
                }

                // ポットの金額
                int pot = checked(
                    (level - previousLevel) * participants.Count
                );

                previousLevel = level;

                // 参加者が1人なら余剰掛金を返却
                if (participants.Count == 1)
                {
                    int seatID = participants[0];
                    payouts[seatID] = checked(payouts[seatID] + pot);
                    continue;
                }

                // 参加者の中で最高スコアを取得
                int maxScore = int.MinValue;

                foreach (int seatID in participants)
                {
                    if (score[seatID] > maxScore)
                    {
                        maxScore = score[seatID];
                    }
                }

                // 勝者を取得（同点対応）
                List<int> winners = new List<int>();

                foreach (int seatID in participants)
                {
                    if (score[seatID] == maxScore)
                    {
                        winners.Add(seatID);
                    }
                }

                // ポットを勝者に分配
                int payout = pot / winners.Count;
                int remainder = pot % winners.Count;

                foreach (int seatID in winners)
                {
                    payouts[seatID] = checked(
                        payouts[seatID] + payout
                    );
                }

                // 端数は破棄します。
                // // 端数をSeatID順に分配
                // for (int i = 0; i < remainder; i++)
                // {
                //     payouts[winners[i]]++;
                // }

                Debug.Log(
                    $"Pot = {pot}, " +
                    $"Participants = {string.Join(",", participants)}, " +
                    $"Winners = {string.Join(",", winners)}"
                );
            }

            return payouts;
        }
    }
}