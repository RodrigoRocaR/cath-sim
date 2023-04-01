namespace Player
{
    public static class PlayerStats
    {
        private static int _jumps;
        private static int _blocksWalked;
        private static int _totalActions;

        public static void AddJump()
        {
            _jumps++;
            _totalActions++;
        }

        public static void AddBlocksWalked()
        {
            _blocksWalked++;
            _totalActions++;
        }

        public static int GetJumps()
        {
            return _jumps;
        }

        public static int GetBlocksWalked()
        {
            return _blocksWalked;
        }

        public static int GetTotalActions()
        {
            return _totalActions;
        }
    }
}
