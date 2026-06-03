using UnityEngine;
using System;
using System.Collections.Generic;

using HanafudaPoker.Cards;

namespace HanafudaPoker.Yakus
{
    public enum Yaku
    {
        Tsui,
        Nitsui, 
        Santsui,
        Yontsui,
        Mangetsu, // 同じ月4枚
        Akatan,
        Aotan,
        Tan,
        Gokou,
        Yonkou,
        Ameshikou,
        Sankou,
        Inoshikacho,
        Sakeutage, // 酒、桜の光、芒の月
        
        Mizu, // 菖蒲の種、柳のカス、柳の光
        Murasaki, // 桐、藤、菖蒲を三種類集める カス限定
        Hanaikada, // 連続する月の札を四枚、カスのみで成立
        Adabana, // 花以外の札（松、芒、紅葉、桐）を4種類集める　（徒花）
        Chidori, // 鳥が映っている札を集める。4まい
        MidareChidori,// 鳳凰抜きの５枚
        Houou, // 鳳凰込みの５枚
        Hououraigi // 6まい
    }
    public static class YakuData
    {
        public static List<Yaku> YakuCheck(List<CardData> field, List<CardData> hand)
        {
            List<Yaku> yakus = new();

            List<CardData> cards = new();

            foreach(CardData c in field)
            {
                cards.Add(c);
            }
            foreach(CardData c in hand)
            {
                cards.Add(c);
            }

            // 役を全部調べてチェックしてリストに足していく
            // 役は何個もある状態、役がかぶっても墓に持ってる役で勝負
            // 役が完全に同じ場合は月がデカい方が勝ち
            if(hasYontsui(cards))
            {
                yakus.Add(Yaku.Yontsui);
            }
            else if(hasSantsui(cards))
            {
                yakus.Add(Yaku.Santsui);
            }
            else if(hasNitsui(cards))
            {
                yakus.Add(Yaku.Nitsui);
            }
            else if(hasTsui(cards))
            {    
                yakus.Add(Yaku.Tsui);
            }

            if(hasMangetsu(cards))
                yakus.Add(Yaku.Mangetsu);

            if(hasAkatan(cards))
                yakus.Add(Yaku.Akatan);

            if(hasAotan(cards))
                yakus.Add(Yaku.Aotan);

            if(hasTan(cards))
                yakus.Add(Yaku.Tan);

            if(hasGokou(cards))
            {
                yakus.Add(Yaku.Gokou);
            }
            else if(hasYonkou(cards))
            {
                yakus.Add(Yaku.Yonkou);
            }
            else if(hasAmeshikou(cards))
            {
                yakus.Add(Yaku.Ameshikou);
            }
            else if(hasSankou(cards))
            {
                yakus.Add(Yaku.Sankou);
            }

            if(hasInoshikacho(cards))
                yakus.Add(Yaku.Inoshikacho);

            if(hasSakeutage(cards))
                yakus.Add(Yaku.Sakeutage);

            if(hasMizu(cards))
                yakus.Add(Yaku.Mizu);

            if(hasMurasaki(cards))
                yakus.Add(Yaku.Murasaki);

            if(hasHanaikada(cards))
                yakus.Add(Yaku.Hanaikada);

            if(hasAdabana(cards))
                yakus.Add(Yaku.Adabana);

            // 鳥役（排他的）
            if(hasHououraigi(cards))
            {
                yakus.Add(Yaku.Hououraigi);
            }
            else if(hasHouou(cards))
            {
                yakus.Add(Yaku.Houou);
            }
            else if(hasMidareChidori(cards))
            {
                yakus.Add(Yaku.MidareChidori);
            }
            else if(hasChidori(cards))
            {
                yakus.Add(Yaku.Chidori);
            }

            return yakus;
        }

        public static bool hasTsui(List<CardData> cards)
        {
            int[] monthCount = new int[12];

            foreach(CardData card in cards) // 各月何枚ずつあるか確認
            {
                monthCount[(int)card.Month - 1]++;
            }

            foreach(int count in monthCount)
            {
                if(count == 2 || count == 3) // 二枚か三枚なら
                {
                    return true;
                }
            }

            return false; // 満月だとfalse判定
        }

