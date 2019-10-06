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

        private readonly IMouseManager MouseManager;
        private readonly IHexPathfinder HexPathfinder;

        private Unit m_SelectedUnit;

        public UnitMovementManager(IMouseManager MouseManager, IHexPathfinder HexPathfinder)
        {
            this.MouseManager = MouseManager;
            this.HexPathfinder = HexPathfinder;

            MouseManager.SelectableSelected += OnSelectableClicked;
            MouseManager.SelectableUnselected += OnSelectableUnselected;

            MouseManager.HexSelected += OnHexSelected;
        }

        private void OnHexSelected(HexCell hex)
        {
            if (m_SelectedUnit != default && hex != default && Paths.ContainsKey(hex.Position))
            {
                var path = Paths.CalculatePath(hex.Position);
                UnitPositionChange?.Invoke(m_SelectedUnit, path);
                m_SelectedUnit.Cell = hex.Position;

                m_SelectedUnit = null;
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
                m_SelectedUnit = unit;
                UnitUnselected?.Invoke(unit);
            }
        }

        public void Update()
        {

        }
    }
}
