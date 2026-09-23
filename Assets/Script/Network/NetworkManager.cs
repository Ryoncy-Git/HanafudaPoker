using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;
using UnityEngine;

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
        // RP (Room Properties) (Only MasterClient can change)
        private const string Key_TurnState = "Turn";
        private const string Key_Round = "Round";
        private const string Key_Deck = "Deck";
        private const string Key_Field = "Field";
        private const string Key_DiscardPile = "Pile";
        private const string Key_WinnerIDBeforeKoikoi = "Winner1";
        private const string Key_WinnerIDAfterKoikoi = "Winner2";
        private const string Key_IsKoikoi = "Iskoikoi";


        // PP (Player Properties)
        private const string Key_SeatID = "SeatID";
        private const string Key_Hands = "Hands";
        private const string Key_IsReady = "Ready";
        private const string Key_WillChangeCards = "Changes";

        // valuable 
        private static readonly Hashtable props = new Hashtable();

        // private functions
        public /*private */static Player GetPlayerBySeatID(int seatID)
        {
            Player player = null;
            var players = PhotonNetwork.PlayerList;
            foreach (Player p in players)
            {
                int id = (p.CustomProperties[Key_SeatID] is int value) ? value : -1;

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
            if (!PhotonNetwork.IsMasterClient)
                return;

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

            if (player == null)
            {
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            }
            else
            {
                player.SetCustomProperties(props);
            }
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
            Player player;

            if (seatID == -1) // �����i�V�Ȃ玩�����g��
            {
                player = PhotonNetwork.LocalPlayer;
            }
            else
            {
                player = GetPlayerBySeatID(seatID);
            }

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
            Player player;

            if (seatID == -1) // �����i�V�Ȃ玩�����g��
            {
                player = PhotonNetwork.LocalPlayer;
            }
            else
            {
                player = GetPlayerBySeatID(seatID);
            }

            if (player == null)
                return new bool[] { false, false, false };

            return (player.CustomProperties[Key_WillChangeCards] is bool[] value) ? value : new bool[] { false, false, false };
        }
        public static int[] GetHands(int seatID = -1)
        {
            Player player;

            if (seatID == -1) // �����i�V�Ȃ玩�����g��
            {
                player = PhotonNetwork.LocalPlayer;
            }
            else
            {
                player = GetPlayerBySeatID(seatID);
            }


            if (player == null)
            {
                Debug.Log("Failed to get hand on ID : " + seatID);
                return new int[] { };
            }
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
    }
}