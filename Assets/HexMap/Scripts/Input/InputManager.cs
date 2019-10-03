using UnityEngine;

namespace Assets
{
    [RegisterDependency(typeof(IInputManager), false)]
    public class InputManager : IInputManager
    {
        public Vector3 mousePosition => Input.mousePosition;
        public bool GetKey(KeyCode keyCode) => Input.GetKey(keyCode);
        public bool GetKeyDown(KeyCode keyCode) => Input.GetKeyDown(keyCode);
        public bool GetKeyUp(KeyCode keyCode) => Input.GetKeyUp(keyCode);
    }
}
