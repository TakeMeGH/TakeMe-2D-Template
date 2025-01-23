using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace TKM
{
    public class VolumeSettings : MonoBehaviour
    {
        [SerializeField] AudioMixer _mixer;
        [SerializeField] Slider _masterSlider;
        [SerializeField] Slider _SFXSlider;
        [SerializeField] Slider _BGMSlider;

        const string MASTER_VOLUME = "MasterVolume";
        const string SFX_VOLUME = "SFXVolume";
        const string BGM_VOLUME = "BGMVolume";
        const float MULTIPILER = 20;

        // TODO: Kayaknya masih bisa ada bug ketika playprefs belom ada sama sekali
        public void Setup()
        {
            if (PlayerPrefs.HasKey(MASTER_VOLUME))
            {
                LoadVolume();
            }

            SetMasterVolume();
            SetSFXVolume();
            SetBGMVolume();
        }

        public void SetMasterVolume()
        {
            float volume = _masterSlider.value;
            _mixer.SetFloat(MASTER_VOLUME, Mathf.Log10(volume) * MULTIPILER);
            PlayerPrefs.SetFloat(MASTER_VOLUME, volume);
        }

        public void SetSFXVolume()
        {
            float volume = _SFXSlider.value;
            _mixer.SetFloat(SFX_VOLUME, Mathf.Log10(volume) * MULTIPILER);
            PlayerPrefs.SetFloat(SFX_VOLUME, volume);
        }


        public void SetBGMVolume()
        {
            float volume = _BGMSlider.value;
            _mixer.SetFloat(BGM_VOLUME, Mathf.Log10(volume) * MULTIPILER);
            PlayerPrefs.SetFloat(BGM_VOLUME, volume);
        }


        void LoadVolume()
        {
            _masterSlider.value = PlayerPrefs.GetFloat(MASTER_VOLUME);
            _BGMSlider.value = PlayerPrefs.GetFloat(BGM_VOLUME);
            _SFXSlider.value = PlayerPrefs.GetFloat(SFX_VOLUME);
        }
    }
}
