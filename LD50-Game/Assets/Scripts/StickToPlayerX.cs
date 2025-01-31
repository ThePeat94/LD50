﻿using UnityEngine;

namespace Nidavellir
{
    public class StickToPlayerX : MonoBehaviour
    {
        private void LateUpdate()
        {
            if (PlayerController.Instance == null)
                return;

            var playerX = PlayerController.Instance.transform.position.x;
            var pos = this.transform.position;
            pos.x = playerX;
            this.transform.position = pos;
        }
    }
}