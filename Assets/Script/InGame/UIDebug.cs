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
        [SerializeField]private TextMeshProUGUI fieldCardsText;

        [SerializeField]private TextMeshProUGUI[] handCardsText;
        [SerializeField]private TextMeshProUGUI stateText;
        [SerializeField]private TextMeshProUGUI[] yakuText;
        [SerializeField]private GameObject[] willChangeMarker;
         // private GameManager gameManager;

        private static readonly Dictionary<CardMonth, string> dictMonth = new()
        {
            { CardMonth.Matsu, "松" },
            { CardMonth.Ume, "梅" },
            { CardMonth.Sakura, "桜" },
            { CardMonth.Fuji, "藤" },
            { CardMonth.Ayame, "菖蒲" },
            { CardMonth.Botan, "牡丹" },
            { CardMonth.Hagi, "萩" },
            { CardMonth.Susuki, "芒" },
            { CardMonth.Kiku, "菊" },
            { CardMonth.Momiji, "紅葉" },
            { CardMonth.Yanagi, "柳" },
            { CardMonth.Kiri, "桐" }
        };

        private static readonly Dictionary<CardRank, string> dictRank = new()
        {
            { CardRank.Hikari, "光" },
            { CardRank.Tane, "種" },
            { CardRank.Tanzaku, "短冊" },
            { CardRank.Kasu, "カス" }
        };

        private void Start()
        {
            // gameManager = this.gameObject.GetComponent<GameManager>();
        }


        // ---------------受け取り関数たち----------------
        
        public void SetTextFieldCards(List<CardData> field)
        {
            string str = "";
            for(int i = 0; i < field.Count; i++)
            {
                str += dictMonth[field[i].Month] + " " + dictRank[field[i].Rank] + "\n";
            }

            fieldCardsText.text = str;
        }

        public void SetTextHandCards()
        {
            // var players = PhotonNetwork.PlayerList;

            string str = "";
            for(int seatID = 0; seatID < GameConst.PLAYER_NUMBER; seatID++)
            {
                str = "";
                var handsIDs = NetworkManager.GetHands(seatID);
                var playerName = NetworkManager.GetPlayerBySeatID(seatID).NickName;
                Debug.Log(playerName);
                
                if(handsIDs != null)
                {
                    var hands = CardDataBase.GetCardDataListByID(handsIDs);  

                    for(int j = 0; j < GameConst.HAND_CARD_NUMBER; j++)
                    {
                        str += dictMonth[hands[j].Month] + " " + dictRank[hands[j].Rank] + "\n";
                    }
                }
                handCardsText[seatID].text = str;
            }
        }

        public void ShowState(TurnState state)
        {
            switch(state)
            {
                case TurnState.BeforeGame:
                    stateText.text = "山札を準備中";
                break;

                case TurnState.WaitForFirstChange:
                    stateText.text = "変えるカードを選んでください（1回目）";
                break;

                case TurnState.WaitForSecondChange:
                    stateText.text = "変えるカードを選んでください（2回目）";
                break;

                case TurnState.ShowResult:
                    stateText.text = "結果";
                break;

                case TurnState.WaitForNextRound:
                    stateText.text = "次のラウンドへ（準備完了したら進みます）";
                break;

                default:
                break;
            }
        }

        public void ShowYaku(List<Yaku>[] yakus)
        {
            for(int seatID = 0; seatID < GameConst.PLAYER_NUMBER; seatID++)
            {
                if(yakus[seatID] == null)
                {
                    yakuText[seatID].text = "";
                    // Debug.Log($"Num {i} is null List");
                    continue;
                }

                // Debug.Log($"Check for {i} Player");
                // Debug.Log($"Yaku Num = {yakus[i].Count}");

                string tex = "";
                if(yakus[seatID].Contains(Yaku.Tsui))
                    tex += "対\n";

                if(yakus[seatID].Contains(Yaku.Nitsui))
                    tex += "二対\n";

                if(yakus[seatID].Contains(Yaku.Santsui))
                    tex += "三対\n";

                if(yakus[seatID].Contains(Yaku.Yontsui))
                    tex += "四対\n";

                if(yakus[seatID].Contains(Yaku.Mangetsu))
                    tex += "満月\n";

                if(yakus[seatID].Contains(Yaku.Akatan))
                    tex += "赤タン\n";

                if(yakus[seatID].Contains(Yaku.Aotan))
                    tex += "青タン\n";

                if(yakus[seatID].Contains(Yaku.Tan))
                    tex += "タン\n";

                if(yakus[seatID].Contains(Yaku.Gokou))
                    tex += "五光\n";

                if(yakus[seatID].Contains(Yaku.Yonkou))
                    tex += "四光\n";

                if(yakus[seatID].Contains(Yaku.Ameshikou))
                    tex += "雨四光\n";

                if(yakus[seatID].Contains(Yaku.Sankou))
                    tex += "三光\n";

                if(yakus[seatID].Contains(Yaku.Inoshikacho))
                    tex += "猪鹿蝶\n";

                if(yakus[seatID].Contains(Yaku.Sakeutage))
                    tex += "酒宴\n";

                if(yakus[seatID].Contains(Yaku.Mizu))
                    tex += "水鏡\n";

                if(yakus[seatID].Contains(Yaku.Murasaki))
                    tex += "紫苑花\n";

                if(yakus[seatID].Contains(Yaku.Hanaikada))
                    tex += "花筏\n";

                if(yakus[seatID].Contains(Yaku.Adabana))
                    tex += "徒花\n";

                if(yakus[seatID].Contains(Yaku.Chidori))
                    tex += "千鳥\n";

                if(yakus[seatID].Contains(Yaku.MidareChidori))
                    tex += "乱れ千鳥\n";

                if(yakus[seatID].Contains(Yaku.Houou))
                    tex += "鳳凰\n";

                if(yakus[seatID].Contains(Yaku.Hououraigi))
                    tex += "鳳凰来儀\n";

                yakuText[seatID].text = tex;
            }
        }

        public void ShowWillChange()
        {
            // // 実装の時は、ここを自分のPlayer IDで制御?
            // PlayerData player = gameManager.Players[0];

            // for(int i = 0; i < GameConst.HAND_CARD_NUMBER; i++)
            // {
            //     if(player.WillChangeCards[i])
            //     {
            //         willChangeMarker[i].SetActive(true);
            //     }
            //     else
            //     {
            //         willChangeMarker[i].SetActive(false);
            //     }
            // }
        }


        // ---------------送り関数たち----------------
        // public void SelectCard(int n)
        // {
        //     // gameManager.Players[0].WillChangeCards[n] = ! gameManager.Players[0].WillChangeCards[n];
        // }
    }
}