﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    [RegisterDependency(typeof(ITurnManager), true)]
    public class TurnManager : ITurnManager
    {
        public Selectable CurrentTurnOwner => m_ObjectQueue.First.Value;

        public event Action<Selectable> TurnStarted;
        public event Action<Selectable> TurnEnded;
        
        public event Action TurnQueueChanged;
        public event Action<Selectable> TurnQueueElementRemoved;

        private LinkedList<Selectable> m_ObjectQueue;

        private readonly IHexDatabase HexDatabase;
        public TurnManager(IHexDatabase HexDatabase)
        {
            this.HexDatabase = HexDatabase;

            m_ObjectQueue = new LinkedList<Selectable>();

            HexDatabase.SelectableAdded += OnSelectableAdded;
            HexDatabase.SelectableRemoved += OnSelectableRemoved;
        }

        public void Start()
        {
            m_ObjectQueue.Clear();

            foreach (var sel in HexDatabase.Selectables)
                m_ObjectQueue.AddLast(sel);

            TurnStarted?.Invoke(CurrentTurnOwner);
            TurnQueueChanged?.Invoke();
        }

        public void EndTurn(Selectable sel)
        {
            if (CurrentTurnOwner != sel)
            {
                Debug.LogWarning($"{sel} Selectable is not turn owner, thus cannot end turn for another object");
                return;
            }

            TurnEnded?.Invoke(CurrentTurnOwner);

            var first = CurrentTurnOwner;
            m_ObjectQueue.RemoveFirst();
            m_ObjectQueue.AddLast(first);

            TurnStarted?.Invoke(CurrentTurnOwner);
            TurnQueueChanged?.Invoke();
        }

        public IEnumerable<Selectable> GetCurrentTurnQueue() => m_ObjectQueue;

        // Private ---

        private void OnSelectableAdded(Selectable obj)
        {
            m_ObjectQueue.AddLast(obj);
            TurnQueueChanged?.Invoke();
        }

        private void OnSelectableRemoved(Selectable obj)
        {
            m_ObjectQueue.Remove(obj);
            TurnQueueElementRemoved?.Invoke(obj);
            TurnQueueChanged?.Invoke();
        }
    }
}
