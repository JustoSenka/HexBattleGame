﻿using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets
{
    public class GameLoopBehaviour : MonoBehaviour
    {
#pragma warning disable CS0649
        [Dependency(typeof(IGameLoop))]
        public IGameLoop GameLoop;

        [Dependency(typeof(IMouseInputManager))]
        public IMouseInputManager UserInputManager;

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

        [Dependency(typeof(SceneLoader))]
        public SceneLoader SceneLoader;

        [Dependency(typeof(IMonoDatabase))]
        public IMonoDatabase MonoDatabase;

        [Dependency(typeof(ITurnManager))]
        public ITurnManager TurnManager;

        [Dependency(typeof(ICrossPlayerController))]
        public ICrossPlayerController CrossPlayerController;

        [Dependency(typeof(IEnemyAI))]
        public IEnemyAI EnemyAI;

        [Dependency(typeof(IHealthBarManager))]
        public IHealthBarManager HealthBarManager;

        [Dependency(typeof(IDatabase))]
        public IDatabase Database;

#pragma warning restore CS0649



        void Awake()
        {
            
        }

        void Start()
        {
            CrossPlayerController.LocalTeam = 0;
            EnemyAI.LocalTeam = 1;
            GameLoop.Start();

            Database.Start();

            MonoDatabase.Start();
            HexHighlighter.Start();

            HealthBarManager.Start();

            TurnManager.Start();

            // Optional
            StartDebugOrEditorOnlySystems();
        }

        void Update()
        {
            // GameLoop.Update();

            UserInputManager.Update();
            HealthBarManager.Update();

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
