using System.Collections.Generic;

namespace Assets
{
    public interface IUnitAttackManager
    {
        IEnumerable<int2> AttackRadius { get; }
    }
}
