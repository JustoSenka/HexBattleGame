using UnityEngine;

namespace Assets
{
    public class GameLoop : MonoBehaviour
    {
        [Dependency(typeof(IMouseManager))]
        public IMouseManager MouseManager;

        [Dependency(typeof(IHexHighlighter))]
        public IHexHighlighter HexHighlighter;

        [Dependency(typeof(ISelectableHighlightManager))]
        public ISelectableHighlightManager SelectableHighlightManager;

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
        }
    }
}
