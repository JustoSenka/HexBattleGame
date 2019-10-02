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
        public float Size { get; private set; }

        public float OuterRadius { get; private set; }
        public float InnerRadius { get; private set; }

        [SerializeField]
        private int2 _position;
        public int2 Position { get => _position; private set => _position = value; }

        public Vector3 WorldPosition { get; private set; }

        public bool IsValid { get; private set; }

        public Vector3[] WorldCorners => new[]
            {
                new Vector3(WorldPosition.x, 0f, OuterRadius + WorldPosition.z),
                new Vector3(InnerRadius + WorldPosition.x, 0f, 0.5f * OuterRadius + WorldPosition.z),
                new Vector3(InnerRadius + WorldPosition.x, 0f, -0.5f * OuterRadius + WorldPosition.z),
                new Vector3(WorldPosition.x, 0f, -OuterRadius + WorldPosition.z),
                new Vector3(-InnerRadius + WorldPosition.x, 0f, -0.5f * OuterRadius + WorldPosition.z),
                new Vector3(-InnerRadius + WorldPosition.x, 0f, 0.5f * OuterRadius + WorldPosition.z)
            };

        public HexCell(int2 Position, float size = 1)
        {
            _position = Position;
            this.Size = size;

            OuterRadius = HexUtility.k_OuterRadius * size;
            InnerRadius = HexUtility.k_InnerRadius * size;

            WorldPosition = HexUtility.HexToWorldPoint(Position, size);

            IsValid = true;
        }

        public override string ToString() => Position.ToString();

        public override int GetHashCode() => Position.GetHashCode();
        public override bool Equals(object obj) => GetHashCode() == obj.GetHashCode();
        public static bool operator ==(HexCell c1, HexCell c2) => c1.Equals(c2);
        public static bool operator !=(HexCell c1, HexCell c2) => !c1.Equals(c2);

        // Static ---

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
}
