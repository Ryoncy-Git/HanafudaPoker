using System;
using System.Collections.Generic;

using HanafudaPoker.Games;

namespace HanafudaPoker.Cards
{
    public enum CardMonth
    {
        Matsu = 1,
        Ume,
        Sakura,
        Fuji,
        Ayame,
        Botan,
        Hagi,
        Susuki,
        Kiku,
        Momiji,
        Yanagi,
        Kiri
    }

    public enum CardRank
    {
        Hikari,
        Tane,
        Tanzaku,
        Kasu
    }

    [Flags]
    public enum CardFeature
    {
        None = 0,
        Akatan = 1 << 0,
        Akajitan = 1 << 1,
        Aotan = 1 << 2,
        Tori = 1 << 3,
        Inoshikacho = 1 << 4,
        Mizu = 1 << 5
    }

    public class CardData
    {
        public CardMonth Month;
        public CardRank Rank;
        public CardFeature Feature;
        public int CardID;

        public CardData(CardMonth month, CardRank rank, CardFeature feature, int id)
        {
            Month = month;
            Rank = rank;
            Feature = feature;
            CardID = id;
        }

        
    }

    public static class CardDataBase
    {
        private static Dictionary<int, CardData> cards;
        static void CardDatabase()
        {
            cards = new();

            foreach(var card in CardMovementManager.CreateDeck())
            {
                cards.Add(card.CardID, card);
            }
            return;
        }

        public static CardData GetCardDataByID(int id)
        {
            return cards[id];
        }

        public static List<CardData> GetCardDataListByID(int[] ids)
        {
            List<CardData> list = new();
            foreach(int id in ids)
            {
                list.Add(GetCardDataByID(id));
            }

            return list;
        }

        public static int[] GetIDsByList(List<CardData> c)
        {
            int[] ids = new int[c.Count];
            for(int i = 0; i < c.Count; i++)
            {
                ids[i] = c[i].CardID;
            }

            return ids;
        }
    }
}