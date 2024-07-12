using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;


namespace SG
{

public class PlayerLocomotionManager : CharaterLocomotionManager
{
    private PlayerManager player;

    public float verticalMovement;
    public float horizontalMovement;
    public float moveAmount;
    private Vector3 targetRotationDirection;


    private Vector3 moveDirection;
    [SerializeField] private float walkingSpeed=2;
    [SerializeField] private float runningSpeed=5;
    [SerializeField] private float rotationSpeed=15;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

    protected override void Update()
    {
        base.Update();

        if(player.IsOwner)
        {
            player.charaterNetworkManager.verticalMovement.Value = verticalMovement;
            player.charaterNetworkManager.HorizontalMovement.Value = horizontalMovement;
            player.charaterNetworkManager.MoveAmount.Value = moveAmount;
        }
        else
        {
            verticalMovement = player.charaterNetworkManager.verticalMovement.Value;
            horizontalMovement = player.charaterNetworkManager.HorizontalMovement.Value;
            moveAmount = player.charaterNetworkManager.MoveAmount.Value;

            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0,moveAmount);



        }
    }
    public void HandleAllMovement()
   {
        HandleGroundedMovement();
        HandleRotation();
    

   }

   private void GetMovementValues()
   {
       verticalMovement = PlayerInputManager.instance.verticalInput;
       horizontalMovement = PlayerInputManager.instance.horizontalInput;
       moveAmount = PlayerInputManager.instance.moveAmount;


   }

   private void HandleGroundedMovement()
   {
        GetMovementValues();
        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
         moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
         moveDirection.Normalize();
         moveDirection.y = 0;

         if(PlayerInputManager.instance.moveAmount>0.5f)
         {  
            player.characterController.Move(moveDirection * runningSpeed*Time.deltaTime);
         }
         else if( PlayerInputManager.instance.moveAmount<=0.5f)
         {
             player.characterController.Move(moveDirection * walkingSpeed*Time.deltaTime);
         }
   }

   private void HandleRotation()
   {
        targetRotationDirection = Vector3.zero;
        targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward*verticalMovement;
        targetRotationDirection = targetRotationDirection + PlayerCamera.instance.cameraObject.transform.right*horizontalMovement;
        targetRotationDirection.Normalize();
        targetRotationDirection.y = 0;

        if(targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = transform.forward;
        }

        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation,rotationSpeed * Time.deltaTime );
        transform.rotation = targetRotation;
   }
}

}
