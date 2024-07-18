using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace SG
{
    // 타이틀 화면에서 네트워크 호스트를 시작하고 새로운 게임을 시작하는 기능을 제공하는 클래스
    public class TimeScreenManager : MonoBehaviour
    {
        // 네트워크를 호스트로 시작하는 함수
        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        // 새로운 게임을 시작하는 함수
        public void StartNewGame()
        {
            WorldSaveGameManager.instance.CreateNewGame();

            // 새로운 게임을 로드하는 코루틴 함수를 호출
            StartCoroutine(WorldSaveGameManager.instance.LoadWorldGame());
        }
    }
}

