using UnityEngine;

namespace Assets
{
    [ExecuteInEditMode]
    public class ColorPropertyBlock : MonoBehaviour
    {
        public Color Color;

        private Renderer _renderer;
        private MaterialPropertyBlock _propBlock;

        void Awake()
        {
            _propBlock = new MaterialPropertyBlock();
            _renderer = GetComponent<Renderer>();
        }

        void Update()
        {
            _renderer.GetPropertyBlock(_propBlock);
            _propBlock.SetColor("_Color", Color);
            _renderer.SetPropertyBlock(_propBlock);
        }
    }
}
