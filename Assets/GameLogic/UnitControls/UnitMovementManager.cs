using System;
using System.Collections.Generic;

namespace Assets
{
    [RegisterDependency(typeof(IUnitMovementManager), true)]
    public class UnitMovementManager : IUnitMovementManager
    {
        public PathfinderDictionary Paths { get; private set; }

        public event Action<Unit> UnitSelected;
        public event Action<Unit> UnitUnselected;

        public event Action<Unit, IEnumerable<int2>> UnitPositionChange;

        private Unit m_SelectedUnit;

        private readonly ISelectionManager SelectionManager;
        private readonly IHexPathfinder HexPathfinder;
        private readonly IHexDatabase HexDatabase;
        public UnitMovementManager(ISelectionManager SelectionManager, IHexPathfinder HexPathfinder, IHexDatabase HexDatabase)
        {
            this.SelectionManager = SelectionManager;
            this.HexPathfinder = HexPathfinder;
            this.HexDatabase = HexDatabase;

            SelectionManager.SelectableSelected += OnSelectableClicked;
            SelectionManager.SelectableUnselected += OnSelectableUnselected;

            SelectionManager.HexSelected += OnHexSelected;
        }

        public void MoveSelectedUnitTo(HexCell hexTo)
        {
            var path = Paths.CalculatePath(hexTo.Position);
            UnitPositionChange?.Invoke(m_SelectedUnit, path);

            var hexFrom = HexDatabase.GetHex(m_SelectedUnit.Cell);
            m_SelectedUnit.Cell = hexTo.Position;

            hexFrom.Type = HexType.Empty;
            hexTo.Type = HexType.Unit;

            HexDatabase.UpdateHexCell(hexFrom);
            HexDatabase.UpdateHexCell(hexTo);

            m_SelectedUnit = null;
        }

        // Private ---

        private void OnHexSelected(HexCell hex)
        {
            if (m_SelectedUnit != default && hex != default && Paths.ContainsKey(hex.Position))
            {
                MoveSelectedUnitTo(hex);
            }
        }

        private void OnSelectableClicked(Selectable obj)
        {
            if (obj is Unit unit)
            {
                m_SelectedUnit = unit;
                Paths = HexPathfinder.FindAllPaths(unit.Cell, HexType.Empty, unit.Movement);
                UnitSelected?.Invoke(unit);
            }
        }

        private void OnSelectableUnselected(Selectable obj)
        {
            if (obj is Unit unit)
            {
                m_SelectedUnit = default;
                UnitUnselected?.Invoke(unit);
            }
        }

        public void Update()
        {

        }
    }
}
