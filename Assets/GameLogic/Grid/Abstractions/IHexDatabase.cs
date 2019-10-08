namespace Assets
{
    public interface IHexDatabase
    {
        HexCell GetHex(int2 pos);
        Selectable GetSelectable(int2 pos);

        void UpdateHexCell(HexCell hex);
        void AddNewSelectable(Selectable obj);
    }
}
