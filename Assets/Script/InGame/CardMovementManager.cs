using UnityEngine;
using System.Collections.Generic;

using HanafudaPoker.Cards;
using HanafudaPoker.Players;


namespace HanafudaPoker.Games
{
    public static class CardMovementManager
    {
        public static List<CardData> CreateDeck()
        {
            List<CardData> deck = new();

            deck.Add(new CardData(CardMonth.Matsu,   CardRank.Hikari,  CardFeature.Tori));      // 鶴
            deck.Add(new CardData(CardMonth.Matsu,   CardRank.Tanzaku, CardFeature.Akajitan));
            deck.Add(new CardData(CardMonth.Matsu,   CardRank.Kasu,    CardFeature.None));
            deck.Add(new CardData(CardMonth.Matsu,   CardRank.Kasu,    CardFeature.None));

            deck.Add(new CardData(CardMonth.Ume,     CardRank.Tane,    CardFeature.Tori));      // 鶯
            deck.Add(new CardData(CardMonth.Ume,     CardRank.Tanzaku, CardFeature.Akajitan));
            deck.Add(new CardData(CardMonth.Ume,     CardRank.Kasu,    CardFeature.None));
            deck.Add(new CardData(CardMonth.Ume,     CardRank.Kasu,    CardFeature.None));

            deck.Add(new CardData(CardMonth.Sakura,  CardRank.Hikari,  CardFeature.None));      // 幕
            deck.Add(new CardData(CardMonth.Sakura,  CardRank.Tanzaku, CardFeature.Akajitan));
            deck.Add(new CardData(CardMonth.Sakura,  CardRank.Kasu,    CardFeature.None));
            deck.Add(new CardData(CardMonth.Sakura,  CardRank.Kasu,    CardFeature.None));

            deck.Add(new CardData(CardMonth.Fuji,    CardRank.Tane,    CardFeature.Tori));      // ほととぎす
            deck.Add(new CardData(CardMonth.Fuji,    CardRank.Tanzaku, CardFeature.Akatan));
            deck.Add(new CardData(CardMonth.Fuji,    CardRank.Kasu,    CardFeature.None));
            deck.Add(new CardData(CardMonth.Fuji,    CardRank.Kasu,    CardFeature.None));

            deck.Add(new CardData(CardMonth.Ayame,   CardRank.Tane,    CardFeature.Mizu));      // 八ツ橋
            deck.Add(new CardData(CardMonth.Ayame,   CardRank.Tanzaku, CardFeature.Akatan));
            deck.Add(new CardData(CardMonth.Ayame,   CardRank.Kasu,    CardFeature.None));
            deck.Add(new CardData(CardMonth.Ayame,   CardRank.Kasu,    CardFeature.None));

            deck.Add(new CardData(CardMonth.Botan,   CardRank.Tane,    CardFeature.Inoshikacho)); // 蝶
            deck.Add(new CardData(CardMonth.Botan,   CardRank.Tanzaku, CardFeature.Aotan));
            deck.Add(new CardData(CardMonth.Botan,   CardRank.Kasu,    CardFeature.None));
            deck.Add(new CardData(CardMonth.Botan,   CardRank.Kasu,    CardFeature.None));

            deck.Add(new CardData(CardMonth.Hagi,    CardRank.Tane,    CardFeature.Inoshikacho)); // 猪
            deck.Add(new CardData(CardMonth.Hagi,    CardRank.Tanzaku, CardFeature.Akatan));
            deck.Add(new CardData(CardMonth.Hagi,    CardRank.Kasu,    CardFeature.None));
            deck.Add(new CardData(CardMonth.Hagi,    CardRank.Kasu,    CardFeature.None));

            deck.Add(new CardData(CardMonth.Susuki,  CardRank.Hikari,  CardFeature.None));      // 月
            deck.Add(new CardData(CardMonth.Susuki,  CardRank.Tane,    CardFeature.Tori));      // 雁
            deck.Add(new CardData(CardMonth.Susuki,  CardRank.Kasu,    CardFeature.None));
            deck.Add(new CardData(CardMonth.Susuki,  CardRank.Kasu,    CardFeature.None));

            deck.Add(new CardData(CardMonth.Kiku,    CardRank.Tane,    CardFeature.None));      // 盃
            deck.Add(new CardData(CardMonth.Kiku,    CardRank.Tanzaku, CardFeature.Aotan));
            deck.Add(new CardData(CardMonth.Kiku,    CardRank.Kasu,    CardFeature.None));
            deck.Add(new CardData(CardMonth.Kiku,    CardRank.Kasu,    CardFeature.None));

            deck.Add(new CardData(CardMonth.Momiji,  CardRank.Tane,    CardFeature.Inoshikacho)); // 鹿
            deck.Add(new CardData(CardMonth.Momiji,  CardRank.Tanzaku, CardFeature.Aotan));
            deck.Add(new CardData(CardMonth.Momiji,  CardRank.Kasu,    CardFeature.None));
            deck.Add(new CardData(CardMonth.Momiji,  CardRank.Kasu,    CardFeature.None));

            deck.Add(new CardData(CardMonth.Yanagi,  CardRank.Hikari,  CardFeature.Mizu));      // 小野道風
            deck.Add(new CardData(CardMonth.Yanagi,  CardRank.Tane,    CardFeature.Tori));      // 燕
            deck.Add(new CardData(CardMonth.Yanagi,  CardRank.Tanzaku, CardFeature.Akatan));
            deck.Add(new CardData(CardMonth.Yanagi,  CardRank.Kasu,    CardFeature.Mizu));

            deck.Add(new CardData(CardMonth.Kiri,    CardRank.Hikari,  CardFeature.Tori));      // 鳳凰
            deck.Add(new CardData(CardMonth.Kiri,    CardRank.Kasu,    CardFeature.None));
            deck.Add(new CardData(CardMonth.Kiri,    CardRank.Kasu,    CardFeature.None));
            deck.Add(new CardData(CardMonth.Kiri,    CardRank.Kasu,    CardFeature.None));

            // デバッグ
            Debug.Log("Create Deck");
            return deck;
        }

