using Photon.Realtime;
using ExitGames.Client.Photon;

namespace HanafudaPoker.Network
{
    /* Room Propaties
    int TurnState
    int Round
    int[] Deck
    int[] Field
    int[] DiscardPile
    */

    /* Player Propaties
    int SeatID
    int[] Hands
    bool IsReady
    int WillCardChange (3bit bool, 000 ~ 111)
    */


    public static class NetworkManager
    {
        // RP 
        private const string Key_TurnState = "Turn";
        private const string Key_Round = "Round";
        private const string Key_Deck = "Deck";
        private const string Key_Field = "Field";
        private const string Key_DiscardPile = "Pile";


        // PP
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
                int id = (p.CustomPropaties[Key_SeatID] is int value) ? value : -1;

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
            props[Key_TurnState] = state;
            PhotonNetwork.CurrentRoom.SetCustomPropaties(props);
            props.Clear();
        }

        public static void SetIsReady(bool state)
        {
            props[Key_IsReady] = state;
            // 自分のPPを変更
            PhotonNetwork.LocalPlayer.SetCustomPropaties(props);
            props.Clear();
        }

        // getter
        public static int GetTurnState()
        {
            // Room propatiesに目当てのものがあればそれを、なければ0を返す
            return (PhotonNetwork.CurrentRoom.CustomPropaties[Key_TurnState] is int value) ? value : 0;
        }

        public static bool GetIsReady(int seatID)
        {
            Player player = GetPlayerBySeatID(seatID);

            if(player == null)  
                return false;

            // そのIDのPlayerの準備状況を返す
            return (player.CustomPropaties[Key_IsReady] is bool value) ? value : false;
        }
    }
}