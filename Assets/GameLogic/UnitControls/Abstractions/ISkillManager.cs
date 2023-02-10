using System;

namespace Assets
{
    public interface ISkillManager
    {
        event Action<Action<Unit, int2, SkillType>, Unit, int2, SkillType> SkillPerformed;
        event Action<Unit, int2, SkillType> SkillPerformedEnd;
    }
}