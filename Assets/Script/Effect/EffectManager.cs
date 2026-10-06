using UnityEngine;

using HanafudaPoker.Cards;
using HanafudaPoker.Yakus;

// ho6:各札特有の演出や役の演出を行う
// どのエフェクトを再生するか
namespace HanafudaPoker.Animation
{
    public class EffectManager : MonoBehaviour
    {
        public static EffectManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        // 札用
        // 光札ならパーティクル再生
        public void OnCardUpdated(CardData card, CardView view)
        {
            view.StopHikariEffect();

            if (card.Rank == CardRank.Hikari)
                view.PlayHikariEffect();
        }

        // 役用
        public void PlayYakuEffect(Yaku yaku)
        {
            switch (yaku)
            {
                case Yaku.Gokou:
                    // 五光演出
                    break;

                case Yaku.Sankou:
                    // 三光演出
                    break;
            }
        }

    }
}