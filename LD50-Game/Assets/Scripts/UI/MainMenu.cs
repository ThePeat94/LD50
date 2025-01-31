﻿using Nidavellir;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject m_startMenu;
        [SerializeField] private GameObject m_credits;
        [SerializeField] private Slider m_musicVolumeSlider;
        [SerializeField] private Slider m_sfxVolumeSlider;
        [SerializeField] private GameObject m_objectSpawner;
        [SerializeField] private GameObject m_optionsPanel;


        private void Awake()
        {
            this.m_musicVolumeSlider.onValueChanged.AddListener(this.MusicVolumeSliderChanged);
            this.m_sfxVolumeSlider.onValueChanged.AddListener(this.SfxVolumeSliderChanged);
            this.m_objectSpawner.SetActive(false);
        }

        private void Start()
        {
            this.m_musicVolumeSlider.value = GlobalSettings.Instance.MusicVolume;
            this.m_sfxVolumeSlider.value = GlobalSettings.Instance.SfxVolume;
        }

        public void BackFromCreditsToStart()
        {
            this.m_startMenu.SetActive(true);
            this.m_credits.SetActive(false);
            this.m_objectSpawner.SetActive(false);
        }

        public void BackToStartFromOptions()
        {
            this.m_optionsPanel.SetActive(false);
            this.m_startMenu.SetActive(true);
        }

        public void MusicVolumeSliderChanged(float volume)
        {
            GlobalSettings.Instance.MusicVolume = volume;
        }

        public void OpenLink(string url)
        {
            Application.OpenURL(url);
        }

        public void QuitApplication()
        {
            Application.Quit();
        }

        public void SfxVolumeSliderChanged(float volume)
        {
            GlobalSettings.Instance.SfxVolume = volume;
        }

        public void ShowCredits()
        {
            this.m_startMenu.SetActive(false);
            this.m_credits.SetActive(true);
            this.m_objectSpawner.SetActive(true);
        }

        public void ShowOptions()
        {
            this.m_optionsPanel.SetActive(true);
            this.m_startMenu.SetActive(false);
        }

        public void StartGame()
        {
            SceneManager.LoadScene(1);
        }
    }
}