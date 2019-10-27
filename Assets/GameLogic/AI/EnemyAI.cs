using System;
using System.Linq;

namespace Assets
{
    [RegisterDependency(typeof(IEnemyAI), false)]
    public class EnemyAI : IEnemyAI
    {
        public int LocalTeam { get; set; } = -1;

        private readonly ICrossPlayerController CrossPlayerController;
        private readonly ITurnManager TurnManager;
        private readonly IHexPathfinder HexPathfinder;
        private readonly IUnitMovementManager UnitMovementManager;
        public EnemyAI(ICrossPlayerController CrossPlayerController, ITurnManager TurnManager, IHexPathfinder HexPathfinder, IUnitMovementManager UnitMovementManager)
        {
            this.CrossPlayerController = CrossPlayerController;
            this.TurnManager = TurnManager;
            this.HexPathfinder = HexPathfinder;
            this.UnitMovementManager = UnitMovementManager;

            TurnManager.TurnStarted += OnTurnStarted;

            UnitMovementManager.UnitPositionChangeEnd += OnUnitPositionChangeEnd;
        }

        private void OnTurnStarted(Selectable obj)
        {
            if (obj.Team != LocalTeam)
                return;

            if (obj is Unit unit)
            {
                var Paths = HexPathfinder.FindAllPaths(unit.Cell, HexType.Empty, unit.Movement);

                var possibleDestinations = Paths.Keys;
                var index = new Random().Next(0, possibleDestinations.Count - 1);
                var cellToMoveTo = possibleDestinations.ElementAt(index);

                var path = Paths.CalculatePathArray(cellToMoveTo);
                CrossPlayerController.MoveUnit(unit, path, cellToMoveTo);
            }
        }

        private void OnUnitPositionChangeEnd(Unit unit)
        {
            // It is possible that movement ended the turn already
            if (unit.Team != LocalTeam || TurnManager.CurrentTurnOwner != unit)
                return;

            CrossPlayerController.PerformSkill(unit, unit.Cell, SkillType.Guard);
        }
    }
}
