using System;

namespace Assets
{
    public interface ISelectionManager
    {
        bool DoNotAllowOtherSystemsToChangeSelection { get; set; }

        HexCell CurrentlySelectedHex { get; }
        Selectable CurrentlySelectedSelectable { get; }

        event Action<HexCell> HexClicked;
        event Action<HexCell> HexSelected;
        event Action<HexCell> HexUnselected;
        event Action<HexCell> HexSelectionAborted;

        event Action<Selectable> SelectableClicked;
        event Action<Selectable> SelectableSelected;
        event Action<Selectable> SelectableUnselected;
        event Action<Selectable> SelectableSelectionAborted;

        void SelectHexCell(HexCell hexCell);
        void SelectSelectable(Selectable selectable);
        void UnselectAll();
    }
}
