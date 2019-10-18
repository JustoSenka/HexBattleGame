using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    /// <summary>
    /// This class is respnsible for player movement, path finding and accepting move commands.
    /// Command will be forwarded to CrossPlayerController to be applied to all connected clients.
    /// </summary>
    /// <remarks>
    /// This class acts on its own based on SelectionManager callbacks.
    /// It should not be used as a tool.
    /// </remarks>
    [RegisterDependency(typeof(IUnitMovementManager), true)]
    public class UnitMovementManager : IUnitMovementManager
    {
        public event Action<Action<Unit>, Unit, IEnumerable<int2>> UnitPositionChange;
        public event Action<Unit> UnitPositionChangeEnd;

        private readonly IUnitSelectionManager UnitSelectionManager;
        private readonly ISelectionManager SelectionManager;
        private readonly IHexDatabase HexDatabase;
        private readonly ICrossPlayerController CrossPlayerController;
        public UnitMovementManager(IUnitSelectionManager UnitSelectionManager, ISelectionManager SelectionManager, IHexDatabase HexDatabase, ICrossPlayerController CrossPlayerController)
        {
            this.UnitSelectionManager = UnitSelectionManager;
            this.SelectionManager = SelectionManager;
            this.HexDatabase = HexDatabase;
            this.CrossPlayerController = CrossPlayerController;

            SelectionManager.HexClicked += OnHexClicked;
            CrossPlayerController.MoveUnitCallback += OnMoveUnitCallback;
        }

        // Callbacks ---

        private void OnHexClicked(HexCell hex)
        {
            // Intercept SelectionManager unselecting unit when user is trying to move an unit

            if (!UnitSelectionManager.CanSelectedUnitPerformActions)
                return;

            var paths = UnitSelectionManager.Paths;
            if (hex.IsValid && paths.ContainsKey(hex.Position))
            {
                var path = paths.CalculatePathArray(hex.Position);
                var unit = UnitSelectionManager.SelectedUnit;

                // Unselect unit here so highlights are not shown while unit is moving
                SelectionManager.SelectSelectable(default);

                CrossPlayerController.MoveUnit(unit, path, hex.Position);

                // Do not allow to select hex cell which was clicked
                SelectionManager.DoNotAllowOtherSystemsToChangeSelection = true;
                SelectionManager.HexSelectionAborted += OnHexSelectionAborted;
            }
        }

        private void OnHexSelectionAborted(HexCell hex)
        {
            // Do not select hex which was clicked, instead we select our unit if it moved after UnitPositionUpdatedFromUI was called
            SelectionManager.HexSelectionAborted -= OnHexSelectionAborted;
            SelectionManager.DoNotAllowOtherSystemsToChangeSelection = false;
        }


        // Private ---

        private void OnMoveUnitCallback(Unit unit, int2[] path, int2 cell)
        {
            if (!IsMoveCommandCorrect(unit, cell))
            {
                Debug.LogWarning($"Move command received from network or AI was incorrect. Unit {unit.Cell} is physically unable to move to: {cell}");
                return;
            }

            var hexFrom = HexDatabase.GetHex(unit.Cell);
            var hexTo = HexDatabase.GetHex(cell);
            unit.Cell = cell;
            unit.Movement -= path.Length;

            hexFrom.Type = HexType.Empty;
            hexTo.Type = HexType.Unit;

            HexDatabase.UpdateHexCell(hexFrom);
            HexDatabase.UpdateHexCell(hexTo);

            UnitPositionChange?.Invoke(UnitPositionUpdatedFromUI, unit, path);

            // If no UI is running, update instantly
            if (UnitPositionChange == null)
                UnitPositionUpdatedFromUI(unit);
        }

        private bool IsMoveCommandCorrect(Unit unit, int2 cell)
        {
            var Paths = UnitSelectionManager.FindAllPathsWhereUnitCanMove(unit);
            return Paths.ContainsKey(cell);
        }

        private void UnitPositionUpdatedFromUI(Unit unit)
        {
            if (UnitSelectionManager.CanLocalPlayerControlThisUnit(unit))
                SelectionManager.SelectSelectable(unit);

            UnitPositionChangeEnd?.Invoke(unit);
        }
    }
}
