using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using HanafudaPoker.Cards;
using HanafudaPoker.UIs;
using HanafudaPoker.Yakus;
using HanafudaPoker.Network;

using HanafudaPoker.Animation;
using HanafudaPoker.Bet;

namespace HanafudaPoker.Games
{
    public class GameManager : MonoBehaviour
    {

        // その他
        private TurnState CurrentState; // 現在がどんなターンなのかを管理する
        private TurnState PreviousState;
        private int MySeatID = -1;
        public int koikoiIndex = -1;
        public int selectedKoikoiCardID = -1;
        private int betMoney = 0;
        private int[] score; // スコープまたいで保持したいだけなんで大したものではない

        // インスタンス
        // [SerializeField]private UIManager uiManager;
        [SerializeField] private UIDebug uiDebug;

        [SerializeField] private VisualManager visualManager;
        [SerializeField] private BetFactory betFactory;
        [SerializeField] private CardSelector cardSelector;

        private CardView selectedKoikoiCard;

        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            CurrentState = (TurnState)NetworkManager.GetTurnState();

            if (CurrentState != PreviousState)
            {
                Debug.Log($"change state to {CurrentState}");
                PreviousState = CurrentState;

                if (NetworkManager.IsMasterClient())
                    OnEnterState(CurrentState);
                // OnEnterはマスターのみじっこう

                // debug
                uiDebug.ShowState(CurrentState);
                uiDebug.SetTextFieldCards();
                uiDebug.SetTextHandCards();

            }
            uiDebug.ShowWillChange();
            uiDebug.SetTextFieldCards();

            OnUpdateState(CurrentState);
        }

