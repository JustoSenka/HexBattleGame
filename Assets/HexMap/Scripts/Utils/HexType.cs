using System;

namespace Assets
{
    [Flags]
    public enum HexType
    {
        Empty = 1,
        Obstacle = 2,
        River = 4,
    }
}
