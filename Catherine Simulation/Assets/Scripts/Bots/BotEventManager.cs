using Bots.Action;

namespace Bots
{
    public static class BotEventManager
    {
        public delegate void ExplorationFinished();
        public static event ExplorationFinished OnExplorationFinished;
        
        public delegate void ClimbFinished();
        public static event ClimbFinished OnClimbFinished;

        public static void ActionExecutorFinished(ActionExecutorPurpose executorPurpose)
        {
            if (executorPurpose == ActionExecutorPurpose.Exploration) OnExplorationFinished?.Invoke();
            else if (executorPurpose == ActionExecutorPurpose.Climbing) OnClimbFinished?.Invoke();
        }
    }
}