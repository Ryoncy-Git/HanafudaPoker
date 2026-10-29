using UnityEngine;
using TMPro;
using Photon.Pun;

namespace HanafudaPoker.Lobby
{
    public class UILobby : MonoBehaviour
    {
        [SerializeField] private TMP_Text roomIDText;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            if (PhotonNetwork.InRoom)
            {
                roomIDText.text = $"•”‰®ID:{PhotonNetwork.CurrentRoom.Name}";
            }
        }
    }
}