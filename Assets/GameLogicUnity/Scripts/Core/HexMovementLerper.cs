using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets
{
    [RegisterDependency(typeof(HexMovementLerper), true)]
    public class HexMovementLerper
    {
        [Dependency(typeof(CoroutineManager))]
        private CoroutineManager Coroutine;

        private const int k_FramesToLerp = 10;

        private readonly IUnitMovementManager UnitMovementManager;
        private readonly IMonoDatabase MonoDatabase;
        public HexMovementLerper(IUnitMovementManager UnitMovementManager, IMonoDatabase MonoDatabase)
        {
            this.UnitMovementManager = UnitMovementManager;
            this.MonoDatabase = MonoDatabase;

            UnitMovementManager.UnitPositionChange += OnUnitPositionChange;
        }

        private void OnUnitPositionChange(Action callback, Movable unit, IEnumerable<int2> pathInReverse)
        {
            var obj = MonoDatabase.GetBehaviourFor<MovableBehaviour>(unit);
            var path = pathInReverse.Reverse();

            Coroutine.StartCoroutine(MovePosition(callback, obj, path));
        }

        IEnumerator MovePosition(Action callback, MovableBehaviour obj, IEnumerable<int2> pathInReverse)
        {
            var transform = obj.gameObject.transform;
            var height = new Vector3(0, transform.position.y, 0);

            foreach (var nextCell in pathInReverse)
            {
                var startPos = Vector3.Scale(transform.position, new Vector3(1, 0, 1));
                var targetPos = HexUtility.HexToWorldPoint(nextCell, 1);
                var direction = Vector3.Scale((targetPos - startPos), new Vector3(1, 0, 1));

                for (int i = 0; i < k_FramesToLerp; i++)
                {
                    var pos = Vector3.Slerp(startPos, targetPos, i * 1f / k_FramesToLerp) + height;
                    transform.SetPositionAndRotation(pos, Quaternion.LookRotation(direction, Vector3.up));
                    yield return null;
                }

                obj.Cell = nextCell;
                transform.position = targetPos + height;
            }

            callback.Invoke();
        }
    }
}
