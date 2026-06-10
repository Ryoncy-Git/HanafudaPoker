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
        [SerializeField]private GameManager gameManager;

        public bool IsMasterClient()
        {
            return PhotonNetwork.IsMasterClient;
        }

        public int[] GetPlayerActorNumbers()
        {
            Player[] players = PhotonNetwork.PlayerList;
            int[] ids = new int[players.Length];

            for(int i = 0; i < players.Length; i++)
            {
                ids[i] = players[i].ActorNumber;
            }

            return ids;
        }

        // ------------------------ RPC送信側 ------------------------

        public void SetPlayerReady(int seatID, bool state)
        {
            photonView.RPC(
                nameof(RPC_SetPlayerReady),
                RpcTarget.All,
                seatID,
                state
            );
        }

        public void SetPlayerWillChangeCard(int playerNum, bool state, int num)
        {
            photonView.RPC(
                nameof(RPC_SetPlayerWillChangeCard),
                RpcTarget.All,
                playerNum,
                state,
                num
            );
        }


        // --------------------- master側のみが送れるRPC 送信側 --------------

        public void SetPlayerNumber()
        {
            if(!PhotonNetwork.IsMasterClient)
                return; 

            int n = PhotonNetwork.PlayerList.Length;

            photonView.RPC(
                nameof(RPC_SetPlayerNumber),
                RpcTarget.All,
                n
            );
        }
        public void SendGameData
        (
            TurnState currentState, 
            TurnState prevState, 
            int[] DeckIDs, 
            int[] FieldIDs, 
            int[] discardIDs,
            int[] playerHand
        )
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            photonView.RPC(
                nameof(RPC_SendGameData),
                RpcTarget.All,
                currentState, 
                prevState, 
                DeckIDs, 
                FieldIDs, 
                discardIDs,
                playerHand
            );
        }

        // ------------------------ PunRPC 受信側 -----------------------
        // こっちはmaster clientじゃなくても呼べる。
        [PunRPC]
        private void RPC_SetPlayerReady(int seatID, bool state)
        {
            // 私は準備できました。という情報を開示する
            gameManager.Players[seatID].IsReady = state;
            // Debug.Log($"Player{seatID} , Set state To {state}");
            return;
        }

        [PunRPC]
        private void RPC_SetPlayerWillChangeCard(int seatID, bool state, int num)
        {
            // 私はこのカードを変更します。という情報を開示する
            gameManager.Players[seatID].WillChangeCards[num] = state;
        }

        // --------------------- masterのみが送れるRPCの受信側 -------------------------
        [PunRPC]
        private void RPC_SetPlayerNumber(int n)
        {
            GameConst.PLAYER_NUMBER = n;
        }

        [PunRPC]
        private void RPC_SendGameData
        (   
            TurnState currentState, 
            TurnState prevState, 
            int[] DeckIDs, 
            int[] FieldIDs, 
            int[] discardIDs,
            int[] playerHand
        )
        {
            gameManager.CurrentState = currentState;
            gameManager.PreviousState = prevState;

            // sync deck data 
            gameManager.Deck = CardDataBase.GetCardDataListByID(DeckIDs);

            // sync field data 
            gameManager.FieldCard = CardDataBase.GetCardDataListByID(FieldIDs);
            gameManager.DiscardPile = CardDataBase.GetCardDataListByID(discardIDs);

            // Sync Player hand data
            for(int i = 0; i < GameConst.PLAYER_NUMBER; i++)
            {
                PlayerData player = gameManager.Players[i];
                int[] handInt = 
                new int[]
                {
                    playerHand[i * GameConst.HAND_CARD_NUMBER + 0], 
                    playerHand[i * GameConst.HAND_CARD_NUMBER + 1], 
                    playerHand[i * GameConst.HAND_CARD_NUMBER + 2]
                };

                List<CardData> hand = CardDataBase.GetCardDataListByID(handInt);

                player.HandCards = hand;
            }
        }
    }
}