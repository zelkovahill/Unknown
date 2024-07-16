using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


namespace SG
{
    // 캐릭터의 기본적인 동작을 관리하는 클래스
    public class CharaterManager : NetworkBehaviour
    {
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;
        [HideInInspector] public CharaterNetworkManager charaterNetworkManager;

        // 캐릭터의 상태를 나타내는 플래그 변수들
        [Header("Flags")]
        public bool isPerformingAction = false;
        public bool applyRootMotion = false;
        public bool canRotate = true;
        public bool canMove = true;


        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);

            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            charaterNetworkManager = GetComponent<CharaterNetworkManager>();
        }

        protected virtual void Update()
        {
            // 로컬 플레이어인 경우 네트워크 위치와 회전값을 업데이트
            if (IsOwner)
            {
                charaterNetworkManager.networkPosition.Value = transform.position;
                charaterNetworkManager.networkRotation.Value = transform.rotation;
            }
            else
            {
                // 네트워크 위치로 부드럽게 이동
                transform.position = Vector3.SmoothDamp
                    (transform.position,
                    charaterNetworkManager.networkPosition.Value,
                    ref charaterNetworkManager.networkPositionVelocity,
                    charaterNetworkManager.networkPositionSmoothTime);

                // 네트워크 회전으로 부드럽게 회전
                transform.rotation = Quaternion.Slerp
                    (transform.rotation,
                    charaterNetworkManager.networkRotation.Value,
                    charaterNetworkManager.networkRotationSmoothTime);
            }

        }

        protected virtual void LateUpdate()
        {

        }
    }
}
