using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    // 캐릭터의 이동을 관리하는 클래스
    public class CharaterLocomotionManager : MonoBehaviour
    {
        private CharacterManager character;

        [Header("Ground & Jumping")]
        [SerializeField] protected float gravityForce = -5.55f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundCheckSphereRadius = 1;
        [SerializeField] protected Vector3 yVelocity;   // the force at which our charcter is pulled up or down (Jumping or Falling)
        [SerializeField] protected float groundedYVelocity = -20;  // the force at which our character is sticking to the ground whilst they are grounded
        [SerializeField] protected float fallStartYVelocity = -5;  // the force at which our character begins to fall when they become ungrounded (rises as they fall longer)
        protected bool fallingVelocityHasBeenSet = false;
        protected float inAirTimer = 0;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Update()
        {
            HandleGroundCheck();

            if (character.isGrounded)
            {
                // if we are not attempting to jump or move upward
                if (yVelocity.y < 0)
                {
                    inAirTimer = 0;
                    fallingVelocityHasBeenSet = false;
                    yVelocity.y = groundedYVelocity;
                }
            }
            else
            {
                // if we are not jumping, and our falling velocity has not been set
                if (!character.isJumping && !fallingVelocityHasBeenSet)
                {
                    fallingVelocityHasBeenSet = true;
                    yVelocity.y = fallStartYVelocity;
                }

                inAirTimer = inAirTimer + Time.deltaTime;
                character.animator.SetFloat("InAirTimer", inAirTimer);

                yVelocity.y += gravityForce * Time.deltaTime;
                
            }
            // there should alway be some force applied to the y Velocity
            character.characterController.Move(yVelocity * Time.deltaTime);
        }

        protected void HandleGroundCheck()
        {
            character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
        }

        // draws our ground check sphere in scene view
        protected void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
        }
    }
}
