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

        private bool CheckMasterClient()
        {
            return PhotonNetwork.IsMasterClient;
        }

        public bool IsEveryoneReady()
        {
            foreach(PlayerData player in gameManager.Players)
            {
                // 一人でも準備完了してない人を見つけたら
                if(player.IsReady == false)
                    return false;
            }

            return true;
        }

        // ------------------------ RPC送信側 ------------------------

        public void SetPlayerReady(int playerNum, bool state)
        {
            photonView.RPC(
                nameof(RPC_SetPlayerReady),
                RpcTarget.All,
                playerNum,
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

        public void SetTurnState(TurnState state)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            photonView.RPC(
                nameof(RPC_SetTurnState),
                RpcTarget.All,
                state
            );
        }

        public void SetDeck(List<CardData> deck)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            photonView.RPC(
                nameof(RPC_SetDeck),
                RpcTarget.All,
                deck
            );
        }

        public void SetFieldCards(List<CardData> field)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            photonView.RPC(
                nameof(RPC_SetFieldCards),
                RpcTarget.All,
                field
            );
        }

        public void SetPlayersHandCards(
            List<CardData> hands0,
            List<CardData> hands1,
            List<CardData> hands2,
            List<CardData> hands3)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            int[] handsInt0 = new int[GameConst.HAND_CARD_NUMBER];
            int[] handsInt1 = new int[GameConst.HAND_CARD_NUMBER];
            int[] handsInt2 = new int[GameConst.HAND_CARD_NUMBER];
            int[] handsInt3 = new int[GameConst.HAND_CARD_NUMBER];

            for(int i = 0; i < GameConst.HAND_CARD_NUMBER; i++)
            {
                handsInt0[i] = hands0[i].CardID;
                handsInt1[i] = hands1[i].CardID;
                handsInt2[i] = hands2[i].CardID;
                handsInt3[i] = hands3[i].CardID;
            }
            
            photonView.RPC(
                nameof(RPC_SetPlayersHandCards),
                RpcTarget.All,
                handsInt0,
                handsInt1,
                handsInt2,
                handsInt3
            );
        }

        public void ResetPlayersReady()
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            photonView.RPC(
                nameof(RPC_ResetPlayersReady),
                RpcTarget.All
            );
        }
                
        // ------------------------ PunRPC 受信側 -----------------------
        // こっちはmaster clientじゃなくても呼べる。
        [PunRPC]
        private void RPC_SetPlayerReady(int playerNum, bool state)
        {
            // 私は準備できました。という情報を開示する
            gameManager.Players[playerNum].IsReady = state;
            Debug.Log($"Player{playerNum} , Set state To {state}");
            return;
        }

        [PunRPC]
        private void RPC_SetPlayerWillChangeCard(int playerNum, bool state, int num)
        {
            // 私はこのカードを変更します。という情報を開示する
            gameManager.Players[playerNum].WillChangeCards[num] = state;
        }

        // --------------------- masterのみが送れるRPCの受信側 -------------------------
        [PunRPC]
        private void RPC_SetTurnState(TurnState state)
        {
            gameManager.turnState = state;
        }

        [PunRPC]
        private void RPC_SetDeck(List<CardData> deck)
        {
            gameManager.Deck = deck;
        }  

        [PunRPC]
        private void RPC_SetFieldCards(List<CardData> field)
        {
            gameManager.FieldCard = field;
        }

        [PunRPC]
        private void RPC_SetPlayersHandCards(int[] hands0, int[] hands1, int[] hands2, int[] hands3)
        {
            gameManager.Players[0].HandCards = CardDataBase.GetCardDataListByID(hands0);
            gameManager.Players[1].HandCards = CardDataBase.GetCardDataListByID(hands1);
            gameManager.Players[2].HandCards = CardDataBase.GetCardDataListByID(hands2);
            gameManager.Players[3].HandCards = CardDataBase.GetCardDataListByID(hands3);
        }

        [PunRPC]
        private void RPC_ResetPlayersReady()
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
    }
}