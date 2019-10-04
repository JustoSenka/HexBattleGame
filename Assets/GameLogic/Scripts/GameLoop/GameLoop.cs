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
        public ISelectableHexHighlights SelectableHexHighlights;

        [Dependency(typeof(IHexDatabase))]
        public IHexDatabase HexDatabase;

        [Dependency(typeof(HexDebugger))]
        public HexDebugger HexDebugger;

        [Dependency(typeof(IUnitHexHighlights))]
        public IUnitHexHighlights UnitHexHighlights;

        [Dependency(typeof(IUnitMovementManager))]
        public IUnitMovementManager UnitMovementManager;

        void Awake()
        {

        }

        void Start()
        {
            HexDatabase.Start();
            HexHighlighter.Start();
            SelectableHexHighlights.Start();

            // Optional
            StartDebugOrEditorOnlySystems();
        }

        void Update()
        {
            MouseManager.Update();
            UnitMovementManager.Update();
            SelectableHexHighlights.Update();
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
