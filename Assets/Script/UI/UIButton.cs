using UnityEngine;

namespace HanafudaPoker.UIs
{
    public class UIButton : MonoBehaviour
    {
        [SerializeField] private GameObject stateButtonPanel;

        public void Init()
        {
            stateButtonPanel.SetActive(false);
        }

        public void ShowStateButtonUI()
        {
            stateButtonPanel.SetActive(true);
        }

        public void HideStateButtonUI()
        {
            stateButtonPanel.SetActive(false);
        }
    }
}