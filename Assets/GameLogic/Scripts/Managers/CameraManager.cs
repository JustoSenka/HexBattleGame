using UnityEngine;

namespace Assets
{
    [RequireComponent(typeof(Camera))]
    public class CameraManager : MonoBehaviour
    {
        public float moveSpeedHorizontal = 15f;
        public float moveSpeedVertical = 15f;
        public float zoomSpeed = 300f;

        public float cameraMinHeigh = 4f;
        public float cameraMaxHeigh = 30f;

        public Vector3 cameraFarAngle = new Vector3(50, 0, 0);
        public Vector3 cameraNearAngle = new Vector3(30, 0, 0);

        public float nearAngleThreshold = 10f;

        private float computedSpeedHorizontal;
        private float computedSpeedVertical;
        private float computedZoomSpeed;

        private float cameraZoom = 10;

        private Vector3 oldMousePosition;
        private Vector3 oldCameraPosition;

        void Update()
        {
            ComputeSpeeds();
            ApplyCameraMovement();
            ApplyFancyRotationWhenCameraIsNearGround();
            ApplyMidleMouseButtonMovementSpeed();
        }

        private void ApplyCameraMovement()
        {
            transform.position += new Vector3(computedSpeedHorizontal, 0, computedSpeedVertical);
            Vector3 zoomedPosition = Vector3.Lerp(transform.position, new Vector3(transform.position.x, cameraZoom, transform.position.z), 10 * Time.deltaTime);
            transform.position = zoomedPosition + new Vector3(0, 0, transform.position.y - zoomedPosition.y);
        }

        private void ApplyFancyRotationWhenCameraIsNearGround()
        {
            Quaternion rotation = new Quaternion();
            rotation.eulerAngles = Vector3.Lerp(cameraFarAngle, cameraNearAngle, (nearAngleThreshold - transform.position.y) / (nearAngleThreshold - cameraMinHeigh));
            transform.localRotation = rotation;
        }

        private void ComputeSpeeds()
        {
            computedSpeedVertical = Input.GetAxis("Vertical") * moveSpeedVertical * Time.deltaTime;
            computedSpeedHorizontal = Input.GetAxis("Horizontal") * moveSpeedHorizontal * Time.deltaTime;
            computedZoomSpeed = -Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * 0.015f;

            cameraZoom += computedZoomSpeed;
            cameraZoom = (cameraZoom > cameraMaxHeigh) ? cameraMaxHeigh : cameraZoom;
            cameraZoom = (cameraZoom < cameraMinHeigh) ? cameraMinHeigh : cameraZoom;
        }

        private void ApplyMidleMouseButtonMovementSpeed()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                oldMousePosition = Input.mousePosition;
                oldCameraPosition = transform.position;
            }

            // Mouse drag
            if (Input.GetKey(KeyCode.Mouse0) && Vector3.Distance(Input.mousePosition, oldMousePosition) > 3f)
            {
                var mouseWorldPoint = GetWorldMousePoint(Input.mousePosition);
                var mouseWorldPointBefore = GetWorldMousePoint(oldMousePosition);

                transform.position = oldCameraPosition - mouseWorldPoint + mouseWorldPointBefore;
            }
        }

        private Vector3 GetWorldMousePoint(Vector3 mousePosition)
        {
            var ray = Camera.main.ScreenPointToRay(mousePosition);
            var success = Physics.Raycast(ray, out RaycastHit hit, 500f, Layer.GroundMask);
            return success ? hit.point : Vector3.zero;
        }
    }
}
