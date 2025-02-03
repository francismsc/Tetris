using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private Slider musicSlider;

    private void Start()
    {
        musicSource.Play();
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("Master", Mathf.Log10(volume) *20);
    }
}
