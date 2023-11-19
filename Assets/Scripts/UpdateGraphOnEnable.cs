using UnityEngine;
using Pathfinding;

namespace rkoning.RTS {

    [RequireComponent(typeof(Collider))]
    public class UpdateGraphOnEnable : MonoBehaviour
    {
        Bounds bounds;
        public void OnEnable() {
            bounds = GetComponent<Collider>().bounds;
            AstarPath.active.UpdateGraphs(bounds);
        }

        public void OnDisable() {
            if (AstarPath.active)
                AstarPath.active.UpdateGraphs(bounds);
        }
    }

}