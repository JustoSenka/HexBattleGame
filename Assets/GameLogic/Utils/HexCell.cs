using System;
using System.Diagnostics;
using UnityEngine;

namespace Assets
{
    /// <summary>
    /// Calculations in static functions are only valid in Even-R Hexagon arrangement
    /// </summary>
    [Serializable]
    [DebuggerDisplay("HexCell {IsValid}: ({Position.x}, {Position.y})")]
    public struct HexCell
    {
        public bool IsValid { get; private set; }

        public HexType Type { get; set; }
        private readonly HexType m_DefaultType;

        public float Size { get; private set; }

        [SerializeField]
        private int2 _position;
        public int2 Position { get => _position; private set => _position = value; }

        public float OuterRadius => HexUtility.k_OuterRadius * Size;
        public float InnerRadius => HexUtility.k_InnerRadius * Size;
        public Vector3 WorldPosition => HexUtility.HexToWorldPoint(Position, Size);

        public Vector3[] WorldCorners => new[]
            {
                new Vector3(WorldPosition.x, 0f, OuterRadius + WorldPosition.z),
                new Vector3(InnerRadius + WorldPosition.x, 0f, 0.5f * OuterRadius + WorldPosition.z),
                new Vector3(InnerRadius + WorldPosition.x, 0f, -0.5f * OuterRadius + WorldPosition.z),
                new Vector3(WorldPosition.x, 0f, -OuterRadius + WorldPosition.z),
                new Vector3(-InnerRadius + WorldPosition.x, 0f, -0.5f * OuterRadius + WorldPosition.z),
                new Vector3(-InnerRadius + WorldPosition.x, 0f, 0.5f * OuterRadius + WorldPosition.z)
            };

        public HexCell(int2 Position, HexType hexType = HexType.Empty, float size = 1)
        {
            _position = Position;
            this.Size = size;

            IsValid = true;
            m_DefaultType = hexType;
            Type = hexType;
        }

        public void ResetHexType()
        {
            Type = m_DefaultType;
        }

        public override string ToString() => Position.ToString();

        public override int GetHashCode() => IsValid ? Position.GetHashCode() : int.MinValue;
        public override bool Equals(object obj) => GetHashCode() == obj.GetHashCode();
        public static bool operator ==(HexCell c1, HexCell c2) => c1.Equals(c2);
        public static bool operator !=(HexCell c1, HexCell c2) => !c1.Equals(c2);
    }

#pragma warning disable IDE1006 // Naming Styles
    [Serializable]
    [DebuggerDisplay("int2: ({x}, {y})")]
    public struct int2
#pragma warning restore IDE1006 // Naming Styles
    {
        public int x;
        public int y;

        public int2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString() => $"({x}, {y})";

        public override int GetHashCode() => x + (y << 15);
        public override bool Equals(object obj) => GetHashCode() == obj.GetHashCode();

        public static bool operator ==(int2 c1, int2 c2) => c1.Equals(c2);
        public static bool operator !=(int2 c1, int2 c2) => !c1.Equals(c2);
    }

#pragma warning disable IDE1006 // Naming Styles
    [Serializable]
    [DebuggerDisplay("int2: ({x}, {y}, {z})")]
    public struct int3
#pragma warning restore IDE1006 // Naming Styles
    {
        public int x;
        public int y;
        public int z;

        public int3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString() => $"({x}, {y}, {z})";

        public override int GetHashCode() => x + (y << 15) + (z << 7);
        public override bool Equals(object obj) => GetHashCode() == obj.GetHashCode();

        public static bool operator ==(int3 c1, int3 c2) => c1.Equals(c2);
        public static bool operator !=(int3 c1, int3 c2) => !c1.Equals(c2);

        public static int3 operator +(int3 c1, int3 c2) => new int3(c1.x + c2.x, c1.y + c2.y, c1.z + c2.z);
        public static int3 operator -(int3 c1, int3 c2) => new int3(c1.x - c2.x, c1.y - c2.y, c1.z - c2.z);
    }
}
