using Photon.Realtime;
using Photon.Pun;

using HanafudaPoker.Network;

public class RPCManager : MonoBehaviourPunCallbacks
{
    public static RPCManager Instance {get; private set;}

    private void Awake()
    {
        Instance = this;
    }


    // RPCs
    public void SetAllPlayersReady(bool state)
    {
        if(! PhotonNetwork.IsMasterClient)
            return;

        photonView.RPC(
            nameof(RPC_SetAllPlayersReady), 
            RpcTarget.All,
            state
        );
    }

    [PunRPC]
    private void RPC_SetAllPlayersReady(bool state)
    {
        // 各個人が受信したのち、自分の変数を変更する
        NetworkManager.SetIsReady(state);
    } 
}
