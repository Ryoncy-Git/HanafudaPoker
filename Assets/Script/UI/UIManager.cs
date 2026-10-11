using UnityEngine;
using System.Collections.Generic;
using System.Collections;

using HanafudaPoker.Network;
using HanafudaPoker.Games;
using HanafudaPoker.Yakus;

// ho6:
namespace HanafudaPoker.UIs
{
    public class UIManager : MonoBehaviour
    {
        [Header("UIパネル管理")]
        [SerializeField] private UIYaku uiYaku;
        [SerializeField] private UIKoikoi uiKoikoi;
        [SerializeField] private UIButton uiButton;

        // UIManagerは現在の状態で制御される
        private TurnState previousState = TurnState.Other;
        private int previousKoikoiChoice = -2;

        // 役表示が終わるまではこいこいボタンへの入力は受け付けない
        private bool canUpdateKoikoiUI = false;

        // Croutineが終わる前にBeforeGameに移ったら終了させる
        private Coroutine yakuCoroutineBeforeKoikoi;
        private Coroutine yakuCoroutineAfterKoikoi;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            uiKoikoi.Init();
            uiYaku.Init();
            uiButton.Init();
        }

        private void Update()
        {
            TurnState currentState =
                (TurnState)NetworkManager.GetTurnState();

            // ゲーム全体の状態変化
            if (currentState != previousState)
            {
                previousState = currentState;
                OnStateChanged(currentState);
            }

            // 同じTurnState内での選択変化も確認
            if (currentState == TurnState.WaitForKoikoi && canUpdateKoikoiUI)
            {
                UpdateKoikoiUI();
            }
        }

        private void OnStateChanged(TurnState state)
        {
            int seatID = NetworkManager.GetMySeatID();

            switch (state)
            {
                case TurnState.BeforeGame:

                    if (yakuCoroutineAfterKoikoi != null)
                    {
                        StopCoroutine(yakuCoroutineAfterKoikoi);
                        yakuCoroutineAfterKoikoi = null;
                    }

                    if (yakuCoroutineBeforeKoikoi != null)
                    {
                        StopCoroutine(yakuCoroutineBeforeKoikoi);
                        yakuCoroutineBeforeKoikoi = null;
                    }

                    canUpdateKoikoiUI = false;
                    uiKoikoi.Hide();
                    uiYaku.CloseYaku();
                    //uiButton.HideStateButtonUI();
                    break;

                case TurnState.ShowField:
                    //uiButton.ShowStateButtonUI();
                    break;

                case TurnState.ShowResult:
                    previousKoikoiChoice = -2;

                    if (yakuCoroutineBeforeKoikoi != null)
                        StopCoroutine(yakuCoroutineBeforeKoikoi);

                    // こいこいするか表示
                    yakuCoroutineBeforeKoikoi = StartCoroutine(ShowYakuThenKoikoiUI(seatID));
                    break;

                case TurnState.WaitForKoikoi:
                    //uiButton.HideStateButtonUI();
                    break;

                case TurnState.ShowFinalResult:
                    uiKoikoi.Hide();
                    //uiButton.ShowStateButtonUI();
                    break;

                case TurnState.WaitForNextRound:

                    if (yakuCoroutineAfterKoikoi != null)
                        StopCoroutine(yakuCoroutineAfterKoikoi);

                    // こいこいも含めた役
                    yakuCoroutineAfterKoikoi = StartCoroutine(ShowFinalYaku(seatID));

                    break;

            }
        }

        /*
         * 役を表示・こいこいするかを問うタイミング 
         * 
         * こいこいする前：役を表示->こいこいするか？
         * 
         * こいこいした後：役を表示->次のラウンド　or　結果発表
         * 
         */


        // こいこいする前の役を出す。その後にこいこいするか聞く
        private IEnumerator ShowYakuThenKoikoiUI(int seatID)
        {
            canUpdateKoikoiUI = false;

            yield return ShowYakuAfterDelayBeforeKoikoi(seatID, 3f);

            canUpdateKoikoiUI = true;
            UpdateKoikoiUI();

            yakuCoroutineBeforeKoikoi = null;
        }

        // こいこいして、役を出す
        private IEnumerator ShowFinalYaku(int seatID)
        {
            yield return ShowYakuAfterDelayAfterKoikoi(seatID, 100f);

            yakuCoroutineAfterKoikoi = null;
            // 次のラウンドへの進行処理
        }



        /*-- 補助関数 --*/

        private IEnumerator ShowYakuAfterDelayBeforeKoikoi(int seatID, float displaySeconds)
        {
            yield return new WaitForSeconds(3f);

            uiYaku.ShowYaku(GetPlayerYaku(seatID));

            yield return new WaitForSeconds(displaySeconds);

            uiYaku.CloseYaku();
        }

        private IEnumerator ShowYakuAfterDelayAfterKoikoi(int seatID, float displaySeconds)
        {
            yield return new WaitForSeconds(3f);

            uiYaku.ShowYaku(GetPlayerYaku(seatID));

            yield return new WaitForSeconds(displaySeconds);

            uiYaku.CloseYaku();
        }

        private void UpdateKoikoiUI()
        {
            int mySeatID = NetworkManager.GetMySeatID();
            int winnerID = NetworkManager.GetWinnerIDBeforeKoikoi();

            // 選択権のないプレイヤーには表示しない
            if (mySeatID != winnerID)
            {
                uiKoikoi.WaitKoikoiSelected();
                return;
            }

            int choice = NetworkManager.GetIsKoikoi();

            if (choice == previousKoikoiChoice)
                return;

            previousKoikoiChoice = choice;

            switch (choice)
            {
                case -1:    // こいこいするか選択中
                    uiKoikoi.ShowKoikoiChoice();
                    break;

                case 1:     // こいこいする
                    uiKoikoi.ShowCardSelection();
                    break;

                case 0:     // こいこいしない
                    uiKoikoi.Hide();
                    break;
            }
        }

        private List<Yaku> GetPlayerYaku(int seatID)
        {
            bool[] flags = NetworkManager.GetYaku(seatID);
            List<Yaku> yakus = new List<Yaku>();

            for (int i = 0; i < flags.Length; i++)
            {
                if (flags[i])
                    yakus.Add((Yaku)i);
            }

            return yakus;
        }
    }
}
