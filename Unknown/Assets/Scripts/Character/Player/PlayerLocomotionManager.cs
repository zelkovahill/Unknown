using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;


namespace SG
{
    // 플레이어의 이동을 관리하는 역할을 하는 클래스
    public class PlayerLocomotionManager : CharaterLocomotionManager
    {
        private PlayerManager player;

        // 이동 관련 입력값을 저장하는 변수들
        [HideInInspector] public float verticalMovement;
        [HideInInspector] public float horizontalMovement;
        [HideInInspector] public float moveAmount;

        // 이동 및 회전 설정값을 저장하는 변수들
        [Header("Movement Settings")]
        private Vector3 targetRotationDirection;
        private Vector3 moveDirection;
        [SerializeField] private float walkingSpeed = 2;
        [SerializeField] private float runningSpeed = 5;
        [SerializeField] private float spintingSpeed = 6.5f;
        [SerializeField] private float rotationSpeed = 15;
        [SerializeField] private int sprintingStaminaCost = 2;


        [Header("Jump")]
        [SerializeField] private float jumpStaminaCost = 25;
        [SerializeField] private float jumpHeight = 4f;
        [SerializeField] private float jumpForwardSpeed = 5;
        [SerializeField] private float freeFallSpeed = 2;
        private Vector3 jumpDirection;


        // 회피 동작 시의 방향을 저장하는 변수
        [Header("Dodge")]
        private Vector3 rollDirection;
        [SerializeField] private float dodgeStaminaCost = 25;



        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Update()
        {
            base.Update();

            // 플레이어가 로컬 오너일 때 네트워크로 이동값을 동기화
            if (player.IsOwner)
            {
                player.characterNetworkManager.verticalMovement.Value = verticalMovement;
                player.characterNetworkManager.HorizontalMovement.Value = horizontalMovement;
                player.characterNetworkManager.MoveAmount.Value = moveAmount;
            }
            else
            {
                // 로컬 오너가 아닐 때 네트워크에서 이동값을 받아오긔
                verticalMovement = player.characterNetworkManager.verticalMovement.Value;
                horizontalMovement = player.characterNetworkManager.HorizontalMovement.Value;
                moveAmount = player.characterNetworkManager.MoveAmount.Value;

                player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);



            }
        }

        // 모든 이동 관련 처리를 수행하는 함수
        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();
            HandleJumpingMovement();
            HandleFreeFallMovement();
        }

        // 이동값을 가져오는 함수
        private void GetMovementValues()
        {
            verticalMovement = PlayerInputManager.instance.verticalInput;
            horizontalMovement = PlayerInputManager.instance.horizontalInput;
            moveAmount = PlayerInputManager.instance.moveAmount;
        }

        // 지면에서의 이동을 처리하는 함수
        private void HandleGroundedMovement()
        {
            if (!player.canMove)
            {
                return;
            }

            GetMovementValues();

            // 이동 방향을 계산
            moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;

            // 이동 속도를 적용
            if (player.playerNetworkManager.isSprinting.Value)
            {
                player.characterController.Move(moveDirection * spintingSpeed * Time.deltaTime);
            }
            else
            {
                if (PlayerInputManager.instance.moveAmount > 0.5f)
                {
                    player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
                }
                else if (PlayerInputManager.instance.moveAmount <= 0.5f)
                {
                    player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
                }

            }


        }

        private void HandleJumpingMovement()
        {
            if (player.isJumping)
            {
                player.characterController.Move(jumpDirection * jumpForwardSpeed * Time.deltaTime);
            }
        }

        private void HandleFreeFallMovement()
        {
            if (!player.isGrounded)
            {
                Vector3 freeFallDirection;
                freeFallDirection = PlayerCamera.instance.transform.forward * PlayerInputManager.instance.verticalInput;
                freeFallDirection = freeFallDirection + PlayerCamera.instance.transform.right * PlayerInputManager.instance.horizontalInput;
                freeFallDirection.y = 0;

                player.characterController.Move(freeFallDirection * freeFallSpeed * Time.deltaTime);
            }

        }

        // 회전을 처리하는 함수
        private void HandleRotation()
        {
            if (!player.canRotate)
            {
                return;
            }

            targetRotationDirection = Vector3.zero;
            targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
            targetRotationDirection = targetRotationDirection + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;

            if (targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection = transform.forward;
            }

            // 회전을 적용
            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }

        // 달리기 상태를 처리하는 함수
        public void HandleSprinting()
        {
            if (player.isPerformingAction)
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }

            if (player.playerNetworkManager.currentStamina.Value <= 0)
            {
                player.playerNetworkManager.isSprinting.Value = false;
                return;
            }

            if (moveAmount >= 0.5f)
            {
                player.playerNetworkManager.isSprinting.Value = true;
            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }

            if (player.playerNetworkManager.isSprinting.Value)
            {
                player.playerNetworkManager.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
            }
        }

        // 회피 동작을 시도하는 함수
        public void AttmptToPerformDodge()
        {
            if (!player.isPerformingAction)
            {
                player.isPerformingAction = true;
            }

            if (player.playerNetworkManager.currentStamina.Value <= 0)
            {
                return;
            }

            if (PlayerInputManager.instance.moveAmount > 0)
            {
                // 이동 입력이 있을 때 회피 방향을 설정
                rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
                rollDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
                rollDirection.y = 0;
                rollDirection.Normalize();

                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                player.transform.rotation = playerRotation;

                player.playerAnimatorManager.PlayTargetActionAnimation("Roll_Forward_01", true, true);

            }
            else
            {
                // 이동 입력이 없을 때 백스텝 동작을 실행
                player.playerAnimatorManager.PlayTargetActionAnimation("Back_Step_01", true, true);
            }

            player.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost;
        }

        public void AttmptToPerformJump()
        {
            // if we are performing a general actionm we do not want to allow a jump (will change when combat is added)
            if (!player.isPerformingAction)
            {
                player.isPerformingAction = true;
            }

            // if we are out of stamina we do not wish to allow a jump
            if (player.playerNetworkManager.currentStamina.Value <= 0)
            {
                return;
            }

            // if we are already in a jump, we do not want to allow a jump again until the current jump has finished
            if (player.isJumping)
            {
                return;
            }

            // if we are not Grounded, we do not want to allow a jump
            if (!player.isGrounded)
            {
                return;
            }

            // if we are two handing our weapon, play the two handed jump animation, otherwise play the one handed animation ( to do )
            player.playerAnimatorManager.PlayTargetActionAnimation("Main_Jump_01", false);

            player.isJumping = true;
            ApplyJumpingVelocity(); // 애니메이션이 휴먼노이드가 아니라서 코드에서 처리해줘야함
            player.playerNetworkManager.currentStamina.Value -= jumpStaminaCost;

            jumpDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
            jumpDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
            jumpDirection.y = 0;

            if (jumpDirection != Vector3.zero)
            {
                // if we are sprinting, jump direction is at full distance
                if (player.playerNetworkManager.isSprinting.Value)
                {
                    jumpDirection *= 1;
                }
                // if we are running, jump direction is at Half distance
                else if (PlayerInputManager.instance.moveAmount > 0.5f)
                {
                    jumpDirection *= 0.5f;
                }
                // if we are walking, jump direction is at quarter distance
                else if (PlayerInputManager.instance.moveAmount <= 0.5f)
                {
                    jumpDirection *= 0.25f;
                }
            }




        }

        public void ApplyJumpingVelocity()
        {
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
        }
    }
}
