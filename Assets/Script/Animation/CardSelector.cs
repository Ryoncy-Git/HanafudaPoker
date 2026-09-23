using UnityEngine;

using HanafudaPoker.Network;

namespace HanafudaPoker.Animation
{
    // �D��I��
    public class CardSelector : MonoBehaviour
    {
        public void SelectCardFromPlayer()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out RaycastHit hit))
                return;

            CardView card =
                hit.collider.GetComponent<CardView>();

            if (card == null)
                return;

            if (card.Owner != CardOwner.PlayerHand)
                return;

            card.ToggleSelect();

            UpdateNetworkState(card);
        }

        private void UpdateNetworkState(CardView card)
        {
            int mySeatID = NetworkManager.GetMySeatID();
            bool[] state = NetworkManager.GetWillChangeCards(mySeatID);

            state[card.HandIndex] = card.isSelected;

            NetworkManager.SetWillChangeCards(state);
        }
    }
}