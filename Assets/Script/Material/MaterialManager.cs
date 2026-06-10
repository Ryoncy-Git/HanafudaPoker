using UnityEngine;

using HanafudaPoker.Cards;

namespace HanafudaPoker.Mat
{
    // 札の表に付け替えるマテリアルを担当
    public class MaterialManager : MonoBehaviour
    {
        public static MaterialManager Instance;

        [SerializeField] private Material[] cardMaterials;

        private void Awake()
        {
            Instance = this;
        }

        public Material GetMaterial(CardData card)
        {
            return cardMaterials[card.CardId];
        }
    }
}