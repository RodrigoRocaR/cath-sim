using System;
using LevelDS;
using UnityEngine;
using Action = Bots.Action.Action;

namespace Player
{
    public class Inputs
    {
        private static Inputs _instance;
        
        private bool _forward, _backward, _right, _left, _multipleInputs, _anyInputs, _jump, _pull, _push;
        private bool _isHuman;

        private Inputs()
        {
            _isHuman = Level.GetPlayerIdentity() == PlayerIdentity.Player;
        }

        public static Inputs GetInstance()
        {
            if (_instance == null)
                _instance = new Inputs();
            return _instance;
        }

        public void UpdateInputs()
        {
            _multipleInputs = (_forward && _backward) || (_forward && _right) || (_forward && _left) ||
                              (_backward && _right) || (_backward && _left) || (_right && _left);
            _anyInputs = _forward || _backward || _right || _left;
            if (!_isHuman) return;
            _forward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
            _backward = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
            _right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
            _left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
            _jump = Input.GetKey(KeyCode.Space);
            _pull = Input.GetKey(KeyCode.Q);
            _push = Input.GetKey(KeyCode.E);
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

        public void StartAction(Action a)
        {
            TriggerAction(a, true);
        }
        
        public void StopAction(Action a)
        {
            TriggerAction(a, false);
        }
        
        private void TriggerAction(Action a, bool newValue)
        {
            switch (a)
            {
                case Action.Forward:
                    _forward = newValue;
                    break;
                case Action.Backward:
                    _backward = newValue;
                    break;
                case Action.Right:
                    _right = newValue;
                    break;
                case Action.Left:
                    _left = newValue;
                    break;
                case Action.Jump:
                    _jump = newValue;
                    break;
                case Action.Push:
                    _push = newValue;
                    break;
                case Action.Pull:
                    _pull = newValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(a), a, "Action not recognized at inputs level");
            }
        }
    }
}