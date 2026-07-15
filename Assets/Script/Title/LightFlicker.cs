using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ho6:
namespace HanafudaPoker.Title
{
    public class LightFlicker : MonoBehaviour
    {
        [SerializeField] private Light targetLight;

        private float seed;

        private void Start()
        {
            seed = Random.Range(0f, 100f);
            StartCoroutine(Blink());
        }

        private IEnumerator Blink()
        {
            while (true)
            {
                targetLight.enabled = !targetLight.enabled;

                yield return new WaitForSeconds(
                    Random.Range(1, 10));
            }
        }
    }
}