using System.Collections.Generic;
using System.Linq;

namespace Assets
{
    [RegisterDependency(typeof(IHexPathfinder), true)]
    public class HexPathfinder : IHexPathfinder
    {
        private readonly IHexDatabase HexDatabase;
        public HexPathfinder(IHexDatabase HexDatabase)
        {
            this.HexDatabase = HexDatabase;
        }

        public PathfinderDictionary FindAllPaths(int2 cell, HexType typeFlags, int distance)
        {
            return FindAllPaths(HexDatabase.GetCell(cell), typeFlags, distance);
        }

        public PathfinderDictionary FindAllPaths(HexCell hex, HexType typeFlags, int distance)
        {
            var queue = new PathfinderQueue();
            var map = new PathfinderDictionary();
            queue.Enqueue((hex.Position, hex.Position, 0));
            TraverseAll(queue, map, typeFlags, distance);
            return map;
        }

        private void TraverseAll(
            Queue<(int2 Cell, int2 CameFrom, int DistanceToMe)> queue,
            Dictionary<int2, (int2 CameFrom, int DistanceToMe)> map,
            HexType typeFlags, int maxDistance)
        {
            var startingPos = queue.Peek().Cell;
            while (queue.Count > 0)
            {
                var (Cell, CameFrom, DistanceToMe) = queue.Dequeue();
                var hex = HexDatabase.GetCell(Cell);

                // If newly visited node or new distance is shorter, update path map
                if (!map.ContainsKey(Cell) || map[Cell].DistanceToMe > DistanceToMe)
                    map[Cell] = (CameFrom, DistanceToMe);

                // Stop if max distance was reached
                if (DistanceToMe == maxDistance)
                    continue;

                // Add neighbouring cells to the queue to be visited
                foreach (var nCell in HexUtility.FindNeighbours(Cell))
                {
                    // If already visited the cell, don't visit it again since we know the distance cannot be smaller anymore
                    if (map.ContainsKey(nCell))
                        continue;
                    
                    // Check flags, and if possible to traverse this terrain, enqueue and continue
                    var nHex = HexDatabase.GetCell(nCell);
                    if (typeFlags.HasFlag(nHex.Type))
                        queue.Enqueue((nCell, Cell, DistanceToMe + 1));
                }
            }

            map.Remove(startingPos);
        }
    }

    public class PathfinderDictionary : Dictionary<int2, (int2 CameFrom, int DistanceToMe)>
    {
        public int2[] CalculatePathArray(int2 CellTo)
        {
            return CalculatePath(CellTo).ToArray();
        }

        public IEnumerable<int2> CalculatePath(int2 CellTo)
        {
            var el = this[CellTo];
            yield return CellTo;
            while (el.DistanceToMe > 1)
            {
                yield return el.CameFrom;
                el = this[el.CameFrom]; // This should not reach the origin point, because dictionary does not store it
            } 
        }

        public int2[] CoveredCellsArray()
        {
            return CoveredCells().ToArray();
        }

        public IEnumerable<int2> CoveredCells()
        {
            return Keys;
        }
    }

    public class PathfinderQueue : Queue<(int2 Cell, int2 CameFrom, int DistanceToMe)> { }
}
