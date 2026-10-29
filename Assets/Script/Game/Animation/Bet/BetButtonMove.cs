using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace HanafudaPoker.Animation
{
    // betボタンUIの動きを定義
    public class BetButtonMove : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("BetButtonUIに関する定義")]
        [SerializeField] private float moveDistance = 20.0f;

        private RectTransform rectTransform;
        private Vector2 defaultPosition;

        private Coroutine moveCoroutine;

        private void Awake()
        {
            // ButtonのRectTransformを取得
            rectTransform = GetComponent<RectTransform>();

            // 親(ここではCanvas)から見たUIの位置
            defaultPosition = rectTransform.anchoredPosition;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            MoveTo(defaultPosition + Vector2.left * moveDistance);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            MoveTo(defaultPosition);
        }

        private void MoveTo(Vector2 target)
        {
            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);

            moveCoroutine =
                StartCoroutine(MoveAnimation(target));
        }

        private IEnumerator MoveAnimation(Vector2 target)
        {
            Vector2 start = rectTransform.anchoredPosition;

            float elapsed = 0f;
            float duration = 0.15f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;

                rectTransform.anchoredPosition =
                    Vector2.Lerp(start, target, elapsed / duration);

                yield return null;
            }

            rectTransform.anchoredPosition = target;
        }
    }
}