namespace Assets
{
    [RegisterDependency(typeof(TurnOwnerHighlight), true)]
    public class TurnOwnerHighlight
    {
        private PoolItem m_HighlightItem;

        private readonly ITurnManager TurnManager;
        private readonly IHexHighlighter HexHighlighter;
        public TurnOwnerHighlight(ITurnManager TurnManager, IHexHighlighter HexHighlighter)
        {
            this.HexHighlighter = HexHighlighter;
            this.TurnManager = TurnManager;

            TurnManager.TurnStarted += OnTurnStarted;
            TurnManager.TurnEnded += OnTurnEnded;
        }

        private void OnTurnStarted(Selectable obj)
        {
            m_HighlightItem = HexHighlighter.PlaceHighlighter(obj.Cell, Highlighter.white, m_HighlightItem);
        }

        private void OnTurnEnded(Selectable obj)
        {
            m_HighlightItem.Release();
            m_HighlightItem = null;
        }
    }
}
