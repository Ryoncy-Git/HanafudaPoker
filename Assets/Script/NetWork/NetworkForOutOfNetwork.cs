using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace HanafudaPoker.Networks
{
    public class NetworkForOutOfNetwork : MonoBehaviourPunCallbacks
    {
        public void OnPressedButtonConnectToMasterServer()
        {
            // ボタンを押したら
            // マスターサーバーへ接続する
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            // マスターサーバーへ接続ができたら
            // シーンを変更
            SceneManager.LoadScene("InLobby");
        }
    }
}