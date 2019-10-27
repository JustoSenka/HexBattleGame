using System;
using System.Collections.Generic;

namespace Assets
{
    public interface IHexDatabase
    {
        event Action<Map> BeforeMapUnload;
        event Action<Map> AfterMapLoaded;

        event Action<Selectable> SelectableAdded;
        event Action<Selectable> SelectableRemoved;

        HexCell GetHex(int2 pos);
        Selectable GetSelectable(int2 pos);

        IEnumerable<Selectable> Selectables { get; }

        void UnloadMap();
        void LoadMap(Map map);

        void UpdateHexCell(HexCell hex);
        void AddNewSelectable(Selectable obj);
        void RemoveSelectable(Selectable obj);
    }
}
