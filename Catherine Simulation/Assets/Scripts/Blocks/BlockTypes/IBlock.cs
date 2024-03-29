﻿using Player;
using UnityEngine;

namespace Blocks.BlockTypes
{
    public interface IBlock
    {
        public void TriggerPull(Transform playerTransform, PlayerState playerState, bool goingToHang = false);
        public void TriggerPush(Transform playerTransform, PlayerState playerState);
        public void OnPlayerStepOn();
    }
}