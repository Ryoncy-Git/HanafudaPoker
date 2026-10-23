using UnityEngine;

// ho6: ƒ{ƒ^ƒ“‚Ì‘I‘ð‰¹
namespace HanafudaPoker.Audio
{
    public class AudioButton : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip buttonSE;

        public void PlayButtonSE()
        {
            audioSource.PlayOneShot(buttonSE);
        }
    }
}