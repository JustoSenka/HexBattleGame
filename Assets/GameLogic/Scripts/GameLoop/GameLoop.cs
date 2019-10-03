using System.Diagnostics;
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

        [Dependency(typeof(IHexDatabase))]
        public IHexDatabase HexDatabase;

        [Dependency(typeof(HexDebugger))]
        public HexDebugger HexDebugger;

        [Dependency(typeof(UnitHexHighlights))]
        public UnitHexHighlights UnitHexHighlights;

        void Awake()
        {

        }

        void Start()
        {
            HexDatabase.Start();
            HexHighlighter.Start();
            SelectableHighlightManager.Start();

            // Optional
            StartDebugOrEditorOnlySystems();
        }

        void Update()
        {
            MouseManager.Update();
            SelectableHighlightManager.Update();
            UnitHexHighlights.Update();

            HexDebugger.Update();
        }

        [Conditional("Debug")]
        [Conditional("UNITY_EDITOR")]
        private void StartDebugOrEditorOnlySystems()
        {
            HexDebugger.Start();
        }
    }
}
