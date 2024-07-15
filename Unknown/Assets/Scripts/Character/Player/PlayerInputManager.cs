using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SG
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;
        public PlayerManager player;



        [Header("Camera Movement Input")]
        [SerializeField] Vector2 CameraInput;
        public float cameraHorizontalInput;
        public float cameraVerticalInput;


        [Header("Player Movement Input")]
        private PlayerControls playerControls;
        [SerializeField] Vector2 movementInput;
        public float horizontalInput;
         public float verticalInput;
         public float moveAmount;

        [Header("Player Action Input")]
        [SerializeField] private bool dodgeInput = false;


        private void Awake()
        {
            if(instance == null)
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

            instance.enabled=false;
        }

        private void OnEnable()
        {
            if(playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerMovement.Movement.performed += i => movementInput  = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.Movement.performed += i => CameraInput  = i.ReadValue<Vector2>();
                playerControls.PlayerAction.Dodge.performed += i => dodgeInput = true;
            }

            playerControls.Enable();
        }

        private void OnSceneChange(Scene oldScene ,Scene newScene)
        {
             if(newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
             {
                instance.enabled=true;
             }
             else
             {
                instance.enabled=false;
             }
        }



        private  void OnDestory()
        {
             SceneManager.activeSceneChanged -= OnSceneChange;
        }

        private void OnApplicationFocus(bool focus)
        {
            if(enabled)
            {
                if(focus)
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

        private void HandleAllInputs(){
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
            HandleDodgeInput();
        }

// Movement
        private void HandlePlayerMovementInput()
        {
            horizontalInput = movementInput.x;
            verticalInput = movementInput.y;

            
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));


            if(moveAmount<=0.5 && moveAmount>=0.1)
            {
                moveAmount = 0.5f;
            }
            else if(moveAmount>0.5 && moveAmount<=1)
            {
                moveAmount = 1f;
            }

            if(player ==null)
            {
                return;
            }

            
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0,moveAmount);

        }

        private void HandleCameraMovementInput(){
            cameraHorizontalInput = CameraInput.x;
            cameraVerticalInput = CameraInput.y;


        }

// Action
        private void HandleDodgeInput(){
            if(dodgeInput){
                dodgeInput = false;

                player.playerLocomotionManager.AttmptToPerformDodge();
            }
            

        }
        

    }

    

}

