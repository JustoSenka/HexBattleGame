using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Assets
{
    [DebuggerDisplay("PoolItem: {IsReserved} - {GameObject.name}")]
    public class PoolItem
    {
        public bool IsReserved { get; private set; }
        public GameObject GameObject { get; private set; }

        public PoolItem(bool reserved, GameObject item)
        {
            IsReserved = reserved;
            GameObject = item;
        }

        public void Reserve()
        {
            IsReserved = true;
            GameObject.SetActive(true);
        }

        public void Release()
        {
            IsReserved = false;
            GameObject.SetActive(false);
        }

        public void Destroy()
        {
            GameObject.Destroy(GameObject);
        }
    }

    public static class PoolItemExtension
    {
        public static void Release<T>(this IEnumerable<T> items) where T : PoolItem
        {
            foreach (var item in items)
                item.Release();
        }

        public static void Reserve<T>(this IEnumerable<T> items) where T : PoolItem
        {
            foreach (var item in items)
                item.Reserve();
        }

        public static void Destroy<T>(this IEnumerable<T> items) where T : PoolItem
        {
            foreach (var item in items)
                item.Destroy();
        }
    }
}
