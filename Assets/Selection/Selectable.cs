using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rkoning.RTS {
    public class Selectable : MonoBehaviour
    {
        public delegate void SelectionEvent(Selectable subject);
        public event SelectionEvent OnSelected;
        public event SelectionEvent OnDeselected;

        public void OnEnable() {
            SelectionManager.Register(this);
        }

        public void OnDisable() {
            SelectionManager.Deregister(this);
        }

        public void Select() {
            OnSelected?.Invoke(this);
        }
        
        public void Deselect() {
            OnDeselected?.Invoke(this);
        }
    }
}
