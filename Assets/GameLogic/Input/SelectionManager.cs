using System;

namespace Assets
{
    [RegisterDependency(typeof(ISelectionManager), true)]
    public class SelectionManager : ISelectionManager
    {
        public bool DoNotAllowOtherSystemsToChangeSelection { get; set; }

        public event Action<HexCell> HexClicked;
        public event Action<HexCell> HexSelected;
        public event Action<HexCell> HexUnselected;

        public event Action<Selectable> SelectableClicked;
        public event Action<Selectable> SelectableSelected;
        public event Action<Selectable> SelectableUnselected;

        public HexCell CurrentlySelectedHex { get; private set; }
        public Selectable CurrentlySelectedSelectable { get; private set; }

        public SelectionManager() { }

        public void SelectSelectable(Selectable selectable)
        {
            if (selectable != default)
                SelectableClicked?.Invoke(selectable);

            if (DoNotAllowOtherSystemsToChangeSelection)
                return;

            if (selectable != CurrentlySelectedSelectable)
            {
                if (CurrentlySelectedSelectable != default)
                    SelectableUnselected?.Invoke(CurrentlySelectedSelectable);

                CurrentlySelectedSelectable = selectable;

                if (selectable != default)
                    SelectableSelected?.Invoke(selectable);
            }
        }

        public void SelectHexCell(HexCell hexCell)
        {
            if (hexCell.IsValid)
                HexClicked?.Invoke(hexCell);

            if (DoNotAllowOtherSystemsToChangeSelection)
                return;

            if (hexCell != CurrentlySelectedHex)
            {
                if (CurrentlySelectedHex.IsValid)
                    HexUnselected?.Invoke(CurrentlySelectedHex);

                CurrentlySelectedHex = hexCell;

                if (hexCell.IsValid)
                    HexSelected?.Invoke(hexCell);
            }
        }
    }
}
