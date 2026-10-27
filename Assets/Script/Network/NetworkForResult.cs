using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

using HanafudaPoker.Network;
using HanafudaPoker.Games;

namespace HanafudaPoker.Networks
{
    public class NetworkForResult : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TextMeshProUGUI resultText;
        public void OnPressedBackToRoom()
        {
            SceneManager.LoadScene("InWaitngRoom");
        }

        [PunRPC]
        private void RPC_BackToRoom()
        {
            SceneManager.LoadScene("InWaitngRoom");
        }

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


        private void ShowText()
        {
            int[] money = new int[GameConst.PLAYER_NUMBER];

            // for(int i = 0; i < money.Length; i++)
            // {
            //     money[i] = NetworkManager.GetMoney(i);
            // }

            // private playerList = PhotonNetwork.PlayerList;
            
            // resultText.text = "";

            // foreach(int m in money)
            // {
            //     resultText.text += "";
            // }
        }
        
    }
}