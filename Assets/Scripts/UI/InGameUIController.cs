using System;
using UnityEngine;

namespace TKM
{
    public class InGameUIController : MonoBehaviour
    {
        [SerializeField] InputReader _inputReader;
        [Header("Pause Menu")]
        [SerializeField] CanvasGroup _pauseMenu;

        void OnEnable()
        {
            _inputReader.PausePerformed += OnPause;
            _inputReader.UnPausePerformed += OnUnPause;
        }


        void OnDisable()
        {

        }
        private void OnPause()
        {
            _pauseMenu.alpha = 1;
            _pauseMenu.blocksRaycasts = true;
            _pauseMenu.interactable = true;

            _inputReader.EnableUIInput();
            Time.timeScale = 0f;
        }
        private void OnUnPause()
        {
            Time.timeScale = 1f;
            _inputReader.EnableGameplayInput();
            _pauseMenu.alpha = 0;
            _pauseMenu.blocksRaycasts = false;
            _pauseMenu.interactable = false;

        }
    }
}
