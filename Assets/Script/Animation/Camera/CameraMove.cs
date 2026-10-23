using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HanafudaPoker.Animation
{
    public class CameraMove : MonoBehaviour
    {
        [SerializeField] private GameObject camObj;
        [Header("こいこい時のカメラ設定")]
        // どれだけ動かすか (変化量)
        [SerializeField] private Vector3 moveOffset = new Vector3(0f, 0f, 0f);
        
        // 変化先の角度 (これは変化量ではない)
        [SerializeField] private Vector3 targetRotation = new Vector3(0f, 0f, 0f);

        [Header("移動設定")]
        [SerializeField] private float moveDuration = 1.0f;

        private Vector3 defaultPosition;
        private Quaternion defaultRotation;
        private Coroutine moveCoroutine;

        private void Start()
        {
            if (camObj == null)
                return;

            defaultPosition = camObj.transform.position;
            defaultRotation = camObj.transform.rotation;
        }

        // こいこい時にカメラを移動
        public void MoveCamera()
        {
            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);

            Vector3 destination = defaultPosition + moveOffset;
            Quaternion destinationRotation = Quaternion.Euler(targetRotation);

            moveCoroutine = StartCoroutine(MoveCameraCoroutine(destination, destinationRotation));
        }

        public void ResetCamera()
        {
            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);

            moveCoroutine = StartCoroutine(MoveCameraCoroutine(defaultPosition, defaultRotation));
        }

        /*-- 補助関数 --*/
        private IEnumerator MoveCameraCoroutine(Vector3 destination, Quaternion destinationRotation)
        {
            Vector3 startPosition = camObj.transform.position;
            Quaternion startRotation = camObj.transform.rotation;

            float elapsedTime = 0f;

            while(elapsedTime < moveDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / moveDuration);

                // 位置と回転を同時に
                camObj.transform.position = Vector3.Lerp(startPosition, destination, t);
                camObj.transform.rotation = Quaternion.Slerp(startRotation, destinationRotation, t);

                yield return null;
            }

            // 目標のTransformに合わせる
            camObj.transform.position = destination;
            camObj.transform.rotation = destinationRotation;

            moveCoroutine = null;
        }
    }
}