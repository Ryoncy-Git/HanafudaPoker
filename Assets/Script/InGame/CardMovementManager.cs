using UnityEngine;
using System.Collections.Generic;

using HanafudaPoker.Cards;
using HanafudaPoker.Network;


namespace HanafudaPoker.Games
{
    public static class CardMovementManager
    {
        public static void CreateAndShuffleDeck()
        {
            List<CardData> deck = new();
            int id = 0;

            deck.Add(new CardData(CardMonth.Matsu,   CardRank.Hikari,  CardFeature.Tori,        id++));      // 鶴
            deck.Add(new CardData(CardMonth.Matsu,   CardRank.Tanzaku, CardFeature.Akajitan,    id++));
            deck.Add(new CardData(CardMonth.Matsu,   CardRank.Kasu,    CardFeature.None,        id++));
            deck.Add(new CardData(CardMonth.Matsu,   CardRank.Kasu,    CardFeature.None,        id++));

            deck.Add(new CardData(CardMonth.Ume,     CardRank.Tane,    CardFeature.Tori,        id++));      // 鶯
            deck.Add(new CardData(CardMonth.Ume,     CardRank.Tanzaku, CardFeature.Akajitan,    id++));
            deck.Add(new CardData(CardMonth.Ume,     CardRank.Kasu,    CardFeature.None,        id++));
            deck.Add(new CardData(CardMonth.Ume,     CardRank.Kasu,    CardFeature.None,        id++));

            deck.Add(new CardData(CardMonth.Sakura,  CardRank.Hikari,  CardFeature.None,        id++)); // 幕
            deck.Add(new CardData(CardMonth.Sakura,  CardRank.Tanzaku, CardFeature.Akajitan,    id++));
            deck.Add(new CardData(CardMonth.Sakura,  CardRank.Kasu,    CardFeature.None,        id++));
            deck.Add(new CardData(CardMonth.Sakura,  CardRank.Kasu,    CardFeature.None,        id++));

            deck.Add(new CardData(CardMonth.Fuji,    CardRank.Tane,    CardFeature.Tori,        id++)); // ほととぎす
            deck.Add(new CardData(CardMonth.Fuji,    CardRank.Tanzaku, CardFeature.Akatan,      id++));
            deck.Add(new CardData(CardMonth.Fuji,    CardRank.Kasu,    CardFeature.None,        id++));
            deck.Add(new CardData(CardMonth.Fuji,    CardRank.Kasu,    CardFeature.None,        id++));

            deck.Add(new CardData(CardMonth.Ayame,   CardRank.Tane,    CardFeature.Mizu,        id++)); // 八ツ橋
            deck.Add(new CardData(CardMonth.Ayame,   CardRank.Tanzaku, CardFeature.Akatan,      id++));
            deck.Add(new CardData(CardMonth.Ayame,   CardRank.Kasu,    CardFeature.None,        id++));
            deck.Add(new CardData(CardMonth.Ayame,   CardRank.Kasu,    CardFeature.None,        id++));

            deck.Add(new CardData(CardMonth.Botan,   CardRank.Tane,    CardFeature.Inoshikacho, id++)); // 蝶
            deck.Add(new CardData(CardMonth.Botan,   CardRank.Tanzaku, CardFeature.Aotan,       id++));
            deck.Add(new CardData(CardMonth.Botan,   CardRank.Kasu,    CardFeature.None,        id++));
            deck.Add(new CardData(CardMonth.Botan,   CardRank.Kasu,    CardFeature.None,        id++));

            deck.Add(new CardData(CardMonth.Hagi,    CardRank.Tane,    CardFeature.Inoshikacho, id++)); // 猪
            deck.Add(new CardData(CardMonth.Hagi,    CardRank.Tanzaku, CardFeature.Akatan,      id++));
            deck.Add(new CardData(CardMonth.Hagi,    CardRank.Kasu,    CardFeature.None,        id++));
            deck.Add(new CardData(CardMonth.Hagi,    CardRank.Kasu,    CardFeature.None,        id++));

            deck.Add(new CardData(CardMonth.Susuki,  CardRank.Hikari,  CardFeature.None,        id++)); // 月
            deck.Add(new CardData(CardMonth.Susuki,  CardRank.Tane,    CardFeature.Tori,        id++)); // 雁
            deck.Add(new CardData(CardMonth.Susuki,  CardRank.Kasu,    CardFeature.None,        id++));
            deck.Add(new CardData(CardMonth.Susuki,  CardRank.Kasu,    CardFeature.None,        id++));

            deck.Add(new CardData(CardMonth.Kiku,    CardRank.Tane,    CardFeature.None,        id++)); // 盃
            deck.Add(new CardData(CardMonth.Kiku,    CardRank.Tanzaku, CardFeature.Aotan,       id++));
            deck.Add(new CardData(CardMonth.Kiku,    CardRank.Kasu,    CardFeature.None,        id++));
            deck.Add(new CardData(CardMonth.Kiku,    CardRank.Kasu,    CardFeature.None,        id++));

            deck.Add(new CardData(CardMonth.Momiji,  CardRank.Tane,    CardFeature.Inoshikacho, id++)); // 鹿
            deck.Add(new CardData(CardMonth.Momiji,  CardRank.Tanzaku, CardFeature.Aotan,       id++));
            deck.Add(new CardData(CardMonth.Momiji,  CardRank.Kasu,    CardFeature.None,        id++));
            deck.Add(new CardData(CardMonth.Momiji,  CardRank.Kasu,    CardFeature.None,        id++));

            deck.Add(new CardData(CardMonth.Yanagi,  CardRank.Hikari,  CardFeature.Mizu,        id++)); // 小野道風
            deck.Add(new CardData(CardMonth.Yanagi,  CardRank.Tane,    CardFeature.Tori,        id++)); // 燕
            deck.Add(new CardData(CardMonth.Yanagi,  CardRank.Tanzaku, CardFeature.Akatan,      id++));
            deck.Add(new CardData(CardMonth.Yanagi,  CardRank.Kasu,    CardFeature.Mizu,        id++));

            deck.Add(new CardData(CardMonth.Kiri,    CardRank.Hikari,  CardFeature.Tori,        id++)); // 鳳凰
            deck.Add(new CardData(CardMonth.Kiri,    CardRank.Kasu,    CardFeature.None,        id++));
            deck.Add(new CardData(CardMonth.Kiri,    CardRank.Kasu,    CardFeature.None,        id++));
            deck.Add(new CardData(CardMonth.Kiri,    CardRank.Kasu,    CardFeature.None,        id++));


            Debug.Log("Create Deck");
            // NetworkManager.SetDeck(CardDataBase.GetIDsByList(deck));

            ShuffleDeck(deck);
        }

