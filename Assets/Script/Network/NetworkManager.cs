using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;
using UnityEngine;

// ho6:
using System;

namespace HanafudaPoker.Network
{
    /* Room Properties
    int TurnState
    int Round
    int[] Deck
    int[] Field
    (int[] DiscardPile) // �����v��Ȃ�
    */

    /* Player Properties
    int SeatID
    int[] Hands
    bool IsReady
    bool[] WillCardChange
    */


    public static class NetworkManager
    {
        // RP (Room Properties) (Only MasterClient can change) // CAPITAL LETTER
        private const string Key_TurnState = "T";
        private const string Key_Round = "R";
        private const string Key_Deck = "D";
        public const string Key_Field = "F";        // ho6「ほんまごめんpublicにしちゃう」 
        private const string Key_DiscardPile = "P";
        private const string Key_WinnerIDBeforeKoikoi = "W1";
        private const string Key_WinnerIDAfterKoikoi = "W2";
        private const string Key_IsKoikoi = "K"; // -1 = not define, 0 = false, 1 = true
        


        // PP (Player Properties) // small letter
        private const string Key_SeatID = "s";
        private const string Key_Hands = "h";
        private const string Key_IsReady = "r";
        private const string Key_WillChangeCards = "w";
        private const string Key_yaku = "y";    
        private const string Key_Money = "m";
        private const string Key_Bet = "b";
        private const string Key_Insurance = "i";

        // valuable 
        private static readonly Hashtable props = new Hashtable();
        private const int DOESNOT_EXIST = -2; // -1はもう使っているので-2で

        // private functions
        public /*private */static Player GetPlayerBySeatID(int seatID)
        {
            // 変更
            // seatID == -1のとき、nullを返すのではなく、LocalPlayerを返すように変更
            // 理由、こっちの方がコードがシンプルになるので
            Player player = PhotonNetwork.LocalPlayer;

            var players = PhotonNetwork.PlayerList;

            foreach (Player p in players)
            {
                int id = (p.CustomProperties[Key_SeatID] is int value) ? value : DOESNOT_EXIST;

                if (id == seatID)
                {
                    player = p;
                    break;
                }
            }

            return player;
        }

        // setter
        public static void SetUpSeatID()
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            var players = PhotonNetwork.PlayerList;
            int seatID = 0;

