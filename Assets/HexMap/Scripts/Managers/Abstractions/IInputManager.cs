using UnityEngine;

namespace Assets
{
    public interface IInputManager
    {
        Vector3 mousePosition { get; }

        bool GetKey(KeyCode keyCode);
        bool GetKeyDown(KeyCode keyCode);
        bool GetKeyUp(KeyCode keyCode);
    }
}
