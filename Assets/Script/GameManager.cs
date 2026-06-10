using UnityEngine;
using System.Collections.Generic;

using HanafudaPoker.Cards;
using HanafudaPoker.Players;
using HanafudaPoker.UIs;
using HanafudaPoker.Yakus;

namespace HanafudaPoker.Games
{
    public class GameManager : MonoBehaviour
    {
        // ココで使う変数を置いておく
        // カード関係
        private List<CardData> Deck;
        public List<CardData> FieldCard;
        private List<CardData> FieldCardForShow;
        private List<CardData> DiscardPile;
        public PlayerData[] Players;
        // 手札はPlayers内にあるので、呼ぶときはPlayers[0].HandCards

        // その他
        private TurnState turnState; // 現在がどんなターンなのかを管理する
        private TurnState prevState_Debug;
        private int round;

        // インスタンス
        private UIManager uiManager;
        private UIDebug uiDebug;

        // ho6:カード表示
        [SerializeField]
        private GameObject cardPrefab;
        private List<CardView> cardViewField;

        private void Start()
        {
            Initialize(); 
            SetInstances();
            // ネット対戦にする前段階のデバッグ用initialize
            Players = new PlayerData[4];
            for(int i = 0; i < 4; i++)
            {
                Players[i] = new PlayerData(i);
            }
        }

        private void Update()
        {
            // デバッグ用
            if(turnState != prevState_Debug)
            {
                uiDebug.ShowState(turnState);
                prevState_Debug = turnState;

                uiDebug.SetTextFieldCards(FieldCardForShow);
                uiDebug.SetTextHandCards(Players);
            }


            switch(turnState)
            {

                // カード配布前
                case TurnState.BeforeGame:
                    // なんかカード配るアニメーションとか
                    ResetFields();
                    Deck = CardMovementManager.CreateDeck();
                    CardMovementManager.ShuffleDeck(Deck);
                    CardMovementManager.DealCards(Deck, FieldCard, Players);

                    FieldCardForShow = new List<CardData> {FieldCard[0], FieldCard[1], FieldCard[2]};

                    // ho6:カード表示
                    for (int i = 0; i < FieldCardForShow.Count; i++)
                    {
                        ViewCardUI(i, 'F');
                    }

                    List<Yaku>[] emptyList =
                    {
                        new List<Yaku>(),
                        new List<Yaku>(),
                        new List<Yaku>(),
                        new List<Yaku>()
                    };

                    uiDebug.ShowYaku(emptyList);
                    turnState = TurnState.WaitForFirstChange;

                    
                    foreach(PlayerData player in Players)
                    {
                        player.IsReady = false;
                        for(int i = 0; i < 3; i++)
                        {
                            player.WillChangeCards[i] = false;
                        }
                    }
                break;
                
                
                // 場に3枚開示、最初の交換タイム
                case TurnState.WaitForFirstChange:

                    if(isEveryoneReady())
                    {
                        CardMovementManager.ChangeHandCards(Deck, Players, DiscardPile);
                        // 次のターンに進むのであれこれ処理
                        ShowFirstFieldCard();

                        // ho6:
                        ViewCardUI(3, 'S');

                        foreach (PlayerData player in Players)
                        {
                            player.IsReady = false;
                            for(int i = 0; i < GameConst.HAND_CARD_NUMBER; i++)
                            {
                                player.WillChangeCards[i] = false;
                            }
                        }
                        uiDebug.ShowWillChange();
                        turnState = TurnState.WaitForSecondChange;
                    }
                break;

                // 追加で1枚開示、最後の交換タイム
                case TurnState.WaitForSecondChange:
                
                    if(isEveryoneReady())
                    {
                        CardMovementManager.ChangeHandCards(Deck, Players, DiscardPile);

                        ShowSecondFieldCard();

                        // ho6:
                        ViewCardUI(4, 'T');

                        foreach (PlayerData player in Players)
                        {
                            player.IsReady = false;
                            for(int i = 0; i < GameConst.HAND_CARD_NUMBER; i++)
                            {
                                player.WillChangeCards[i] = false;
                            }
                        }
                        uiDebug.ShowWillChange();
                        turnState = TurnState.ShowResult;
                    }
                break;

                // 全て開示、役判定
                case TurnState.ShowResult:

                    // debug
                    List<Yaku>[] yakus = 
                    {
                        YakuData.YakuCheck(FieldCard, Players[0].HandCards), 
                        YakuData.YakuCheck(FieldCard, Players[1].HandCards), 
                        YakuData.YakuCheck(FieldCard, Players[2].HandCards), 
                        YakuData.YakuCheck(FieldCard, Players[3].HandCards), 
                    };

                    uiDebug.ShowYaku(yakus);

                    // なんかいい感じに役とか表示して次へ
                    // 今はデバッグで無条件で次のターンへ
                    turnState = TurnState.WaitForNextRound;
                break;

                case TurnState.WaitForNextRound:

                    // 全員がOKボタン押したら次のラウンドへ、とか
                    if(isEveryoneReady())
                    {
                        foreach(PlayerData player in Players)
                        {
                            player.IsReady = false;
                        }
                        round++;

                        if(round <= GameConst.ROUND_NUMBER)
                        {
                            // 親が一周したら終わり
                            // シーン遷移とか
                        }

                        turnState = TurnState.BeforeGame;
                    }
                break;

                case TurnState.Other:
                    // ボタン押したらスタート的な
                    // このクラスできてすぐUpdate内をループするのは少し不安なので緩衝材としてOtherを作りました。
                break;

                default:
                break;
            }
        }



