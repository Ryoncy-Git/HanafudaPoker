using UnityEngine;
using TMPro;
using System.Collections.Generic;

using HanafudaPoker.Cards;
// using HanafudaPoker.Players;
using HanafudaPoker.Games;
using HanafudaPoker.Yakus;
using HanafudaPoker.Network;

namespace HanafudaPoker.UIs
{
    public class UIDebug : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI fieldCardsText;

        [SerializeField] private TextMeshProUGUI[] handCardsText;
        [SerializeField] private TextMeshProUGUI stateText;
        [SerializeField] private TextMeshProUGUI[] yakuText;
        [SerializeField] private GameObject[] willChangeMarker_0; // [SeatID][index]
        [SerializeField] private GameObject[] willChangeMarker_1; // [SeatID][index]
        [SerializeField] private GameObject[] willChangeMarker_2; // [SeatID][index]
        [SerializeField] private GameObject[] willChangeMarker_3; // [SeatID][index]
                                                                  // private GameManager gameManager;
                                                                  
        [SerializeField] private GameObject KoikoiUI;
        [SerializeField] private GameObject KoikoiUI_selectCard;
        [SerializeField] private GameObject KoikoiUI_Button;


        private static readonly Dictionary<CardMonth, string> dictMonth = new()
        {
            { CardMonth.Matsu, "��" },
            { CardMonth.Ume, "�~" },
            { CardMonth.Sakura, "��" },
            { CardMonth.Fuji, "��" },
            { CardMonth.Ayame, "�Ҋ�" },
            { CardMonth.Botan, "���O" },
            { CardMonth.Hagi, "��" },
            { CardMonth.Susuki, "�" },
            { CardMonth.Kiku, "�e" },
            { CardMonth.Momiji, "�g�t" },
            { CardMonth.Yanagi, "��" },
            { CardMonth.Kiri, "��" }
        };

        private static readonly Dictionary<CardRank, string> dictRank = new()
        {
            { CardRank.Hikari, "��" },
            { CardRank.Tane, "��" },
            { CardRank.Tanzaku, "�Z��" },
            { CardRank.Kasu, "�J�X" }
        };


        // ---------------�󂯎��֐�����----------------

        public void SetTextFieldCards()
        {
            TurnState currentTurnState = (TurnState)NetworkManager.GetTurnState();
            var field = CardDataBase.GetCardDataListByID(NetworkManager.GetField());

            string str = "";

            if (currentTurnState == TurnState.WaitForFirstChange || currentTurnState == TurnState.ShowField)
            {
                for (int i = 0; i < 3; i++) // ���ڂ̎�D�ύX�̎��͎O������������悤��
                {
                    str += dictMonth[field[i].Month] + " " + dictRank[field[i].Rank] + "\n";
                }
            }
            else if (currentTurnState == TurnState.WaitForSecondChange)
            {
                for (int i = 0; i < 4; i++) // ���ڂ̎�D�ύX�̎��͂S������������悤��
                {
                    str += dictMonth[field[i].Month] + " " + dictRank[field[i].Rank] + "\n";
                }
            }
            else if (currentTurnState == TurnState.ShowResult || currentTurnState == TurnState.WaitForNextRound)
            {
                for (int i = 0; i < 5; i++)
                {
                    str += dictMonth[field[i].Month] + " " + dictRank[field[i].Rank] + "\n";
                }
            }


            fieldCardsText.text = str;
        }

        public void SetTextHandCards()
        {
            string str = "";
            for (int seatID = 0; seatID < GameConst.PLAYER_NUMBER; seatID++)
            {
                str = "";
                var hands = CardDataBase.GetCardDataListByID(NetworkManager.GetHands(seatID));

                if (hands.Count <= 0)
                    return;

                for (int j = 0; j < hands.Count; j++)
                {
                    str += dictMonth[hands[j].Month] + " " + dictRank[hands[j].Rank] + "\n";
                }
                handCardsText[seatID].text = str;
            }
        }

        public void ShowState(TurnState state)
        {
            switch (state)
            {
                case TurnState.WaitForInitialize:
                    stateText.text = "�S���̏�����҂��Ă��܂�";
                    break;

                case TurnState.BeforeGame:
                case TurnState.CreateDeck:
                case TurnState.DealCards:
                case TurnState.ShowField:
                    stateText.text = "�R�D��������";
                    break;

                case TurnState.WaitForFirstChange:
                    stateText.text = "�ς���J�[�h��I��ł��������i1��ځj";
                    break;

                case TurnState.WaitForSecondChange:
                    stateText.text = "�ς���J�[�h��I��ł��������i2��ځj";
                    break;

                case TurnState.ShowResult:
                    stateText.text = "����";
                    break;

                case TurnState.WaitForNextRound:
                    stateText.text = "���̃��E���h�ցi��������������i�݂܂��j";
                    break;

                default:
                    break;
            }
        }

