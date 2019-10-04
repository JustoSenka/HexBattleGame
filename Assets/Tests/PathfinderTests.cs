using NUnit.Framework;
using System.Linq;

namespace Assets
{
    public class PathfinderTests
    {
        private static int2[] OddAndEvenInt2()
        {
            return new[]
            {
                new int2(4, 4),
                new int2(4, 5),
                new int2(5, 5),
            };
        }

        private static int2[] SomeRandomInt2()
        {
            return new[]
            {
                new int2(6, 5),
                new int2(-5, 10),
            };
        }

        [Test]
        // This test is a bit slow, so doing both things, checking Covered Area and Computed path
        public void Pathfinder_CoveredCells_AndPathSize_AreCorrectOnEmptyMap([ValueSource("OddAndEvenInt2")] int2 cellFrom, [ValueSource("SomeRandomInt2")] int2 cellTo)
        {
            var c = TestUtility.CreateContainer();
            var pathfinder = c.Resolve<IHexPathfinder>();

            var distance = HexUtility.ManhattanDistance(cellFrom, cellTo);
            var coveredCellsExpected = HexUtility.FindNeighbours(cellFrom, distance);
            var pathMap = pathfinder.FindAllPaths(cellFrom, HexType.Empty, distance);
            var coveredCellsActual = pathMap.CoveredCells();
            var pathToDestination = pathMap.CalculatePath(cellTo);

            Assert.AreEqual(coveredCellsExpected.Length, coveredCellsActual.Count(), "Collection sizes were different");
            CollectionAssert.AreEquivalent(coveredCellsExpected, coveredCellsActual, "Elements inside were different");
            Assert.AreEqual(distance, pathToDestination.Count(), "Calculated path size was incorrect");
        }
    }
}
