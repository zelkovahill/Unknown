using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SG
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;


        [Header("Player Movement Input")]
        private PlayerControls playerControls;
        [SerializeField] Vector2 movementInput;
        public float horizontalInput;
         public float verticalInput;
         public float moveAmount;

        [Header("Camera Movement Input")]
        [SerializeField] Vector2 CameraInput;
          public float cameraHorizontalInput;
         public float cameraVerticalInput;

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
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
        }

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
        }

        private void HandleCameraMovementInput(){
            cameraHorizontalInput = CameraInput.x;
            cameraVerticalInput = CameraInput.y;


        }

    }

    

}

