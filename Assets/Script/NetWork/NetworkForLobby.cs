using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace HanafudaPoker.Networks
{
    public class NetworkForLobby : MonoBehaviourPunCallbacks
    {
        public TMP_InputField inputField;

        public void OnPressedConnectToRoomOfName()
        {
            // ボタンを押したら
            // Nameの名前の部屋に参加する
            // 尚、部屋の前にgame serverへ接続してから部屋に入る
            string roomName = inputField.text;

            PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions(), TypedLobby.Default);
        }

        public override void OnJoinedRoom()
        {
            // 部屋に入ったら（ゲームサーバーに入ってその後部屋に入ったら）
            // s－ん遷移
            
            SceneManager.LoadScene("InWaitingRoom");
        }
    }
}