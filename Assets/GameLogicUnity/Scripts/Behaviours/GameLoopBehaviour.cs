﻿using System.Diagnostics;
using UnityEngine;

namespace Assets
{
    public class GameLoopBehaviour : MonoBehaviour
    {
        [Dependency(typeof(IGameLoop))]
        public IGameLoop GameLoop;

        [Dependency(typeof(IUserInputManager))]
        public IUserInputManager UserInputManager;

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

        [Dependency(typeof(IMonoDatabase))]
        public IMonoDatabase MonoDatabase;

        void Awake()
        {
            // GameLoop.Awake();
        }

        void Start()
        {
            // GameLoop.Start();

            PopulateHexDatabase.Start();
            MonoDatabase.Start();

            HexHighlighter.Start();

            // Optional
            StartDebugOrEditorOnlySystems();
        }

        void Update()
        {
            // GameLoop.Update();

            UserInputManager.Update();

            HexDebugger.Update(); // Currently empty and commented out
        }

        [Conditional("Debug")]
        [Conditional("UNITY_EDITOR")]
        private void StartDebugOrEditorOnlySystems()
        {
            HexDebugger.Start();
        }
    }
}
