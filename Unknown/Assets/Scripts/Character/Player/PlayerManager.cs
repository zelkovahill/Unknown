using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class PlayerManager : CharaterManager
{
    private PlayerLocomotionManager playerLocomotionManager;
    protected override void Awake()
    {
        base.Awake();

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
    }

        protected override void Update()
        {
            base.Update();

            // playerLocomotionManager.HandleAllMovement();
           
        }

    }

}

