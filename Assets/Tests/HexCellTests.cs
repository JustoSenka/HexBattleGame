using Assets;
using NUnit.Framework;
using UnityEngine;

public class HexCellTests
{
    private static int2[] DifferentTypesOfInt2()
    {
        return new[]
        {
            new int2(5, 5),
            new int2(16, 64),
            new int2(21, 44),
            new int2(-97, 85),
            new int2(25, -16),
            new int2(-40, -50),
            new int2(-5, -5),
        };
    }

    private static object[][] DifferentTypesOfPositions()
    {
        return new[]
        {
           new object[] {new Vector3(4.028f, 0, 9.211f), new int2(5, 11) },
           new object[] {new Vector3(1.668f, 0, 5.935f), new int2(2, 7) },
           new object[] {new Vector3(3.557f, 0, 6.681f), new int2(4, 8) },
           new object[] {new Vector3(3.761f, 0, -1.439f), new int2(4, -2) },
           new object[] {new Vector3(-1.190f, 0, 7.962f), new int2(-1, 9) },
           new object[] {new Vector3(-2.241f, 0, -7.902f), new int2(-2, -9) },
           new object[] {new Vector3(87.826f, 0, -50.961f), new int2(88, -59) },
           new object[] {new Vector3(-18.951f, 0, 67.654f), new int2(-18, 79) },
           new object[] {new Vector3(-26.481f, 0, -54.710f), new int2(-26, -64) },
           new object[] {new Vector3(25.396f, 0, 71.796f), new int2(25, 84) },
        };
    }

    [TestCaseSource("DifferentTypesOfInt2")]
    public void GettingWolrdCoordsFromHex_AndBack_ReturnsSameHex(int2 hex)
    {
        var point = new HexCell(hex).WorldPosition;
        var newHex = HexCell.WorldPointToHex(point, 1);
        Assert.AreEqual(hex, newHex);
    }

    [TestCaseSource("DifferentTypesOfPositions")]
    public void GettingHexFromRealWorld_AndBack_ReturnsSameHex(Vector3 point, int2 expectedHex)
    {
        var originalHex = HexCell.WorldPointToHex(point, 1);

        var centerOfHex = new HexCell(originalHex).WorldPosition;
        var newHex = HexCell.WorldPointToHex(centerOfHex, 1);

        Assert.AreEqual(expectedHex, originalHex, "Original coordinates produced incorect hex cell");
        Assert.AreEqual(originalHex, newHex, "Hexes should be the same before and after conversion");
    }

    [Test]
    public void FindingHexNeighbours_ReturnsCorrectNeighbours_ForLevel1_Even()
    {
        var neighbours = HexCell.FindNeighbours(new int2(0, 0));

        var expectedNneighbours = new[] { new int2(1, 0), new int2(-1, 0), new int2(0, 1), new int2(1, 1), new int2(0, -1), new int2(1, -1) };

        CollectionAssert.AreEquivalent(expectedNneighbours, neighbours);
    }

    [Test]
    public void FindingHexNeighbours_ReturnsCorrectNeighbours_ForLevel1_Odd()
    {
        var neighbours = HexCell.FindNeighbours(new int2(1, 1));

        var expectedNneighbours = new[] { new int2(2, 1), new int2(0, 1), new int2(0, 2), new int2(1, 2), new int2(0, 0), new int2(1, 0) };

        CollectionAssert.AreEquivalent(expectedNneighbours, neighbours);
    }

    [TestCase(1, 6)]
    [TestCase(2, 18)]
    [TestCase(3, 36)]
    [TestCase(4, 50)]
    [TestCase(5, 80)]
    public void FindingHexNeighbours_ReturnsCorrectAmountOfNeighbours(int level, int amountOfNeighbours)
    {
        var neighbours = HexCell.FindNeighbours(new int2(1, 1));
        Assert.AreEqual(amountOfNeighbours, neighbours.Length);
    }
}
