using System;
using UnityEngine;

namespace Assets
{
    [RegisterDependency(typeof(ISkillManager), true)]
    public class SkillManager : ISkillManager
    {
        public event Action<Action<Unit, int2, SkillType>, Unit, int2, SkillType> SkillPerformed;
        public event Action<Unit, int2, SkillType> SkillPerformedEnd;

        private readonly ICrossPlayerController CrossPlayerController;
        private readonly ITurnManager TurnManager;
        private readonly IHexDatabase HexDatabase;
        public SkillManager(ICrossPlayerController CrossPlayerController, ITurnManager TurnManager, IHexDatabase HexDatabase)
        {
            this.CrossPlayerController = CrossPlayerController;
            this.TurnManager = TurnManager;
            this.HexDatabase = HexDatabase;

            CrossPlayerController.PerformSkillCallback += OnPerformSkillCallback;
            TurnManager.TurnEnded += OnTurnEnded;
        }

        private void OnTurnEnded(Selectable obj)
        {
            if (obj is Movable mov)
            {
                mov.Movement = mov.MaxMovement;
            }
        }

        private void OnPerformSkillCallback(Unit unit, int2 target, SkillType skill)
        {
            if (skill == SkillType.Attack)
            {
                var targetUnit = HexDatabase.GetSelectable(target) as Unit;
                if (targetUnit == null)
                {
                    Debug.LogWarning("targetUnit was null thus cannot be attacked. It should not be possible to initiate attacks on empty cells: " + target);
                }
                else
                {
                    targetUnit.Health -= Math.Abs(unit.Attack - targetUnit.Defense);

                    SkillPerformed?.Invoke(SkillPerformedFinishedFromUI, unit, target, skill);
                    if (SkillPerformed == null)
                        SkillPerformedFinishedFromUI(unit, target, skill);
                }
            }

            TurnManager.EndTurn(unit);
        }

        private void SkillPerformedFinishedFromUI(Unit unit, int2 target, SkillType skill)
        {
            SkillPerformedEnd?.Invoke(unit, target, skill);
        }
    }
}
