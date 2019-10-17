using System;

namespace Assets
{
    public interface ICrossPlayerController
    {
        int LocalTeam { get; set; }

        event Action<Unit, int2[], int2> MoveUnitCallback;
        void MoveUnit(Unit unit, int2[] reversePath, int2 hexTo);

        event Action<Unit, int2, Skill> PerformSkillCallback;
        void PerformSkill(Unit unit, int2 target, Skill skill);
    }
}