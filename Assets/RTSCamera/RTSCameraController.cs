using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.Video;
namespace rkoning.RTS {
    public class RTSCameraController : MonoBehaviour
    {
        [SerializeField]
        private Camera mainCamera;
        [SerializeField]
        private Transform cameraRotationPivot;


        [SerializeField]
        private float rotationSpeed;
        [SerializeField]
        private float zoomSpeed;
        [SerializeField]
        private float minZoom = 10f, maxZoom = 50f;
        [SerializeField]
        private float minZoomAngle = 35, maxZoomAngle = 70;
        [SerializeField]
        private float panSpeed;

        private Vector2 panInput;
        private float zoomInput;
        private float rotationInput;

        // Start is called before the first frame update
        void Start()
        {
            if (!mainCamera) {
                mainCamera = Camera.main;
            }
        }

        // Update is called once per frame
        void Update()
        {
            CaptureInputs();
            MoveCamera();
        }

        void MoveCamera() {
            var flatDirection = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
            Quaternion rotation = Quaternion.LookRotation(flatDirection);

            var delta = rotation * new Vector3(panInput.x * panSpeed, zoomInput * zoomSpeed, panInput.y * panSpeed) * Time.deltaTime;

            Vector3 nextPosition = transform.position + delta;
            float clampedY = Mathf.Clamp(nextPosition.y, minZoom, maxZoom);
            nextPosition.y = clampedY;
            transform.position = nextPosition;

            transform.Rotate(Vector3.up, rotationInput * rotationSpeed * Time.deltaTime);
            float zoomPercentage = (clampedY - minZoom) / (maxZoom - minZoom);
        
            float zoomAngle = minZoomAngle + (maxZoomAngle - minZoomAngle) * zoomPercentage;
            cameraRotationPivot.localEulerAngles = new Vector3(zoomAngle, 0f, 0f);
        }

        void CaptureInputs() {
            // TODO: Pan if mouse at edge of screen
            panInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

            zoomInput = -Input.mouseScrollDelta.y;

            if (Input.GetKey(KeyCode.Q)) {
                rotationInput = -1;
            } else if (Input.GetKey(KeyCode.E)) {
                rotationInput = 1;
            } else {
                rotationInput = 0;
            }
        }
    }
}
