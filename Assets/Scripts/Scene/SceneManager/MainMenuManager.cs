using UnityEngine;

namespace TKM
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] InputReader _inputReader;
        [SerializeField] CanvasGroup _settingsCanvasGroup;
        [SerializeField] CanvasGroup _creditsCanvasGroup;
        [SerializeField] CanvasGroup _tutorialCanvasGroup;
        GameObject _selectedMenu = null;

        void Start()
        {
            _inputReader.EnableUIInput();
        }

        public void OpenSettingsMenu()
        {
            _selectedMenu = _settingsCanvasGroup.gameObject;
            EnableCanvasGroup(_settingsCanvasGroup);
            _selectedMenu.GetComponent<VolumeSettings>().Setup();
        }

        public void OpenCreditsMenu()
        {
            _selectedMenu = _creditsCanvasGroup.gameObject;
            EnableCanvasGroup(_creditsCanvasGroup);

        }

        public void OpenTutorialMenu()
        {
            _selectedMenu = _tutorialCanvasGroup.gameObject;
            EnableCanvasGroup(_tutorialCanvasGroup);
        }
        public void EnableCanvasGroup(CanvasGroup selectedCanvasGroup)
        {
            selectedCanvasGroup.alpha = 1;
            selectedCanvasGroup.interactable = true;
            selectedCanvasGroup.blocksRaycasts = true;
        }

        public void DisableCanvasGroup(CanvasGroup selectedCanvasGroup)
        {
            selectedCanvasGroup.alpha = 0;
            selectedCanvasGroup.interactable = false;
            selectedCanvasGroup.blocksRaycasts = false;
        }

    }
}
