namespace Bots.DS.MonteCarlo
{
    public static class Parameters
    {
        public const int C = 2; // Balance between exploration and exploitation
        public const int RolloutDepth = 10; // Nodes to be explored when a rollout happens
        public const int MaxIterations = 10_000; // To prevent the game from crashing in case of bug
    }
}