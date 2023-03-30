using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{

    public Vector3 Target { get; set; }
    public bool IsMoving { get; set; }
    public bool IsFalling { get; set; }
    public bool HasFoundation { get; set; }
    public bool IsBlockInFront { get; set; }
    public bool IsWallInFront { get; set; }
    public bool IsJumping { get; set; }

    public void UpdateIsFalling(Rigidbody rb) 
    {
        IsFalling = !rb.IsSleeping() && rb.velocity.y < -0.1;;
    }
    
    public void CheckForBlocksInFront()
    {
        Vector3 checkPos = Target; // on top of the block we are going (even height)
        
        checkPos.y -= 1;
        HasFoundation = Level.GetBlock(checkPos) != -1;
        checkPos.y += Level.BlockScale;
        IsBlockInFront = Level.GetBlock(checkPos) != -1;
        checkPos.y += Level.BlockScale;
        IsWallInFront = Level.GetBlock(checkPos) != -1;
    }
}