using System;
using UnityEngine;

namespace Assets
{
    [RegisterDependency(typeof(IMouseManager), true)]
    public class MouseManager : IMouseManager
    {
        private readonly IInputManager InputManager;
        private readonly IHexDatabase HexDatabase;
        private readonly ITeam Team;

        public bool IsUnderCell { get; private set; }
        public HexCell HexUnderMouse { get; private set; }

        public bool IsUnderSelectable { get; private set; }
        public Selectable SelectableUnderMouse { get; private set; }

        public event Action<HexCell> HexPressedDown;
        public event Action<HexCell> HexSelected;
        public event Action<HexCell> HexUnselected;

        public event Action<Selectable> SelectableSelected; 
        public event Action<Selectable> SelectableUnselected;

        public event Action MouseReleased;

        private HexCell m_MouseDownOnHex; // Used to remember which hex user pressed down but didn't release yet

        private HexCell m_CurrentlySelectedHex;
        private Selectable m_CurrentlySelectedSelectable;

        private Vector3 m_OldMousePosition;
        private bool m_IsDraggingMouse;

        public MouseManager(IInputManager InputManager, IHexDatabase HexDatabase, ITeam Team)
        {
            this.InputManager = InputManager;
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
                {
                    if (m_CurrentlySelectedHex != HexUnderMouse)
                    {
                        if (m_CurrentlySelectedHex.IsValid)
                            HexUnselected?.Invoke(m_CurrentlySelectedHex);

                        m_CurrentlySelectedHex = HexUnderMouse;
                        HexSelected?.Invoke(HexUnderMouse);
                    }
                }
                // If released mouse, but not on any hex, unselect old hex
                else if (m_CurrentlySelectedHex.IsValid)
                {
                    HexUnselected?.Invoke(m_CurrentlySelectedHex);
                    m_CurrentlySelectedHex = default;
                }

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
                    // Unselect old one if different one is selected now
                    if (m_CurrentlySelectedSelectable != SelectableUnderMouse)
                    {
                        if (m_CurrentlySelectedSelectable != null)
                            SelectableUnselected?.Invoke(m_CurrentlySelectedSelectable);

                        m_CurrentlySelectedSelectable = SelectableUnderMouse;
                        SelectableSelected?.Invoke(SelectableUnderMouse);

                        // If selectable was selected, unselect hex as well
                        if (m_CurrentlySelectedHex.IsValid)
                            HexUnselected?.Invoke(m_CurrentlySelectedHex);
                    }

                    // else do nothing since same item was again selected
                    // Even if same item was selected, still return true so Hex selection code doesn't run
                    return true;
                }
                // Mouse was released but not while dragging or not on a selectable. Deselect previously selected item
                else if (m_CurrentlySelectedSelectable != null)
                {
                    SelectableUnselected?.Invoke(m_CurrentlySelectedSelectable);
                    m_CurrentlySelectedSelectable = null;
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
                    IsUnderCell = true;
                    var pos = HexUtility.WorldPointToHex(hit.point, 1);
                    HexUnderMouse = new HexCell(pos);

                    IsUnderSelectable = false;
                    SelectableUnderMouse = default;
                }
                else // Select selectable and hex it is in
                {
                    IsUnderSelectable = true;
                    SelectableUnderMouse = HexDatabase.GetSelectable(selectableBehaviour.Selectable.Cell);

                    IsUnderCell = true;
                    HexUnderMouse = HexDatabase.GetHex(selectableBehaviour.Selectable.Cell);
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
