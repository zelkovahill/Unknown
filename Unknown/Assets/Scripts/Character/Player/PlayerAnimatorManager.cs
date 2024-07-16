using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SG
{
    // 플레이어의 애니메이션을 관리하는 클래스
    public class PlayerAnimatorManager : CharaterAnimatorManager
    {
        private PlayerManager player;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        // 애니메이터의 움직임이 적용될 때 호출되는 함수
        private void OnAnimatorMove()
        {
            // applyRootMotion 이 true 일 때만 루트 모션을 적용
            if (player.applyRootMotion)
            {
                // 애니메이터의 델터 위치를 가져와서 캐릭터 컨트롤러를 통해 이동을 시킨다.
                Vector3 velocity = player.animator.deltaPosition;
                player.characterController.Move(velocity);

                // 애니메이터의 델타 회전을 적용하여 플레이어의 회전을 업데이트 한다.
                player.transform.rotation *= player.animator.deltaRotation;
            }

        }
    }
}
