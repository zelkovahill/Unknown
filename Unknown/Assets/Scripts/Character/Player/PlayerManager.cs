using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    // 플레이어의 전체적인 관리를 담당하는 클래스
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;

        protected override void Awake()
        {
            base.Awake();

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
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

            
            playerStatsManager.RegenerateStamina();
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

                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue;
                playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenTimer;

                playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
                playerNetworkManager.currentStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
                PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);
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

