using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

namespace SG
{
    // 타이틀 화면에서 네트워크 호스트를 시작하고 새로운 게임을 시작하는 기능을 제공하는 클래스
    public class TitleScreenManager : MonoBehaviour
    {
        public static TitleScreenManager instance;

        [Header("Menu")]
        [SerializeField] private GameObject titleScreenMainMenu;
        [SerializeField] private GameObject titleScreenLoadMenu;

        [Header("Button")]
        [SerializeField] private Button LoadMenuReturnButton;
        [SerializeField] private Button mainMenuLoadGameButton;
        [SerializeField] private Button mainMenuNewGameButton;

        [Header("Pop Ups")]
        [SerializeField] private GameObject noCharacterSlotsPopUp;
        [SerializeField] private Button noCharacterSlotsOkayButton;

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
        }

        // 네트워크를 호스트로 시작하는 함수
        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        // 새로운 게임을 시작하는 함수
        public void StartNewGame()
        {
            WorldSaveGameManager.instance.AttemptToCreateNewGame();

            // 새로운 게임을 로드하는 코루틴 함수를 호출
            StartCoroutine(WorldSaveGameManager.instance.LoadWorldGame());
        }

        public void OpenLoadGameMenu()
        {
            // Close main menu
            titleScreenMainMenu.SetActive(false);

            // open Load Menu
            titleScreenLoadMenu.SetActive(true);

            // select the return button first
            LoadMenuReturnButton.Select();


        }


        public void CloseLoadGameMenu()
        {
            // Close Load Menu
            titleScreenLoadMenu.SetActive(false);

            // Open main menu
            titleScreenMainMenu.SetActive(true);

            // select the load game button
            mainMenuLoadGameButton.Select();

        }

        public void DisplayNoFreeCharacterSlotsPopUp()
        {
            noCharacterSlotsPopUp.SetActive(true);
            noCharacterSlotsOkayButton.Select();

        }

        public void CloseNoFreeCharacterSlotsPopUp()
        {
            noCharacterSlotsPopUp.SetActive(false);
            mainMenuNewGameButton.Select();
        }
    }
}

