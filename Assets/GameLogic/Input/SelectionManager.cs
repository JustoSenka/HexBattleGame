using System;

namespace Assets
{
    /// <summary>
    /// Shows selections only for LOCAL player.
    /// This will work independently for each user on network and is not used by enemy AIs.
    /// Visual feedback on local clients is depending mostly on this class.
    /// </summary>
    [RegisterDependency(typeof(ISelectionManager), true)]
    public class SelectionManager : ISelectionManager
    {
        public bool DoNotAllowOtherSystemsToChangeSelection { get; set; }

        public event Action<HexCell> HexClicked;
        public event Action<HexCell> HexSelected;
        public event Action<HexCell> HexUnselected;
        public event Action<HexCell> HexSelectionAborted;

        public event Action<Selectable> SelectableClicked;
        public event Action<Selectable> SelectableSelected;
        public event Action<Selectable> SelectableUnselected;
        public event Action<Selectable> SelectableSelectionAborted;


        public HexCell CurrentlySelectedHex { get; private set; }
        public Selectable CurrentlySelectedSelectable { get; private set; }

        public SelectionManager() { }

        public void SelectSelectable(Selectable selectable)
        {
            if (selectable != default)
                SelectableClicked?.Invoke(selectable);

            if (DoNotAllowOtherSystemsToChangeSelection)
            {
                SelectableSelectionAborted?.Invoke(selectable);
                return;
            }

            if (selectable != CurrentlySelectedSelectable)
            {
                if (CurrentlySelectedSelectable != default)
                    SelectableUnselected?.Invoke(CurrentlySelectedSelectable);

                CurrentlySelectedSelectable = selectable;

                if (selectable != default)
                    SelectableSelected?.Invoke(selectable);
            }

            // Unselect hexCell if selectable was selected
            if (selectable != default)
                SelectHexCell(default);
        }

        public void SelectHexCell(HexCell hexCell)
        {
            if (hexCell.IsValid)
                HexClicked?.Invoke(hexCell);

            if (DoNotAllowOtherSystemsToChangeSelection)
            {
                HexSelectionAborted?.Invoke(hexCell);
                return;
            }

            if (hexCell != CurrentlySelectedHex)
            {
                if (CurrentlySelectedHex.IsValid)
                    HexUnselected?.Invoke(CurrentlySelectedHex);

                CurrentlySelectedHex = hexCell;

                if (hexCell.IsValid)
                    HexSelected?.Invoke(hexCell);
            }

            // Unselect selectable if hexCell was selected
            if (hexCell.IsValid)
                SelectSelectable(default);
        }

        public void UnselectAll()
        {
            SelectSelectable(default);
            SelectHexCell(default);
        }
    }
}
