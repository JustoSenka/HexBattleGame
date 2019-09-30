namespace Assets
{
    public interface IHexDatabase
    {
        HexCell GetCell(int2 pos);
        void UpdateCell(HexCell hex);
    }
}
