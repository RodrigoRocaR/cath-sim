using UnityEngine;

namespace Player
{
    public class Inputs
    {
        private bool _forward, _backward, _right, _left, _multipleInputs, _anyInputs, _jump, _pull, _push;

        public void UpdateInputs()
        {
            _forward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
            _backward = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
            _right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
            _left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
            _jump = Input.GetKey(KeyCode.Space);
            _pull = Input.GetKey(KeyCode.Q);
            _push = Input.GetKey(KeyCode.E);
            _multipleInputs = (_forward && _backward) || (_forward && _right) || (_forward && _left) || 
                              (_backward && _right) || (_backward && _left) || (_right && _left);
            _anyInputs = _forward || _backward || _right || _left;
        }

        public bool Forward()
        {
            return _forward && !_multipleInputs;
        }
        public bool Backward()
        {
            return _backward && !_multipleInputs;
        }
        public bool Right()
        {
            return _right && !_multipleInputs;
        }
        public bool Left()
        {
            return _left && !_multipleInputs;
        }
        public bool AnyInputs()
        {
            return _anyInputs && !_multipleInputs;
        }

        public bool Horizontal()
        {
            return (_right || _left) && !_multipleInputs;
        }
        
        public bool Vertical()
        {
            return (_forward || _backward) && !_multipleInputs;
        }

        public bool Jump()
        {
            return _jump;
        }

        public bool Pull()
        {
            return _pull;
        }

        public bool Push()
        {
            return _push;
        }
    }
}
