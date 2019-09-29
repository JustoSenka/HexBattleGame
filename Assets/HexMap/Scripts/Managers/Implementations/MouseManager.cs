using System;
using UnityEngine;

namespace Assets
{
    [RegisterDependency(typeof(IMouseManager), true)]
    public class MouseManager : IMouseManager
    {
        private readonly IInputManager InputManager;

        public bool IsUnderCell { get; private set; }
        public HexCell HexUnderMouse { get; private set; }

        public event Action<HexCell> HexClicked;
        public event Action<HexCell> HexSelected;
        public event Action MouseReleased;

        private HexCell m_MouseDownOnHex;
        private HexCell m_MouseReleasedOnHex;

        private Vector3 m_OldMousePosition;
        private bool m_IsDraggingMouse;

        public MouseManager(IInputManager InputManager)
        {
            this.InputManager = InputManager;
        }

        public void Update()
        {
            if (IsOrWasDraggingMouse())
                return;

            // Mouse down ont hex
            if (InputManager.GetKeyDown(KeyCode.Mouse0) && HexUnderMouse.IsValid)
            {
                m_MouseDownOnHex = HexUnderMouse;
                HexSelected?.Invoke(HexUnderMouse);
            }

            // Mouse released on hex
            if (InputManager.GetKeyUp(KeyCode.Mouse0) && HexUnderMouse.IsValid)
            {
                m_MouseReleasedOnHex = HexUnderMouse;

                // Fire hex clicked callback
                if (m_MouseDownOnHex == m_MouseReleasedOnHex)
                    HexClicked?.Invoke(m_MouseReleasedOnHex);

                // Clear the state of all hexes
                m_MouseDownOnHex = default;
                m_MouseReleasedOnHex = default;
            }

            HandleHoveringHighlight();
        }

        private bool IsOrWasDraggingMouse()
        {
            if (InputManager.GetKeyUp(KeyCode.Mouse0))
                MouseReleased?.Invoke();

            if (InputManager.GetKeyDown(KeyCode.Mouse0))
                m_OldMousePosition = InputManager.mousePosition;

            // return if was dragging frame before and now release
            if (m_IsDraggingMouse && InputManager.GetKeyUp(KeyCode.Mouse0))
                return true;

            // Return if still dragging mouse
            m_IsDraggingMouse = InputManager.GetKey(KeyCode.Mouse0) && Vector3.Distance(InputManager.mousePosition, m_OldMousePosition) > 3;
            if (m_IsDraggingMouse)
                return true;

            return false;
        }

        private void HandleHoveringHighlight()
        {
            var ray = Camera.main.ScreenPointToRay(InputManager.mousePosition);
            IsUnderCell = Physics.Raycast(ray, out RaycastHit hit, 500f, Layer.GroundMask);

            if (IsUnderCell)
            {
                var point = hit.point;
                var pos = HexCell.WorldPointToHex(point, 1);
                HexUnderMouse = new HexCell(pos);
            }
            else
            {
                HexUnderMouse = default;
            }
        }
    }
}