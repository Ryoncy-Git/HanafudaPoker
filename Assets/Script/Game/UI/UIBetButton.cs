using UnityEngine;
namespace HanafudaPoker.UIs
{
    public class UIBetButton : MonoBehaviour
    {
        [Header("BetPanel")]
        [SerializeField] private GameObject betPanel;

        public void Init()
        {
            betPanel.SetActive(false);
        }

        public void Show()
        {
            betPanel.SetActive(true);
        }

        public void Hide()
        {
            betPanel.SetActive(false);
        }
    }
}