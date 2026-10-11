using Photon.Realtime;
using Photon.Pun;

using HanafudaPoker.Network;

// ho6が追加
using ExitGames.Client.Photon;
using UnityEngine;

public class RPCManager : MonoBehaviourPunCallbacks
{
    public static RPCManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    // RPCs
    public void SetAllPlayersReady(bool state)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        photonView.RPC(
            nameof(RPC_SetAllPlayersReady),
            RpcTarget.All,
            state
        );
    }
    public void SetAllPlayersWillChangeCards(bool state)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        photonView.RPC(
            nameof(RPC_SetAllPlayersWillChangeCards),
            RpcTarget.All,
            state
        );
    }

    // ho6が追加 ネットワークの場札更新を検知し、同期ズレを防ぐために泣く泣く勝手に追加しました
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (!NetworkManager.IsFieldUpdated(propertiesThatChanged))
            return;

        int[] field =
            propertiesThatChanged[NetworkManager.Key_Field] as int[];

        if (field == null)
            return;

        Debug.Log($"場札更新: {field.Length}枚");

        if (field.Length == 8)
        {
            Debug.Log("こいこい用の場札更新を検知");

            // 場札が8枚になるのは、こいこいのみ。という信頼のもとこういう条件分岐になっている
            // 場の札が変わったことを通知
            NetworkManager.NotifyFieldUpdated(field);
        }
        
    }

    [PunRPC]
    private void RPC_SetAllPlayersReady(bool state)
    {
        // �e�l����M�����̂��A�����̕ϐ���ύX����
        NetworkManager.SetIsReady(state);
    }

    [PunRPC]
    private void RPC_SetAllPlayersWillChangeCards(bool state)
    {
        NetworkManager.SetIsReady(state);
    }
}
