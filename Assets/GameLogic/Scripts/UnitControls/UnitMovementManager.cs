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

        private readonly IMouseManager MouseManager;
        private readonly IHexPathfinder HexPathfinder;

        private IEnumerable<PoolItem> m_MovementItems;

        public UnitMovementManager(IMouseManager MouseManager, IHexPathfinder HexPathfinder)
        {
            this.MouseManager = MouseManager;
            this.HexPathfinder = HexPathfinder;

            MouseManager.SelectableSelected += OnSelectableClicked;
            MouseManager.SelectableUnselected += OnSelectableUnselected;
        }

        private void OnSelectableClicked(Selectable obj)
        {
            if (obj is Unit unit)
            {
                Paths = HexPathfinder.FindAllPaths(unit.HexCell.Position, HexType.Empty, unit.Movement);
                UnitSelected?.Invoke(unit);
            }
        }

        private void OnSelectableUnselected(Selectable obj)
        {
            if (obj is Unit unit)
            {
                UnitSelected?.Invoke(unit);
            }
        }

        public void Update()
        {

        }
    }
}
