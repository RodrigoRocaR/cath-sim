using System.Collections.Generic;
using UnityEngine;

public static class GameConstants
{
    public const int BlockScale = 2;
    
    public const int EmptyBlock = -1;
    public const int SolidBlock = 0;
    public const int VictoryBlock = 1;
    public const int ImmovableBlock = 2;

    public const string SolidBlockName = "BlockSolid";
    public const string VictoryBlockName = "BlockVictory";
    public const string ImmovableBlockName = "BlockImmovable";
    
    public const string TimerTextName = "TextTimer"; // text shown on victory on the center 
    public const string TimerDisplayOnCornerName = "TimerDisplay"; // text on top right screen while playing

    public const float GameOverMinimumY = -20;

    public static readonly IReadOnlyDictionary<string, Vector3> PlayerInitialPosition = new Dictionary<string, Vector3>
    {   
        { "TestScene", new Vector3(2, 10, 2) },
        { "Level01", new Vector3(12, 3, 10) }
    };
}