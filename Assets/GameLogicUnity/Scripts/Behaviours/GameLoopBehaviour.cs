using System.Diagnostics;
using UnityEngine;

namespace Assets
{
    public class GameLoopBehaviour : MonoBehaviour
    {
        [Dependency(typeof(IGameLoop))]
        public IGameLoop GameLoop;

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

        [Dependency(typeof(IPopulateHexDatabase))]
        public IPopulateHexDatabase PopulateHexDatabase;

        void Awake()
        {
            // GameLoop.Awake();
        }

        void Start()
        {
            // GameLoop.Start();

            PopulateHexDatabase.Start();

            HexHighlighter.Start();
            SelectableHexHighlights.Start();

            // Optional
            StartDebugOrEditorOnlySystems();
        }

        void Update()
        {
            // GameLoop.Update();

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
