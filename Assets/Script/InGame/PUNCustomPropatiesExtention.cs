using ExitGames.Client.Photon;
using Photon.Realtime;

namespace HanafudaPoker.Network
{
    public static class PunPlayerPropatiesExtentions
    {   
        // Player Custom Propaties
        private const string Key_WillChangeCards = "WillChangeCards";
        private const string Key_IsReady = "IsReady";
        private const string Key_HandCards = "HandCards";
        private const string Key_SeatID = "SeatID";

        private static readonly Hashtable propsToSet = new Hashtable();

        public static void SetIsReady(this Player player, bool state)
        {
            propsToSet[Key_IsReady] = state;
            // photon.localplayer.SetCustomPropaties()ってのがpunで実装されてる
            player.SetCustomPropaties(propsToSet);
            propsToSet.Clear();

        }
    }
}

