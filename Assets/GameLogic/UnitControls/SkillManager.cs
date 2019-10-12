using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    [RegisterDependency(typeof(SkillManager), true)]
    public class SkillManager
    {
        private readonly ICrossPlayerController CrossPlayerController;
        private readonly ITurnManager TurnManager;
        public SkillManager(ICrossPlayerController CrossPlayerController, ITurnManager TurnManager)
        {
            this.CrossPlayerController = CrossPlayerController;
            this.TurnManager = TurnManager;

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

        private void OnPerformSkillCallback(Unit unit, Skill skill)
        {
            TurnManager.EndTurn(unit);
        }
    }
}
