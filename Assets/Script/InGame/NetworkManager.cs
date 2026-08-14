using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

namespace HanafudaPoker.Network
{
    /* Room Properties
    int TurnState
    int Round
    int[] Deck
    int[] Field
    (int[] DiscardPile) // 多分要らない
    */

    /* Player Properties
    int SeatID
    int[] Hands
    bool IsReady
    int WillCardChange (3bit bool, 000 ~ 111)
    */


    public static class NetworkManager
    {
        // RP (Room Properties)
        private const string Key_TurnState = "Turn";
        private const string Key_Round = "Round";
        private const string Key_Deck = "Deck";
        private const string Key_Field = "Field";
        private const string Key_DiscardPile = "Pile";


        // PP (Player Properties)
        private const string Key_SeatID = "SeatID";
        private const string Key_Hands = "Hands";
        private const string Key_IsReady = "Ready";
        private const string Key_WillChangeCards = "Changes";

        // valuable 
        private static readonly Hashtable props = new Hashtable();  
        
        // private functions
        private static Player GetPlayerBySeatID(int seatID)
        {
            Player player = null;
            var players = PhotonNetwork.PlayerList;
            foreach(Player p in players)
            {
                int id = (p.CustomProperties[Key_SeatID] is int value) ? value : -1;

                if(id == seatID)
                {
                    player = p;
                    break;
                }
            }

            return player;
        }

        // setter
        public static void SetTurnState(int state)
        {
            Hashtable props = new Hashtable();
            props[Key_TurnState] = state;
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }
        public static void SetRound(int round)
        {
            Hashtable props = new Hashtable();
            props[Key_Round] = round;
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }
        public static void SetDeck(int[] deck)
        {
            Hashtable props = new Hashtable();
            props[Key_Deck] = deck;
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
             ;
        }
        public static void SetField(int[] field)
        {
            Hashtable props = new Hashtable();
            props[Key_Field] = field;
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }
        // 多分使わない
        // public static void AddDiscardPile(int DiscardPile)
        // {
        //     props[Key_Round] = state;
        //     PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        // }

        public static void SetIsReady(bool state)
        {
            Hashtable props = new Hashtable();
            props[Key_IsReady] = state;
            // 自分のPPを変更
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
        public static void SetHands(int[] hands)
        {
            Hashtable props = new Hashtable();
            props[Key_Hands] = hands;
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }
        public static void SetWillChangeCards(int state)
        {
            Hashtable props = new Hashtable();
            props[Key_WillChangeCards] = state;
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }

        // getter
        public static int GetTurnState()
        {
            // Room Propertiesに目当てのものがあればそれを、なければ0を返す
            return (PhotonNetwork.CurrentRoom.CustomProperties[Key_TurnState] is int value) ? value : 0;
        }
        public static int GetRound()
        {
            return (PhotonNetwork.CurrentRoom.CustomProperties[Key_Round] is int value) ? value : 0;
        }
        public static int[] GetDeck()
        {
            return (PhotonNetwork.CurrentRoom.CustomProperties[Key_Deck] is int[] value) ? value : null;
        }
        public static int[] GetField()
        {
            return (PhotonNetwork.CurrentRoom.CustomProperties[Key_Field] is int[] value) ? value : null;
        }


        public static bool GetIsReady(int seatID)
        {
            Player player = GetPlayerBySeatID(seatID);

            if(player == null)  
                return false;

            // そのIDのPlayerの準備状況を返す
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
        
        public static int GetWillChangeCards(int seatID)
        {
            Player player = GetPlayerBySeatID(seatID);

            if(player == null)
                return 0;
            
            return (player.CustomProperties[Key_WillChangeCards] is int value) ? value : 0;
        }
        public static int[] GetHands(int seatID)
        {
            Player player = GetPlayerBySeatID(seatID);

            if(player == null)
                return null;

            return (player.CustomProperties[Key_Hands] is int[] value) ? value : null;
        }
    }
}