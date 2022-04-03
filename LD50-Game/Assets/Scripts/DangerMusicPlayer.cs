﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nidavellir
{
    public class DangerMusicPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip m_slowDangerMusic;
        [SerializeField] private AudioClip m_fastDangerMusic;

        private AudioSource m_audioSource;
        private BlackHole m_blackHole;
        private AudioClip m_currentClip;

        public static DangerMusicPlayer Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                SceneManager.sceneLoaded += this.OnSceneChanged;
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }

            this.m_audioSource = this.GetComponent<AudioSource>();
            this.m_audioSource.loop = true;
        }

        private void Update()
        {
            if (this.m_blackHole != null)
            {
                var pos = this.transform.position;
                pos.z = this.m_blackHole.Sphere.position.z;
                this.transform.position = pos;
            }
        }

        public void PlayFastDanger()
        {
            if (this.m_currentClip == this.m_fastDangerMusic)
                return;

            this.m_currentClip = this.m_fastDangerMusic;
            this.m_audioSource.clip = this.m_fastDangerMusic;
            this.m_audioSource.Play();
        }

        public void PlaySlowDanger()
        {
            if (this.m_currentClip == this.m_slowDangerMusic)
                return;

            this.m_currentClip = this.m_slowDangerMusic;
            this.m_audioSource.clip = this.m_slowDangerMusic;
            this.m_audioSource.Play();
        }

        public void Stop()
        {
            this.m_audioSource.Stop();
            this.m_currentClip = null;
        }

        private void OnSceneChanged(Scene arg0, LoadSceneMode arg1)
        {
            // TODO: CHANGE ME!!!!!
            if (arg0.buildIndex == 0)
                this.Stop();

            this.m_blackHole = FindObjectOfType<BlackHole>();
        }
    }
}