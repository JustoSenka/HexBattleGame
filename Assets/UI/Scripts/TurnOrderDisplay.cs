using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets
{
    public class TurnOrderDisplay : MonoBehaviour
    {
        [Dependency(typeof(ITurnManager))]
        private ITurnManager TurnManager;

        public RectTransform ParentPanel;
        public RectTransform TurnOrderIndicatorPrefab;

        public int MaxTurnOrderIndicatorCount = 8;

        private Dictionary<Selectable, TurnOrderIndicator> m_Dictionary;

        public void Start()
        {
            m_Dictionary = new Dictionary<Selectable, TurnOrderIndicator>();
            UpdateTurnQueue();

            TurnManager.TurnQueueChanged += UpdateTurnQueue;
        }

        private void UpdateTurnQueue()
        {
            var queue = TurnManager.GetCurrentTurnQueue();
            var index = 0;
            var queueLength = queue.Count();

            foreach (var selectable in queue)
            {
                if (!m_Dictionary.ContainsKey(selectable))
                {
                    var go = Instantiate(TurnOrderIndicatorPrefab, Vector3.zero, Quaternion.identity, ParentPanel);

                    m_Dictionary[selectable] = go.GetComponent<TurnOrderIndicator>();
                }

                var turnIndicator = m_Dictionary[selectable];
                UpdateSingleTurnIndicatorObject(selectable, turnIndicator, index, queueLength);

                index++;
            }
        }

        private void UpdateSingleTurnIndicatorObject(Selectable selectable, TurnOrderIndicator turnIndicator, int index, int queueLength)
        {
            turnIndicator.turnOrder = index;
            turnIndicator.text.text = selectable.Cell.ToString();

            var spaceForOneElement = turnIndicator.rect.rect.width + 35;

            var indicatorCount = Math.Min(queueLength, MaxTurnOrderIndicatorCount);
            var xOffset = (1 - indicatorCount) * spaceForOneElement / 2 + index * spaceForOneElement;
            turnIndicator.rect.anchoredPosition = new Vector3(xOffset, 0);
        }
    }
}
