using System;
using System.Collections.Generic;

namespace Assets
{
    public interface ITurnManager
    {
        Selectable CurrentTurnOwner { get; }

        event Action<Selectable> TurnEnded;
        event Action<Selectable> TurnStarted;
        event Action TurnQueueChanged;

        void Start();

        void EndTurn(Selectable sel);
        IEnumerable<Selectable> GetCurrentTurnQueue();
    }
}
