using UnityEngine;

namespace HanafudaPoker.Bet
{
    // UIボタンから生成
    public class BetButton : MonoBehaviour
    {
        [SerializeField] private BetFactory betFactory;

        public void OnBetMinCoinButton()
        {
            if (!BetSystem.TryBet(5))
            {
                Debug.Log("持ち金不足:5");
                return;
            }
            
            betFactory.CreateBetCoin(BetCoin.BunkyuEiho);
        }

        public void OnBetMidCoinButton()
        {
            if (!BetSystem.TryBet(10))
            {
                Debug.Log("持ち金不足:10");
                return;
            }

            betFactory.CreateBetCoin(BetCoin.KaneiTsuho);
        }

        public void OnBetMaxCoinButton()
        {
            if (!BetSystem.TryBet(50))
            {
                Debug.Log("持ち金不足:50");
                return;
            }

            betFactory.CreateBetCoin(BetCoin.Wadokaichin);
        }
    }
}