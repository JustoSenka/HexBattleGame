using System;

namespace Assets
{
    public interface ISkillManager
    {
        event Action<Action<Unit>, Unit, int2, SkillType> SkillPerformed;
        event Action<Unit> SkillPerformedEnd;
    }
}