        private void OnEnterState(TurnState turnState)
        {
            switch (turnState)
            {
                case TurnState.BeforeGame:
                    // ほとんどが初期化社業
                    NetworkManager.SetField(null);
                    // this.ResetGames();
                    NetworkManager.SetIsReady(false);
                    NetworkManager.SetWillChangeCards(new bool[] { false, false, false });
                    NetworkManager.SetIsKoikoi(-1);

                    koikoiIndex = -1;
                    selectedKoikoiCardID = -1;

                    Debug.Log("[GameManager] Before Game");

                    // ho6:
                    visualManager.ResetCardObjects();
                    betFactory.ResetBetObjects();

                    NetworkManager.SetTurnState((int)TurnState.CreateDeck);
                    break;

                case TurnState.CreateDeck:
                    CardMovementManager.CreateAndShuffleDeck();
                    // SetDeckはCMMの方でやる
                    Debug.Log("[GameManager] Create Deck");

                    NetworkManager.SetTurnState((int)TurnState.DealCards);
                    break;

                case TurnState.DealCards:
                    NetworkManager.SetAllPlayersReady(false);
                    CardMovementManager.DealCards();
                    Debug.Log("[GameManager] Deal Cards");

                    NetworkManager.SetTurnState((int)TurnState.ShowField);
                    break;

                case TurnState.ShowField:
                    // ho6:
                    visualManager.CreateAllCardObjects();

                    Debug.Log("[GameManager] Show Field");

                    NetworkManager.SetTurnState((int)TurnState.WaitForFirstBet);
                    break;

                case TurnState.WaitForFirstChange:

                    Debug.Log("[GameManager] WaitForFirstChange");

                    break;

                case TurnState.WaitForSecondChange:

                    Debug.Log("[GameManager] WaitForSecondChange");

                    break;

                case TurnState.ShowResult:
                    // マスターが全員の約を判定
                    if (!NetworkManager.IsMasterClient())
                        return;

                    var playerYaku = new List<Yaku>[GameConst.PLAYER_NUMBER];
                    var field = CardDataBase.GetCardDataListByID(NetworkManager.GetField());

                    for (int seatID = 0; seatID < GameConst.PLAYER_NUMBER; seatID++)
                    {
                        var hands = CardDataBase.GetCardDataListByID(NetworkManager.GetHands(seatID));
                        playerYaku[seatID] = YakuData.YakuCheck(field, hands);

                        // send yaku data to network
                        bool[] yakuIDs = YakuDataBase.GetIDsByYakuList(playerYaku[seatID]);
                        NetworkManager.SetYaku(yakuIDs, seatID);
                    }

                    //uiDebug.ShowYaku(playerYaku);


                    Debug.Log("[GameManager] Show Yaku");

                    // 勝者を判定する
                    // int winnerID = 0; // デバッグ用にいったん0で
                    // 役のデータを渡してスコアの計算
                    score = YakuData.CalculateScore(playerYaku);
                    Debug.Log("beforekoikoi score [0] = " + score[0]);
                    int winnerID = YakuData.JudgeWinner(score);

                    NetworkManager.SetWinnerIDBeforeKoikoi(winnerID);


                    // こいこいはこのタイミングで
                    NetworkManager.SetTurnState((int)TurnState.WaitForKoikoi);

                    break;

                case TurnState.WaitForKoikoi:

                    Debug.Log("[GameManager] WaitForKoikoi");

                    break;

                case TurnState.ShowFinalResult:
                    // 処理自体はShowResultとほぼ同じ
                    // マスターが全員の約を判定
                    if (!NetworkManager.IsMasterClient())
                        return;

                    playerYaku = new List<Yaku>[GameConst.PLAYER_NUMBER];
                    field = CardDataBase.GetCardDataListByID(NetworkManager.GetField());

                    for (int seatID = 0; seatID < GameConst.PLAYER_NUMBER; seatID++)
                    {
                        var hands = CardDataBase.GetCardDataListByID(NetworkManager.GetHands(seatID));
                        playerYaku[seatID] = YakuData.YakuCheck(field, hands);

                        bool[] yakuIDs = YakuDataBase.GetIDsByYakuList(playerYaku[seatID]);
                        NetworkManager.SetYaku(yakuIDs, seatID);
                    }

                    uiDebug.ShowYaku(playerYaku);

                    // 勝者を判定する
                    // int winnerID = 0; // デバッグ用にいったん0で
                    // 役のデータを渡してスコアの計算
                    score = YakuData.CalculateScore(playerYaku);
                    Debug.Log("afterkoikoi score [0] = " + score[0]);

                    // winnerIDはこいこい宣言権のためだけに使うのでここでは不要
                    // NetworkManager.SetWinnerIDAfterKoikoi(winnerID);

                    NetworkManager.SetTurnState((int)TurnState.Settlement);

                    Debug.Log("[GameManager] ShowFinalResult");
                    break;

                case TurnState.Settlement: 
                    // 清算のターン
                    // ベットした金額と役の強さによってお金の移動を行う

                    // 清算はマスタークライアントのみ実行
                    if(! NetworkManager.IsMasterClient())
                        return;

                    Settlement.Execute(score);
                    

                    NetworkManager.SetTurnState((int)TurnState.WaitForNextRound);
                    break;

                case TurnState.WaitForNextRound:

                    Debug.Log("[GameManager] WaitForNextRound");

                    break;

                case TurnState.WaitForInitialize:

                    Debug.Log("[GameManager] WaitForInitialize");

                    break;

                case TurnState.WaitSomeSecondForResult:

                    Debug.Log("[GameManager] WaitSomeSecondForResult");

                    break;

                case TurnState.Other:
                    // ボタン押したらスタート的な
                    // このクラスできてすぐUpdate内をループするのは少し不安なので緩衝材としてOtherを作りました。
                    Debug.Log("[GameManager] Other");

                    break;

                default:
                    break;
            }
        }

