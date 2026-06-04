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
        public List<CardData> Deck;
        public List<CardData> FieldCard;
        private List<CardData> FieldCardForShow;
        public List<CardData> DiscardPile;
        public PlayerData[] Players;
        // 手札はPlayers内にあるので、呼ぶときはPlayers[0].HandCards

        // その他
        private TurnState turnState; // 現在がどんなターンなのかを管理する
        private TurnState prevState_Debug;
        private int round;


        // インスタンス
        private UIManager uiManager;
        private UIDebug uiDebug;
        private NetworkManager networkManager;

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
                case TurnState.BeforeGame:
                    ResetFields();
                    turnState = TurnState.CreateDeck;
                break;
                
                case TurnState.CreateDeck:
                    Deck = CardMovementManager.CreateDeck();
                    turnState = TurnState.Shuffling;
                break;

                case TurnState.Shuffling:
                    if(networkManager.IsMasterClient())
                    {
                        CardMovementManager.ShuffleDeck(Deck);
                        // マスタークライアントはシャッフルしたでーたを送信する必要がある

                    }

                    // TurnState.DealCards
                break;

                case TurnState.DealCards:
                    CardMovementManager.DealCards(Deck, FieldCard, Players);
                    FieldCardForShow = new List<CardData> {FieldCard[0], FieldCard[1], FieldCard[2]};

                    networkManager.ResetPlayersReady();

                    turnState = TurnState.WaitForFirstChange;
                break;
                
                
                case TurnState.WaitForFirstChange:

                    if(networkManager.IsEveryoneReady)
                    {
                        CardMovementManager.ChangeHandCards(Deck, Players, DiscardPile);
                        // 次のターンに進むのであれこれ処理
                        ShowFirstFieldCard();
                        
                        networkManager.ResetPlayersReady();

                        uiDebug.ShowWillChange();
                        turnState = TurnState.WaitForSecondChange;
                    }
                break;

                case TurnState.WaitForSecondChange:
                
                    if(networkManager.IsEveryoneReady)
                    {
                        CardMovementManager.ChangeHandCards(Deck, Players, DiscardPile);

                        ShowSecondFieldCard();

                        networkManager.ResetPlayersReady();

                        uiDebug.ShowWillChange();
                        turnState = TurnState.ShowResult;
                    }
                break;

                case TurnState.ShowResult:

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


                    // こいこいはこのタイミングで
                    turnState = TurnState.WaitForNextRound;
                break;

                case TurnState.WaitForNextRound:

                    // 全員がOKボタン押したら次のラウンドへ、とか
                    if(networkManager.IsEveryoneReady)
                    {
                        networkManager.ResetPlayersReady();
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

            return;
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

        private void ResetFields()
        {
            Deck.Clear();
            FieldCard.Clear();
            foreach(PlayerData player in Players)
            {
                player.HandCards.Clear();
            }

            List<Yaku>[] emptyList =
            {
                new List<Yaku>(),
                new List<Yaku>(),
                new List<Yaku>(),
                new List<Yaku>()
            };
            uiDebug.ShowYaku(emptyList);
        }
    }

    public enum TurnState
    {
        Other, // = 0
        BeforeGame,
        CreateDeck,
        Shuffling,
        DealCards,
        WaitForFirstChange,
        WaitForSecondChange,
        ShowResult,
        WaitForNextRound
    }
}