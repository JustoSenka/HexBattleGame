using System;
using UnityEngine;

namespace Assets
{
    [RegisterDependency(typeof(IUnitLifetimeManager), true)]
    public class UnitLifetimeManager : IUnitLifetimeManager
    {
        private readonly ISkillManager SkillManager;
        private readonly ICrossPlayerController CrossPlayerController;
        private readonly ITurnManager TurnManager;
        private readonly IHexDatabase HexDatabase;

        public event Action<Action<Unit>, Unit> UnitDestroyed;
        public event Action<Unit> UnitDestroyedEnd;

        public UnitLifetimeManager(ISkillManager SkillManager, ICrossPlayerController CrossPlayerController, ITurnManager TurnManager, IHexDatabase HexDatabase)
        {
            this.SkillManager = SkillManager;
            this.CrossPlayerController = CrossPlayerController;
            this.TurnManager = TurnManager;
            this.HexDatabase = HexDatabase;

            SkillManager.SkillPerformedEnd += OnSkillPerformedEnd;
        }

        private void OnSkillPerformedEnd(Unit _, int2 target, SkillType skill)
        {
            var unit = HexDatabase.GetSelectable(target) as Unit;
            if (unit == default)
            {
                Debug.LogError("Skill performed on something that is not a unit");
                return;
            }

            if (unit.Health <= 0)
            {
                UnitDestroyed.Invoke(OnUnitDestroyedFinishedUICallback, unit);
                if (UnitDestroyedEnd == null)
                    OnUnitDestroyedFinishedUICallback(unit);
            }
        }

        private void OnUnitDestroyedFinishedUICallback(Unit unit)
        {
            HexDatabase.RemoveSelectable(unit);
        }
    }
}
