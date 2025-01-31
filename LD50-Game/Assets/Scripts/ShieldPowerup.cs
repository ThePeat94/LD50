﻿using Scriptables;
using UnityEngine;

namespace Nidavellir
{
    public class ShieldPowerup : MonoBehaviour
    {
        [SerializeField] private SfxData m_collectedSfx;
        private BlackHole m_blackHole;

        private OneShotSfxPlayer m_oneShotSfxPlayer;

        private void Awake()
        {
            this.m_oneShotSfxPlayer = this.GetComponent<OneShotSfxPlayer>();
            this.m_blackHole = FindObjectOfType<BlackHole>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<ShieldController>(out var shieldController))
            {
                shieldController.AddCharge();
                this.DisableMesh();
                this.m_oneShotSfxPlayer.PlayOneShot(this.m_collectedSfx);
                this.m_blackHole.EffectVelocity(-0.5f);
                Destroy(this.gameObject, this.m_collectedSfx.AudioClip.length);
            }
        }

        private void DisableMesh()
        {
            this.GetComponentInChildren<Collider>()
                .enabled = false;

            var renderers = this.GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in renderers)
                meshRenderer.enabled = false;

            var canvasses = this.GetComponentsInChildren<Canvas>();
            foreach (var canvas in canvasses)
                canvas.gameObject.SetActive(false);
        }
    }
}