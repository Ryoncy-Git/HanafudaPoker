using UnityEngine;
using System.Collections.Generic;
using System.Collections;

using HanafudaPoker.Network;
using HanafudaPoker.Games;
using HanafudaPoker.Yakus;

/*

    WaitForInitialize
    BeforeGame
    CreateDeck
    DealCards
    ShowField
    WaitForFirstChange
    WaitForSecondChange
    ShowYaku
    WaitForKoikoi
    ShowFinalResult
    WaitForNextRound
    
    �Q�[���͏�L�̏�ԑJ�ڂ��s���Ă���
    
    uiYaku�FShowyaku, ShowFinalResult�̎�
    uiKoikoi:WaitForKoikoi�̎�
    uiButton:BeforeGame~WaitForSecondChange, WaitForNextRound��t�@
    
    �ɕ\������B
*/

// ho6:
namespace HanafudaPoker.UIs
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI�p�l���Ǘ�")]
        [SerializeField] private UIYaku     uiYaku;
        [SerializeField] private UIKoikoi   uiKoikoi;
        [SerializeField] private UIButton   uiButton;

        // UIManager�͌��݂̏�ԂŐ��䂳���
        private TurnState previousState = TurnState.Other;
        private int previousKoikoiChoice = -2;

        // ��\�����I���܂ł͂��������{�^���ւ̓��͎͂󂯕t���Ȃ�
        private bool canUpdateKoikoiUI = false;

        // Croutine���I���O��BeforeGame�Ɉڂ�����I��������
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
            TurnState currentState = (TurnState)NetworkManager.GetTurnState();

            // �Q�[���S�̂̏�ԕω�
            if (currentState != previousState)
            {
                previousState = currentState;
                OnStateChanged(currentState);
            }

            // ����TurnState���ł̑I��ω����m�F
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
                case TurnState.WaitForInitialize:
                    uiButton.Show();
                    break;

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

                    uiYaku.Hide();
                    uiKoikoi.Hide();
                    uiButton.Show();

                    break;

                case TurnState.ShowField:
                    break;

                case TurnState.ShowResult:
                    previousKoikoiChoice = -2;

                    if (yakuCoroutineBeforeKoikoi != null)
                        StopCoroutine(yakuCoroutineBeforeKoikoi);

                    // �����������邩�\��
                    yakuCoroutineBeforeKoikoi = StartCoroutine(ShowYakuThenKoikoiUI(seatID));

                    break;

                case TurnState.WaitForKoikoi:
                    uiButton.Hide();
                    break;

                case TurnState.ShowFinalResult:
                    uiKoikoi.Hide();
                    break;

                case TurnState.WaitForNextRound:

                    if (yakuCoroutineAfterKoikoi != null)
                        StopCoroutine(yakuCoroutineAfterKoikoi);

                    // �����������܂߂���
                    yakuCoroutineAfterKoikoi = StartCoroutine(ShowFinalYaku(seatID));

                    break;

            }
        }

        /*
         * ����\���E�����������邩��₤�^�C�~���O 
         * 
         * ������������O�F����\��->�����������邩�H
         * 
         * ��������������F����\��->���̃��E���h�@or�@���ʔ��\
         * 
         */


        // ������������O�̖����o���B���̌�ɂ����������邩����
        private IEnumerator ShowYakuThenKoikoiUI(int seatID)
        {
            canUpdateKoikoiUI = false;

            yield return ShowYakuBeforeKoikoi(seatID, 3f);

            canUpdateKoikoiUI = true;
            UpdateKoikoiUI();

            yakuCoroutineBeforeKoikoi = null;
        }

        // �����������āA�����o��
        private IEnumerator ShowFinalYaku(int seatID)
        {
            yield return new WaitForSeconds(3f);

            // ����\������
            uiYaku.Show(GetPlayerYaku(seatID));

            // 2�b��Ɂu���ցv�{�^����\��
            yield return new WaitForSeconds(2f);
            uiButton.Show();

            // �\���J�n���獇�v10�b�ɂȂ�܂őҋ@
            yield return new WaitForSeconds(8f);

            // ������ā@�܂��{�^����������ĂȂ���Ύ����I�Ɏ���
            uiYaku.Hide();

            yakuCoroutineAfterKoikoi = null;

            // TODO: ���̃��E���h�ւ̐i�s����
        }



        /*-- �⏕�֐� --*/

        private IEnumerator ShowYakuBeforeKoikoi(int seatID, float displaySeconds)
        {
            yield return new WaitForSeconds(3f);

            uiYaku.Show(GetPlayerYaku(seatID));

            yield return new WaitForSeconds(displaySeconds);

            uiYaku.Hide();
        }

        private void UpdateKoikoiUI()
        {
            int mySeatID = NetworkManager.GetMySeatID();
            int winnerID = NetworkManager.GetWinnerIDBeforeKoikoi();

            // �I�����̂Ȃ��v���C���[�ɂ͕\�����Ȃ�
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
                case -1:    // �����������邩�I��
                    uiKoikoi.ShowKoikoiChoice();
                    break;

                case 1:     // ������������
                    uiKoikoi.ShowCardSelection();
                    break;

                case 0:     // �����������Ȃ�
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
