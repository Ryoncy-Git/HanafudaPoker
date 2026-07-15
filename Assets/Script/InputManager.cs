using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using HanafudaPoker.Players;
using HanafudaPoker.Cards;
using HanafudaPoker.UIs;

namespace HanafudaPoker.Games
{
    public class InputManager : MonoBehaviour
    {
        private GameManager gameManager;
        private UIDebug uiDebug;


        private void Start()
        {
            gameManager = this.gameObject.GetComponent<GameManager>();
            uiDebug = this.gameObject.GetComponent<UIDebug>();
        }
        
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                // デバッグ用
                foreach(PlayerData player in gameManager.Players)
                {
                    player.IsReady = true;
                }
            }

            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                gameManager.Players[0].WillChangeCards[0] = ! gameManager.Players[0].WillChangeCards[0];
                uiDebug.ShowWillChange();
            }

            if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                gameManager.Players[0].WillChangeCards[1] = ! gameManager.Players[0].WillChangeCards[1];
                uiDebug.ShowWillChange();
            }

            if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                gameManager.Players[0].WillChangeCards[2] = ! gameManager.Players[0].WillChangeCards[2];
                uiDebug.ShowWillChange();
            }

            if(Input.GetKeyDown(KeyCode.P))
            {
                gameManager.Players[0].HandCards = new List<CardData>()
                {
                    new(CardMonth.Matsu,  CardRank.Hikari, CardFeature.Tori, 0),
                    new(CardMonth.Ume,    CardRank.Tane,   CardFeature.Tori, 4),
                    new(CardMonth.Fuji,   CardRank.Tane,   CardFeature.Tori, 12)
                };

                gameManager.FieldCard = new List<CardData>()
                {
                    new(CardMonth.Yanagi, CardRank.Tane,   CardFeature.Tori, 41),
                    new(CardMonth.Kiri  , CardRank.Hikari, CardFeature.Tori, 44),
                    new(CardMonth.Susuki, CardRank.Tane,   CardFeature.Tori, 29),
                    new(CardMonth.Sakura, CardRank.Kasu,   CardFeature.None, 8),
                    new(CardMonth.Sakura, CardRank.Kasu,   CardFeature.None, 9)
                };
            }


            // ho6:
            // 他の手札デバッグ表示
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                uiDebug.ShowSecondPlayerCardUIs();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                uiDebug.ShowThirdPlayerCardUIs();
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                uiDebug.ShowForthPlayerCardUIs();
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                uiDebug.CloseAllPlayerCardUIs();
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                uiDebug.ShowAllPlayerCardUIs();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                uiDebug.SetBackTitleScenePanel();
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                uiDebug.ShowFieldCardsPanel();
            }
        }

        // ho6: タイトル画面に戻るかどうかのボタン
        // この画面遷移に関してはInputManagerが持つ責務ではないが、今はここに置きます
        public void OnClickYes()
        {
            SceneManager.LoadScene("Title");
        }

        public void OnClickNo()
        {
            uiDebug.SetBackTitleScenePanel();
        }
    }
}