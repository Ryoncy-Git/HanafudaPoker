using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using HanafudaPoker.Cards;
using HanafudaPoker.Mat;

namespace HanafudaPoker.UIs
{
    // 札の見た目を変更
    public class CardView : MonoBehaviour
    {
        // 絵のマテリアルを貼る面
        [SerializeField]
        private MeshRenderer frontRenderer;

        public void SetCard(CardData card)
        {
            frontRenderer.material =
                MaterialManager.Instance.GetMaterial(card);
        }

        // カードを反転
        public IEnumerator FlipCard()
        {
            float elapsed = 0f;

            Quaternion start = transform.rotation;
            Quaternion end = Quaternion.Euler(0f, 0f, 0f);

            while(elapsed < 0.5f)
            {
                elapsed += Time.deltaTime;

                transform.rotation =
                    Quaternion.Lerp(start, end, elapsed / 0.5f);

                yield return null;
            }

            transform.rotation = end;
        }
    }
}