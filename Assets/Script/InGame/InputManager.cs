using UnityEngine;
using System.Collections.Generic;

// using HanafudaPoker.Players;
// using HanafudaPoker.Cards;
using HanafudaPoker.UIs;
using HanafudaPoker.Network;

namespace HanafudaPoker.Games
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField]private UIDebug uiDebug;
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                NetworkManager.SetIsReady(true);
            }

            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                int mySeatID = NetworkManager.GetMySeatID();
                bool[] state = NetworkManager.GetWillChangeCards(mySeatID);

                // change state[0]
                state[0] != state[0];
                NetworkManager.SetWillChangeCards(state);
            }

            if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                int mySeatID = NetworkManager.GetMySeatID();
                bool[] state = NetworkManager.GetWillChangeCards(mySeatID);

                // change state[1]
                state[1] != state[1];
                NetworkManager.SetWillChangeCards(state);
            }

            if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                int mySeatID = NetworkManager.GetMySeatID();
                bool[] state = NetworkManager.GetWillChangeCards(mySeatID);

                // change state[2]
                state[2] != state[2];
                NetworkManager.SetWillChangeCards(state);
            }
        }
    }
}