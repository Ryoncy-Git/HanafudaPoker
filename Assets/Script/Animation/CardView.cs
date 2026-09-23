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

        [SerializeField]
        private ParticleSystem hikariParticle;

        // カード更新後のアニメーションのためにカードIDが必要
        private int cardID;

        public int CardID => cardID;

        // カード変更がされたか
        public bool isChanging;

        // この札の所有権は誰か
        public CardOwner Owner { get; set; }
        public bool isSelected { get; private set; }

        // 選択された札は、少し上げることで選択を表現する
        // 元の位置を保持しておく
        private Vector3 defaultLocalPosition;
        private Coroutine selectCoroutine;
        public int HandIndex { get; set; }  // 札選択時に使用

        private void Awake()
        {
            defaultLocalPosition = cardVisual.localPosition;
            hikariParticle.Stop();
        }

        // クリックで、選択/解除
        public void ToggleSelect()
        {
            isSelected = !isSelected;

            Debug.Log($"Card {CardID} Selected = {isSelected}");

            if (selectCoroutine != null)
                StopCoroutine(selectCoroutine);

            Vector3 targetPos = isSelected
                ? defaultLocalPosition + Vector3.up * 0.1f
                : defaultLocalPosition;

            selectCoroutine = StartCoroutine(SelectedCardAnimation(targetPos));
        }

        // アニメーション終了後に解除
        public void DeSelect()
        {
            if (!isSelected)
                return;

            isSelected = false;

            if (selectCoroutine != null)
                StopCoroutine(selectCoroutine);

            selectCoroutine =
                StartCoroutine(
                    SelectedCardAnimation(defaultLocalPosition));
        }

        // 札を交換する
        public IEnumerator ChangeCardAnimation(CardData newCard)
        {
            yield return FlipCard();

            SetCard(newCard);

            yield return FlipCard();

            DeSelect();
        }

        /*-- 札自体のTransform操作 --*/

        // 札が選択されたときの処理
        private IEnumerator SelectedCardAnimation(Vector3 endPos)
        {
            Vector3 startPos = cardVisual.localPosition;

            float elapsed = 0f;
            float duration = 0.2f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;

                cardVisual.localPosition =
                    Vector3.Lerp(
                        startPos,
                        endPos,
                        elapsed / duration);

                yield return null;
            }

            cardVisual.localPosition = endPos;
        }

        // カードを反転 cardVisualを動かす
        private IEnumerator FlipCard()
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
        private IEnumerator ScaleAnimation(float scale)
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
        private IEnumerator MoveAnimation(Transform card)
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
        private IEnumerator RotateAnimation(Transform card)
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

        // 上記の処理を一括で行う
        public IEnumerator PlayAnimation(Transform target, float scale, bool flip)
        {
            StartCoroutine(MoveAnimation(target));
            StartCoroutine(RotateAnimation(target));
            StartCoroutine(ScaleAnimation(scale));

            if (flip) yield return FlipCard();

        }

        /*-- 札に付与されるエフェクト操作 --*/
        public void PlayHiakariEffect()
        {
            Debug.Log("光札のエフェクトを再生");
            hikariParticle.Play();
        }

        public void SetCard(CardData card)
        {
            if (frontRenderer == null)
                return;

            cardID = card.CardID;
            Debug.Log($"SetCard : {card.CardID}");

            frontRenderer.material =
                MaterialManager.Instance.GetMaterial(card);
        }

    }

    public enum CardOwner
    {
        PlayerHand,
        Field,
        OtherPlayer
    }
}