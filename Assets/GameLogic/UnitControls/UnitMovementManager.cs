﻿using System;
using System.Collections.Generic;

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
                SelectionManager.DoNotAllowOtherSystemsToChangeSelection = true;
                SelectionManager.HexSelectionAborted += OnHexSelectionAborted;

                var path = paths.CalculatePathArray(hex.Position);
                CrossPlayerController.MoveUnit(UnitSelectionManager.SelectedUnit, path, hex.Position);
            }
        }

        private void OnHexSelectionAborted(HexCell hex)
        {
            SelectionManager.HexSelectionAborted -= OnHexSelectionAborted;
            SelectionManager.DoNotAllowOtherSystemsToChangeSelection = false;

            SelectionManager.SelectSelectable(default);
        }


        // Private ---

        private void OnMoveUnitCallback(Unit unit, int2[] path, int2 cell)
        {
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

        private void UnitPositionUpdatedFromUI(Unit unit)
        {
            if (unit.Movement > 0 && UnitSelectionManager.CanIControlThisUnit(unit))
                SelectionManager.SelectSelectable(unit);
            else if (unit.Movement <= 0)
                CrossPlayerController.PerformSkill(unit, Skill.Guard); // This one here is temporarily to end the turn. in future turns will end after attack or skills

            UnitPositionChangeEnd?.Invoke(unit);
        }
    }
}
