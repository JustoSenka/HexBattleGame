using System;
using System.Collections.Generic;

namespace Assets
{
    public interface IHexDatabase
    {
        IEnumerable<Selectable> Selectables { get; }

        event Action<Selectable> SelectableAdded;
        event Action<Selectable> SelectableRemoved;

        HexCell GetHex(int2 pos);
        Selectable GetSelectable(int2 pos);

        void UpdateHexCell(HexCell hex);
        void AddNewSelectable(Selectable obj);
        void RemoveSelectable(Selectable obj);
    }
}
