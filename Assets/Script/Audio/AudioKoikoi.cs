using UnityEngine;

using HanafudaPoker.Network;
using HanafudaPoker.Games;

// ho6:
namespace HanafudaPoker.Audio
{
    public class AudioKoikoi : MonoBehaviour
    {
        private TurnState previousState = TurnState.Other;

        [Header("BGM")]
        [SerializeField] private AudioSource mainBgmSource;
        [SerializeField] private AudioSource subBgmSource;
        [SerializeField] private AudioSource koikoiBgmSource;

        [Header("こいこい演出")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip spotLightSE;
        [SerializeField] private Light koikoiLight;

        private void Start()
        {
            // 初期状態ではライトを消灯
            koikoiLight.enabled = false;
        }

        private void Update()
        {
            TurnState currentState =
                (TurnState)NetworkManager.GetTurnState();

            if (currentState != previousState)
            {
                previousState = currentState;
                OnStateChanged(currentState);
            }
        }

        private void OnStateChanged(TurnState state)
        {
            switch (state)
            {
                case TurnState.BeforeGame:
                    PlayMatchBGM();
                    break;

                case TurnState.ShowFinalResult:
                    PlayKoikoiBGM();
                    break;
            }
        }

        private void PlayMatchBGM()
        {
            koikoiBgmSource.Stop();
            koikoiLight.enabled = false;

            // 通常BGMがすでに再生中なら、抽選し直さない
            if (mainBgmSource.isPlaying || subBgmSource.isPlaying)
                return;

            int index = Random.Range(0,2);

            AudioSource selectedBGM = index == 0 ? mainBgmSource : subBgmSource;

            selectedBGM.Play();
        }

        private void PlayKoikoiBGM()
        {
            // 効果音
            audioSource.PlayOneShot(spotLightSE);

            // ライト点灯
            koikoiLight.enabled = true;

            // BGM切り替え
            if (!koikoiBgmSource.isPlaying)
            {
                mainBgmSource.Stop();
                subBgmSource.Stop();
                koikoiBgmSource.Play();
            }
        }
    }
}