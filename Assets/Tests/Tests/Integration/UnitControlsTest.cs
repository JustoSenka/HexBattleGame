using Assets;
using NUnit.Framework;

namespace Tests.Integration
{
    public class UnitControlsTest
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

            HexDatabase.AddNewSelectable(m_Unit0);
            HexDatabase.AddNewSelectable(m_Unit1);
        }

        [Test]
        public void SelectingAndDeselectingUnits_WorksAsIntended()
        {
            SelectionManager.ClickAndSelectSelectable(m_Unit1);
            Assert.AreEqual(m_Unit1, UnitSelectionManager.SelectedUnit, "First time unit was not selected");

            SelectionManager.ClickAndSelectSelectable(m_Unit0);
            Assert.AreEqual(m_Unit0, UnitSelectionManager.SelectedUnit, "Second time unit was not selected");

            SelectionManager.ClickAndSelectCell(new int2(15, 15));
            Assert.AreEqual(default, UnitSelectionManager.SelectedUnit, "Unselecting unit did not work");

            SelectionManager.ClickAndSelectSelectable(m_Unit0);
            Assert.AreEqual(m_Unit0, UnitSelectionManager.SelectedUnit, "Third time unit was not selected");
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

        [TestCase(true)]
        [TestCase(false)]
        public void UnitAttacking_EndsTheirTurn(bool useSelectionManager)
        {
            MoveUnit(m_Unit0, new int2(1, 0), true);
            Attack(m_Unit0, m_Unit1.Cell, useSelectionManager);

            Assert.AreEqual(m_Unit1, TurnManager.CurrentTurnOwner, "unit1 should be turn owner now");

            CrossPlayerController.PerformSkill(m_Unit1, m_Unit0.Cell, Skill.Attack);
            Assert.AreEqual(m_Unit0, TurnManager.CurrentTurnOwner, "unit0 should be turn owner now");
        }

        [Test]
        public void AI_ActsOnItsOwn_AndEndsTurnAtTheEnd()
        {
            var AI = Container.Resolve<IEnemyAI>();
            AI.LocalTeam = 1;

            CrossPlayerController.PerformSkill(m_Unit0, m_Unit0.Cell, Skill.Guard);

            // Enemy AI acts here based on TurnManager.TurnEnd event

            Assert.AreNotEqual(new int2(2, 0), m_Unit1.Cell, "unit1 should have moved to different position");
            Assert.AreEqual(m_Unit0, TurnManager.CurrentTurnOwner, "unit0 should have turn again after enemy AI acted");
        }

        [Test]
        public void SelectingUnit_WillCalculateMovementPaths_AndAttackCoverage()
        {
            SelectionManager.ClickAndSelectSelectable(m_Unit0);

            var moveCoverage = Container.Resolve<IHexPathfinder>().FindAllPaths(m_Unit0.Cell, HexType.Empty, m_Unit0.Movement).CoveredCells(); 
            var attackCoverage = HexUtility.FindNeighbours(m_Unit0.Cell, m_Unit0.RangeMin, m_Unit0.RangeMax);

            CollectionAssert.AreEquivalent(moveCoverage, UnitSelectionManager.Paths.CoveredCells(), "Movement coverage is incorrect");
            CollectionAssert.AreEquivalent(attackCoverage, UnitAttackManager.AttackRadius, "Attack coverage is incorrect");
        }

        [Test]
        public void MovingUnitWillDecreaseItsMovement()
        {
            MoveUnit(m_Unit0, new int2(-3, 0), true);
            Assert.AreEqual(m_Unit0.MaxMovement - 3, m_Unit0.Movement, "First move did not remove movement value from unit");

            MoveUnit(m_Unit0, new int2(-2, 0), true);
            Assert.AreEqual(m_Unit0.MaxMovement - 4, m_Unit0.Movement, "Second move did not remove movement value from unit");

            MoveUnit(m_Unit0, new int2(0, 0), true); // Too far, cannot move there
            Assert.AreEqual(m_Unit0.MaxMovement - 4, m_Unit0.Movement, "Third move should not do anything since target is too far");
            Assert.AreEqual(new int2(-2, 0), m_Unit0.Cell, "Unit position was incorrect");
        }

        [Test]
        public void UnitIsSelectedAndPathsRecalculated_AfterMovingUnit_WithSelectionManager()
        {
            MoveUnit(m_Unit0, new int2(-3, 0), true);
            Assert.AreEqual(m_Unit0, UnitSelectionManager.SelectedUnit, "Selected unit was incorrect");

            var moveCoverage = Container.Resolve<IHexPathfinder>().FindAllPaths(m_Unit0.Cell, HexType.Empty, m_Unit0.Movement).CoveredCells();
            CollectionAssert.AreEquivalent(moveCoverage, UnitSelectionManager.Paths.CoveredCells(), "Movement coverage was incorrect");
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
