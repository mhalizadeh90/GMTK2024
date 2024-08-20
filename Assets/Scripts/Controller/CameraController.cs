using UnityEngine;

namespace Controller
{
    public class CameraController : MonoBehaviour
    {
        #region Fields

        public static float CameraDistance => _camera.orthographicSize;
        private static Camera _camera;

        [Header("Zoom Setting")] [SerializeField]
        private float zoomSpeed = 2f;
        [SerializeField] private float zoomSpeedMultiplier = 1;
        [SerializeField] private float zoomButtonSpeed = 5f;
        [SerializeField] private float smoothZoomSpeed = 10f;
        private float zoomInMax;
        float zoomOutMax;
        private float targetZoom;
        private bool zoomInBtnActivated, zoomOutBtnActivated;

        [Header("Pan Setting")] [SerializeField]
        private float basePanSpeed = 5f;

        [SerializeField] private float panSpeedMultiplier = 0.1f;

        private Vector3 lastMousePosition;
        private bool isPanning = false;

        [Header("Boundary Setting")] [SerializeField]
        private BoxCollider2D cameraBounds;

        [SerializeField] private float bumpStrength = 0.1f;
        [SerializeField] private float bumpDuration = 0.3f;
        private Vector3 bumpOffset;
        private float bumpTimer;

        #endregion

        private void Awake()
        {
            #region Safety Check

            if (cameraBounds == null)
            {
                Debug.LogError("Camera Bounds not set. Please assign a BoxCollider2D to the CameraController.");
                return;
            }

            if (Camera.main == null)
            {
                Debug.LogError("Main Camera not found.");
                return;
            }

            #endregion

            _camera = Camera.main;
            targetZoom = _camera.orthographicSize;
            zoomInMax = _camera.nearClipPlane;
            zoomOutMax = _camera.farClipPlane;
        }

        private void Update()
        {
            HandleZoom();
            HandlePan();
            ApplyBumpEffect();
        }

        private void HandleZoom()
        {
            float scrollDelta = Input.mouseScrollDelta.y;

            if (zoomInBtnActivated)
            {
                scrollDelta = 1 * zoomButtonSpeed;
                zoomInBtnActivated = false;
            }

            if (zoomOutBtnActivated)
            {
                scrollDelta = -1 * zoomButtonSpeed;
                zoomOutBtnActivated = false;
            }

            if (scrollDelta != 0)
            {
                // Adjust zoom speed based on the current orthographic size.
                float adjustedZoomSpeed = CalculateAdjustedZoomSpeed();
              //  print($"Zoom: {adjustedZoomSpeed * zoomSpeedMultiplier}");
                // Update targetZoom with the adjusted speed.
                targetZoom -= scrollDelta * adjustedZoomSpeed * zoomSpeedMultiplier;
                targetZoom = Mathf.Clamp(targetZoom, zoomInMax, zoomOutMax);
            }

            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, targetZoom, Time.deltaTime * smoothZoomSpeed);
        }

        private void HandlePan()
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastMousePosition = Input.mousePosition;
                isPanning = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isPanning = false;
            }

            if (isPanning && cameraBounds != null)
            {
                Vector3 delta = Input.mousePosition - lastMousePosition;
                float adjustedPanSpeed = CalculateAdjustedPanSpeed();
                Vector3 newPosition = _camera.transform.position -
                                      new Vector3(delta.x, delta.y, 0) * adjustedPanSpeed * Time.deltaTime;

                // Clamp the new position within bounds
                Vector3 minBounds = cameraBounds.bounds.min;
                Vector3 maxBounds = cameraBounds.bounds.max;
                newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
                newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);

                // Check if we hit a boundary and apply bump
                if (newPosition.x == minBounds.x || newPosition.x == maxBounds.x ||
                    newPosition.y == minBounds.y || newPosition.y == maxBounds.y)
                {
                    ApplyBump(newPosition - _camera.transform.position);
                }

                _camera.transform.position = newPosition;
                lastMousePosition = Input.mousePosition;
            }
        }

        private void ApplyBump(Vector3 direction)
        {
            bumpOffset = -direction.normalized * bumpStrength;
            bumpTimer = bumpDuration;
        }

        private void ApplyBumpEffect()
        {
            if (bumpTimer > 0)
            {
                bumpTimer -= Time.deltaTime;
                float t = 1 - (bumpTimer / bumpDuration); // Normalized time
                Vector3 currentBumpOffset = Vector3.Lerp(bumpOffset, Vector3.zero, t);
                _camera.transform.position += currentBumpOffset;
            }
        }

        public void ZoomInStart()
        {
            zoomInBtnActivated = true;
        }

        public void ZoomOutStart()
        {
            zoomOutBtnActivated = true;
        }

        private float CalculateAdjustedPanSpeed()
        {
            // Calculate a factor to scale the pan speed based on the camera's orthographic size.
            // When orthographic size is large, this factor will be close to 1 (normal speed).
            // When orthographic size is small, the factor will reduce, slowing down the pan speed.
            float sizeFactor = Mathf.Clamp(_camera.orthographicSize / zoomOutMax, 0.01f, 1f);

            // Adjusted pan speed is calculated by multiplying the base pan speed with this factor.
            float adjustedPanSpeed = basePanSpeed * sizeFactor * panSpeedMultiplier;

            return adjustedPanSpeed;
        }
    
        private float CalculateAdjustedZoomSpeed()
        {
            // Calculate the factor to adjust the zoom speed.
            // When orthographic size is small, the zoom speed will be lower.
            // When orthographic size is large, the zoom speed will be higher.
            float sizeFactor = Mathf.Clamp(_camera.orthographicSize / zoomOutMax, 0.1f, 1f);
    
            // Adjust the zoom speed based on the size factor.
            float adjustedZoomSpeed = zoomSpeed * sizeFactor;

            return adjustedZoomSpeed;
        }
    }
}