using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ho6:
namespace HanafudaPoker.Title
{
    public class TitleManager : MonoBehaviour
    {
        [SerializeField] private GameObject titlePanel;
        [SerializeField] private GameObject selectPanel;
        [SerializeField] private GameObject hostPanel;
        [SerializeField] private GameObject hostPanel_inRoom;
        [SerializeField] private GameObject clientPanel;
        [SerializeField] private GameObject clientPanel_inHostRoom;

        private Stack<GameObject> panelHistory = new Stack<GameObject>();

        /*-- UIパネルの遷移 --*/
        private void Start()
        {
            panelHistory.Clear();
            ShowTitle();
        }

        public void ShowTitle()
        {
            panelHistory.Clear();
            ShowPanel(titlePanel);
        }

        public void ShowSelect()
        {
            ShowPanel(selectPanel);
        }

        public void ShowHost()
        {
            ShowPanel(hostPanel);
        }

        public void ShowHostRoom()
        {
            ShowPanel(hostPanel_inRoom);
        }

        public void ShowClient()
        {
            ShowPanel(clientPanel);
        }

        public void ShowClientInHostRoom()
        {
            ShowPanel(clientPanel_inHostRoom);
        }

        // 直近のパネルに戻る
        public void BackPanel()
        {
            if (panelHistory.Count <= 1)
                return;

            // 現在のパネル
            GameObject current = panelHistory.Pop();
            current.SetActive(false);

            // 一つ前のパネル
            GameObject previous = panelHistory.Peek();
            previous.SetActive(true);
        }

        private void ShowPanel(GameObject panel)
        {
            if (panelHistory.Count > 0 && panelHistory.Peek() == panel)
                return;

            AllReset();

            panel.SetActive(true);

            panelHistory.Push(panel);
        }

        private void AllReset()
        {
            titlePanel.SetActive(false);
            selectPanel.SetActive(false);
            hostPanel.SetActive(false);
            clientPanel.SetActive(false);
            hostPanel_inRoom.SetActive(false);
            clientPanel_inHostRoom.SetActive(false);
        }

        /*-- UIパネルの遷移 --*/

        public void StartGame()
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}