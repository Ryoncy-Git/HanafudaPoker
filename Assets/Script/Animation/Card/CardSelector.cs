using UnityEngine;

using HanafudaPoker.Network;

using HanafudaPoker.Games;

namespace HanafudaPoker.Animation
{
    // ・ｽD・ｽ・ｽI・ｽ・ｽ Debug.Log($"{}");
    public class CardSelector : MonoBehaviour
    {
        [SerializeField]
        private VisualManager visualManager;
        private int cardID = -1;    // ゲームマネージャーへ渡すものを保持

        // GameManagerへ選択された札を伝える
        public int SelectedKoikoiCardID => cardID;

        [SerializeField]
        private CardSelectMode selectMode = CardSelectMode.None;

        TurnState previousState;

        private void Update()
        {
            FieldTurn();
        }

        private void FieldTurn()
        {
            TurnState currentState =
                (TurnState)NetworkManager.GetTurnState();

            if (currentState == previousState)
                return;

            previousState = currentState;

            switch (currentState)
            {
                case TurnState.BeforeGame:
                case TurnState.WaitForFirstChange:
                case TurnState.WaitForSecondChange:
                    SetSelectMode(CardSelectMode.Exchange);
                    break;

                case TurnState.WaitForKoikoi:
                    if (NetworkManager.GetIsKoikoi() == -1)
                        SetSelectMode(CardSelectMode.Koikoi);
                    else
                        SetSelectMode(CardSelectMode.None);
                    break;

                case TurnState.ShowFinalResult:
                case TurnState.WaitForNextRound:
                case TurnState.WaitForInitialize:
                case TurnState.ShowResult:
                    SetSelectMode(CardSelectMode.None);
                    break;
            }
        }

        public void SetSelectMode(CardSelectMode mode)
        {
            selectMode = mode;

            Debug.Log($"現在のCardSelectorの状態:{selectMode}");

            // 前回のIDは引き継がない
            cardID = -1;
        }

        /*-- 札交換の選択処理 --*/
        // マウス操作から受け取る
        public void SelectCardFromPlayer()
        {
            if (selectMode == CardSelectMode.None)
                return;

            CardView card = GetCardFromMouse();

            if (card == null)
                return;

            if (card.Owner != CardOwner.PlayerHand)
                return;

            switch (selectMode)
            {
                case CardSelectMode.Exchange:
                    SelectCardForExchange(card);
                    break;

                case CardSelectMode.Koikoi:
                    SelectCardForKoikoi(card);
                    break;
            }
        }

        // キー操作から受け取る 呼び出し元はInputManager
        public void ToggleCardByIndex(int handIndex)
        {
            if (handIndex < 0 || handIndex >= 3)
            {
                Debug.LogWarning($"その引数はカードの枚数に適してない 引数={handIndex}");
                return;
            }

            CardView card = visualManager.GetPlayerCard(handIndex);

            card.ToggleSelect();

            UpdateNetworkState(card);
        }
        /*-- 札交換の選択処理 --*/

        // マウス位置から札を取得する共通処理
        private CardView GetCardFromMouse()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out RaycastHit hit))
                return null;

            return hit.collider.GetComponent<CardView>();
        }

        // 札交換用のネットワーク更新
        private void UpdateNetworkState(CardView card)
        {
            int mySeatID = NetworkManager.GetMySeatID();
            bool[] state = NetworkManager.GetWillChangeCards(mySeatID);

            state[card.HandIndex] = card.isSelected;

            NetworkManager.SetWillChangeCards(state);
        }

        private void SelectCardForExchange(CardView card)
        {
            card.ToggleSelect();
            UpdateNetworkState(card);
        }

        // こいこいする札を選ぶ　札生成は行わない
        private void SelectCardForKoikoi(CardView card)
        {
            // 選択済みの札をもう一度クリックした場合は解除
            if (cardID == card.CardID)
            {
                card.ToggleSelect();
                cardID = -1;

                Debug.Log("こいこいの札選択を解除しました");
                return;
            }

            // 別の札がすでに選択されている場合は変更しない
            if (cardID != -1)
                return;

            card.ToggleSelect();
            cardID = card.CardID;

            Debug.Log($"こいこいに選ばれた札は: {cardID}");
        }
    }

    public enum CardSelectMode
    {
        None,       // 入力を受け付けない
        Exchange,   // 札交換時
        Koikoi      // こいこい用
    }
}