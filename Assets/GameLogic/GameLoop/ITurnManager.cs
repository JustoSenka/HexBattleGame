using System;

namespace Assets
{
    public interface ITurnManager
    {
        Selectable CurrentTurnOwner { get; }

        event Action<Selectable> TurnEnded;
        event Action<Selectable> TurnStarted;

        void Start();

        void EndTurn();
    }
}