        public static List<CardData> ShuffleDeck(List<CardData> deck)
        {
            for(int i = 0; i < deck.Count; i++)
            {
                int rand = Random.Range(0, deck.Count);

                (deck[i], deck[rand]) = (deck[rand], deck[i]);
            }

            // デバッグ用
            Debug.Log("Shuffle Deck");
            return deck;
        }

        public static void DealCards(List<CardData> deck, List<CardData> field, PlayerData[] players)
        {
            CardData dealtCard;

            for(int i = 0; i < players.Length; i++)
            {
                for(int j = 0; j < GameConst.HAND_CARD_NUMBER; j++)
                {
                    // Debug.LogError($"Deck:{deck.Count}");
                    dealtCard = deck[deck.Count - 1];

                    players[i].HandCards.Add(dealtCard);
                    deck.Remove(dealtCard);
                }
            }

            for(int k = 0; k < GameConst.FIELD_CARD_NUMBER; k++) // 場のカードが5枚なので
            {
                dealtCard = deck[deck.Count - 1];
                field.Add(dealtCard);
                deck.Remove(dealtCard);
            }
            
            Debug.Log("Deal Cards");
            return;   
        }

        public static void ChangeHandCards(List<CardData> deck, PlayerData[] players, List<CardData> discardPile)
        {
            CardData dealtCard;

            foreach(PlayerData player in players)
            {
                for(int i = 0; i < GameConst.HAND_CARD_NUMBER; i++)
                {
                    if(player.WillChangeCards[i])
                    {
                        CardData changedCard = player.HandCards[i];
                        dealtCard = deck[deck.Count - 1];

                        discardPile.Add(changedCard);
                        player.HandCards[i] = dealtCard;
                        deck.Remove(dealtCard);
                    }
                }
            }
        }

    }
}