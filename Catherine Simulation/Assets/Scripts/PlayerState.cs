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
    
}