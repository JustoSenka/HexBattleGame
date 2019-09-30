using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets
{
    [Serializable]
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
                new int2(c.x + 1, c.y + 1),
                new int2(c.x - 1, c.y - 1),
            };
        }

        /// <summary>
        /// Finds all neighbours to the cell within level.
        /// Level 1 means direct neighbours. (6 neighbours in total surounding one cell)
        /// Level 2 will return all neighbours which are 2 units away from the cell (18 neighbours in total)
        /// Return array does not contain any duplicates
        /// </summary>
        public static int2[] FindNeighbours(int2 c, int level)
        {
            var set = new HashSet<int2>();
            AddAllNeighboursToTheSetRecursively(FindNeighbours(c), set, level);
            return set.ToArray();
        }

        private static void AddAllNeighboursToTheSetRecursively(int2[] lastNeighbours, HashSet<int2> set, int level)
        {
            level--;
            foreach (var n in lastNeighbours)
            {
                // This one is here on purpose so we do not call recursive funcion if neighbouring cell was already added. Will result in less function calls
                // even though set.Add doesn't care if value already exists or not
                if (!set.Contains(n)) 
                {
                    set.Add(n);

                    if (level > 0)
                        AddAllNeighboursToTheSetRecursively(FindNeighbours(n), set, level);
                }
            }
        }
    }
#pragma warning disable IDE1006 // Naming Styles
    [Serializable]
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

        public override int GetHashCode() => x + (y << 15);
        public override bool Equals(object obj) => GetHashCode() == obj.GetHashCode();

        public static bool operator ==(int2 c1, int2 c2) => c1.Equals(c2);
        public static bool operator !=(int2 c1, int2 c2) => !c1.Equals(c2);
    }
}
