using System;
using System.Collections;
using UnityEngine;

namespace Assets
{
    [RegisterDependency(typeof(UnitDestroyer), true)]
    public class UnitDestroyer
    {
#pragma warning disable CS0649
        [Dependency(typeof(CoroutineManager))]
        private CoroutineManager Coroutine;
#pragma warning restore CS0649

        private const int k_FramesToLerp = 10;

        private readonly IUnitLifetimeManager UnitLifetimeManager;
        private readonly IMonoDatabase MonoDatabase;
        public UnitDestroyer(IUnitLifetimeManager UnitLifetimeManager, IMonoDatabase MonoDatabase)
        {
            this.UnitLifetimeManager = UnitLifetimeManager;
            this.MonoDatabase = MonoDatabase;

            UnitLifetimeManager.UnitDestroyed += OnUnitDestroyed;
        }

        private void OnUnitDestroyed(Action<Unit> callback, Unit unit)
        {
            var obj = MonoDatabase.GetBehaviourFor<SelectableBehaviour>(unit);
            Coroutine.StartCoroutine(DestroyUnitAnimation(callback, unit, obj));
        }

        private IEnumerator DestroyUnitAnimation(Action<Unit> callback, Unit unit, SelectableBehaviour obj)
        {
            var transform = obj.gameObject.transform;

            for (int i = 0; i < k_FramesToLerp * 60 / Time.deltaTime; i++)
            {
                var scale = Mathf.Lerp(0f, 1f, i);
                transform.localScale = new Vector3(1, scale, 1);
                yield return null;
            }

            GameObject.Destroy(obj);
            callback.Invoke(unit);
        }
    }
}
