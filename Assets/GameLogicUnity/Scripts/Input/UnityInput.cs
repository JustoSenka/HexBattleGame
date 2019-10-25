using UnityEngine;

namespace Assets
{
    [RegisterDependency(typeof(IUnityInput), false)]
    public class UnityInput : IUnityInput
    {
        public Vector3 mousePosition => Input.mousePosition;
        public bool GetKey(KeyCode keyCode) => Input.GetKey(keyCode);
        public bool GetKeyDown(KeyCode keyCode) => Input.GetKeyDown(keyCode);
        public bool GetKeyUp(KeyCode keyCode) => Input.GetKeyUp(keyCode);
    }
}