        private static void ShuffleDeck(List<CardData> deck)
        {
            for(int i = 0; i < deck.Count; i++)
            {
                int rand = Random.Range(0, deck.Count);

                (deck[i], deck[rand]) = (deck[rand], deck[i]);
            }

            NetworkManager.SetDeck(CardDataBase.GetIDsByList(deck));

            // デバッグ用
            Debug.Log("Deck Shuffle Done");
        }

        public static void DealCards()
        {
            var deck = CardDataBase.GetCardDataListByID(NetworkManager.GetDeck());
            // var field = CardDataBase.GetCardDataListByID(NetworkManager.GetField());

            CardData dealtCard;
            List<CardData> dealtCards = new List<CardData>();

            for(int seatID = 0; seatID < GameConst.PLAYER_NUMBER; seatID++)
            {
                dealtCards = new List<CardData>();

                for(int j = 0; j < GameConst.HAND_CARD_NUMBER; j++)
                {
                    dealtCard = deck[deck.Count - 1];

                    dealtCards.Add(dealtCard);
                    deck.Remove(dealtCard);
                }

                NetworkManager.SetHands(CardDataBase.GetIDsByList(dealtCards), seatID);
            }


            dealtCards = new List<CardData>();

            for(int k = 0; k < GameConst.FIELD_CARD_NUMBER; k++) // 場のカードが5枚なので
            {
                dealtCard = deck[deck.Count - 1];
                dealtCards.Add(dealtCard);
                deck.Remove(dealtCard);
            }

            NetworkManager.SetField(CardDataBase.GetIDsByList(dealtCards));
            NetworkManager.SetDeck(CardDataBase.GetIDsByList(deck));
            
            Debug.Log("Deal Cards");
            return;   
        }

        public static void ChangeHandCards()
        {
            var deck = CardDataBase.GetCardDataListByID(NetworkManager.GetDeck());

            CardData dealtCard;
            List<CardData> newHands;


            for(int seatID = 0; seatID < GameConst.PLAYER_NUMBER; seatID++)
            {
                newHands = new List<CardData>();
                bool[] readys = NetworkManager.GetWillChangeCards(seatID);
                var hands = CardDataBase.GetCardDataListByID(NetworkManager.GetHands(seatID));

                for(int i = 0; i < GameConst.HAND_CARD_NUMBER; i++)
                {
                    if(readys[i] == false) // 変えないのなら
                    {
                        newHands.Add(hands[i]);
                    }
                    else
                    {
                        dealtCard = deck[deck.Count - 1];
                        newHands.Add(dealtCard);
                        deck.Remove(dealtCard);
                    }

                    NetworkManager.SetHands(CardDataBase.GetIDsByList(newHands), seatID);
                }
            }

            NetworkManager.SetDeck(CardDataBase.GetIDsByList(deck));

            Debug.Log("Deal done");
        }
    }
}