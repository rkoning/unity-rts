using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rkoning.RTS {
    public class ObjectPlacer : MonoBehaviour
    {
        [SerializeField]
        private Camera mainCamera;
        [SerializeField]
        private List<GameObject> prefabs;

        private bool isHittingWorld;
        private Vector3 mousePosition;

        [SerializeField]
        private LayerMask worldMask;

        private readonly List<GameObject> placed = new();

        // Update is called once per frame
        void Update()
        {
            UpdateMouseWorldPoint();
            if (isHittingWorld) {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                    placed.Add(Instantiate(prefabs[0], mousePosition, Quaternion.identity));
                else if (Input.GetKeyDown(KeyCode.Alpha2)) 
                    placed.Add(Instantiate(prefabs[1], mousePosition, Quaternion.identity));
                else if (Input.GetKeyDown(KeyCode.Alpha3)) 
                    placed.Add(Instantiate(prefabs[2], mousePosition, Quaternion.identity));
            }

            if (Input.GetKeyDown(KeyCode.R)) {
                Destroy(PopLastPlaced());
            }
        }

        GameObject PopLastPlaced() {
            int lastIdx = placed.Count - 1;
            GameObject last = placed[lastIdx];
            placed.RemoveAt(lastIdx);
            return last;
        }

        void UpdateMouseWorldPoint() {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, worldMask)) {
                mousePosition = hit.point;
                isHittingWorld = true;
            } else {
                isHittingWorld = false;
            }
        }
    }
}