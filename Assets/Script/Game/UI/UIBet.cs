using UnityEngine;
using TMPro;

using HanafudaPoker.Bet;
using HanafudaPoker.Network;

// ho6:Œ»İ‚Ì‚¿‹à‚âbetŠz‚ğ•\¦‚·‚é
namespace HanafudaPoker.UIs
{
    public class UIBet : MonoBehaviour
    {
        [Header("BetŠz")]
        [SerializeField]
        private TMP_Text betAmountText;

        [Header("Œ»İ‚Ì‚¿‹à")]
        [SerializeField]
        private TMP_Text moneyText;
        private void Start()
        {
            betAmountText.text = "";

            moneyText.text = "Œ»İ‚ÌŠ‹àF100";
        }

        private void Update()
        {
            CheckBetValueAndMoneyValue();
        }

        private void CheckBetValueAndMoneyValue()
        {
            int bet = NetworkManager.GetBet();
            int money = NetworkManager.GetMoney();

            betAmountText.text = $"Œ»İ‚ÌbetŠz:{bet}";
            moneyText.text = $"Œ»İ‚ÌŠ‹à:{money}";
        }
    }
}