        public static bool hasNitsui(List<CardData> cards)
        {
            int[] monthCount = new int[12];
            int tsuiCount = 0;

            foreach(CardData card in cards)
            {
                monthCount[(int)card.Month - 1]++;
            }

            foreach(int count in monthCount)
            {
                if(count == 2 || count == 3)
                {
                    tsuiCount++;
                }
            }

            return tsuiCount >= 2;
        }

        public static bool hasSantsui(List<CardData> cards)
        {
            int[] monthCount = new int[12];
            int tsuiCount = 0;

            foreach(CardData card in cards)
            {
                monthCount[(int)card.Month - 1]++;
            }

            foreach(int count in monthCount)
            {
                if(count == 2 || count == 3)
                {
                    tsuiCount++;
                }
            }

            return tsuiCount >= 3;
        }

        public static bool hasYontsui(List<CardData> cards)
        {
            int[] monthCount = new int[12];
            int tsuiCount = 0;

            foreach(CardData card in cards)
            {
                monthCount[(int)card.Month - 1]++;
            }

            foreach(int count in monthCount)
            {
                if(count == 2 || count == 3)
                {
                    tsuiCount++;
                }
            }

            return tsuiCount >= 4;
        }

        public static bool hasMangetsu(List<CardData> cards)
        {
            int[] monthCount = new int[12];

            foreach(CardData card in cards)
            {
                monthCount[(int)card.Month - 1]++;
            }

            foreach(int count in monthCount)
            {
                if(count == 4)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool hasAkatan(List<CardData> cards)
        {
            int count = 0;

            foreach(CardData card in cards)
            {
                if(card.Feature.HasFlag(CardFeature.Akajitan))
                {
                    count++;
                }
            }

            return count >= 3;
        }

        public static bool hasAotan(List<CardData> cards)
        {
            int count = 0;

            foreach(CardData card in cards)
            {
                if(card.Feature.HasFlag(CardFeature.Aotan))
                {
                    count++;
                }
            }

            return count >= 3;
        }

        public static bool hasTan(List<CardData> cards)
        {
            int count = 0;

            foreach(CardData card in cards)
            {
                if(card.Rank == CardRank.Tanzaku)
                {
                    count++;
                }
            }

            return count >= 5;
        }
        public static bool hasGokou(List<CardData> cards)
        {
            int hikariCount = 0;

            foreach(CardData card in cards)
            {
                if(card.Rank == CardRank.Hikari)
                {
                    hikariCount++;
                }
            }

            return hikariCount == 5;
        }

        public static bool hasYonkou(List<CardData> cards)
        {
            int hikariCount = 0;
            bool hasRain = false;

            foreach(CardData card in cards)
            {
                if(card.Rank == CardRank.Hikari)
                {
                    hikariCount++;

                    if(card.Month == CardMonth.Yanagi)
                    {
                        hasRain = true;
                    }
                }
            }

            return hikariCount == 4 && !hasRain;
        }

        public static bool hasAmeshikou(List<CardData> cards)
        {
            int hikariCount = 0;
            bool hasRain = false;

            foreach(CardData card in cards)
            {
                if(card.Rank == CardRank.Hikari)
                {
                    hikariCount++;

                    if(card.Month == CardMonth.Yanagi)
                    {
                        hasRain = true;
                    }
                }
            }

            return hikariCount == 4 && hasRain;
        }

        public static bool hasSankou(List<CardData> cards)
        {
            int hikariCount = 0;

            foreach(CardData card in cards)
            {
                if(card.Rank == CardRank.Hikari &&
                card.Month != CardMonth.Yanagi)
                {
                    hikariCount++;
                }
            }

            return hikariCount == 3;
        }
        public static bool hasInoshikacho(List<CardData> cards)
        {
            bool hasIno = false;
            bool hasShika = false;
            bool hasCho = false;

            foreach(CardData card in cards)
            {
                if(card.Month == CardMonth.Hagi &&
                card.Rank == CardRank.Tane)
                {
                    hasIno = true;
                }

                if(card.Month == CardMonth.Momiji &&
                card.Rank == CardRank.Tane)
                {
                    hasShika = true;
                }

                if(card.Month == CardMonth.Botan &&
                card.Rank == CardRank.Tane)
                {
                    hasCho = true;
                }
            }

            return hasIno && hasShika && hasCho;
        }

        public static bool hasSakeutage(List<CardData> cards)
        {
            bool hasMoon = false;
            bool hasSakura = false;
            bool hasSake = false;

            foreach(CardData card in cards)
            {
                if(card.Month == CardMonth.Susuki &&
                card.Rank == CardRank.Hikari)
                {
                    hasMoon = true;
                }

                if(card.Month == CardMonth.Sakura &&
                card.Rank == CardRank.Hikari)
                {
                    hasSakura = true;
                }

                if(card.Month == CardMonth.Kiku &&
                card.Rank == CardRank.Tane)
                {
                    hasSake = true;
                }
            }

            return hasMoon && hasSakura && hasSake;
        }
        public static bool hasChidori()
        {
            // debug
            return true;
        }
        public static bool hasMizu(List<CardData> cards)
        {
            int count = 0;

            foreach(CardData card in cards)
            {
                if(card.Feature.HasFlag(CardFeature.Mizu))
                {
                    count++;
                }
            }

            return count == 3;
        }

        public static bool hasMurasaki(List<CardData> cards)
        {
            bool hasFuji = false;
            bool hasAyame = false;
            bool hasKiri = false;

            foreach(CardData card in cards)
            {
                if(card.Month == CardMonth.Fuji && card.Rank == CardRank.Kasu)
                {
                    hasFuji = true;
                }

                if(card.Month == CardMonth.Ayame && card.Rank == CardRank.Kasu)
                {
                    hasAyame = true;
                }

                if(card.Month == CardMonth.Kiri && card.Rank == CardRank.Kasu)
                {
                    hasKiri = true;
                }
            }

            return hasFuji && hasAyame && hasKiri;
        }

        public static bool hasHanaikada(List<CardData> cards)
        {
            bool[] kasuMonths = new bool[12];

            foreach(CardData card in cards)
            {
                if(card.Rank == CardRank.Kasu)
                {
                    kasuMonths[(int)card.Month - 1] = true;
                }
            }

            for(int start = 0; start <= 8; start++)
            {
                if(
                    kasuMonths[start] &&
                    kasuMonths[start + 1] &&
                    kasuMonths[start + 2] &&
                    kasuMonths[start + 3]
                )
                {
                    return true;
                }
            }

            return false;
        }

        public static bool hasAdabana(List<CardData> cards)
        {
            bool hasMatsu = false;
            bool hasSusuki = false;
            bool hasMomiji = false;
            bool hasKiri = false;

            foreach(CardData card in cards)
            {
                if(card.Month == CardMonth.Matsu)
                {
                    hasMatsu = true;
                }

                if(card.Month == CardMonth.Susuki)
                {
                    hasSusuki = true;
                }

                if(card.Month == CardMonth.Momiji)
                {
                    hasMomiji = true;
                }

                if(card.Month == CardMonth.Kiri)
                {
                    hasKiri = true;
                }
            }

            return hasMatsu && hasSusuki && hasMomiji && hasKiri;
        }

        public static bool hasChidori(List<CardData> cards)
        {
            return GetBirdCount(cards) >= 4;
        }

        public static bool hasMidareChidori(List<CardData> cards)
        {
            bool hasHououBool = false;

            foreach(CardData card in cards)
            {
                if(card.Month == CardMonth.Kiri &&
                card.Rank == CardRank.Hikari)
                {
                    hasHououBool = true;
                    break;
                }
            }

            return GetBirdCount(cards) == 5 && !hasHououBool;
        }

        public static bool hasHouou(List<CardData> cards)
        {
            bool hasHououBool = false;

            foreach(CardData card in cards)
            {
                if(card.Month == CardMonth.Kiri &&
                card.Rank == CardRank.Hikari)
                {
                    hasHououBool = true;
                    break;
                }
            }

            return GetBirdCount(cards) == 5 && hasHououBool;
        }

        public static bool hasHououraigi(List<CardData> cards)
        {
            return GetBirdCount(cards) == 6;
        }


        // ---------------補助関数---------------
        private static int GetBirdCount(List<CardData> cards)
        {
            int count = 0;

            foreach(CardData card in cards)
            {
                if(card.Feature.HasFlag(CardFeature.Tori))
                {
                    count++;
                }
            }

            return count;
        }
    }
}