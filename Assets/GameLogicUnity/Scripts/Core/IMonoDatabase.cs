using System.Collections.Generic;

namespace Assets
{
    public interface IMonoDatabase
    {
        void Start();

        void AddBehaviour(Selectable selectable, SelectableBehaviour behaviour);
        T GetBehaviourFor<T>(Selectable selectable) where T : class;

        IEnumerable<T> GetAllBehavioursOfType<T>() where T : class;
    }
}
