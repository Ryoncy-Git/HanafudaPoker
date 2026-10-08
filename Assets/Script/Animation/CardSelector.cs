using UnityEngine;

using HanafudaPoker.Network;

namespace HanafudaPoker.Animation
{
    // ï¿½Dï¿½ï¿½Iï¿½ï¿½
    public class CardSelector : MonoBehaviour
    {
        [SerializeField]
        private VisualManager visualManager;

        // ƒ}ƒEƒX‘€ì‚©‚çó‚¯æ‚é
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

        // ƒL[‘€ì‚©‚çó‚¯æ‚é
        public void ToggleCardByIndex(int handIndex)
        {
            if (handIndex < 0 || handIndex >= 3)
            {
                Debug.LogWarning($"‚»‚Ìˆø”‚ÍƒJ[ƒh‚Ì–‡”‚É“K‚µ‚Ä‚È‚¢ ˆø”={handIndex}");
                return;
            }

            CardView card =
                visualManager.GetPlayerCard(handIndex);

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