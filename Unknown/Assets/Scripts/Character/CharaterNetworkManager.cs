using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


namespace SG
{
    // 네트워크를 통해 캐릭터의 상태를 동기화하는 클래스
    public class CharacterNetworkManager : NetworkBehaviour
    {
        private CharacterManager character;

        // 네트워크를 통해 동기화할 위치와 회전값을 저장하는 변수들
        [Header("Position")]
        public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        // 네트워크 위치와 속도와 스무스 타임을 저장하는 변수들
        public Vector3 networkPositionVelocity;
        public float networkPositionSmoothTime = 0.1f;
        public float networkRotationSmoothTime = 0.1f;


        // 네트워크를 통해 동기화할 애니메이터 파라미터를 저장하는 변수들
        [Header("Animator")]
        public NetworkVariable<float> HorizontalMovement = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> verticalMovement = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> MoveAmount = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        // 네트워크를 통해 동기화할 플래그를 저장하는 변수
        [Header("Flags")]
        public NetworkVariable<bool> isSprinting = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Stats")]
        public NetworkVariable<int> endurance = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> currentStamina = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> maxStamina = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }


        // 서버 RPC 함수 : 서버에 애니메이션 실행을 알림
        [ServerRpc]
        public void NotifyServerOfActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion)
        {

            if (IsServer)
            {
                // 모든 클라이언트에서 애니메이션을 실행하도록 함
                playActionAnimationForAllClientsClientRpc(clientID, animationID, applyRootMotion);
            }
        }

        // 클라이언트 RPC 함수 : 모든 클라이언트에서 애니메이션 실행
        [ClientRpc]
        public void playActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion)
        {

            if (clientID != NetworkManager.Singleton.LocalClientId)
            {
                // 로컬 클라이언트가 아닌 경우 애니메이션을 실행
                PerrformActionAnimation(animationID, applyRootMotion);
            }
        }

        // 애니메이션을 실행하는 함수
        private void PerrformActionAnimation(string animationID, bool applyRootMotion)
        {
            character.animator.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(animationID, 0.2f);
        }
    }
}

