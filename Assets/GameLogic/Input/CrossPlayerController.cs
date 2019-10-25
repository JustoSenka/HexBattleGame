using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    /// <summary>
    /// Class responsible for all gameplay changes based on local user, remote user or AI actions.
    /// This class is supposed to be used to control all player actions.
    /// It is used from both AI and player inputs
    /// </summary>
    [RegisterDependency(typeof(ICrossPlayerController), true)]
    public class CrossPlayerController : ICrossPlayerController
    {
        public int LocalTeam { get; set; } = -1;

        private readonly IHexDatabase HexDatabase;
        private readonly ITurnManager TurnManager;
        public CrossPlayerController(IHexDatabase HexDatabase, ITurnManager TurnManager)
        {
            this.HexDatabase = HexDatabase;
            this.TurnManager = TurnManager;
        }

        public event Action<Unit, int2[], int2> MoveUnitCallback;
        public void MoveUnit(Unit unit, int2[] Path, int2 hexTo)
        {
            if (IsNotOwner(unit))
                return;

            MoveUnitCallback?.Invoke(unit, Path, hexTo);
            // Send same message over network to update other players
        }

        // Temporary skill. Used to end turn right now.
        public event Action<Unit, int2, Skill> PerformSkillCallback;
        public void PerformSkill(Unit unit, int2 target, Skill skill)
        {
            if (IsNotOwner(unit))
                return;

            PerformSkillCallback?.Invoke(unit, target, skill);
        }

        private bool IsNotOwner(Unit unit)
        {
            if (TurnManager.CurrentTurnOwner != unit)
            {
                Debug.LogWarning($"{unit} is not an owner of the turn anymore. Cannot perform operation.");
                return true;
            }

            return false;
        }
    }

    public enum Skill
    {
        None = 0,
        Attack = 1,
        Guard = 2,
    }
}
