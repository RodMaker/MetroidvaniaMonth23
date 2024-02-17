using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Bardent
{
    public class MenuManager : MonoBehaviour
    {
        public AudioMixer audioMixer;

        public Slider musicSlider;
        public Slider sfxSlider;

        private bool firstTime = true;

        private void Start()
        {
            LoadVolume();

            if (firstTime)
            {   
                MusicManager.Instance.PlayMusic("Menu");
            }
        }

        public void ChangeScene(string name)
        {
            firstTime = false;
            MusicManager.Instance.PlayMusic("Game");
            SceneManager.LoadScene(name);
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void UpdateMusicVolume(float volume)
        {
            audioMixer.SetFloat("MusicVolume", volume);
        }

        public void UpdateSoundVolume(float volume)
        {
            audioMixer.SetFloat("SFXVolume", volume);
        }

        public void SaveVolume()
        {
            audioMixer.GetFloat("MusicVolume", out float musicVolume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);

            audioMixer.GetFloat("SFXVolume", out float sfxVolume);
            PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        }

        public void LoadVolume()
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        }
    }
}
