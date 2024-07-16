using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    // 플레이어의 시점을 제어하는 카메라 기능을 제공하는 클래스
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera instance;
        public Camera cameraObject;
        public PlayerManager player;
        [SerializeField] private Transform cameraPivotTransform;



        [Header("Camera Settings")]
        private float cameraSmoothSpeed = 1;
        [SerializeField] private float leftAndRightRotationSpeed = 220;
        [SerializeField] private float upAndDownRotationSpeed = 220;
        [SerializeField] private float minimiumPivot = -30;
        [SerializeField] private float maximumPivot = 60;
        [SerializeField] private float cameraCollisionRadius = 0.2f;
        [SerializeField] private LayerMask collisionLayer;


        [Header("Camera Values")]
        private Vector3 cameraVelocity;
        private Vector3 cameraObjectPosition;
        [SerializeField] private float leftAndRightLookAngle;
        [SerializeField] private float upAndDownLookAngle;
        private float cameraZPosition;
        private float targetCameraZPosition;



        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            cameraZPosition = cameraObject.transform.localPosition.z;
        }

        // 모든 카메라 동작을 처리하는 함수
        public void HandleAllCameraActions()
        {
            if (player != null)
            {
                HandleFollowTarget();   // 타겟을 따라가도록 처리
                HandleToRotations();    // 회전 처리
                HandleCollisions();         // 충돌 처리
            }

        }

        // 카메라가 플레이를 부드럽게 따라가도록 처리하는 함수
        private void HandleFollowTarget()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;

        }

        // 카메라의 회전을 처리하는 함수
        private void HandleToRotations()
        {
            // 좌우 회전 각도를 업데이트
            leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;

            // 상하 회전 각도를 업데이트
            upAndDownLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;

            // 상하 회전 각도를 제한
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimiumPivot, maximumPivot);


            Vector3 cameraRotation = Vector3.zero;
            Quaternion targetRotation;

            // 좌우 회전 적용
            cameraRotation.y = leftAndRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;

            // 상하 회전 적용
            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = targetRotation;
        }

        // 카메라 충돌을 처리하는 함수
        private void HandleCollisions()
        {
            targetCameraZPosition = cameraZPosition;
            RaycastHit hit;
            Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
            direction.Normalize();

            // 카메라가 충돌하는지 확인하고, 충돌 시 카메라 위치를 조정
            if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collisionLayer))
            {
                float distanceFormHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetCameraZPosition = -(distanceFormHitObject - cameraCollisionRadius);
            }

            if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
            {
                targetCameraZPosition = -cameraCollisionRadius;
            }

            cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
            cameraObject.transform.localPosition = cameraObjectPosition;
        }

    }
}