        private void OnUpdateState(TurnState turnState)
        {
            switch (turnState)
            {
                case TurnState.WaitForFirstChange:
                    // wait player input
                    if (NetworkManager.IsMasterClient())
                    {
                        if (IsEveryoneReady())
                        {
                            CardMovementManager.ChangeHandCards();
                            NetworkManager.SetAllPlayersReady(false);
                            NetworkManager.SetAllPlayersWillChangeCards(false);

                            NetworkManager.SetWillChangeCards(new bool[] { false, false, false });

                            uiDebug.ShowWillChange();
                            NetworkManager.SetTurnState((int)TurnState.WaitForSecondBet);
                        }
                    }

                    break;

                case TurnState.WaitForSecondChange:

                    if (NetworkManager.IsMasterClient())
                    {
                        if (IsEveryoneReady())
                        {
                            CardMovementManager.ChangeHandCards();
                            NetworkManager.SetAllPlayersReady(false);
                            NetworkManager.SetAllPlayersWillChangeCards(false);

                            NetworkManager.SetWillChangeCards(new bool[] { false, false, false });

                            uiDebug.ShowWillChange();
                            NetworkManager.SetTurnState((int)TurnState.WaitForThirdBetAndInsurance);
                        }
                    }

                    break;

                case TurnState.WaitForNextRound:

                    if (NetworkManager.IsMasterClient())
                    {
                        // 全員がOKボタン押したら次のラウンドへ、とか
                        if (IsEveryoneReady())
                        {
                            NetworkManager.SetAllPlayersReady(false);
                            int round = NetworkManager.GetRound();
                            round++;
                            NetworkManager.SetRound(round);

                            NetworkManager.SetWillChangeCards(new bool[] { false, false, false });

                            if (round >= GameConst.ROUND_NUMBER)
                            {
                                // 親が一周したら終わり
                                // シーン遷移とか
                                Debug.Log("round finished");
                                SceneManager.LoadScene("ResultScene");

                            }

                            NetworkManager.SetTurnState((int)TurnState.BeforeGame);
                        }
                    }

                    break;

                case TurnState.WaitForInitialize:
                    if (NetworkManager.IsMasterClient())
                    {
                        if (IsEveryoneReady())
                        {
                            NetworkManager.SetTurnState((int)TurnState.BeforeGame);
                        }
                    }
                    break;

                case TurnState.WaitForKoikoi:
                    int winnerID = NetworkManager.GetWinnerIDBeforeKoikoi();

                    // 勝者以外は札を選べない
                    // 自分が勝者ならこいこいの選択権がある
                    if (NetworkManager.GetMySeatID() == winnerID)
                    {
                        uiDebug.SetActiveKoikoiUI(true);

                        //プレーヤーの入力町
                        if (NetworkManager.GetIsKoikoi() == -1) // 未選択
                            return;

                        if(NetworkManager.GetIsKoikoi() == 1) // こいこいをするなら
                        {
                            if(NetworkManager.GetIsReady())
                            {
                                // ロジック用
                            selectedKoikoiCardID = cardSelector.SelectedKoikoiCardID;
                            
                            Debug.Log($"こいこいの札:{selectedKoikoiCardID}");

                            if (selectedKoikoiCardID != -1)
                            {
                                // アニメーション用
                                CardView selectedCard = visualManager.GetPlayerCardByID(selectedKoikoiCardID);

                                if (selectedCard == null)
                                {
                                    Debug.LogWarning(
                                        $"手札に該当するCardViewがありません: {selectedKoikoiCardID}"
                                    );
                                    return;
                                }

                                // アニメーション的に札の更新
                                selectedKoikoiCard = selectedCard;

                                // ロジック的に札の更新
                                CardMovementManager.AddToFieldAsKoikoi(selectedKoikoiCardID);

                                NetworkManager.SetTurnState((int)TurnState.WaitForKoikoiBetAndInsurance);
                                uiDebug.SetActiveKoikoiUI(false);
                            }
                            
                            // ボタン制御
                            //if (koikoiIndex != -1 /*何かカードを選択しているなら */)
                            //{
                            //    CardMovementManager.AddToFieldAsKoikoiButton(koikoiIndex);
                            //    NetworkManager.SetTurnState((int)TurnState.ShowFinalResult);
                            //    uiDebug.SetActiveKoikoiUI(false);
                            //}
                            }
                        }
                        else //(NetworkManager.GetIskoikoi() == 0)
                        {
                            Debug.Log("こいこいしないを選択");
                            NetworkManager.SetTurnState((int)TurnState.Settlement);
                            uiDebug.SetActiveKoikoiUI(false);
                        }
                    }
                    break;

                    case TurnState.WaitForFirstBet:
                        // プレイヤーのベットを待つ

                        // 全員のベットが終了し、全員の準備ができたら次のステートに移動

                        // ho6ho6へ
                        // ベットはWaitFor~~Betの時だけできるようにしてほしいのでUIのボタンを出すタイミングをそちらで実装してください
                        // 意訳：関係ないときにベットできないようにしてほしい

                        // 追加関数
                        // NetworkManager.GetBet(int seatID = -1)
                        // NetworkManager.SetBet(int bet, seatID = -1)

                        if (NetworkManager.IsMasterClient())
                        {
                            if (IsEveryoneReady())
                            {
                                NetworkManager.SetAllPlayersReady(false);
                                NetworkManager.SetAllPlayersWillChangeCards(false);
                                NetworkManager.SetWillChangeCards(new bool[] { false, false, false });

                                NetworkManager.SetTurnState((int)TurnState.WaitForFirstChange);
                            }
                        }
                    break;

                    case TurnState.WaitForSecondBet:
                        // プレイヤーのベットを待つ

                        // 全員のベットが終了し、全員の準備ができたら次のステートに移動
                        if (NetworkManager.IsMasterClient())
                        {
                            if (IsEveryoneReady())
                            {
                                NetworkManager.SetAllPlayersReady(false);
                                NetworkManager.SetAllPlayersWillChangeCards(false);
                                NetworkManager.SetWillChangeCards(new bool[] { false, false, false });

                                NetworkManager.SetTurnState((int)TurnState.WaitForSecondChange);
                            }
                        }
                    break;

                    case TurnState.WaitForThirdBetAndInsurance:
                        // プレイヤーのベットを待つ

                        // 全員のベットが終了し、全員の準備ができたら次のステートに移動
                        if (NetworkManager.IsMasterClient())
                        {
                            if (IsEveryoneReady())
                            {
                                NetworkManager.SetAllPlayersReady(false);
                                NetworkManager.SetAllPlayersWillChangeCards(false);
                                NetworkManager.SetWillChangeCards(new bool[] { false, false, false });

                                NetworkManager.SetTurnState((int)TurnState.ShowResult);
                            }
                        }
                    break;
                    
                    case TurnState.WaitForKoikoiBetAndInsurance:
                        // プレイヤーのベットを待つ

                        // 全員のベットが終了し、全員の準備ができたら次のステートに移動
                        if (NetworkManager.IsMasterClient())
                        {
                            if (IsEveryoneReady())
                            {
                                NetworkManager.SetAllPlayersReady(false);
                                NetworkManager.SetAllPlayersWillChangeCards(false);
                                NetworkManager.SetWillChangeCards(new bool[] { false, false, false });
                                
                                NetworkManager.SetTurnState((int)TurnState.ShowFinalResult);
                            }
                        }
                    break;
            }
        }

