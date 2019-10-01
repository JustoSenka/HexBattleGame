using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

            OuterRadius = k_OuterRadius * size;
            InnerRadius = k_InnerRadius * size;

            WorldPosition = HexToWorldPoint(Position, size);

            IsValid = true;
        }

        public override string ToString() => Position.ToString();

        public override int GetHashCode() => Position.GetHashCode();
        public override bool Equals(object obj) => GetHashCode() == obj.GetHashCode();
        public static bool operator ==(HexCell c1, HexCell c2) => c1.Equals(c2);
        public static bool operator !=(HexCell c1, HexCell c2) => !c1.Equals(c2);

        // Static ---
        public const float k_CellSizeMultiplier = 1.154700538379252f; // Will make cell inner radius equal to 1 unity meter, will equally enlarge outer radius as well

        public const float k_OuterRadius = k_InnerRadius * k_CellSizeMultiplier;
        public const float k_InnerRadius = 1;

        public const float k_HeightIrrationality = -8f / 675f; // Based in image we're using. Provides accuracy for up to 500 cells

        public static Vector3[] k_Corners =
        {
            new Vector3(0f, 0f, k_OuterRadius),
            new Vector3(k_InnerRadius, 0f, 0.5f * k_OuterRadius),
            new Vector3(k_InnerRadius, 0f, -0.5f * k_OuterRadius),
            new Vector3(0f, 0f, -k_OuterRadius),
            new Vector3(-k_InnerRadius, 0f, -0.5f * k_OuterRadius),
            new Vector3(-k_InnerRadius, 0f, 0.5f * k_OuterRadius)
        };


        /// <summary>
        /// Fancy mathematical function to transpose from two integeres which define cell position in array to
        /// real world coordinates
        /// </summary>
        public static Vector3 HexToWorldPoint(int x, int y, float cellSize) => HexToWorldPoint(new int2(x, y), cellSize);
        public static Vector3 HexToWorldPoint(int2 p, float cellSize)
        {
            var newX = p.y % 2 == 0 ? HexCell.k_InnerRadius * p.x : HexCell.k_InnerRadius * (p.x - 0.5f);
            var newZ = p.y * 0.75f * (HexCell.k_OuterRadius + k_HeightIrrationality);
            return new Vector3(newX * cellSize, 0, newZ * cellSize);
        }

        /// <summary>
        /// Reverse of the function HexToWorldPoint
        /// </summary>
        public static int2 WorldPointToHex(Vector3 p, float cellSize)
        {
            var newZ = Mathf.RoundToInt(p.z / (0.75f * (HexCell.k_OuterRadius + k_HeightIrrationality) * cellSize));
            var newX = newZ % 2 == 0 ? Mathf.RoundToInt(p.x / (HexCell.k_InnerRadius * cellSize)) : Mathf.RoundToInt(p.x / (HexCell.k_InnerRadius * cellSize) + 0.5f);
            return new int2(newX, newZ);
        }

        public static int2[] FindNeighbours(int2 c)
        {
            return new[]
            {
                new int2(c.x + 1, c.y),
                new int2(c.x - 1, c.y),
                new int2(c.x, c.y + 1),
                new int2(c.x, c.y - 1),
                new int2(c.y % 2 == 0 ? c.x + 1 : c.x - 1, c.y + 1),
                new int2(c.y % 2 == 0 ? c.x + 1 : c.x - 1, c.y - 1),
            };
        }

        /// <summary>
        /// Finds all neighbours to the cell within level.
        /// Level 1 means direct neighbours. (6 neighbours in total surounding one cell)
        /// Level 2 will return all neighbours which are 2 units away from the cell (18 neighbours in total)
        /// Return array does not contain any duplicates
        /// </summary>
        public static int2[] FindNeighboursRecursive(int2 c, int level)
        {
            var set = new HashSet<int2>();
            AddAllNeighboursToTheSetRecursively(FindNeighbours(c), set, level);
            set.Remove(c);
            return set.ToArray();
        }

        private static void AddAllNeighboursToTheSetRecursively(int2[] lastNeighbours, HashSet<int2> set, int level)
        {
            level--;
            foreach (var n in lastNeighbours)
            {
                set.Add(n);

                if (level > 0)
                    AddAllNeighboursToTheSetRecursively(FindNeighbours(n), set, level);
            }
        }


        /// <summary>
        /// Very inneficient solution to find manhattan distance.
        /// Allocates a lot of garbage. But works.
        /// </summary>
        public static int ManhattanDistance(int2 s, int2 d)
        {
            for (int i = 1; i < 50; i++)
            {
                if (FindNeighbours(s, i).Contains(d))
                    return i;
            }
            return 51;
        }

        /// <summary>
        /// A more efficient way to find all neighbours around a cell.
        /// Doesn't recursivelly go through all nodes, does not graph walk as well.
        /// Searches for all valid nodes on a rectangle which is level*2 wide and tall. 
        /// Does fancy math to remove nodes which are at far corners.
        /// Also handles odd/even starting positions as well
        /// </summary>
        public static int2[] FindNeighbours(int2 c, int level)
        {
            var set = new HashSet<int2>();
            if (level == 0)
                return new[] { c };

            for (int y = -level + c.y; y <= level + c.y; y++)
            {
                var absDiff = Math.Abs(-y + c.y);
                var xDiffInTheRow = Mathf.RoundToInt(absDiff / 2f);

                // Adjustments for odd/even X to since on hex grid their coords alternate by 0.5 everytime going up
                // it also differs if we start on odd or even cell on Y axis. When going up an edge of hexagon, x changes every 2 y's
                int xMin, xMax;
                if (c.y % 2 == 0)
                {
                    xMin = absDiff % 4 == 1 ? 1 : 0;
                    xMax = absDiff % 4 == 3 ? 1 : 0;
                }
                else
                {
                    xMin = absDiff % 4 == 3 ? -1 : 0;
                    xMax = absDiff % 4 == 1 ? -1 : 0;
                }

                // Final computation. Fill add all cells in specific Y row
                for (int x = -level + xDiffInTheRow + xMin + c.x; x <= level - xDiffInTheRow + xMax + c.x; x++)
                    set.Add(new int2(x, y));
            }

            set.Remove(c);
            return set.ToArray();
        }
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
