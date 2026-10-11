using UnityEngine;

namespace HanafudaPoker.UIs
{
    public class UIKoikoi : MonoBehaviour
    {
        [Header("勝った人がこいこいを選択する。そのパネル")]
        [SerializeField] private GameObject koikoiPanel;
        [Header("負けた人は、選択を待つ。そのパネル")]
        [SerializeField] private GameObject koikoiOtherPanel;
        [SerializeField] private GameObject koikoiUI_Button;    // yakuパネルが出た後に出す

        // こいこいの札をボタンで選ぶやつのパネル debug用
        [Header("こいこいに出す札を選ぶボタン3つ。今はデバック用でしかない")]
        [SerializeField] private GameObject koikoiUI_selectCard;

        private bool isKoikoi;

        public void Init()
        {
            Hide();
        }

        // こいこい未選択の状態
        public void ShowKoikoiChoice()
        {
            koikoiPanel.SetActive(true);
            koikoiUI_Button.SetActive(true);
            koikoiUI_selectCard.SetActive(false);
        }

        // こいこいを選択し、追加する札を選ぶ段階
        public void ShowCardSelection()
        {
            koikoiPanel.SetActive(false);
            koikoiUI_Button.SetActive(false);
            // koikoiUI_selectCard.SetActive(true);
        }

        // 勝者以外の人：こいこい選択待ち用UI
        public void WaitKoikoiSelected()
        {
            Hide();
            koikoiOtherPanel.SetActive(true);
        }

        // UIを閉じる
        public void Hide()
        {
            koikoiPanel.SetActive(false);
            koikoiUI_Button.SetActive(false);
            // koikoiUI_selectCard.SetActive(false);
            koikoiOtherPanel.SetActive(false);
        }
    }
}