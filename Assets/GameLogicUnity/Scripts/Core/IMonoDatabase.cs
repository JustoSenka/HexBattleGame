using System;
using System.Collections.Generic;

namespace Assets
{
    public interface IMonoDatabase
    {
        event Action<Selectable, SelectableBehaviour> SelectableAdded;
        event Action<Selectable> SelectableRemoved;

        void Start();

        void AddBehaviour(Selectable selectable, SelectableBehaviour behaviour);
        T GetBehaviourFor<T>(Selectable selectable) where T : class;

        IEnumerable<T> GetAllBehavioursOfType<T>() where T : class;
    }
}
