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
        [SerializeField] private GameObject startButton;
        private Player[] playerList;
        public void Start()
        {
            // debug
            // PhotonNetwork.ConnectUsingSettings(); Room�ɓ��������_�Őڑ��ς݂�����s�K�v�H
            UpdatePlayerList();

            if (PhotonNetwork.IsMasterClient)
            {
                startButton.SetActive(true);
            }
            else
            {
                startButton.SetActive(false);
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            UpdatePlayerList();
        }

        public override void OnPlayerLeftRoom(Player newPlayer)
        {
            UpdatePlayerList();

            if (PhotonNetwork.IsMasterClient)
            {
                startButton.SetActive(true);
            }
            else
            {
                startButton.SetActive(false);
            }
        }

        [PunRPC]
        public void UpdatePlayerList()
        {
            playerList = PhotonNetwork.PlayerList;
            playerNameListText.text = "";

            foreach (var player in playerList)
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

        public void OnPressedStartGame()
        {
            photonView.RPC(nameof(RPC_StartGame), RpcTarget.All);
        }

        [PunRPC]
        private void RPC_StartGame()
        {
            SceneManager.LoadScene("Game");
        }

        [PunRPC]
        private void RPCSendMessage(string message)
        {
            Debug.Log(message + " via photon");
        }

        // ho6:
        // �����𔲂��鎞
        public void OnPressedBackToTitle()
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnLeftRoom()
        {
            PhotonNetwork.Disconnect();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            SceneManager.LoadScene("OutOfNetwork");
        }

        // ---------------for debug--------------

        // �}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
        public override void OnConnectedToMaster()
        {
            // "Room"�Ƃ������O�̃��[���ɎQ������i���[�������݂��Ȃ���΍쐬���ĎQ������j
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