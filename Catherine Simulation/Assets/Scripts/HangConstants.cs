public static class HangConstants
{
    // Distance constants
    public const float DistancePercentageToBlock = 1.5f;

    public const float HorizontalOffset =
        GameConstants.BlockScale / 2f * DistancePercentageToBlock;

    public const float VerticalOffset = GameConstants.BlockScale / (0.75f * GameConstants.BlockScale);

    private const float VerticalOffsetInverted = GameConstants.BlockScale - VerticalOffset;

    public const float CorneringForwardDistanceFromMidPosToTarget = GameConstants.BlockScale * 0.75f;
    public const float CorneringBackwardDistanceFromMidPosToTarget = GameConstants.BlockScale * 0.25f;

    // Times
    public const float FromBlockToEdgeTime = 0.35f;
    public const float FromBlockToEdgeTimeWhenGrabbing = 0.15f;
    public const float FromEdgeToHangTime = 0.15f;

    public const float HangSlideTime = 0.5f;

    public const float HangSlideToCornerEdgeTime = 0.25f;
    public const float HangSlideFromCornerEdgeToTargetTime = 0.25f;

    public const float SeparateFromBorderToFallTime = 0.35f;

    public static bool IsHanging(float y)
    {
        string inputStr = y.ToString("F3");
        string targetStr = VerticalOffsetInverted.ToString("F3");

        return inputStr[^3..] == targetStr[^3..];
    }
}