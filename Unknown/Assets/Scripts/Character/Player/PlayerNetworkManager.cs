using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;


namespace SG
{
    // 플레이어의 네트워크 관련 기능을 관리하는 역할을 하는 클래스
    public class PlayerNetworkManager : CharacterNetworkManager
    {
      public NetworkVariable<FixedString64Bytes> characterName = new NetworkVariable<FixedString64Bytes>("Character",NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);


    }
}

