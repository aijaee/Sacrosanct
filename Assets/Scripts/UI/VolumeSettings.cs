using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider mainSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Slider enviSlider;
    [SerializeField] private Slider UISlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("mainVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMainVolume();
            SetMusicVolume();
            SetSFXVolume();
            SetEnviVolume();
            SetUIVolume();
        }
    }
    public void SetMainVolume()
    {
        float volume = mainSlider.value;
        myMixer.SetFloat("Main", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("mainVolume", volume);
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void SetEnviVolume()
    {
        float volume = enviSlider.value;
        myMixer.SetFloat("Environment", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("enviVolume", volume);
    }

    public void SetUIVolume()
    {
        float volume = UISlider.value;
        myMixer.SetFloat("UI", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("UIVolume", volume);
    }

    private void LoadVolume()
    {
        mainSlider.value = PlayerPrefs.GetFloat("mainVolume");
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        enviSlider.value = PlayerPrefs.GetFloat("enviVolume");
        UISlider.value = PlayerPrefs.GetFloat("UIVolume");

        SetMainVolume();
        SetMusicVolume();
        SetSFXVolume();
        SetEnviVolume();
        SetUIVolume();
    }
}
