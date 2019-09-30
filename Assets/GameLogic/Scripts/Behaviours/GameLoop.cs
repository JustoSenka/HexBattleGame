using UnityEngine;

namespace Assets
{
    public class GameLoop : MonoBehaviour
    {
        [Dependency(typeof(IMouseManager))]
        public IMouseManager MouseManager;

        [Dependency(typeof(IHexHighlighter))]
        public IHexHighlighter HexHighlighter;

        [Dependency(typeof(ISelectableHexHighlights))]
        public ISelectableHexHighlights SelectableHighlightManager;

        [Dependency(typeof(UnitHexHighlights))]
        public UnitHexHighlights UnitHexHighlights;

        void Awake()
        {

        }

        void Start()
        {
            HexHighlighter.Start();
            SelectableHighlightManager.Start();
        }

        void Update()
        {
            MouseManager.Update();
            SelectableHighlightManager.Update();
            UnitHexHighlights.Update();
        }
    }
}
