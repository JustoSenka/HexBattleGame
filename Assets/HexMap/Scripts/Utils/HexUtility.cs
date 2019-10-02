using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets
{
    public static class HexUtility
    {
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

        // Methods ----- 

        /// <summary>
        /// Fancy mathematical function to transpose from two integeres which define cell position in array to
        /// real world coordinates
        /// </summary>
        public static Vector3 HexToWorldPoint(int x, int y, float cellSize) => HexToWorldPoint(new int2(x, y), cellSize);
        public static Vector3 HexToWorldPoint(int2 p, float cellSize)
        {
            var newX = p.y % 2 == 0 ? k_InnerRadius * p.x : k_InnerRadius * (p.x - 0.5f);
            var newZ = p.y * 0.75f * (k_OuterRadius + k_HeightIrrationality);
            return new Vector3(newX * cellSize, 0, newZ * cellSize);
        }

        /// <summary>
        /// Reverse of the function HexToWorldPoint
        /// </summary>
        public static int2 WorldPointToHex(Vector3 p, float cellSize)
        {
            var newZ = Mathf.RoundToInt(p.z / (0.75f * (k_OuterRadius + k_HeightIrrationality) * cellSize));
            var newX = newZ % 2 == 0 ? Mathf.RoundToInt(p.x / (k_InnerRadius * cellSize)) : Mathf.RoundToInt(p.x / (k_InnerRadius * cellSize) + 0.5f);
            return new int2(newX, newZ);
        }

        /// <summary>
        /// Returns dirrect neighbours to the cell
        /// </summary>
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
        /// An optimized way to find manhattan distance between to hex cells using intersection bettween those two points.
        /// O(1) complexity.
        /// </summary>
        public static int ManhattanDistance(int2 a, int2 b)
        {
            var diffX = Math.Abs(a.x - b.x);
            var diffY = Math.Abs(a.y - b.y);

            var midCell = FindIntersectingHexCell(a, b);

            if (diffX > Math.Abs(a.x - midCell.x))
                return Math.Abs(midCell.x - b.x) + Math.Abs(midCell.y - a.y);
            else
                return diffY;
        }

        /// <summary>
        /// An optimized algorithm to find intersecting cell between two points.
        /// Resulting cell will always have the same Y as second arguemnt B. And will be on 60° angle line above or below A
        /// Distance from A to intersecting cell is always difference in Y values.
        /// Distance from B to intersecting cell is always difference in X values since they are on same horizonal line
        /// O(1) complexity.
        /// </summary>
        public static int2 FindIntersectingHexCell(int2 a, int2 b)
        {
            var diffY = Math.Abs(a.y - b.y);

            // Adjustments for odd/even X to since on hex grid their coords alternate by 0.5 everytime going up
            // it also differs if we start on odd or even cell on Y axis. When going up an edge of hexagon, x changes every 2 y's
            int xMin, xMax;
            if (a.y % 2 == 0)
            {
                xMin = diffY % 4 == 1 ? 1 : 0;
                xMax = diffY % 4 == 3 ? 1 : 0;
            }
            else
            {
                xMin = diffY % 4 == 3 ? -1 : 0;
                xMax = diffY % 4 == 1 ? -1 : 0;
            }

            var xDiffInTheRow = Mathf.RoundToInt(diffY / 2f);
            var smallX = -diffY + xDiffInTheRow + xMin + a.x;
            var bigX = diffY - xDiffInTheRow + xMax + a.x;

            // smallX and bigX are both valid intersecting X coords, but we choose the one which is closer to the b hex
            var midCellX = Math.Abs(b.x - smallX) < Math.Abs(b.x - bigX) ? smallX : bigX;
            return new int2(midCellX, b.y);
        }


        /// <summary>
        /// An efficient algorithm to find all neighbours around a cell.
        /// Doesn't recursivelly go through all nodes, does not graph walk as well.
        /// Searches for all valid nodes on a rectangle which is level*2 wide and tall. 
        /// Does fancy math to remove nodes which are at far corners.
        /// Also handles odd/even starting positions as well.
        /// O(2level^2) complexity
        /// </summary>
        public static int2[] FindNeighbours(int2 c, int level)
        {
            var set = new HashSet<int2>();
            if (level == 0)
                return new[] { c };

            for (int y = -level + c.y; y <= level + c.y; y++)
            {
                var absDiffY = Math.Abs(-y + c.y);

                // Adjustments for odd/even X to since on hex grid their coords alternate by 0.5 everytime going up
                // it also differs if we start on odd or even cell on Y axis. When going up an edge of hexagon, x changes every 2 y's
                int xMin, xMax;
                if (c.y % 2 == 0)
                {
                    xMin = absDiffY % 4 == 1 ? 1 : 0;
                    xMax = absDiffY % 4 == 3 ? 1 : 0;
                }
                else
                {
                    xMin = absDiffY % 4 == 3 ? -1 : 0;
                    xMax = absDiffY % 4 == 1 ? -1 : 0;
                }

                var xDiffInTheRow = Mathf.RoundToInt(absDiffY / 2f);
                var smallX = -level + xDiffInTheRow + xMin + c.x;
                var bigX = level - xDiffInTheRow + xMax + c.x;

                // Final computation. Fill add all cells in specific Y row
                for (int x = smallX; x <= bigX; x++)
                    set.Add(new int2(x, y));
            }

            set.Remove(c);
            return set.ToArray();
        }

        #region Inefficient solutions lie here

        /// <summary>
        /// A very inefficient solution to find neighbours for a cell.
        /// Level 1 means direct neighbours. (6 neighbours in total surounding one cell)
        /// Level 2 will return all neighbours which are 2 units away from the cell (18 neighbours in total)
        /// Return array does not contain any duplicates.
        /// Should be used only in tests to verify that fast solutions still work as intended.
        /// O(6^level) complexity.
        /// </summary>
        public static int2[] DEPRECATE_FindNeighboursRecursive(int2 c, int level)
        {
            var set = new HashSet<int2>();
            DEPRECATE_AddAllNeighboursToTheSetRecursively(FindNeighbours(c), set, level);
            set.Remove(c);
            return set.ToArray();
        }

        private static void DEPRECATE_AddAllNeighboursToTheSetRecursively(int2[] lastNeighbours, HashSet<int2> set, int level)
        {
            level--;
            foreach (var n in lastNeighbours)
            {
                set.Add(n);

                if (level > 0)
                    DEPRECATE_AddAllNeighboursToTheSetRecursively(FindNeighbours(n), set, level);
            }
        }

        /// <summary>
        /// Very inefficient solution to find manhattan distance.
        /// Allocates a lot of garbage. But works.
        /// Should be used only in tests to verify that fast solutions still work as intended.
        /// O(2n^3) complexity
        /// </summary>
        public static int DEPRECATE_ManhattanDistanceRecursive(int2 s, int2 d)
        {
            for (int i = 1; i < 50; i++)
            {
                if (FindNeighbours(s, i).Contains(d))
                    return i;
            }
            return 51;
        }

        #endregion
    }
}
