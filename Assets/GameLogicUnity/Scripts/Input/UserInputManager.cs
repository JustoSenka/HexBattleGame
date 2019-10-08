using System;
using UnityEngine;

namespace Assets
{
    [RegisterDependency(typeof(IUserInputManager), true)]
    public class UserInputManager : IUserInputManager
    {
        private bool IsUnderCell { get; set; }
        private HexCell HexUnderMouse { get; set; }

        private bool IsUnderSelectable { get; set; }
        private Selectable SelectableUnderMouse { get; set; }

        public event Action<HexCell> HexPressedDown;
        public event Action MouseReleased;

        public event Action<HexCell> HexUnderMouseChanged;
        public event Action<Selectable> SelectableUnderMouseChanged;

        private HexCell m_MouseDownOnHex; // Used to remember which hex user pressed down but didn't release yet

        private Vector3 m_OldMousePosition;
        private bool m_IsDraggingMouse;

        private readonly IInputManager InputManager;
        private readonly ISelectionManager SelectionManager;
        private readonly IHexDatabase HexDatabase;
        private readonly ITeam Team;
        public UserInputManager(IInputManager InputManager, ISelectionManager SelectionManager, IHexDatabase HexDatabase, ITeam Team)
        {
            this.InputManager = InputManager;
            this.SelectionManager = SelectionManager;
            this.HexDatabase = HexDatabase;
            this.Team = Team;
        }

        public void Update()
        {
            if (IsOrWasDraggingMouse())
                return;

            var clickedOnSelectable = HandleClickingOnSelectables();

            // If did not click on selectable, look if clicked on any hex
            // If we already know that selectable was clicked, don't look if clicked on any hexes
            if (!clickedOnSelectable)
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
                HexPressedDown?.Invoke(HexUnderMouse);
            }

            // Mouse released on hex
            if (InputManager.GetKeyUp(KeyCode.Mouse0))
            {
                // If released on valid hex and did not drag the mouse, select the hex (unselect old one if needed)
                if (HexUnderMouse.IsValid && m_MouseDownOnHex == HexUnderMouse)
                    SelectionManager.SelectHexCell(HexUnderMouse);

                else // If released mouse, but not on any hex, unselect old hex
                    SelectionManager.UnselectAll();

                // Clear the state hex being pressed down
                m_MouseDownOnHex = default;
            }
        }

        private bool HandleClickingOnSelectables()
        {
            if (InputManager.GetKeyUp(KeyCode.Mouse0))
            {
                if (SelectableUnderMouse != null && SelectableUnderMouse.Team == Team.TeamID)
                {
                    SelectionManager.SelectSelectable(SelectableUnderMouse);

                    // else do nothing since same item was again selected
                    // Even if same item was selected, still return true so Hex selection code doesn't run
                    return true;
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
                var selectableBehaviour = hit.collider.gameObject.GetComponent<SelectableBehaviour>();
                if (!selectableBehaviour) // Only select hex and deselect selectable if it was selected before
                {
                    var pos = HexUtility.WorldPointToHex(hit.point, 1);

                    IsUnderCell = true;
                    IsUnderSelectable = false;

                    SetSelectableUnderMouse(default);
                    SetHexUnderMouse(pos);
                }
                else // Select selectable and hex it is in
                {
                    IsUnderSelectable = true;
                    SetSelectableUnderMouse(selectableBehaviour.Cell);

                    IsUnderCell = true;
                    SetHexUnderMouse(selectableBehaviour.Cell);
                }
            }
            else // Reset all selections to default
            {
                IsUnderSelectable = false;
                SetSelectableUnderMouse(default);

                IsUnderCell = false;
                SetHexUnderMouse(default);
            }
        }

        private void SetHexUnderMouse(int2 pos)
        {
            var newHex = IsUnderCell ? HexDatabase.GetHex(pos) : default;
            if (newHex != HexUnderMouse)
            {
                HexUnderMouse = newHex;
                HexUnderMouseChanged?.Invoke(HexUnderMouse);
            }
        }

        private void SetSelectableUnderMouse(int2 pos)
        {
            var newSelectable = IsUnderSelectable ? HexDatabase.GetSelectable(pos) : default;
            if (newSelectable != SelectableUnderMouse)
            {
                SelectableUnderMouse = newSelectable;
                SelectableUnderMouseChanged?.Invoke(SelectableUnderMouse);
            }
        }
    }
}
