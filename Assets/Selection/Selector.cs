using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;


namespace rkoning.RTS {
    public class Selector : MonoBehaviour
    {
        [SerializeField]
        private Camera mainCamera;
        private readonly List<Selectable> selected = new();
        public List<Selectable> Selected {
            get { return selected; }
        }

        [SerializeField]
        private readonly LayerMask selectableMask;

        private bool selectDown;
        private bool selectHeld;
        private bool selectUp;

        private Vector2 dragStart;
        private bool isDragging;
        public delegate void DragEvent(Vector2 screenPosition);

        public event DragEvent OnDragStart;
        public event DragEvent OnDrag;
        public event DragEvent OnDragEnd;

        private bool shiftHeld;
        private bool ctrlHeld;
        
        private void Awake() {
            if (!mainCamera) {
                mainCamera = Camera.main;
            }

            OnDragStart += (Vector2 sPos) => {};
            OnDrag += (Vector2 sPos) => {};
            OnDragEnd += (Vector2 sPos) => {};
        }

        private void Update() {
            CaptureInputs();
            if (selectDown) {
                if (TryGetSelectableAtMousePosition(out var s))
                    OnSelectIndividual(s);
                else
                    ClearSelection();
            } else if (selectHeld) {
                if (!isDragging) {
                    dragStart = Input.mousePosition;
                    OnDragStart(dragStart);
                    isDragging = true;
                } else {
                    OnDrag(Input.mousePosition);
                }
            } else if (selectUp && isDragging) {
                OnDragEnd(Input.mousePosition);
                isDragging = false;
                bool hitStart = TryGetWorldPointAtScreenPosition(dragStart, out var startWorldPos);
                bool hitEnd = TryGetWorldPointAtScreenPosition(Input.mousePosition, out var endWorldPos);

                if (hitStart && hitEnd)
                    Debug.DrawLine(
                        startWorldPos,
                        endWorldPos,
                        Color.yellow,
                        3f
                    );
                
                if (TryGetSelectablesInScreenRegion(dragStart, Input.mousePosition, out var inRegion)) {
                    SelectOnly(inRegion);
                }
            }
        }

        private bool TryGetSelectablesInScreenRegion(Vector2 start, Vector2 end, out List<Selectable> inRegion) {
            float xMin = Mathf.Min(start.x, end.x);
            float xMax = Mathf.Max(start.x, end.x);
            float yMin = Mathf.Min(start.y, end.y);
            float yMax = Mathf.Max(start.y, end.y);

            inRegion = new();
            foreach (var s in SelectionManager.GetAllSelectables()) {
                var worldPosition = s.transform.position;
                var screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
                float x = screenPosition.x;
                float y = screenPosition.y;
                if (x > xMin && x < xMax && y > yMin && y < yMax)
                    inRegion.Add(s);
            }
            return inRegion.Count > 0;
        }

        private void OnSelectIndividual(Selectable selectable) {
            if (shiftHeld) {
                AddToSelection(selectable);
            } else if (ctrlHeld) {
                RemoveFromSelection(selectable);
            } else {
                SelectOnly(selectable);
            }

        }

        private void CaptureInputs() {
            selectDown = Input.GetMouseButtonDown(0);
            selectHeld = Input.GetMouseButton(0);
            selectUp = Input.GetMouseButtonUp(0);

            shiftHeld = Input.GetKey(KeyCode.LeftShift);
            ctrlHeld = Input.GetKey(KeyCode.LeftControl);
        }

        bool TryGetWorldPointAtScreenPosition(Vector2 screenPosition, out Vector3 worldPosition) {
            Ray ray = mainCamera.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, selectableMask)) {
                worldPosition = hit.point;
                return true;
            }
            worldPosition = Vector3.zero;
            return false;
        }

        bool TryGetSelectableAtMousePosition(out Selectable selectable) {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, selectableMask) 
                && hit.collider.TryGetComponent(out selectable))
                    return true;
            selectable = null;
            return false;
        }

        /// <summary>
        /// Selects a single selectable, deselecting all others beforehand
        /// </summary>
        /// <param name="s">The Selectable to Select</param>
        public void SelectOnly(Selectable s) {
            ClearSelection();
            AddToSelection(s);
        }

        /// <summary>
        /// Selects a a range of selectables, deselecting all others beforehand
        /// </summary>
        /// <param name="s">The set of Selectables to Select</param>
        public void SelectOnly(List<Selectable> selectables) {
            ClearSelection();
            AddToSelection(selectables);
        }

        /// <summary>
        /// Deselects all currently selected items
        /// </summary>
        public void ClearSelection() {
            foreach(var s in selected) {
                s.Deselect();
            }
            selected.Clear();
        }

        /// <summary>
        /// Adds a new Selectable to the selection, does not clear existing selection
        /// </summary>
        /// <param name="s">Selectable to add</param>
        public void AddToSelection(Selectable s) {
            selected.Add(s);
            s.Select();
        }

        /// <summary>
        /// Adds a List of new Selectables to the selection, does not clear existing selection
        /// </summary>
        /// <param name="items">Range of Selectables to add</param>
        public void AddToSelection(List<Selectable> items) {
            selected.AddRange(items);
            foreach (var s in items) 
                s.Select();
        }

        /// <summary>
        /// Removes a Selectable from the selection if it is currently in it, does not clear existing selection
        /// </summary>
        /// <param name="s">Selectable to remove</param>
        public void RemoveFromSelection(Selectable s) {
            if (selected.Contains(s)) {
                s.Deselect();
                selected.Remove(s);
            }
        }
    }
}