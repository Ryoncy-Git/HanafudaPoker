using UnityEngine;

using HanafudaPoker.Network;

namespace HanafudaPoker.Animation
{
    // 札を選ぶ
    public class CardSelector : MonoBehaviour
    {
        [SerializeField]
        private VisualManager visualManager;

        // マウス操作から受け取る
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

        // キー操作から受け取る
        public void ToggleCardByIndex(int handIndex)
        {
            if (handIndex < 0 || handIndex >= 3)
            {
                Debug.LogWarning($"その引数はカードの枚数に適してない 引数={handIndex}");
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