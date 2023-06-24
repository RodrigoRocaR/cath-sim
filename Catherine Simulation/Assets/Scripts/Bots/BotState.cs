namespace Bots
{
    public class BotState
    {
        private bool _isExploring, _isClimbing, _canExplore;

        public BotState()
        {
            _canExplore = true;
        }

        public bool IsBusy()
        {
            return _isExploring || _isClimbing;
        }
        
        public void StartExploring()
        {
            _isExploring = true;
        }
        
        public void StopExploring()
        {
            _isExploring = false;
            _canExplore = false;
        }

        public bool IsExploring()
        {
            return _isExploring;
        }

        public bool CanExplore()
        {
            return _canExplore && !_isExploring;
        }
        
        
        
        public void StartClimbing()
        {
            _isClimbing = true;
        }
        
        public void StopClimbing()
        {
            _isClimbing = false;
            _canExplore = true;
        }

        public bool IsClimbing()
        {
            return _isClimbing;
        }
        
        public void TriggerNext()
        {
            if (_isExploring)
            {
                _isExploring = false;
                _isClimbing = true;
            }
            else if (_isClimbing)
            {
                _isClimbing = false;
                _isExploring = true;
            }
            else
            {
                _isExploring = true;
            }
        }
    }
}