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

        // 札の見た目を弄る
        private bool isFaceUp = false;

        [SerializeField]
        private Transform cardVisual;

        public void SetCard(CardData card)
        {
            if (frontRenderer == null)
            {
                Debug.Log("このカードは偽札です。正しいですか？");
                return;
            }

            frontRenderer.material =
                MaterialManager.Instance.GetMaterial(card);
        }

        /*-- 札自体のTransform操作 --*/

        // カードを反転 cardVisualを動かす
        public IEnumerator FlipCard()
        {
            isFaceUp = !isFaceUp;

            // ワールド座標 transform.rotationで回すとその親のオブジェクト側でも考慮する必要があるからローカル
            Vector3 euler = cardVisual.localEulerAngles;

            float elapsed = 0f;
            float duration = 0.5f;

            Quaternion start = cardVisual.localRotation;
            Quaternion end = Quaternion.Euler(euler.x, isFaceUp ? 0f : 180f, euler.z);

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;

                cardVisual.localRotation =
                    Quaternion.Lerp(start, end, elapsed / duration);

                yield return null;
            }
            cardVisual.localRotation = end;
        }

        // スケール
        public IEnumerator ScaleAnimation(float scale)
        {
            Vector3 startScale = cardVisual.localScale;
            Vector3 endScale = Vector3.one * scale;

            float elapsed = 0f;
            float duration = 0.5f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;

                cardVisual.localScale =
                    Vector3.Lerp(startScale, endScale, elapsed / duration);

                yield return null;
            }

            cardVisual.localScale = endScale;
        }

        /*-- 札全体の処理 --*/

        // 山札から指定の場所に移動する
        public IEnumerator MoveAnimation(Transform card)
        {
            //Debug.Log($"Target Rot = {card.rotation.eulerAngles}");

            Vector3 startPos = transform.position;
            Vector3 endPos = card.position;

            float elapsed = 0f;
            float duration = 0.5f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;

                transform.position =
                    Vector3.Lerp(startPos, endPos, elapsed / duration);

                yield return null;
            }
            transform.position = card.position;
        }

        // 回転
        public IEnumerator RotateAnimation(Transform card)
        {
            Quaternion startRotate = transform.rotation;
            Quaternion endRortate = card.rotation;

            float elapsed = 0f;
            float duration = 0.5f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;

                transform.rotation =
                    Quaternion.Lerp(startRotate, endRortate, elapsed / duration);

                yield return null;
            }
            transform.rotation = card.rotation;
        }

    }
}