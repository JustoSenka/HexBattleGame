namespace Assets
{
    public interface IHexHighlighter
    {
        PoolItem PlaceHighlighter(HexCell hexCell, Highlighter highlighter, PoolItem reusePoolItem = null);
        void Start();
    }
}
