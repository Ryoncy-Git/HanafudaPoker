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
        public List<CardData> FieldCardForShow;
        public List<CardData> DiscardPile;
        public PlayerData[] Players;
        // 手札はPlayers内にあるので、呼ぶときはPlayers[0].HandCards

        // その他
        public TurnState CurrentState; // 現在がどんなターンなのかを管理する
        private TurnState prevState_Debug;
        public TurnState PreviousState;
        private int round;


        // インスタンス
        // [SerializeField]private UIManager uiManager;
        [SerializeField]private UIDebug uiDebug;
        [SerializeField]private NetworkManager networkManager;

        private void Awake()
        {
            // これは何が何でもぜったい最初にしておきたい
            CurrentState = TurnState.WaitForInitialize;
            PreviousState = TurnState.WaitForInitialize;
        }

        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            if(CurrentState != PreviousState)
            {
                PreviousState = CurrentState;
                OnEnterState(CurrentState);
                Debug.Log($"change state to {CurrentState}");
                SendGameDataIfMasterClient();

                // debug
                uiDebug.ShowState(CurrentState);
                uiDebug.SetTextFieldCards(FieldCardForShow);
                uiDebug.SetTextHandCards(Players);
            }

            OnUpdateState(CurrentState);
        }

        private void OnEnterState(TurnState turnState)
        {
            if(! networkManager.IsMasterClient())
                return;
            
            // ここから先はmaster clientだけが実行、処理する部分


            switch(turnState)
            {
                case TurnState.BeforeGame:
                    ResetFields();

                    CurrentState = TurnState.CreateDeck;
                break;

                case TurnState.CreateDeck:
                    Deck = CardMovementManager.CreateDeck();
                    CardMovementManager.ShuffleDeck(Deck);

                    // Debug.Log("after shuffle");

                    CurrentState = TurnState.DealCards;
                break;

                case TurnState.DealCards:
                    ResetPlayersReady();
                    CardMovementManager.DealCards(Deck, FieldCard, Players);

                    CurrentState = TurnState.ShowField;
                break;

                case TurnState.ShowField: 
                    FieldCardForShow = new List<CardData> {FieldCard[0], FieldCard[1], FieldCard[2]};
                    CurrentState = TurnState.WaitForFirstChange;
                break;

                case TurnState.ShowResult:

                    List<Yaku>[]playerYaku = new List<Yaku>[GameConst.PLAYER_NUMBER];
                    
                    for(int i = 0; i < GameConst.PLAYER_NUMBER; i++)
                    {
                        playerYaku[i] = YakuData.YakuCheck(FieldCard, Players[i].HandCards);
                    }

                    uiDebug.ShowYaku(playerYaku);

                    // なんかいい感じに役とか表示して次へ
                    // 今はデバッグで無条件で次のターンへ


                    // こいこいはこのタイミングで
                    CurrentState = TurnState.WaitForNextRound;
                break;

                case TurnState.Other:
                    // ボタン押したらスタート的な
                    // このクラスできてすぐUpdate内をループするのは少し不安なので緩衝材としてOtherを作りました。
                break;

                case TurnState.WaitForInitialize:
                    if(networkManager.IsMasterClient())
                    {
                        if(IsEveryoneReady())
                        {
                            CurrentState = TurnState.BeforeGame;
                        }
                    }
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
                        if(IsEveryoneReady())
                        {
                            CardMovementManager.ChangeHandCards(Deck, Players, DiscardPile);
                            ShowFirstFieldCard();
                            
                            ResetPlayersReady();

                            uiDebug.ShowWillChange();
                            CurrentState = TurnState.WaitForSecondChange;
                        }
                    }
                break;

                case TurnState.WaitForSecondChange:
                
                    if(networkManager.IsMasterClient())
                    {
                        if(IsEveryoneReady())
                        {
                            CardMovementManager.ChangeHandCards(Deck, Players, DiscardPile);
                            ShowSecondFieldCard();

                            ResetPlayersReady();

                            uiDebug.ShowWillChange();
                            CurrentState = TurnState.ShowResult;
                        }
                    }
                break;

                

                case TurnState.WaitForNextRound:

                    if(networkManager.IsMasterClient())
                    {
                        // 全員がOKボタン押したら次のラウンドへ、とか
                        if(IsEveryoneReady())
                        {
                            ResetPlayersReady();
                            round++;

                            if(round <= GameConst.ROUND_NUMBER)
                            {
                                // 親が一周したら終わり
                                // シーン遷移とか
                            }

                            CurrentState = TurnState.BeforeGame;
                        }
                    }
                break;
            }
        }



        // ーーーーーーーーーーーーこのファイル内のみで使う補助関数たちーーーーーーーーーーーーーーー
        private void Initialize()
        {
            networkManager.SetPlayerNumber();
            Players = new PlayerData[GameConst.PLAYER_NUMBER];

            
            int[] playerActorNumbers = networkManager.GetPlayerActorNumbers();
            for(int seatID = 0; seatID < GameConst.PLAYER_NUMBER; seatID++)
            {
                // Debug.Log("after access to networl managher");
                // Debug.Log($"Players Length = {Players.Length}");
                // Debug.Log($"Player actor number length = {playerActorNumbers.Length}");
                Players[seatID] = new PlayerData(seatID, playerActorNumbers[seatID]);
            }

            Deck = new List<CardData>();
            FieldCard = new List<CardData>();
            DiscardPile = new List<CardData>();
            FieldCardForShow = new List<CardData>();

            networkManager.SetPlayerReady(true);
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

            List<Yaku>[] emptyList = new List<Yaku>[GameConst.PLAYER_NUMBER];
            for(int i = 0; i < emptyList.Length; i++)
            {
                emptyList[i] = new List<Yaku>();
            }
            uiDebug.ShowYaku(emptyList);
        }

        private bool IsEveryoneReady()
        {
            foreach(PlayerData player in Players)
            {
                if(player.IsReady == false)
                    return false;
            }

            return true;
        }

        private void SendGameDataIfMasterClient()
        {
            // まsターくらいあんとだけが呼び出せる関数
            // ゲームデータ（デックや場札）、各プレイヤーの準備完了状況などが変更されたときに必ず呼び出す
            // また、適せん、各クライアントから同期要請があったらこれを返す
            if(networkManager.IsMasterClient())
            {
                int[] deckIDs = CardDataBase.GetIDsByList(Deck);
                int[] fieldIDs = CardDataBase.GetIDsByList(FieldCard);
                int[] discardIDs = CardDataBase.GetIDsByList(DiscardPile);


                int[] hands = new int[GameConst.PLAYER_NUMBER * GameConst.HAND_CARD_NUMBER];

                for(int i = 0; i < GameConst.PLAYER_NUMBER; i++)
                {
                    if(Players[i].HandCards.Count == 0)
                        continue;

                    
                    int[] ids = CardDataBase.GetIDsByList(Players[i].HandCards);

                    for(int j = 0; j < GameConst.HAND_CARD_NUMBER; j++)
                    {
                        hands[i * GameConst.HAND_CARD_NUMBER + j] = ids[j];
                    }
                }

                networkManager.SendGameData
                (
                    CurrentState,
                    PreviousState,
                    deckIDs,
                    fieldIDs,
                    discardIDs,
                    hands
                );
            }
        }

        private void ResetPlayersReady()
        {
            foreach(PlayerData player in Players)
            {
                player.WillChangeCards = new bool[]{false, false, false};
                player.IsReady = false;
            }
        }


    }

    public enum TurnState
    {
        Other, // = 0
        BeforeGame,
        CreateDeck,
        DealCards,
        ShowField,
        WaitForFirstChange,
        WaitForSecondChange,
        ShowResult,
        WaitForNextRound,
        WaitForInitialize
    }
}