using System;
using UnityEngine;

namespace Assets
{
    [RegisterDependency(typeof(IMouseManager), true)]
    public class MouseManager : IMouseManager
    {
        private readonly IInputManager InputManager;
        private readonly ITeam Team;

        public bool IsUnderCell { get; private set; }
        public HexCell HexUnderMouse { get; private set; }

        public bool IsUnderSelectable { get; private set; }
        public Selectable SelectableUnderMouse { get; private set; }

        public event Action<HexCell> HexClicked;
        public event Action<HexCell> HexSelected;
        public event Action MouseReleased;
        public event Action<Selectable> SelectableClicked;

        private HexCell m_MouseDownOnHex;
        private HexCell m_MouseReleasedOnHex;

        private Vector3 m_OldMousePosition;
        private bool m_IsDraggingMouse;

        public MouseManager(IInputManager InputManager, ITeam Team)
        {
            this.InputManager = InputManager;
            this.Team = Team;
        }

        public void Update()
        {
            if (IsOrWasDraggingMouse())
                return;

            var clicked = HandleClickingOnSelectables();

            // If did not click on selectable, look if clicked on any hex
            // If we already know that selectable was clicked, don't look if clicked on any hexes
            if (!clicked)
                HandleClickingAndReleasingOnHexTiles();

            // This will not be in mobile version
            HandleHoveringHighlight();
        }

        private void HandleClickingAndReleasingOnHexTiles()
        {
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
        }

        private bool HandleClickingOnSelectables()
        {
            if (InputManager.GetKeyUp(KeyCode.Mouse0))
            {
                var ray = Camera.main.ScreenPointToRay(InputManager.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 500f, Layer.SelectableMask))
                {
                    var go = hit.collider.gameObject;
                    var selectable = go.GetComponent<Selectable>();
                    if (selectable != null && selectable.Team == Team.TeamID)
                    {
                        SelectableClicked?.Invoke(selectable);
                        return true;
                    }
                }
            }
            return false;
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
            if (Physics.Raycast(ray, out RaycastHit hit, 500f, LayerMask.GetMask(Layer.Ground, Layer.Selectable)))
            {
                var selectable = hit.collider.gameObject.GetComponent<Selectable>();
                if (!selectable) // Only select hex and deselct selectable if it was selected before
                {
                    IsUnderCell = true;
                    var pos = HexCell.WorldPointToHex(hit.point, 1);
                    HexUnderMouse = new HexCell(pos);

                    IsUnderSelectable = false;
                    SelectableUnderMouse = default;
                }
                else // Select selectable and hex it is in
                {
                    IsUnderSelectable = true;
                    SelectableUnderMouse = selectable;

                    IsUnderCell = true;
                    HexUnderMouse = selectable.HexCell;
                }
            }
            else // Reset all selections to default
            {
                IsUnderSelectable = false;
                SelectableUnderMouse = default;

                IsUnderCell = false;
                HexUnderMouse = default;
            }
        }
    }
}
