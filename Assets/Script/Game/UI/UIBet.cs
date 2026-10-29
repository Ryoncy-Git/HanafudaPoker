using UnityEngine;
using TMPro;

using HanafudaPoker.Bet;

// ho6:現在の持ち金やbet額を表示する
namespace HanafudaPoker.UIs
{
    public class UIBet : MonoBehaviour
    {
        [Header("Bet額")]
        [SerializeField]
        private TMP_Text betAmountText;

        [Header("現在の持ち金")]
        [SerializeField]
        private TMP_Text moneyText;

        [Header("これまでの合計bet額")]
        [SerializeField]
        private TMP_Text allBetAmountText;

        private void Start()
        {
            betAmountText.text = "";

            moneyText.text = "現在の所持金：100";
            allBetAmountText.text = "";
        }

        private void Update()
        {
            moneyText.text = $"現在の所持金：{0}";
            allBetAmountText.text = $"合計bet額：{0}";
        }
    }
}