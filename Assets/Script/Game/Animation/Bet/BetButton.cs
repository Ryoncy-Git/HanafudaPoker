using UnityEngine;

namespace HanafudaPoker.Bet
{
    // UI�{�^�����琶��
    public class BetButton : MonoBehaviour
    {
        [SerializeField] private BetFactory betFactory;

        public void OnBetMinCoinButton()
        {
            if (!BetSystem.TryBet(5))
            {
                Debug.Log("�������s��:5");
                return;
            }
            
            betFactory.CreateBetCoin(BetCoin.BunkyuEiho);
        }

        public void OnBetMidCoinButton()
        {
            if (!BetSystem.TryBet(10))
            {
                Debug.Log("�������s��:10");
                return;
            }

            betFactory.CreateBetCoin(BetCoin.KaneiTsuho);
        }

        public void OnBetMaxCoinButton()
        {
            if (!BetSystem.TryBet(50))
            {
                Debug.Log("�������s��:50");
                return;
            }

            betFactory.CreateBetCoin(BetCoin.Wadokaichin);
        }

        public void OnInsuranceButton()
        {
            // NetworkManager.Hogehoge()を使いたいのでいったんBetSystemにたらいまわし
            BetSystem.OnInsuranceButton();
        }
    }
}