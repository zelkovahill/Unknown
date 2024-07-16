using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    // 플레이어의 전체적인 관리를 담당하는 클래스
    public class PlayerManager : CharaterManager
    {
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerNetworkManager PlayerNetworkManager;

        protected override void Awake()
        {
            base.Awake();

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            PlayerNetworkManager = GetComponent<PlayerNetworkManager>();
        }

        protected override void Update()
        {
            base.Update();

            // 로컬 플레이어가 아니면 리턴
            if (!IsOwner)
            {
                return;
            }

            // 모든 이동을 처리
            playerLocomotionManager.HandleAllMovement();
        }

        // 네트워크 스폰 시 실행되는 함수
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            // 로컬 플레이어일 때 카메라와 입력 매니저에 플레이어를 설정
            if (IsOwner)
            {
                PlayerCamera.instance.player = this;
                PlayerInputManager.instance.player = this;
            }
        }


        protected override void LateUpdate()
        {
            if (!IsOwner)
            {
                return;
            }
            base.LateUpdate();

            // 모든 카메라 동작을 처리
            PlayerCamera.instance.HandleAllCameraActions();
        }
    }

}

