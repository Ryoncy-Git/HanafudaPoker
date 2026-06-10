using System;

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

        public int CardId;

        public CardData(CardMonth month, CardRank rank, CardFeature feature, int cardId)
        {
            Month = month;
            Rank = rank;
            Feature = feature;
            CardId = cardId;
        }
    }
}