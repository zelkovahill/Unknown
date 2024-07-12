using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


namespace SG
{
    public class CharaterManager : NetworkBehaviour
    {
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;

        [HideInInspector] public CharaterNetworkManager charaterNetworkManager;

        
        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);

            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            charaterNetworkManager = GetComponent<CharaterNetworkManager>();
        }

        protected virtual void Update()
        {
            if(IsOwner)
            {
                charaterNetworkManager.networkPosition.Value = transform.position;
                charaterNetworkManager.networkRotation.Value = transform.rotation;
            }
            else
            {
                // position
                transform.position = Vector3.SmoothDamp
                    (transform.position,
                    charaterNetworkManager.networkPosition.Value,
                    ref charaterNetworkManager.networkPositionVelocity,
                    charaterNetworkManager.networkPositionSmoothTime);

                // rotation
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
