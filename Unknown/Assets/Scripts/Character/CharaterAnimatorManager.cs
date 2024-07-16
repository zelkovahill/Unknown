using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace SG
{
    // 캐릭터의 애니메이션을 관리하는 클래스
    public class CharaterAnimatorManager : MonoBehaviour
    {
        CharaterManager character;

        // 애니메이터 파라미터의 해시값을 저장하는 변수
        private int vertical;
        private int horizontal;

        protected virtual void Awake()
        {
            character = GetComponent<CharaterManager>();

            // 애니메이터 파라미터의 해시값을 생성
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");

        }

        // 애니메이터의 이동 파라미터를 업데이트하는 함수
        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
        {
            // 수평 및 수직 이동값을 설정
            float horizontalAmount = horizontalMovement;
            float verticalAmount = verticalMovement;

            // 스프린트 중이면 수직 이동값을 2로 설정
            if (isSprinting)
            {
                verticalAmount = 2;
            }

            // 애니메이터 파라미터를 설정
            character.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
            character.animator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);
        }

        // 특정 애니메이션을 재생하는 함수
        public virtual void PlayTargetActionAnimation(
            string targetAnimation,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canRotate = false,
            bool canMove = false)
        {
            // 루트 모션 적용 여부를 설정
            character.applyRootMotion = applyRootMotion;

            // 애니메이션을 페이드 인하여 재생
            character.animator.CrossFade(targetAnimation, 0.2f);

            // 캐릭터의 상태를 업데이트
            character.isPerformingAction = isPerformingAction;
            character.canRotate = canRotate;
            character.canMove = canMove;

            // 서버에 애니메이션 실행을 알림
            character.charaterNetworkManager.NotifyServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }
    }
}

