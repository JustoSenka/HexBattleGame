namespace Assets
{
    public interface IHexDatabase
    {
        void Start();

        HexCell GetCell(int2 pos);
        void UpdateCell(HexCell hex);
    }
}
