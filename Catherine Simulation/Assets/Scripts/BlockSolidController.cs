using System.Collections;
using System.Collections.Generic;
using System.Timers;
using LevelDS;
using UnityEngine;

public class BlockSolidController : MonoBehaviour
{
    public float moveDurationSeconds = 1f; 
        
    private bool _isBeingPulled;
    private Vector3 _targetPos;
    private Vector3 _startPos;
    
    private float _moveProgress;
    private float _moveElapsedTime;
    
    // Start is called before the first frame update
    void Start()
    {
        ResetBlockState();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void Pull(Vector3 playerPos)
    {
        if (_isBeingPulled) return;
        if (Vector3.Distance(playerPos, transform.position) > Level.BlockScale)
        {
            Debug.LogError("Pulling block wrong. Player pos: " + playerPos + "; Block pos: " + transform.position);
            return;
        }
        
        _isBeingPulled = true;
        _targetPos = playerPos;
        _startPos = transform.position;
    }

    private void Move()
    {
        if (_moveElapsedTime >= moveDurationSeconds && _moveElapsedTime != 0f)
        {
            ResetBlockState();
        }
        
        if (_isBeingPulled)
        {
            _moveProgress = _moveElapsedTime / moveDurationSeconds;
            transform.position = Vector3.Lerp(_startPos, _targetPos, _moveProgress);
            _moveElapsedTime += Time.deltaTime;
        }
    }

    private void ResetBlockState()
    {
        _isBeingPulled = false;
        _moveElapsedTime = 0f;
        _moveProgress = 0f;
    }
}
