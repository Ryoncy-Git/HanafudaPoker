using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using HanafudaPoker.Cards;
using HanafudaPoker.Mat;

// ho6:Cardのテクスチャ変更・アニメーションを持たせる処理
namespace HanafudaPoker.Animation
{
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
            float duration = 0.5f;

            Quaternion start = transform.rotation;
            Quaternion end = Quaternion.Euler(0f, 0f, 0f);

            while(elapsed < duration)
            {
                elapsed += Time.deltaTime;

                transform.rotation =
                    Quaternion.Lerp(start, end, elapsed / duration);

                yield return null;
            }

            transform.rotation = end;
        }

        // 山札から指定の場所に移動する
        public void MoveToPosition(Vector3 deckPos, Vector3 cardPos)
        {
            StartCoroutine(MoveAnimation(deckPos, cardPos));
        }

        private IEnumerator MoveAnimation(Vector3 deckPos, Vector3 cardPos)
        {
            Vector3 startPos = deckPos;
            Vector3 endPos = cardPos;

            float elapsed = 0f;
            float duration = 0.5f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;

                transform.position =
                    Vector3.Lerp(startPos, endPos, elapsed / duration);

                yield return null;
            }

            transform.position = cardPos;
        }
    }
}