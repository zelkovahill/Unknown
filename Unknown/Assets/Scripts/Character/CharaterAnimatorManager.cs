using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class CharaterAnimatorManager : MonoBehaviour
    {
        CharaterManager character;

        private float vertical;
        private float horizontal;

        protected virtual void Awake()
        {
            character = GetComponent<CharaterManager>();

        }
        public void UpdateAnimatorMovementParameters(float horizontalMovement,float verticalMovement)
        {
            // 옵션 1
            character.animator.SetFloat("Horizontal", horizontalMovement,0.1f,Time.deltaTime);
            character.animator.SetFloat("Vertical", verticalMovement,0.1f,Time.deltaTime);

            // // 옵션 2
            // float snappedHorizontal = 0;
            // float snappedVertical = 0;

            // #region Horizontal
            // if(horizontalMovement > 0 && horizontalMovement < 0.5f)
            // {
            //     snappedHorizontal = 0.5f;
            // }
            // else if(horizontalMovement > 0.5f && horizontalMovement <= 1f)
            // {
            //     snappedHorizontal = 1f;
            // }
            // else if(horizontalMovement < 0 && horizontalMovement >= -0.5f)
            // {
            //     snappedHorizontal = -0.5f;
            // }
            // else if(horizontalMovement < -0.5f && horizontalMovement >= -1f)
            // {
            //     snappedHorizontal = -1f;
            // }
            // else
            // {
            //     snappedHorizontal = 0;
            // }
            // #endregion

            // #region Vertical
            // if(verticalMovement > 0 && verticalMovement <= 0.5f)
            // {
            //     snappedVertical = 0.5f;
            // }
            // else if(verticalMovement > 0.5f && verticalMovement <= 1f)
            // {
            //     snappedVertical = 1f;
            // }
            // else if(verticalMovement < 0 && verticalMovement >= -0.5f)
            // {
            //     snappedVertical = -0.5f;
            // }
            // else if(verticalMovement < -0.5f && verticalMovement >= -1f)
            // {
            //     snappedVertical = -1f;
            // }
            // else
            // {
            //     snappedVertical = 0;
            
            // }

            // character.animator.SetFloat("Horizontal", snappedHorizontal);
            // character.animator.SetFloat("Vertical", snappedVertical);


            // #endregion
            
            
        }
    
    }


}

