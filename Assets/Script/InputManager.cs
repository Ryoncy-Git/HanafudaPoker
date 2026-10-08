using UnityEngine;
using System.Collections.Generic;

// using HanafudaPoker.Players;
// using HanafudaPoker.Cards;
using HanafudaPoker.UIs;
using HanafudaPoker.Network;
using HanafudaPoker.Animation;

namespace HanafudaPoker.Games
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private UIDebug uiDebug;
        [SerializeField] private GameObject uiDebugPanel;
        [SerializeField] private CardSelector cardSelector;

        private bool isShowDebugUI = false;

        private void Start()
        {
            uiDebugPanel.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                bool state = NetworkManager.GetIsReady();
                NetworkManager.SetIsReady(!state);
                Debug.Log("Change Ready To " + !state);
            }

            if (Input.GetMouseButtonDown(0))
            {
                cardSelector.SelectCardFromPlayer();
            }


            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                cardSelector.ToggleCardByIndex(0);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                cardSelector.ToggleCardByIndex(1);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                cardSelector.ToggleCardByIndex(2);
            }


            if (Input.GetKeyDown(KeyCode.F3))
            {
                if (!isShowDebugUI)
                {
                    uiDebugPanel.SetActive(true);
                    isShowDebugUI = true;
                }
                else
                {
                    uiDebugPanel.SetActive(false);
                    isShowDebugUI = false;
                }
            }
        }

        public void PressedButtonKoikoi(bool state)
        {
            if(state == true)
            {
                NetworkManager.SetIsKoikoi(1);
                uiDebug.ShowKoikoiSelectCard();
            }
            else
            {
                NetworkManager.SetIsKoikoi(0);
                uiDebug.SetActiveKoikoiUI(false);
            }
        }

        public void PressedButtonCardSelectNumber(int num)
        {
            this.gameObject.GetComponent<GameManager>().koikoiIndex = num;
        }
    }
}