namespace Assets
{
    public interface IHexPathfinder
    {
        PathfinderDictionary FindAllPaths(int2 cell, HexType typeFlags, int distance);
        PathfinderDictionary FindAllPaths(HexCell hex, HexType typeFlags, int distance);
    }
}