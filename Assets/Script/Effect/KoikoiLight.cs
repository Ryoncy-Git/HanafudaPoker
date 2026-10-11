using UnityEngine;

namespace HanafudaPoker.Animation
{
    public class KoikoiLight : MonoBehaviour
    {
        [SerializeField] private Light koikoiLight;

        private void Start()
        {
            koikoiLight.enabled = false;
        }

        public void EnableKoikoiLight()
        {
            koikoiLight.enabled = true;
        }
    }
}