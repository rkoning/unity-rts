using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rkoning.RTS {
    [RequireComponent(typeof(Selectable))]
    public class SelectionIndicator : MonoBehaviour
    {
        private Selectable selectable;

        public readonly GameObject indicator;

        public void OnEnable() {
            selectable = GetComponent<Selectable>();
            if (!indicator) {
                Debug.LogWarning($"No Indicator set on selectable {name}!");
                return;
            }

            selectable.OnSelected += ShowIndicator;
            selectable.OnDeselected += HideIndicator;
            HideIndicator(selectable);
        }

        public void OnDisable() {
            selectable.OnSelected -= ShowIndicator;
            selectable.OnDeselected -= HideIndicator;
        }

        private void ShowIndicator(Selectable s) {
            indicator.SetActive(true);
        }

        
        private void HideIndicator(Selectable s) {
            indicator.SetActive(false);
        }
    }
}