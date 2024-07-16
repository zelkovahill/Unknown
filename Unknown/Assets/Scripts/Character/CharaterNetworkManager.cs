using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


namespace SG
{
    public class CharaterNetworkManager : NetworkBehaviour
    {
        private CharaterManager character;

        [Header("Position")]
        public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
        public NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>(Quaternion.identity,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
        public Vector3 networkPositionVelocity;
        public float networkPositionSmoothTime =0.1f;
        public float networkRotationSmoothTime =0.1f;

        [Header("Animator")]
        public NetworkVariable<float> HorizontalMovement = new NetworkVariable<float>(0,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> verticalMovement = new NetworkVariable<float>(0,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> MoveAmount = new NetworkVariable<float>(0,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);

        [Header("Flags")]
        public NetworkVariable<bool> isSprinting = new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);


        protected virtual void Awake()
        {
            character = GetComponent<CharaterManager>();
        }


        // A ServerRcp is a method that is called on the server and executed on the client
        [ServerRpc]
        public void NotifyServerOfActionAnimationServerRpc(ulong clientID, string animationID,bool applyRootMotion){
            
                if(IsServer){
                    playActionAnimationForAllClientsClientRpc(clientID,animationID,applyRootMotion);
                }
        }

        [ClientRpc]
        public void playActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion){

            if(clientID != NetworkManager.Singleton.LocalClientId){
                PerrformActionAnimation(animationID,applyRootMotion);
            }
        }

        private void PerrformActionAnimation(string animationID, bool applyRootMotion){
             character.animator.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(animationID,0.2f);
        }
    }

}

