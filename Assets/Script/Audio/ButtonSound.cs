using UnityEngine;

// ho6: ƒ{ƒ^ƒ“‚Ì‘I‘ð‰¹
namespace HanafudaPoker.Audio
{
    public class ButtonSound : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip buttonSE;

        public void PlayButtonSE()
        {
            Debug.Log("Button SE");
            audioSource.PlayOneShot(buttonSE);
        }
    }
}