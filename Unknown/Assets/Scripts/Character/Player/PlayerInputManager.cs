using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SG
{
    // 플레이어의 입력을 관리하고 처리하는 역할을 하는 클래스
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;
        public PlayerManager player;

        // 카메라 움직임 입력을 저장하는 변수
        [Header("Camera Movement Input")]
        [SerializeField] Vector2 CameraInput;
        public float cameraHorizontalInput;
        public float cameraVerticalInput;

        
        [Header("Player Movement Input")]
        private PlayerControls playerControls;

        // 이동 입력을 저장하는 변수
        [SerializeField] Vector2 movementInput;
        public float horizontalInput;
        public float verticalInput;
        public float moveAmount;

        [Header("Player Action Input")]
        [SerializeField] private bool dodgeInput = false;     // 회피 입력을 저장하는 변수
        [SerializeField] private bool sprintInput = false;      //  달리기 입력을 저장하는 변수


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

            SceneManager.activeSceneChanged += OnSceneChange;

            instance.enabled = false;
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                // 이동 입력을 설정
                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();

                // 카메라 움직임 입력을 설정
                playerControls.PlayerCamera.Movement.performed += i => CameraInput = i.ReadValue<Vector2>();

                // 회피 입력을 설정
                playerControls.PlayerAction.Dodge.performed += i => dodgeInput = true;

                // 달리기 입력을 설정
                playerControls.PlayerAction.Sprint.performed += i => sprintInput = true;
                playerControls.PlayerAction.Sprint.canceled += i => sprintInput = false;
            }

            // 플레이어 컨트롤을 활성화
            playerControls.Enable();
        }

        // 씬이 변경될 때 실행되는 함수
        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
            {
                
                instance.enabled = true;
            }
            else
            {
                
                instance.enabled = false;
            }
        }



        private void OnDestory()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }

        // 애플리케이션이 포커스를 얻거나 잃을 때 실행 되는 함수
        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                if (focus)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();
                }
            }
        }

        private void Update()
        {
            HandleAllInputs();
        }


        // 모든 입력을 처리하는 함수
        private void HandleAllInputs()
        {
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
            HandleDodgeInput();
            HandleSprinting();

        }

        // 플레이어의 이동 입력을 처리하는 함수
        private void HandlePlayerMovementInput()
        {
            horizontalInput = movementInput.x;
            verticalInput = movementInput.y;

            // 이동 양을 계산
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));


            if (moveAmount <= 0.5 && moveAmount >= 0.1)
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0.5 && moveAmount <= 1)
            {
                moveAmount = 1f;
            }

            if (player == null)
            {
                return;
            }

            // 플레이어 애니메이터의 이동 파라미터를 업데이트
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);

        }

        // 카메라의 이동 입력을 처리하는 함수
        private void HandleCameraMovementInput()
        {
            cameraHorizontalInput = CameraInput.x;
            cameraVerticalInput = CameraInput.y;


        }

        // 회피 입력을 처리하는 함수
        private void HandleDodgeInput()
        {
            if (dodgeInput)
            {
                dodgeInput = false;

                // 플레이어의 회피 동작을 시도
                player.playerLocomotionManager.AttmptToPerformDodge();
            }
        }

        // 달리기 입력을 처리하는 함수
        private void HandleSprinting()
        {
            if (sprintInput)
            {
                player.playerLocomotionManager.HandleSprinting();
            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
        }
    }
}

