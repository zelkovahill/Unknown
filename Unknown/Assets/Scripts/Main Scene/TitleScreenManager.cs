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
        [SerializeField] private Button deleteCharacterPopUpConfirmButton;

        [Header("Pop Ups")]
        [SerializeField] private GameObject noCharacterSlotsPopUp;
        [SerializeField] private Button noCharacterSlotsOkayButton;
        [SerializeField] private GameObject deleteCharacterSlotPopUp;

        [Header("Character Slots")]
        public CharacterSlot currentSelectedSlot = CharacterSlot.No_SLOT;



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

        public void SelectCharacterSlot(CharacterSlot characterSlot)
        {
            currentSelectedSlot = characterSlot;
        }

        public void SelectNoSlot()
        {
            currentSelectedSlot = CharacterSlot.No_SLOT;
        }

        public void AttemptToDeleteCharacterSlot()
        {
            if (currentSelectedSlot != CharacterSlot.No_SLOT)
            {
                deleteCharacterSlotPopUp.SetActive(true);
                deleteCharacterPopUpConfirmButton.Select();
            }
        }

        public void DeleteCharacterSlot()
        {
            deleteCharacterSlotPopUp.SetActive(false);
            WorldSaveGameManager.instance.DeleteGame(currentSelectedSlot);

            // we disable and then enable the load menu, to refresh the slots (the deleted slots will now become inactive)
            titleScreenLoadMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);
            LoadMenuReturnButton.Select();
        }

        public void CloseDeleteCharacterPopUp()
        {
            deleteCharacterSlotPopUp.SetActive(false);
            LoadMenuReturnButton.Select();
        }
    }
}

