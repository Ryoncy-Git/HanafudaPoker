using UnityEngine;
using System.Collections.Generic;

using HanafudaPoker.Cards;
using HanafudaPoker.UIs;
using HanafudaPoker.Yakus;

namespace HanafudaPoker.Games
{
    public class GameManager : MonoBehaviour
    {

        // その他
        public TurnState CurrentState; // 現在がどんなターンなのかを管理する
        public TurnState PreviousState;


        // インスタンス
        // [SerializeField]private UIManager uiManager;
        [SerializeField]private UIDebug uiDebug;

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
                    NetworkManager.SetField(null);

                    CurrentState = TurnState.CreateDeck;
                break;

                case TurnState.CreateDeck:
                    var deck = CardMovementManager.CreateDeck();
                    CardMovementManager.ShuffleDeck(deck);
                    // SetDeckはCMMの方でやる

                    CurrentState = TurnState.DealCards;
                break;

                case TurnState.DealCards:
                    SetAllPlayersReady(false);
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
                    if(IsEveryoneReady())
                    {
                        
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
            NetworkManager.SetTurn((int)TurnState.WaitForInitialize);
            GameConst.PLAYER_NUMBER = 
            NetworkManager.SetIsReady(false);
            NetworkManager.SetWillChangeCards(new bool{false, false, false});


            // 空データを入れる作業はここでは　要らないかも
            // var Deck = new List<CardData>();
            // var FieldCard = new List<CardData>();
            // NetworkManager.SetDeck(CardDataBase.GetIDsByList(Deck));

        }

        private void IsEveryoneReady()
        {
            for(int seatID; seatID < GameConst.PLAYER_NUMBER; seatID++)
            {
                
            }
        }

        private void SetAllPlayersReady(bool state)
        {
            if(! PhotonNetwork.IsMasterClient)
                return;

            NetworkManager.SetAllPlayersReady(state);
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