using Assets;
using NUnit.Framework;

namespace Tests.Integration
{
    public class UnitFightTests
    {
        private DependencyContainer Container;
        private IHexDatabase HexDatabase;
        private ITurnManager TurnManager;
        private ISelectionManager SelectionManager;
        private IUnitAttackManager UnitAttackManager;
        private IUnitSelectionManager UnitSelectionManager;
        private ICrossPlayerController CrossPlayerController;

        private Unit m_Unit0, m_Unit1;

        [SetUp]
        public void CreateSomeUnit()
        {
            Container = TestUtility.CreateContainer();
            HexDatabase = Container.Resolve<IHexDatabase>();
            TurnManager = Container.Resolve<ITurnManager>();
            SelectionManager = Container.Resolve<ISelectionManager>();
            UnitAttackManager = Container.Resolve<IUnitAttackManager>();
            UnitSelectionManager = Container.Resolve<IUnitSelectionManager>();
            CrossPlayerController = Container.Resolve<ICrossPlayerController>();

            m_Unit0 = TestUtility.CreateUnit(new int2(0, 0), 0);
            m_Unit1 = TestUtility.CreateUnit(new int2(2, 0), 1);

            m_Unit0.Attack = 25;
            m_Unit1.RangeMax = 2;

            HexDatabase.AddNewSelectable(m_Unit0);
            HexDatabase.AddNewSelectable(m_Unit1);
        }

        [Test]
        public void UnitsGetRemoved_FromHexDatabase_AfterTheirHelthGoesBelowZero()
        {
            MoveUnit(m_Unit0, new int2(1, 0), true);
            Attack(m_Unit0, m_Unit1.Cell, true);

            Assert.IsTrue(m_Unit1.Health <= 0, "Unit 1 health should be <= 0");
            Assert.AreEqual(default, HexDatabase.GetSelectable(m_Unit1.Cell), "Hex database should not have dead unit in it");
            Assert.AreEqual(HexType.Empty, HexDatabase.GetHex(m_Unit1.Cell).Type, "Hex database cell with dead unit should be marked as empty");
        }

        private void MoveUnit(Unit unit, int2 pos, bool useSelectionManager)
        {
            if (useSelectionManager)
            {
                SelectionManager.ClickAndSelectSelectable(unit);
                SelectionManager.ClickAndSelectCell(pos);
            }
            else
            {
                CrossPlayerController.MoveUnit(unit, new[] { pos }, pos);
            }
        }

        private void Attack(Unit unit, int2 pos, bool useSelectionManager)
        {
            if (useSelectionManager)
            {
                SelectionManager.ClickAndSelectSelectable(unit);
                SelectionManager.ClickAndSelectCell(pos);
            }
            else
            {
                CrossPlayerController.PerformSkill(unit, pos, Skill.Attack);
            }
        }
    }
}
