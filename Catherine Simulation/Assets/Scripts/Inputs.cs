using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs
{
    private bool _forward, _backward, _right, _left, _multipleInputs, _anyInputs; // capture inputs

    public void UpdateInputs()
    {
        _forward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        _backward = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        _right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        _left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        _multipleInputs = (_forward && _backward) || (_forward && _right) || (_forward && _left) || 
                          (_backward && _right) || (_backward && _left) || (_right && _left);
        _anyInputs = _forward || _backward || _right || _left;
    }

    public bool Forward()
    {
        return _forward;
    }
    public bool Backward()
    {
        return _backward;
    }
    public bool Right()
    {
        return _right;
    }
    public bool Left()
    {
        return _left;
    }
    public bool AnyInputs()
    {
        return _anyInputs;
    }
    public bool MultipleInputs()
    {
        return _multipleInputs;
    }
}
