using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using HanafudaPoker.Network;
using HanafudaPoker.Cards;
using HanafudaPoker.Games;

namespace HanafudaPoker.Animation
{
    public class VisualManager : MonoBehaviour
    {
        [Header("クライアントのみで完結")]
        // CardViewに移動させるべきの変数かもしれない
        [SerializeField] private float scaleOfCommunityCards = 1.0f;    // 場の札の大きさ
        [SerializeField] private float scaleOfHoldCards = 1.0f; // 手持ちの大きさ

        // 生成されたオブジェクトを管理する
        private List<CardView> cardViewField;   // 場の札　こいこいは含まれない
        private List<CardView> cardViewPlayer;  // 手札
        private List<CardView> cardViewOtherPlayer; // 見せかけ　ゲームとしての意味はない
        private List<CardView> cardViewKoikoiField; // こいこい用の札

        [Header("札の座標を動かす基準点")]
        // デッキの位置
        [SerializeField]
        private Transform deckTransform;

        // 場に置かれるカードの位置
        [SerializeField]
        [Header("場の札-こいこい抜き")]
        private Transform[] fieldDefaultCardPositions;

        [SerializeField]
        [Header("場の札-こいこいのみ")]
        private Transform[] fieldKoikoiCardPositions;

        // 手札の位置
        [SerializeField]
        private Transform[] playerCardPositions;    // 自分の
        [SerializeField]
        private Transform[] playerOtherCardPositions;   // 他の人の見かけの札

        // カードの生成
        [SerializeField] private CardFactory cardFactory;

        // 手札更新を検知
        private int[] previousHandIDs;

        // フィールドに表示する札を出すタイミング
        private TurnState previousState;
        private TurnState currentState;

        [Header("Cameraの移動")]
        [SerializeField] private CameraMove cameraMove;

        private void Awake()
        {
            Debug.Log("VisualManager 初期化");

            cardViewField = new List<CardView>();
            cardViewPlayer = new List<CardView>();
            cardViewOtherPlayer = new List<CardView>();
            cardViewKoikoiField = new List<CardView>();

        }

        private void Update()
        {
            if (previousHandIDs == null)
                return;

            OnStateChanged();
            CheckHandChanged();
        }

        private void OnStateChanged()
        {
            currentState = (TurnState)NetworkManager.GetTurnState();

            if (currentState == previousState)
                return;

            Debug.Log($"State: {previousState} -> {currentState}");

            previousState = currentState;

            switch (currentState)
            {
                case TurnState.WaitForInitialize:
                    cameraMove.ResetCamera();
                    break;

                case TurnState.BeforeGame:
                    cameraMove.ResetCamera();
                    break;

                case TurnState.ShowField:
                    cameraMove.ResetCamera();
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

                case TurnState.ShowFinalResult:
                    ShowFieldKoikoiCard(1);
                    ShowFieldKoikoiCard(2);
                    cameraMove.MoveCamera();
                    break;

                case TurnState.WaitForNextRound:
                    break;
            }
        }

        private void ShowFieldCard(int index)
        {
            cardViewField[index].transform.position = deckTransform.position;
            cardViewField[index].transform.rotation = deckTransform.rotation;

            StartCoroutine(
                cardViewField[index].PlayAnimation(
                    fieldDefaultCardPositions[index],
                    scaleOfCommunityCards, true)
            );
        }

        private void ShowFieldKoikoiCard(int index)
        {
            cardViewKoikoiField[index].transform.position = deckTransform.position;
            cardViewKoikoiField[index].transform.rotation = deckTransform.rotation;

            StartCoroutine(
                cardViewKoikoiField[index].PlayAnimation(
                    fieldKoikoiCardPositions[index],
                    scaleOfCommunityCards, true)
            );
        }

        // どの札が入れ替わったか
        private void CheckHandChanged()
        {
            var hands = CardDataBase.GetCardDataListByID(NetworkManager.GetHands());

            if (hands == null || previousHandIDs == null)
                return;

            // こいこい中は別の表示更新処理で管理する
            // こいこい以降は札交換アニメーションは行われない
            if (currentState == TurnState.WaitForKoikoi ||
                currentState == TurnState.ShowFinalResult)
            {
                PlayKoikoiCardAnimation();
                return;
            }

            // こいこい選択がされなく、そのまま状態が終わりに向かう時
            if (currentState == TurnState.WaitForNextRound)
                return;

            int count = Mathf.Min(cardViewPlayer.Count, hands.Count);

            for (int i = 0; i < count; i++)
            {
                if (cardViewPlayer[i] == null)
                    continue;

                if (!cardViewPlayer[i].isChanging &&
                    previousHandIDs[i] != hands[i].CardID)
                {
                    Debug.Log($"Card Changed {i}");

                    previousHandIDs[i] = hands[i].CardID;

                    StartCoroutine(cardViewPlayer[i].ChangeCardAnimation(hands[i]));
                }
            }
        }

        // こいこい用の札交換 - 正確には出す札を選ぶ だが
        private void PlayKoikoiCardAnimation()
        {
            if (cardViewKoikoiField.Count == 0)
                return;

            cardViewKoikoiField[0].StartKoikoiSelectAnimation
            (
                fieldKoikoiCardPositions[0],
                scaleOfCommunityCards
            );

        }

        // カード生成と配るアニメーションも付いてる
        public void CreateAllCardObjects()
        {
            Debug.Log("カードオブジェクトを生成");
            CreatePlayerCards();
            CreateOtherPlayerCards();
            CreateFieldCards();
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

            foreach (var card in cardViewKoikoiField)
            {
                card.StopAllCoroutines();
                Destroy(card.gameObject);
            }

            cardViewField.Clear();
            cardViewPlayer.Clear();
            cardViewOtherPlayer.Clear();
            cardViewKoikoiField.Clear();
            previousHandIDs = null;
        }

        public void SetSelectKoikoiCardInAnimation(CardView card, int[] field)
        {
            // 選択された札を手札からこいこい用の場所へ
            if (card == null)
                return;

            cardViewKoikoiField.Add(card);

            // 山札の2枚を場へ移動したのを反映
            // var field = CardDataBase.GetCardDataListByID(NetworkManager.GetField());

            Debug.Log($"fieldの枚数={field.Length}");

            // fieldの後ろから2枚、field[5],[6]をデッキから移動させる
            int firstDrawIndex = field.Length - 3;

            for (int i = firstDrawIndex; i < firstDrawIndex + 2; i++)
            {
                CardView view = cardFactory.CreateCard(CardDataBase.GetCardDataByID(field[i]));
                view.Owner = CardOwner.Field;

                // こいこい用の
                cardViewKoikoiField.Add(view);
            }

            // 移動された手札を削除
            cardViewPlayer.Remove(card);
        }

        // getter
        // CardViewPlayterリストの引数と一致する札を返す
        public CardView GetPlayerCard(int index)
        {
            return cardViewPlayer[index];
        }

        // CardIDから一致する札を返す
        public CardView GetPlayerCardByID(int id)
        {
            // 見つからなければnullを返す
            return cardViewPlayer.Find(card => card.CardID == id);
        }

        // VisualManager内の補助関数

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

    }
}