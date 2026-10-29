using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace HanafudaPoker.Networks
{
    public class NetworkForOutOfNetwork : MonoBehaviourPunCallbacks
    {
        // 接続までUI遷移
        // パネル管理用の配列
        [SerializeField] private GameObject[] allPanels;
        // 0 -> title
        // 1 -> NowConnecting
        // 2 -> Lobby
        // 3 -> Back

        private Stack<GameObject> panelHistory = new Stack<GameObject>();

        private void Start()
        {
            panelHistory.Clear();
            AllReset();

            // title
            allPanels[0].SetActive(true);

            panelHistory.Push(allPanels[0]);
        }

        public void OnPressedButtonConnectToMasterServer()
        {
            // ボタンを押したら
            // マスターサーバーへ接続する
            ShowConnecting();

            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            // マスターサーバーへ接続ができたら
            // シーンを変更
            ShowSelect();
        }

        // 直近のパネルに戻る
        public void BackPanel()
        {
            if (panelHistory.Count <= 1)
                return;

            // タイトルに戻る時のみ接続を解除
            GameObject current = panelHistory.Pop();
            GameObject previous = panelHistory.Peek();

            if (PhotonNetwork.IsConnected && previous == allPanels[0])
                PhotonNetwork.Disconnect();

            // 現在のパネル
            current.SetActive(false);

            // 一つ前のパネル
            previous.SetActive(true);
        }

        public void ShowPanel(GameObject panel)
        {
            AllReset();

            panel.SetActive(true);

            if (panelHistory.Count == 0 || panelHistory.Peek() != panel)
            {
                panelHistory.Push(panel);
            }
        }

        public void ShowSelect()
        {
            ShowPanel(allPanels[2]);
            allPanels[3].SetActive(true);
        }

        private void AllReset()
        {
            foreach (var panel in allPanels)
            {
                panel.SetActive(false);
            }
        }

        // 接続中のUIは履歴に残さない
        public void ShowConnecting()
        {
            AllReset();
            allPanels[1].SetActive(true);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("接続解除");
        }
    }
}