            foreach (Player p in players)
            {
                Hashtable props = new Hashtable();
                props[Key_SeatID] = seatID;
                p.SetCustomProperties(props);

                Debug.Log("Player name " + p.NickName + " seatID = " + seatID);
                seatID++;
            }
        }
        public static void SetTurnState(int state)
        {
            // if (!PhotonNetwork.IsMasterClient && !(state == TurnState.WaitForKoikoi))
            //     return;

            Hashtable props = new Hashtable();
            props[Key_TurnState] = state;
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }
        public static void SetRound(int round)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            Hashtable props = new Hashtable();
            props[Key_Round] = round;
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }
        public static void SetDeck(int[] deck)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            Hashtable props = new Hashtable();
            props[Key_Deck] = deck;
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
            ;
        }
        public static void SetField(int[] field)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            Hashtable props = new Hashtable();
            props[Key_Field] = field;
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }
        // �����g��Ȃ�
        // public static void AddDiscardPile(int DiscardPile)
        // {
        //     props[Key_Round] = state;
        //     PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        // }
        
        public static void SetWinnerIDBeforeKoikoi(int seatID)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            Hashtable props = new Hashtable();
            props[Key_WinnerIDBeforeKoikoi] = seatID;
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }
        public static void SetWinnerIDAfterKoikoi(int seatID)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            Hashtable props = new Hashtable();
            props[Key_WinnerIDAfterKoikoi] = seatID;
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }

        public static void SetIsReady(bool state)
        {
            Hashtable props = new Hashtable();
            props[Key_IsReady] = state;
            // ������PP��ύX
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
        public static void SetHands(int[] hands, int seatID = -1)
        {
            // �����������Ă��Ȃ���Ύ����̃J�[�h���A�w�肪����Ύw�肵���l�̎�D��ύX
            Hashtable props = new Hashtable();
            props[Key_Hands] = hands;

            Player player = GetPlayerBySeatID(seatID);

            player.SetCustomProperties(props);
        }
        public static void SetWillChangeCards(bool[] state)
        {
            Hashtable props = new Hashtable();
            props[Key_WillChangeCards] = state;
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }

        public static void SetIsKoikoi(int state)
        {
            Hashtable props = new Hashtable();
            props[Key_IsKoikoi] = state;
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        } 

        public static void SetYaku(bool[] yakus, int seatID = -1)
        {
            Hashtable props = new Hashtable();
            props[Key_yaku] = yakus;

            Player player = GetPlayerBySeatID(seatID);

            player.SetCustomProperties(props);
        }

        public static void SetMoney(int money, int seatID = -1)
        {
            Hashtable props = new Hashtable();
            props[Key_Money] = money;

            Player player = GetPlayerBySeatID(seatID);

            player.SetCustomProperties(props);
        }

        public static void SetBet(int bet, int seatID = -1)
        {
            Hashtable props = new Hashtable();
            props[Key_Bet] = bet;

            Player player = GetPlayerBySeatID(seatID);

            player.SetCustomProperties(props);
        }

        public static void SetInsurace(int insurance, int seatID = -1)
        {
            Hashtable props = new Hashtable();
            props[Key_Insurance] = insurance;

            Player player = GetPlayerBySeatID(seatID);

            player.SetCustomProperties(props);
        }

        // getter
        public static int GetTurnState()
        {
            // Room Properties�ɖړ��Ă̂��̂�����΂�����A�Ȃ����0��Ԃ�
            return (PhotonNetwork.CurrentRoom.CustomProperties[Key_TurnState] is int value) ? value : 0;
        }
        public static int GetRound()
        {
            return (PhotonNetwork.CurrentRoom.CustomProperties[Key_Round] is int value) ? value : 0;
        }
        public static int[] GetDeck()
        {
            return (PhotonNetwork.CurrentRoom.CustomProperties[Key_Deck] is int[] value) ? value : new int[] { };
        }
        public static int[] GetField()
        {
            return (PhotonNetwork.CurrentRoom.CustomProperties[Key_Field] is int[] value) ? value : new int[] { };
        }
        public static bool GetIsReady(int seatID = -1)
        {
            Player player = GetPlayerBySeatID(seatID);

            if (player == null)
                return false;

            // ����ID��Player�̏����󋵂�Ԃ�
            return (player.CustomProperties[Key_IsReady] is bool value) ? value : false;
        }

        public static int GetSeatID(Player player)
        {
            return (player.CustomProperties[Key_SeatID] is int value) ? value : -1;
        }
        public static int GetMySeatID()
        {
            return (PhotonNetwork.LocalPlayer.CustomProperties[Key_SeatID] is int value) ? value : -1;
        }

        public static bool[] GetWillChangeCards(int seatID = -1)
        {
            Player player = GetPlayerBySeatID(seatID);

            if (player == null)
                return new bool[] { false, false, false };

            return (player.CustomProperties[Key_WillChangeCards] is bool[] value) ? value : new bool[] { false, false, false };
        }
        public static int[] GetHands(int seatID = -1)
        {
            Player player = GetPlayerBySeatID(seatID);


            if (player == null)
                return new int[] { };

            return (player.CustomProperties[Key_Hands] is int[] value) ? value : new int[] { };
        }

        public static int GetWinnerIDBeforeKoikoi()
        {
            return (PhotonNetwork.CurrentRoom.CustomProperties[Key_WinnerIDBeforeKoikoi] is int value) ? value : -1;
        }

        public static int GetWinnerIDAfterKoikoi()
        {
            return (PhotonNetwork.CurrentRoom.CustomProperties[Key_WinnerIDAfterKoikoi] is int value) ? value : -1;
        }

        public static int GetIsKoikoi()
        {
            return (PhotonNetwork.CurrentRoom.CustomProperties[Key_IsKoikoi] is int value) ? value : -1;
        }

        public static bool[] GetYaku(int seatID = -1)
        {
            Player player = GetPlayerBySeatID(seatID);

            if (player == null)
            {
                return new bool[] { };
            }

            return (player.CustomProperties[Key_yaku] is bool[] value) ? value : new bool[] { };
        }

        public static int GetMoney(int seatID = -1)
        {
            Player player = GetPlayerBySeatID(seatID);

            if(player == null)
                return 0;

            return (player.CustomProperties[Key_Money] is int value) ? value : 0;
        }

        public static int GetBet(int seatID = -1)
        {
            Player player = GetPlayerBySeatID(seatID);

            if(player == null)
                return 0;
            
            return (player.CustomProperties[Key_Bet] is int value) ? value : 0;
        }

        public static int GetInsurace(int seatID = -1)
        {
            Player player = GetPlayerBySeatID(seatID);

            if(player == null)
                return 0;

            return (player.CustomProperties[Key_Insurance] is int value) ? value : 0;
        }

        // public setter and getter

        public static void SetAllPlayersReady(bool state)
        {
            RPCManager.Instance.SetAllPlayersReady(state);
        }
        public static void SetAllPlayersWillChangeCards(bool state)
        {
            RPCManager.Instance.SetAllPlayersWillChangeCards(state);
        }

        public static bool IsMasterClient()
        {
            return PhotonNetwork.IsMasterClient;
        }

        public static int GetPlayerNumber()
        {
            var list = PhotonNetwork.PlayerList;
            return list.Length;
        }

        // Adder
        public static int AddBet(int delta, int seatID = -1)
        {
            int bet = GetBet(seatID);
            bet += delta;

            SetBet(bet, seatID);
            return bet;
        }

        public static int AddMoney(int delta, int seatID = -1)
        {
            int money = GetMoney(seatID);
            money += delta;

            SetMoney(money, seatID);
            return money;
        }

        // ho6:が勝手に追加
        /*
         * 場の札に、こいこいした時の札が追加されたことを確認するまでは
         * 札を生成してアニメーションを実行することがないようにするために通知を送る 
         */
        public static event Action<int[]> OnFieldUpdated;

        public static bool IsFieldUpdated(Hashtable properties)
        {
            return properties.ContainsKey(Key_Field);
        }

        public static void NotifyFieldUpdated(int[] field)
        {
            OnFieldUpdated?.Invoke(field);
        }

        public static void MoveToResultScene()
        {
            RPCManager.Instance.MoveToResultScene();
        }
    }
}