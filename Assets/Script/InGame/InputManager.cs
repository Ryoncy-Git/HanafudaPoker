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
                NetworkManager.SetWillChangeCards(ChangedStateByIndex(0));
            }

            if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                NetworkManager.SetWillChangeCards(ChangedStateByIndex(1));
            }

            if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                NetworkManager.SetWillChangeCards(ChangedStateByIndex(2));
            }
        }

        private int ChangedStateByIndex(int index)
        {
            int mySeatID = NetworkManager.GetMySeatID();
            int state = NetworkManager.GetWillChangeCards(mySeatID);
            
            // state = state xor (001 << index)
            state ^= 1 << index; 
            return state;
        }
    }
}