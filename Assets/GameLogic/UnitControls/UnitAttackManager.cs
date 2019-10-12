using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public class UnitAttackManager
    {
        private readonly IUnitSelectionManager UnitSelectionManager;
        private readonly ISelectionManager SelectionManager;
        private readonly ICrossPlayerController CrossPlayerController;
        public UnitAttackManager(IUnitSelectionManager UnitSelectionManager, ISelectionManager SelectionManager, ICrossPlayerController CrossPlayerController)
        {
            this.UnitSelectionManager = UnitSelectionManager;
            this.SelectionManager = SelectionManager;
            this.CrossPlayerController = CrossPlayerController;

            SelectionManager.HexClicked += OnHexClicked;

            UnitSelectionManager.UnitSelected += OnUnitSelected;
            UnitSelectionManager.UnitUnselected += UnitUnselected;
        }

        private void OnHexClicked(HexCell hex)
        {

        }

        private void OnUnitSelected(Unit unit)
        {

        }

        private void UnitUnselected(Unit unit)
        {

        }
    }
}