        public void ShowYaku(List<Yaku>[] yakus)
        {
            for (int seatID = 0; seatID < GameConst.PLAYER_NUMBER; seatID++)
            {
                if (yakus[seatID] == null)
                {
                    yakuText[seatID].text = "";
                    // Debug.Log($"Num {i} is null List");
                    continue;
                }

                // Debug.Log($"Check for {i} Player");
                // Debug.Log($"Yaku Num = {yakus[i].Count}");

                string tex = "";
                if (yakus[seatID].Contains(Yaku.Tsui))
                    tex += "��\n";

                if (yakus[seatID].Contains(Yaku.Nitsui))
                    tex += "���\n";

                if (yakus[seatID].Contains(Yaku.Santsui))
                    tex += "�O��\n";

                if (yakus[seatID].Contains(Yaku.Yontsui))
                    tex += "�l��\n";

                if (yakus[seatID].Contains(Yaku.Mangetsu))
                    tex += "����\n";

                if (yakus[seatID].Contains(Yaku.Akatan))
                    tex += "�ԃ^��\n";

                if (yakus[seatID].Contains(Yaku.Aotan))
                    tex += "�^��\n";

                if (yakus[seatID].Contains(Yaku.Tan))
                    tex += "�^��\n";

                if (yakus[seatID].Contains(Yaku.Gokou))
                    tex += "�܌�\n";

                if (yakus[seatID].Contains(Yaku.Yonkou))
                    tex += "�l��\n";

                if (yakus[seatID].Contains(Yaku.Ameshikou))
                    tex += "�J�l��\n";

                if (yakus[seatID].Contains(Yaku.Sankou))
                    tex += "�O��\n";

                if (yakus[seatID].Contains(Yaku.Inoshikacho))
                    tex += "������\n";

                if (yakus[seatID].Contains(Yaku.Sakeutage))
                    tex += "����\n";

                if (yakus[seatID].Contains(Yaku.Mizu))
                    tex += "����\n";

                if (yakus[seatID].Contains(Yaku.Murasaki))
                    tex += "������\n";

                if (yakus[seatID].Contains(Yaku.Hanaikada))
                    tex += "�Ԕ�\n";

                if (yakus[seatID].Contains(Yaku.Adabana))
                    tex += "�k��\n";

                if (yakus[seatID].Contains(Yaku.Chidori))
                    tex += "�璹\n";

                if (yakus[seatID].Contains(Yaku.MidareChidori))
                    tex += "����璹\n";

                if (yakus[seatID].Contains(Yaku.Houou))
                    tex += "�P��\n";

                if (yakus[seatID].Contains(Yaku.Hououraigi))
                    tex += "�P�����V\n";

                yakuText[seatID].text = tex;
            }
        }

        public void ShowWillChange()
        {
            // �����̎��́A������������Player ID�Ő���?
            // PlayerData player = gameManager.Players[0];
            bool[] willchange = NetworkManager.GetWillChangeCards();

            if (willchange.Length == 0)
                return;

            int seatID = NetworkManager.GetMySeatID();

            for (int i = 0; i < GameConst.HAND_CARD_NUMBER; i++)
            {
                switch (seatID)
                {
                    case 0:
                        willChangeMarker_0[i].SetActive(willchange[i]);
                        break;

                    case 1:
                        willChangeMarker_1[i].SetActive(willchange[i]);
                        break;

                    case 2:
                        willChangeMarker_2[i].SetActive(willchange[i]);
                        break;

                    case 3:
                        willChangeMarker_3[i].SetActive(willchange[i]);
                        break;
                }

            }
        }

        public void SetActiveKoikoiUI(bool state)
        {
            if(state == true)
            {
                KoikoiUI.SetActive(true);
            }
            else
            {
                KoikoiUI_Button.SetActive(true);
                KoikoiUI_selectCard.SetActive(false);
                KoikoiUI.SetActive(false);
                
            }
        }

        public void ShowKoikoiSelectCard()
        {
            KoikoiUI.SetActive(true);
            KoikoiUI_Button.SetActive(false);
            KoikoiUI_selectCard.SetActive(true);
        }


        // ---------------����֐�����----------------
        // public void SelectCard(int n)
        // {
        //     // gameManager.Players[0].WillChangeCards[n] = ! gameManager.Players[0].WillChangeCards[n];
        // }
    }
}