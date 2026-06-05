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
        public TurnState currentState; // 現在がどんなターンなのかを管理する
        private TurnState prevState_Debug;
        private TurnState previousState;
        private int round;


        // インスタンス
        private UIManager uiManager;
        private UIDebug uiDebug;
        [SerializeField]private NetworkManager networkManager;

        private void Start()
        {
            
            // ネット対戦にする前段階のデバッグ用initialize
            Players = new PlayerData[4];
            for(int i = 0; i < 4; i++)
            {
                Players[i] = new PlayerData(i);
            }


            Initialize(); 
            SetInstances();
        }

        private void Update()
        {
            if(currentState != previousState)
            {
                OnEnterState(currentState);
                previousState = currentState;
            }

            OnUpdateState(currentState);
        }

        private void OnEnterState(TurnState turnState)
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

                    SetTurnIfMasterClient(TurnState.CreateDeck);
                break;
                
                case TurnState.CreateDeck:
                    if(networkManager.IsMasterClient())
                    {
                        Deck = CardMovementManager.CreateDeck();
                    }
                
                    SetTurnIfMasterClient(TurnState.Shuffling);
                break;

                case TurnState.Shuffling:
                    if(networkManager.IsMasterClient())
                    {
                        CardMovementManager.ShuffleDeck(Deck);
                        // マスタークライアントはシャッフルしたでーたを送信する必要がある
                        networkManager.SetDeck(Deck);
                    }

                    SetTurnIfMasterClient(TurnState.DealCards);
                break;

                case TurnState.DealCards:
                    networkManager.ResetPlayersReady();

                    if(networkManager.IsMasterClient())
                    {
                        CardMovementManager.DealCards(Deck, FieldCard, Players);
                    }
                    SetTurnIfMasterClient(TurnState.ShowField);
                break;

                case TurnState.ShowField: 
                    FieldCardForShow = new List<CardData> {FieldCard[0], FieldCard[1], FieldCard[2]};
                    SetTurnIfMasterClient(TurnState.WaitForFirstChange);
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
                    SetTurnIfMasterClient(TurnState.WaitForNextRound);
                break;

                case TurnState.Other:
                    // ボタン押したらスタート的な
                    // このクラスできてすぐUpdate内をループするのは少し不安なので緩衝材としてOtherを作りました。
                break;

                default:
                break;
            }
        }

        private void OnUpdateState(TurnState turnState)
        {
            switch(turnState)
            {
                case TurnState.WaitForFirstChange:

                    if(networkManager.IsMasterClient())
                    {
                        if(networkManager.IsEveryoneReady())
                        {
                            CardMovementManager.ChangeHandCards(Deck, Players, DiscardPile);
                            // 次のターンに進むのであれこれ処理
                            ShowFirstFieldCard();
                            
                            networkManager.ResetPlayersReady();

                            uiDebug.ShowWillChange();
                            SetTurnIfMasterClient(TurnState.ShowResult);
                        }
                    }
                break;

                case TurnState.WaitForSecondChange:
                
                    if(networkManager.IsMasterClient())
                    {
                        if(networkManager.IsEveryoneReady())
                        {
                            CardMovementManager.ChangeHandCards(Deck, Players, DiscardPile);

                            ShowSecondFieldCard();

                            networkManager.ResetPlayersReady();

                            uiDebug.ShowWillChange();
                            SetTurnIfMasterClient(TurnState.ShowResult);
                        }
                    }
                break;

                

                case TurnState.WaitForNextRound:

                    // 全員がOKボタン押したら次のラウンドへ、とか
                    if(networkManager.IsEveryoneReady())
                    {
                        networkManager.ResetPlayersReady();
                        round++;

                        if(round <= GameConst.ROUND_NUMBER)
                        {
                            // 親が一周したら終わり
                            // シーン遷移とか
                        }

                        SetTurnIfMasterClient(TurnState.BeforeGame);
                    }
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
            currentState = prevState_Debug = TurnState.BeforeGame;
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

        private void SetTurnIfMasterClient(TurnState state)
        {
            if(networkManager.IsMasterClient())
            {
                networkManager.SetTurnState(state);
            }
        }
    }

    public enum TurnState
    {
        Other, // = 0
        BeforeGame,
        CreateDeck,
        Shuffling,
        DealCards,
        ShowField,
        WaitForFirstChange,
        WaitForSecondChange,
        ShowResult,
        WaitForNextRound
    }
}