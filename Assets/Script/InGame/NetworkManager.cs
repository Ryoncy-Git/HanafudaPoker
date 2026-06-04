using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

using HanafudaPoker.Players;
using HanafudaPoker.Cards;

namespace HanafudaPoker.Games
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        private GameManager gameManager;
        public bool IsEveryoneReady;

        public void Start()
        {
            SetInstance();
        }

        private void SetInstance()
        {
            gameManager = this.gameObject.GetComponent<GameManager>();
        }

        public bool IsMasterClient()
        {
            return PhotonNetwork.IsMasterClient;
        }

        private bool CheckIsEveryoneReady()
        {
            foreach(PlayerData player in gameManager.Players)
            {
                // 一人でも準備完了してない人を見つけたら
                if(player.IsReady == false)
                    return false;
            }

            return true;
        }
        
        // ------------------------ PunRPC -----------------------
        [PunRPC]
        private void SetPlayerReady(int playerNum, bool state)
        {
            gameManager.Players[playerNum].IsReady = state;
            Debug.Log($"Player{playerNum} , Set state To {state}");
            return;
        }

        [PunRPC]
        private void SetPlayerWillChangeCard(int playerNum, bool state, int num)
        {
            gameManager.Players[playerNum].WillChangeCards[num] = state;
        }

        [PunRPC]
        public void ResetPlayersReady()
        {
            foreach(PlayerData player in gameManager.Players)
            {
                player.IsReady = false;
                for(int i = 0; i < GameConst.HAND_CARD_NUMBER; i++)
                {
                    player.WillChangeCards[i] = false;
                }
            }
        }

        [PunRPC]
        private void SetDeck(List<CardData> deck)
        {
            gameManager.Deck = deck;
        }

        [PunRPC]
        private void GoToNextTurn() // master client限定
        {
            // gameManager.turnState = 
        }
        
    }
}