        // ho6:ネットワークから場札がこいこいも含めて更新されたことを受け取る
        private void OnEnable()
        {
            NetworkManager.OnFieldUpdated += HandleFieldUpdated;
        }

        private void OnDisable()
        {
            NetworkManager.OnFieldUpdated -= HandleFieldUpdated;
        }

        // ネットワーク上で場札がこいこいも含めて更新されたことを受け取る
        // ゲームオブジェクトを保持するようのリストに伝える
        private void HandleFieldUpdated(int[] field)
        {
            Debug.Log($"GameManagerが場札更新を受信: {field.Length}枚");

            if (selectedKoikoiCard == null)
            {
                Debug.LogWarning("こいこい用の選択カードが設定されていません");
                return;
            }

            visualManager.SetSelectKoikoiCardInAnimation(
                selectedKoikoiCard,
                field
            );

            selectedKoikoiCard = null;
        }


        // ーーーーーーーーーーーーこのファイル内のみで使う補助関数たちーーーーーーーーーーーーーーー
        private void Initialize()
        {
            NetworkManager.SetTurnState((int)TurnState.WaitForInitialize);
            GameConst.PLAYER_NUMBER = NetworkManager.GetPlayerNumber();
            Debug.Log("Initialize : player number = " + GameConst.PLAYER_NUMBER);
            NetworkManager.SetUpSeatID();
            uiDebug.SetActiveKoikoiUI(false);
            koikoiIndex = -1;
            selectedKoikoiCardID = -1;
            
            NetworkManager.SetBet(0);

            // 自分のseatIDを各クライアントが保存しておく
            // do
            // {
            //     MySeatID = NetworkManager.GetMySeatID();
            // }while(MySeatID == -1);

            // 空データを入れる作業はここでは　要らないかも
            // var Deck = new List<CardData>();
            // var FieldCard = new List<CardData>();
            // NetworkManager.SetDeck(CardDataBase.GetIDsByList(Deck));

        }

        private bool IsEveryoneReady()
        {
            for (int seatID = 0; seatID < GameConst.PLAYER_NUMBER; seatID++)
            {
                if (NetworkManager.GetIsReady(seatID) == false)
                {
                    return false;
                }
            }

            return true;
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
        WaitForKoikoi,
        ShowFinalResult,
        WaitForNextRound,
        WaitForInitialize, 
        WaitSomeSecondForResult,
        Settlement, // 清算するターンを設ける
        WaitForFirstBet,
        WaitForSecondBet,
        WaitForThirdBetAndInsurance,
        WaitForKoikoiBetAndInsurance
    }
}