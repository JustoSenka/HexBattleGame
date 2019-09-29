using UnityEngine;

namespace Assets
{
    public class GameLoop : MonoBehaviour
    {
        [Dependency(typeof(IMouseManager))]
        public IMouseManager MouseManager;

        [Dependency(typeof(IHexHighlightManager))]
        public IHexHighlightManager HexHighlightManager;

        void Awake()
        {

        }

        void Start()
        {
            HexHighlightManager.Start();
        }

        void Update()
        {
            MouseManager.Update();
            HexHighlightManager.Update();
        }
    }
}
