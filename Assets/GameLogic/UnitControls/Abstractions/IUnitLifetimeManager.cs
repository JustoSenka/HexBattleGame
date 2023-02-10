using System;

namespace Assets
{
    public interface IUnitLifetimeManager
    {
        event Action<Action<Unit>, Unit> UnitDestroyed;
        event Action<Unit> UnitDestroyedEnd;
    }
}