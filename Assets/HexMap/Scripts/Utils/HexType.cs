using System;

namespace Assets
{
    [Flags]
    public enum HexType
    {
        Empty = 1,
        Obstacle = 2,
        HighObstacle = 4,
        Unit = 8,
        River = 16,
    }
}
