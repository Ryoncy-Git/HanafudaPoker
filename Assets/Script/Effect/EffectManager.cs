using UnityEngine;

using HanafudaPoker.Cards;

// ho6:各札特有の演出や役の演出を行う
namespace HanafudaPoker.Animation
{
    public class EffectManager : MonoBehaviour
    {
        public static EffectManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void CheckCardEffect(CardData data, CardView view)
        {
            if (data.Rank == CardRank.Hikari)
            {
                view.PlayHiakariEffect();
            }
            else
            {
                return;
            }
        }

    }
}