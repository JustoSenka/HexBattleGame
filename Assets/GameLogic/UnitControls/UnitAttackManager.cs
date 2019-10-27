using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    [RegisterDependency(typeof(IUnitAttackManager), true)]
    public class UnitAttackManager : IUnitAttackManager
    {
        public IEnumerable<int2> AttackRadius { get; private set; }

        private readonly IUnitSelectionManager UnitSelectionManager;
        private readonly ISelectionManager SelectionManager;
        private readonly ICrossPlayerController CrossPlayerController;
        private readonly IHexDatabase HexDatabase;
        public UnitAttackManager(IUnitSelectionManager UnitSelectionManager, ISelectionManager SelectionManager, ICrossPlayerController CrossPlayerController, IHexDatabase HexDatabase)
        {
            this.UnitSelectionManager = UnitSelectionManager;
            this.SelectionManager = SelectionManager;
            this.CrossPlayerController = CrossPlayerController;
            this.HexDatabase = HexDatabase;

            SelectionManager.HexClicked += OnHexClicked;
            SelectionManager.SelectableClicked += OnSelectableClicked;

            UnitSelectionManager.UnitSelected += OnUnitSelected;
            UnitSelectionManager.UnitUnselected += UnitUnselected;
        }

        private void OnSelectableClicked(Selectable obj)
        {
            if (!UnitSelectionManager.CanSelectedUnitPerformActions)
                return;

            if (obj is Unit unit && AttackRadius.Contains(obj.Cell))
            {
                CrossPlayerController.PerformSkill(UnitSelectionManager.SelectedUnit, unit.Cell, SkillType.Attack);

                SelectionManager.DoNotAllowOtherSystemsToChangeSelection = true;
                SelectionManager.HexSelectionAborted += OnHexSelectionAborted;
                SelectionManager.SelectableSelectionAborted += OnSelectableSelectionAborted;
            }
        }

        private void OnSelectableSelectionAborted(Selectable s) => OnHexSelectionAborted(default);
        private void OnHexSelectionAborted(HexCell hex)
        {
            // Do not select another unit or hex while we perform the attack
            SelectionManager.HexSelectionAborted -= OnHexSelectionAborted;
            SelectionManager.SelectableSelectionAborted -= OnSelectableSelectionAborted;
            SelectionManager.DoNotAllowOtherSystemsToChangeSelection = false;

            SelectionManager.SelectSelectable(default);
        }

        private void OnHexClicked(HexCell hex)
        {
            var unitInHex = HexDatabase.GetSelectable(hex.Position);
            OnSelectableClicked(unitInHex);
        }

        private void OnUnitSelected(Unit unit)
        {
            AttackRadius = HexUtility.FindNeighbours(unit.Cell, unit.RangeMin, unit.RangeMax);
        }

        private void UnitUnselected(Unit unit)
        {

        }
    }
}
