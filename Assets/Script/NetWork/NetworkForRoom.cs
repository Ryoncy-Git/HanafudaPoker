using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace HanafudaPoker.Networks
{ 
    public class NetworkForRoom : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TextMeshProUGUI playerNameListText;
        [SerializeField] private TMP_InputField inputedName;
        private Player[] playerList;
        public void Start()
        {
            // debug
            PhotonNetwork.ConnectUsingSettings();
            UpdatePlayerList();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            UpdatePlayerList();
        }

        public override void OnPlayerLeftRoom(Player newPlayer)
        {
            UpdatePlayerList();
        }

        [PunRPC]
        public void UpdatePlayerList()
        {
            playerList = PhotonNetwork.PlayerList;
            playerNameListText.text = "";

            foreach(var player in playerList)
            {
                playerNameListText.text += player.ActorNumber + "  " + player.NickName + "\n";
            }
        }

        public void OnPressedChangeName()
        {
            PhotonNetwork.NickName = inputedName.text;
            UpdatePlayerList();
            photonView.RPC(nameof(UpdatePlayerList), RpcTarget.All);
        }

        [PunRPC]
        private void RPCSendMessage(string message)
        {
            Debug.Log(message + " via photon");
        }

        // ---------------for debug--------------

        // マスターサーバーへの接続が成功した時に呼ばれるコールバック
        public override void OnConnectedToMaster() {
            // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
            PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
            Debug.Log("Joined To Master Server");
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.NickName = "Player" + PhotonNetwork.LocalPlayer.ActorNumber;
            UpdatePlayerList();
            Debug.Log("Joied To Room");
        }
    }
}