        // ーーーーーーーーーーーーこのファイル内のみで使う補助関数たちーーーーーーーーーーーーーーー

        private void SetInstances()
        {
            uiManager = this.gameObject.GetComponent<UIManager>();
            uiDebug = this.gameObject.GetComponent<UIDebug>();
        }

        private void Initialize()
        {
            turnState = prevState_Debug = TurnState.BeforeGame;
            Deck = new List<CardData>();
            FieldCard = new List<CardData>();
            DiscardPile = new List<CardData>();

            // ho6:UI表示したカードを記録するリスト
            cardViewField = new List<CardView>();
            return;
        }

        // ho6:カードを表示
        private void ViewCardUI(int num, char times)
        {
            GameObject cardObj = Instantiate(cardPrefab);

            // 何回目の開示かによって座標を変更
            if (times == 'F')
            {
                cardObj.transform.position =
                    new Vector3(num * 80.0f - 80.0f, 20f, 90.0f);
            }
            else if (times == 'S')
            {
                cardObj.transform.position =
                    new Vector3(-40.0f, 0f, 90.0f);
            }
            else if (times == 'T')
            {
                cardObj.transform.position =
                    new Vector3(40.0f, 0f, 90.0f);
            }

            CardView view = cardObj.GetComponent<CardView>();
            view.SetCard(FieldCardForShow[num]);

            // 場のカードを保存
            cardViewField.Add(view);

            RotateCardView(view);
        }

        // ho6:カードを回転
        private void RotateCardView(CardView card)
        {
            card.StartCoroutine(card.FlipCard());
        }

        private void ShowFirstFieldCard()
        {
            FieldCardForShow.Add(FieldCard[3]);
            return;
        }

        private void ShowSecondFieldCard()
        {
            FieldCardForShow.Add(FieldCard[4]);
            return;
        }

        private bool isEveryoneReady()
        {
            foreach(PlayerData player in Players)
            {
                // 一人でも準備完了してない人を見つけたら
                if(player.IsReady == false)
                    return false;
            }

            return true;
        }

        private void ResetFields()
        {
            Deck.Clear();
            FieldCard.Clear();

            // ho6:生成したカードUIを削除
            foreach(CardView card in cardViewField)
            {
                Destroy(card.gameObject);
            }

            cardViewField.Clear();

            foreach(PlayerData player in Players)
            {
                player.HandCards.Clear();
            }
        }

    }

    public enum TurnState
    {
        Other, // = 0
        BeforeGame,
        WaitForFirstChange,
        WaitForSecondChange,
        ShowResult,
        WaitForNextRound
    }
}