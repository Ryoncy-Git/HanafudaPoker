using UnityEngine;
using HanafudaPoker.Network;

namespace HanafudaPoker.UIs
{
    public class UIButton : MonoBehaviour
    {
        [Header("ó‘Ô‘JˆÚ‚ğs‚¤ƒ{ƒ^ƒ“")]
        [SerializeField] private GameObject buttonPanel;

        public void Init()
        {
            buttonPanel.SetActive(false);
        }

        public void Show()
        {
            buttonPanel.SetActive(true);
        }

        public void Hide()
        {
            buttonPanel.SetActive(false);
        }
    }
}