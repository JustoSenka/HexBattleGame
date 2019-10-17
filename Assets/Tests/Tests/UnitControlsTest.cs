using NUnit.Framework;

namespace Assets
{
    public class UnitControlsTest
    {
        private DependencyContainer Container;
        private IHexDatabase HexDatabase;
        private ITurnManager TurnManager;
        private ISelectionManager SelectionManager;
        private ICrossPlayerController CrossPlayerController;

        private Unit m_Unit0, m_Unit1;

        [SetUp]
        public void CreateSomeUnit()
        {
            Container = TestUtility.CreateContainer();
            HexDatabase = Container.Resolve<IHexDatabase>();
            TurnManager = Container.Resolve<ITurnManager>();
            SelectionManager = Container.Resolve<ISelectionManager>();
            CrossPlayerController = Container.Resolve<ICrossPlayerController>();

            m_Unit0 = TestUtility.CreateUnit(new int2(0, 0), 0);
            m_Unit1 = TestUtility.CreateUnit(new int2(2, 0), 1);

            HexDatabase.AddNewSelectable(m_Unit0);
            HexDatabase.AddNewSelectable(m_Unit1);
        }
         
        [TestCase(true)]
        [TestCase(false)]
        public void UnitsCanMove(bool useSelectionManager)
        {
            MoveUnit(m_Unit0, new int2(3, 0), useSelectionManager);
            Assert.AreEqual(new int2(3, 0), m_Unit0.Cell, "Unit did not move to correct position");
        }

        [TestCase(true)]
        [TestCase(false)] // still no protection on data received from other side
        public void UnitsWillNotMove_IfTheyHaveNotEnoughMovement(bool useSelectionManager)
        {
            MoveUnit(m_Unit0, new int2(15, 0), useSelectionManager);
            Assert.AreEqual(new int2(0, 0), m_Unit0.Cell, "Unit should stay in same position");
        }
         
        [TestCase(true)]
        [TestCase(false)] // still no protection on data received from other side
        public void UnitsWillNotMove_ToAnOccupiedPosition(bool useSelectionManager)
        {
            MoveUnit(m_Unit0, new int2(2, 0), useSelectionManager);
            Assert.AreEqual(new int2(0, 0), m_Unit0.Cell, "Unit should stay in same position");
        }

        [Test]
        public void PerformingGuardSkill_EndsUnitTurn()
        {
            Assert.AreEqual(m_Unit0, TurnManager.CurrentTurnOwner, "Incorrect starting unit");

            CrossPlayerController.PerformSkill(m_Unit0, m_Unit0.Cell, Skill.Guard);
            Assert.AreEqual(m_Unit1, TurnManager.CurrentTurnOwner, "Incorrect unit owner after skill was used");
        }

        [TestCase(true)]
        [TestCase(false)]
        public void UnitsWillNotMove_IfItsNotTheirTurn(bool useSelectionManager)
        {
            CrossPlayerController.PerformSkill(m_Unit0, m_Unit0.Cell, Skill.Guard); // Ends turn

            MoveUnit(m_Unit0, new int2(3, 0), useSelectionManager);
            Assert.AreEqual(new int2(0, 0), m_Unit0.Cell, "Unit should stay in same position");
        }

        [Test] // Only applicable to selection manager. 
        public void CannotControlUnit_WhichBelongsToDifferentLocalTeam_EvenIfItsTheirTurn()
        {
            CrossPlayerController.PerformSkill(m_Unit0, m_Unit0.Cell, Skill.Guard); // Ends turn
            MoveUnit(m_Unit1, new int2(4, 0), true);

            Assert.AreEqual(new int2(2, 0), m_Unit1.Cell, "Unit should stay in same position");
        }

        [TestCase(true)]
        [TestCase(false)]
        public void UnitCanMoveAndAttack_TargetLosesHealth(bool useSelectionManager)
        {
            MoveUnit(m_Unit0, new int2(1, 0), true);
            Attack(m_Unit0, m_Unit1.Cell, useSelectionManager);

            Assert.AreEqual(new int2(1, 0), m_Unit0.Cell, "Unit did not move to correct position");
            Assert.AreEqual(16, m_Unit1.Health, "Enemy unit did not lose health");
        }

        [TestCase(true)]
        [TestCase(false)]
        public void UnitCannotAttack_IfNotHisTurn(bool useSelectionManager)
        {
            MoveUnit(m_Unit0, new int2(1, 0), true);
            CrossPlayerController.PerformSkill(m_Unit0, m_Unit0.Cell, Skill.Guard);

            Attack(m_Unit0, m_Unit1.Cell, useSelectionManager);

            Assert.AreEqual(new int2(1, 0), m_Unit0.Cell, "Unit did not move to correct position");
            Assert.AreEqual(20, m_Unit1.Health, "Enemy unit did not lose health");
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
