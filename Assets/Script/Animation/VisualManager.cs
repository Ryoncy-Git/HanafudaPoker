using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using HanafudaPoker.Network;
using HanafudaPoker.Cards;
using HanafudaPoker.Games;

/*
    ネットワークの更新とアニメーションの更新タイミングが合ってないため
    札の更新があってない
*/

namespace HanafudaPoker.Animation
{
    public class VisualManager : MonoBehaviour
    {

        [Header("クライアントのみで完結")]
        // CardViewに移動させるべきの変数かもしれない
        [SerializeField] private float scaleOfCommunityCards = 1.0f;    // 場の札の大きさ
        [SerializeField] private float scaleOfHoldCards = 1.0f; // 手持ちの大きさ

        // 生成されたオブジェクトを管理する
        private List<CardView> cardViewField;
        private List<CardView> cardViewPlayer;
        private List<CardView> cardViewOtherPlayer;

        [Header("札の座標を動かす基準点")]
        // デッキの位置
        [SerializeField]
        private Transform deckTransform;

        // 場に置かれるカードの位置
        [SerializeField]
        private Transform[] fieldCardPositions;

        // こいこいで追加される札も同様に

        // 手札の位置
        [SerializeField]
        private Transform[] playerCardPositions;    // 自分の
        [SerializeField]
        private Transform[] playerOtherCardPositions;   // 他の人の見かけの札

        // カードの生成
        [SerializeField] private CardFactory cardFactory;

        // 手札更新を検知
        private int[] previousHandIDs;

        // フィールドに表示する札を出すタイミングを考える
        private TurnState previousState;

        private void Awake()
        {
            Debug.Log("VisualManager 初期化");

            cardViewField = new List<CardView>();
            cardViewPlayer = new List<CardView>();
            cardViewOtherPlayer = new List<CardView>();
        }

        private void Update()
        {
            if (previousHandIDs == null)
                return;

            CheckFieldReveal();
            CheckHandChanged();
        }

        private void CheckFieldReveal()
        {
            TurnState currentState =
                (TurnState)NetworkManager.GetTurnState();

            if (currentState == previousState)
                return;

            previousState = currentState;

            switch (currentState)
            {
                case TurnState.ShowField:
                    ShowFieldCard(0);
                    ShowFieldCard(1);
                    ShowFieldCard(2);
                    break;

                case TurnState.WaitForSecondChange:
                    ShowFieldCard(3);
                    break;

                case TurnState.ShowResult:
                    ShowFieldCard(4);
                    break;
            }
        }


        private void ShowFieldCard(int index)
        {
            cardViewField[index].transform.position = deckTransform.position;
            cardViewField[index].transform.rotation = deckTransform.rotation;

            StartCoroutine(
                cardViewField[index].PlayAnimation(
                    fieldCardPositions[index],
                    scaleOfCommunityCards, true)
            );
        }

        // どの札が入れ替わったか
        private void CheckHandChanged()
        {
            var hands = CardDataBase.GetCardDataListByID(NetworkManager.GetHands());

            if (previousHandIDs == null)
                return;

            for (int i = 0; i < hands.Count; i++)
            {
                if (!cardViewPlayer[i].isChanging && previousHandIDs[i] != hands[i].CardID)
                {
                    Debug.Log($"Card Changed {i}");

                    previousHandIDs[i] = hands[i].CardID;

                    StartCoroutine(
                        cardViewPlayer[i]
                        .ChangeCardAnimation(hands[i]));
                }
            }
        }


        // カード生成と配るアニメーションも付いてる
        public void CreateAllCardObjects()
        {
            Debug.Log("カードオブジェクトを生成");
            CreatePlayerCards();
            CreateOtherPlayerCards();
            CreateFieldCards();
        }

        public void CreateFieldCards()
        {
            var field = CardDataBase.GetCardDataListByID(NetworkManager.GetField());
            
            for (int i = 0; i < field.Count; i++)
            {
                CardView view = cardFactory.CreateCard(field[i]);
                view.Owner = CardOwner.Field;
                // あらかじめ決められた5枚
                cardViewField.Add(view);
            }
        }

        public void CreatePlayerCards()
        {
            var hands = CardDataBase.GetCardDataListByID(NetworkManager.GetHands());

            previousHandIDs = new int[hands.Count];

            for (int i = 0; i < hands.Count; i++)
            {
                previousHandIDs[i] = hands[i].CardID;
                CardView view = cardFactory.CreateCard(hands[i]);
                cardViewPlayer.Add(view);

                view.transform.position = deckTransform.position;
                view.transform.rotation = deckTransform.rotation;

                view.Owner = CardOwner.PlayerHand;
                view.HandIndex = i;

                StartCoroutine(
                    view.PlayAnimation(playerCardPositions[i], scaleOfHoldCards, true));
            }
        }

        public void CreateOtherPlayerCards()
        {
            Debug.Log("CreateOtherPlayerCards");

            int otherPlayerCount = GameConst.PLAYER_NUMBER - 1;

            // Debug 仮に全員そろっている場合
            otherPlayerCount = 3;


            // 他の人の札は例外が無ければ3枚、こいこいで2枚になる場合もある
            for (int i = 0; i < otherPlayerCount * GameConst.HAND_CARD_NUMBER; i++)
            {
                Debug.Log($"Create Fake Card {i}");

                CardView view = cardFactory.CreateFakeCard();
                cardViewOtherPlayer.Add(view);

                view.transform.position = deckTransform.position;
                view.transform.rotation = deckTransform.rotation;

                view.Owner = CardOwner.OtherPlayer;

                StartCoroutine(
                    view.PlayAnimation(playerOtherCardPositions[i], scaleOfHoldCards, true));
            }
        }

        public void ResetCardObjects()
        {
            Debug.Log("オブジェクトをリセット");

            foreach (var card in cardViewField)
            {
                card.StopAllCoroutines();
                Destroy(card.gameObject);
            }
            
            foreach (var card in cardViewPlayer)
            {
                card.StopAllCoroutines();
                Destroy(card.gameObject);
            }

            foreach (var card in cardViewOtherPlayer)
            {
                card.StopAllCoroutines();
                Destroy(card.gameObject);
            }

            cardViewField.Clear();
            cardViewPlayer.Clear();
            cardViewOtherPlayer.Clear();
            previousHandIDs = null;
        }
    }
}