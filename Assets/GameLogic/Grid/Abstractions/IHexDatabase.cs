namespace Assets
{
    public interface IHexDatabase
    {
        HexCell GetHex(int2 pos);
        Selectable GetSelectable(int2 pos);

        void UpdateHex(HexCell hex);
        void UpdateSelectable(Selectable obj);
    }
}
