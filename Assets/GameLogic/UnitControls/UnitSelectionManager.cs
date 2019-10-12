using System;
using System.Collections.Generic;

namespace Assets
{
    /// <summary>
    /// This class is respnsible for LOCAL player movement highlights, path finding and accepting move commands.
    /// Command will be forwarded to CrossPlayerController to be applied to all connected clients.
    /// Has callbacks when unit is selected, unselected, being moved to different position. 
    /// Also contains PathfinderDictionary for currently selected unit
    /// </summary>
    /// <remarks>
    /// This class acts on its own based on SelectionManager callbacks.
    /// It should not be used as a tool.
    /// </remarks>
    [RegisterDependency(typeof(IUnitSelectionManager), true)]
    public class UnitSelectionManager : IUnitSelectionManager
    {
        public PathfinderDictionary Paths { get; private set; }
        public Unit SelectedUnit { get; private set; }

        public bool CanSelectedUnitPerformActions => SelectedUnit != null && TurnManager.CurrentTurnOwner == SelectedUnit && SelectedUnit.Team == CrossPlayerController.LocalTeam;

        public event Action<Unit> UnitSelected;
        public event Action<Unit> TurnOwnerUnitSelected;

        public event Action<Unit> UnitUnselected;

        private readonly ISelectionManager SelectionManager;
        private readonly IHexPathfinder HexPathfinder;
        private readonly IHexDatabase HexDatabase;
        private readonly ITurnManager TurnManager;
        private readonly ICrossPlayerController CrossPlayerController;
        public UnitSelectionManager(ISelectionManager SelectionManager, IHexPathfinder HexPathfinder, IHexDatabase HexDatabase, ITurnManager TurnManager, ICrossPlayerController CrossPlayerController)
        {
            this.SelectionManager = SelectionManager;
            this.HexPathfinder = HexPathfinder;
            this.HexDatabase = HexDatabase;
            this.TurnManager = TurnManager;
            this.CrossPlayerController = CrossPlayerController;

            TurnManager.TurnStarted += OnTurnStarted;

            SelectionManager.SelectableSelected += OnSelectableSelected;
            SelectionManager.SelectableUnselected += OnSelectableUnselected;
        }

        public bool CanIControlThisUnit(Selectable unit)
        {
            return TurnManager.CurrentTurnOwner == unit && unit.Team == CrossPlayerController.LocalTeam;
        }

        // Callbacks ---

        private void OnTurnStarted(Selectable obj)
        {
            if (CanIControlThisUnit(obj))
                SelectionManager.SelectSelectable(obj);
        }

        private void OnSelectableSelected(Selectable obj)
        {
            if (obj is Unit unit)
                SelectUnit(unit);
        }

        private void OnSelectableUnselected(Selectable obj)
        {
            if (obj is Unit unit)
                UnselectUnit(unit);
        }

        // Private ---

        private void SelectUnit(Unit unit)
        {
            SelectedUnit = unit;
            Paths = HexPathfinder.FindAllPaths(unit.Cell, HexType.Empty, unit.Movement);

            /*if (CanIControlThisUnit(unit))
                TurnOwnerUnitSelected?.Invoke(unit);

            else*/
                UnitSelected?.Invoke(unit);
        }

        private void UnselectUnit(Unit unit)
        {
            SelectedUnit = default;
            UnitUnselected?.Invoke(unit);
        }
    }
}
