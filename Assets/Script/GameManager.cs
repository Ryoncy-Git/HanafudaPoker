using UnityEngine;
using System.Collections.Generic;

using HanafudaPoker.Cards;
using HanafudaPoker.Players;
using HanafudaPoker.UIs;
using HanafudaPoker.Yakus;

using HanafudaPoker.Animation;

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
        private List<CardView> cardViewPlayer;

        // デッキの位置
        [SerializeField]
        private Transform deckPosition;

        // 場に置かれるカードの位置
        [SerializeField]
        private Transform[] fieldCardPositions;

        // こいこいで追加される札も同様に

        // 手札の位置も後々
        [SerializeField]
        private Transform[] playerCardPositions;


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
                    CardMovementManager.DealCards(Deck, FieldCard, Players);      // ho6:この時に各プレイヤーへカードを配っている

                    FieldCardForShow = new List<CardData> {FieldCard[0], FieldCard[1], FieldCard[2]};

                    // ho6:カード表示
                    for (int i = 0; i < FieldCardForShow.Count; i++)
                    {
                        PlayCardAnimation(i, 'F');
                    }

                    // ho6:プレイヤーのカードを表示 player[0]を参照している。
                    for (int i=0; i < Players[0].HandCards.Count; i++)
                    {
                        PlayCardAnimation(i, 'P');
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
                        PlayCardAnimation(3, 'S');

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
                        PlayCardAnimation(4, 'T');

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
            cardViewPlayer = new List<CardView>();
            return;
        }

        // ho6:カードを表示
        private void PlayCardAnimation(int num, char times)
        {
            GameObject cardObj = Instantiate(cardPrefab);

            CardView view = cardObj.GetComponent<CardView>();
            view.SetCard(FieldCardForShow[num]);

            // 何回目の開示かによって座標を変更
            switch (times)
            {
                // どの時に呼ばれるか Fは最初に場に出される3枚
                case 'F':
                    MoveToTargetPosition(view, deckPosition.position, fieldCardPositions[num].position);

                    // 場のカードを保存
                    cardViewField.Add(view);
                    break;

                // Sは一度目の取引が終わったタイミング
                case 'S':
                    MoveToTargetPosition(view, deckPosition.position, fieldCardPositions[3].position);

                    cardViewField.Add(view);
                    break;

                // Tは2度目の取引が終わったタイミング
                case 'T':
                    MoveToTargetPosition(view, deckPosition.position, fieldCardPositions[4].position);

                    cardViewField.Add(view);
                    break;

                // Pは自分が持ってる3枚
                case 'P':
                    view.SetCard(Players[0].HandCards[num]);

                    MoveToTargetPosition(view, deckPosition.position, playerCardPositions[num].position);
                    
                    // Playerのカードを保存
                    cardViewPlayer.Add(view);
                    break;

                default:
                    Debug.LogError("例外が発生しています");
                    break;
            }

            RotateCardView(view);
        }

        // ho6:カードを回転
        private void RotateCardView(CardView card)
        {
            card.StartCoroutine(card.FlipCard());
        }

        // ho6:カードを移動
        private void MoveToTargetPosition(CardView card, Vector3 deckPos, Vector3 targetPos)
        {
            card.MoveToPosition(deckPos, targetPos);
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

            foreach(CardView card in cardViewPlayer)
            {
                Destroy(card.gameObject);
            }

            cardViewField.Clear();
            cardViewPlayer.Clear();

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