using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    
    public Vector3 Direction { get; set; }
    public Vector3 Target { get; set; }
    public bool IsMoving { get; set; }
    public bool IsFalling { get; set; }
    public bool HasFoundation { get; set; }
    public bool IsBlockInFront { get; set; }
    public bool IsWallInFront { get; set; }
    public bool IsJumping { get; set; }
    public bool IsBlockBelow { get; set; }

    public void UpdateIsFalling(Rigidbody rb) 
    {
        IsFalling = !rb.IsSleeping() && rb.velocity.y < -0.1;;
    }
    
    public void CheckForBlocksInFront()
    {
        Vector3 checkPos = Target; // on top of the block we are going (even height)
        
        checkPos.y -= 1 + Level.BlockScale;
        IsBlockBelow = Level.GetBlock(checkPos) != -1; // Block below ground
        checkPos.y += Level.BlockScale;
        HasFoundation = Level.GetBlock(checkPos) != -1; // Ground Level
        checkPos.y += Level.BlockScale;
        IsBlockInFront = Level.GetBlock(checkPos) != -1; // Block in front
        checkPos.y += Level.BlockScale;
        IsWallInFront = Level.GetBlock(checkPos) != -1; // Block on top in front
    }


    private void debugBlocksInFront(Vector3 startCheckPos)
    {
        startCheckPos.y -= 1 - Level.BlockScale;
        Debug.Log("Below: " + IsBlockBelow + "Checked Y: " + startCheckPos.y);
        startCheckPos.y += Level.BlockScale;
        Debug.Log("Foundation: " + HasFoundation + "Checked Y: " + startCheckPos.y);
        startCheckPos.y += Level.BlockScale;
        Debug.Log("Block: " + IsBlockInFront + "Checked Y: " + startCheckPos.y);
        startCheckPos.y += Level.BlockScale;
        Debug.Log("Wall: " + IsWallInFront + "Checked Y: " + startCheckPos.y);
    }
}