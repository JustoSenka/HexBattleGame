using System;

namespace Assets
{
    public interface ISkillManager
    {
        event Action<Action<Unit>, Unit, int2, Skill> SkillPerformed;
        event Action<Unit> SkillPerformedEnd;
    }
}