using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace SG
{
    // 플레이어의 UI를 관리하는 클래스
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager instance;

        // 게임을 클라이언트로 시작할지 여부를 저장하는 변수
        [Header("NETWORK JOIN")]
        [SerializeField] private bool startGameAsClient;

        [HideInInspector] public PlayerUIHudManager playerUIHudManager;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {   
            // 게임을 클라이언트로 시작할지 확인
            if (startGameAsClient)
            {
                // 클라이언트로 시작하도록 설정되었으면, 플래그를 초기화
                startGameAsClient = false;

                // 네트워크 매니저를 종료하고 클라이언트로 시작
                NetworkManager.Singleton.Shutdown();
                NetworkManager.Singleton.StartClient();
            }
        }
    }
}