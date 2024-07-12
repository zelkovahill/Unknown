using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class PlayerManager : CharaterManager
{
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    protected override void Awake()
    {
        base.Awake();

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
    }

        protected override void Update()
        {
            base.Update();

            if(!IsOwner)
            {
                return;
            }

             playerLocomotionManager.HandleAllMovement();
           
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            
            if(IsOwner){
                PlayerCamera.instance.player = this;
                PlayerInputManager.instance.player = this;

            }
        }


        protected override void LateUpdate()
        {
            if(!IsOwner)
            {
                return;
            }
            base.LateUpdate();
            
            PlayerCamera.instance.HandleAllCameraActions();
        }
    }

}

