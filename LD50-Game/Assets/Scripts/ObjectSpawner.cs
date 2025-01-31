﻿using System.Collections.Generic;
using System.Linq;
using EventArgs;
using Nidavellir.ResourceControllers;
using Scriptables;
using UnityEngine;

namespace Nidavellir
{
    public class ObjectSpawner : MonoBehaviour
    {
        [SerializeField] private List<ObjectSpawnerData> m_spawnerData;
        [SerializeField] private ObjectSpawnerData m_emergencyCanisterData;

        private BoxCollider m_collider;
        private float m_framesSinceLastSpawn;
        private FuelResourceController m_fuelResourceController;
        private float m_lastSpawnPositionForX;

        private Dictionary<ObjectSpawnerData, int> m_pastFramesSinceLastSpawn;

        private void Awake()
        {
            this.m_collider = this.GetComponent<BoxCollider>();
            this.m_pastFramesSinceLastSpawn = this.m_spawnerData.ToDictionary(o => o, o => 0);
            this.m_fuelResourceController = FindObjectOfType<FuelResourceController>();
        }

        private void Start()
        {
            if (this.m_fuelResourceController != null && this.m_emergencyCanisterData != null)
                this.m_fuelResourceController.ResourceController.ResourceValueChanged += this.OnFuelValueChanged;
        }

        private void FixedUpdate()
        {
            foreach (var key in this.m_pastFramesSinceLastSpawn.Keys.ToList())
            {
                if (this.m_pastFramesSinceLastSpawn[key] >= key.FrameCoolDown)
                {
                    this.SpawnObject(key);
                    this.m_pastFramesSinceLastSpawn[key] = 0;
                }
                else
                {
                    this.m_pastFramesSinceLastSpawn[key]++;
                }
            }
        }

        public void ActivateData(List<ObjectSpawnerData> data)
        {
            this.m_pastFramesSinceLastSpawn = data.ToDictionary(o => o, o => 0);
            this.m_spawnerData = data;
        }

        private void ActivateEmergencyCanisterRain()
        {
            if (!this.m_pastFramesSinceLastSpawn.ContainsKey(this.m_emergencyCanisterData))
                this.m_pastFramesSinceLastSpawn.Add(this.m_emergencyCanisterData, 0);
        }

        private void DeactivateEmergencyCanisterRain()
        {
            this.m_pastFramesSinceLastSpawn.Remove(this.m_emergencyCanisterData);
        }

        private float GetRandomPositionForX()
        {
            var maxX = this.m_collider.bounds.extents.x + this.transform.position.x;
            var minX = -this.m_collider.bounds.extents.x + this.transform.position.x;
            return Random.Range(minX, maxX);
        }

        private Vector3 GetRandomTorque(ObjectSpawnerData data)
        {
            return new Vector3(Random.Range(data.MinRotationSpeed, data.MaxRotationSpeed + 1), Random.Range(data.MinRotationSpeed, data.MaxRotationSpeed + 1),
                Random.Range(data.MinRotationSpeed, data.MaxRotationSpeed + 1));
        }

        private float GetRandomVelocity(ObjectSpawnerData data)
        {
            return Random.Range(data.MinVelocity, data.MaxVelocity + 1);
        }

        private void OnFuelValueChanged(object sender, ResourceValueChangedEventArgs e)
        {
            if (e.NewValue / this.m_fuelResourceController.ResourceController.MaxValue <= 0.33f)
                this.ActivateEmergencyCanisterRain();
            else
                this.DeactivateEmergencyCanisterRain();
        }

        private void SpawnObject(ObjectSpawnerData data)
        {
            var spawned = Instantiate(data.ToSpawn, new Vector3(this.GetRandomPositionForX(), this.transform.position.y, this.transform.position.z), Quaternion.identity);
            var asteroid = spawned.GetComponent<SpawnableObject>();
            asteroid.SetConstantVelocity(new Vector3(0f, 0f, -this.transform.forward.z * this.GetRandomVelocity(data)));
            asteroid.SetRandomRotation(this.GetRandomTorque(data));
        }
    }
}