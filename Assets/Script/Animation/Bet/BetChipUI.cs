using UnityEngine;

namespace HanafudaPoker.Bet
{
    // UIÉ{É^ÉìÇ©ÇÁê∂ê¨
    public class BetChipUI : MonoBehaviour
    {
        [SerializeField] private BetFactory betFactory;

        public void OnBetMinCoinButton()
        {
            Debug.Log("Create 5 Coin");
            betFactory.CreateBetCoin(BetCoin.BunkyuEiho);
        }

        public void OnBetMidCoinButton()
        {
            Debug.Log("Create 10 Coin");
            betFactory.CreateBetCoin(BetCoin.KaneiTsuho);
        }

        public void OnBetMaxCoinButton()
        {
            Debug.Log("Create 50 Coin");
            betFactory.CreateBetCoin(BetCoin.Wadokaichin);
        }
    }
}