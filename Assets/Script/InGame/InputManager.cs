using UnityEngine;
using System.Collections.Generic;

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
                    new(CardMonth.Matsu,  CardRank.Hikari, CardFeature.Tori),
                    new(CardMonth.Ume,    CardRank.Tane,   CardFeature.Tori),
                    new(CardMonth.Fuji,   CardRank.Tane,   CardFeature.Tori)
                };

                gameManager.FieldCard = new List<CardData>()
                {
                    new(CardMonth.Yanagi, CardRank.Tane,   CardFeature.Tori),
                    new(CardMonth.Kiri  , CardRank.Hikari, CardFeature.Tori),
                    new(CardMonth.Susuki, CardRank.Tane,   CardFeature.Tori),
                    new(CardMonth.Sakura, CardRank.Kasu,   CardFeature.None),
                    new(CardMonth.Sakura, CardRank.Kasu,   CardFeature.None)
                };
            }
        }
    }
}