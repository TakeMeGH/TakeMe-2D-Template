using UnityEngine;
using UnityEngine.Audio;

namespace TKM
{
    public enum SFX
    {
        ChatPopUp
    }

    public enum BGM
    {
        ChatPopUp
    }


    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] AudioMixer _mixer;
        const string MASTER_VOLUME = "MasterVolume";
        const string SFX_VOLUME = "SFXVolume";
        const string BGM_VOLUME = "BGMVolume";
        const float MULTIPILER = 20;
        #region SFX
        [SerializeField] AudioSource Jumping;
        #endregion
        #region BGM
        // [SerializeField] AudioSource InGame;
        #endregion

        void Start()
        {
            LoadMixer();
        }
        void LoadMixer()
        {
            if (!PlayerPrefs.HasKey(MASTER_VOLUME))
            {
                return;
            }

            float volume = PlayerPrefs.GetFloat(MASTER_VOLUME);
            _mixer.SetFloat(MASTER_VOLUME, Mathf.Log10(volume) * MULTIPILER);
            PlayerPrefs.SetFloat(MASTER_VOLUME, volume);

            volume = PlayerPrefs.GetFloat(SFX_VOLUME);
            _mixer.SetFloat(SFX_VOLUME, Mathf.Log10(volume) * MULTIPILER);
            PlayerPrefs.SetFloat(SFX_VOLUME, volume);

            volume = PlayerPrefs.GetFloat(BGM_VOLUME);
            _mixer.SetFloat(BGM_VOLUME, Mathf.Log10(volume) * MULTIPILER);
            PlayerPrefs.SetFloat(BGM_VOLUME, volume);
        }

        public void PlayBGM(BGM bgmType)
        {
            // StopAllBGM();

            // switch (bgmType)
            // {
            //     case GeneraBGM.WinningScene:
            //         WinningScene.Play();
            //         break;
            //     case GeneraBGM.InGame:
            //         InGame.Play();
            //         break;
            //     case GeneraBGM.MainMenu:
            //         MainMenu.Play();
            //         break;
            //     case GeneraBGM.NRoom:
            //         NRoom.Play();
            //         break;
            //     default:
            //         Debug.LogWarning("Unknown BGM type");
            //         break;
            // }
        }
        private void StopAllBGM()
        {
            // WinningScene.Stop();
            // InGame.Stop();
            // MainMenu.Stop();
            // NRoom.Stop();
        }

        public void PlaySFX(SFX sfxType)
        {
            // switch (sfxType)
            // {
            //     case GeneralSFX.Jumping:
            //         Jumping.Play();
            //         break;
            //     case GeneralSFX.Walking:
            //         Walking.Play();
            //         break;
            //     case GeneralSFX.Dipping:
            //         Dipping.Play();
            //         break;
            //     case GeneralSFX.Ded:
            //         Ded.Play();
            //         break;
            //     case GeneralSFX.DoorOpen:
            //         DoorOpen.Play();
            //         break;
            //     case GeneralSFX.Fire:
            //         Fire.Play();
            //         break;
            //     case GeneralSFX.KeyTaken:
            //         KeyTaken.Play();
            //         break;
            //     case GeneralSFX.DraggingBox:
            //         DraggingBox.Play();
            //         break;
            //     case GeneralSFX.IdleMeow:
            //         IdleMeow.Play();
            //         break;
            //     case GeneralSFX.FireHit:
            //         FireHit.Play();
            //         break;
            //     default:
            //         Debug.LogWarning("Unknown SFX type");
            //         break;
            // }
        }

    }
}
