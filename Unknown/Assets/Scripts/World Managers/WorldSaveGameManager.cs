using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SG
{
    // 게임의 월드 씬 로드 및 관리 기능을 제공하는 클래스
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager instance;

        [SerializeField] private PlayerManager player;

        [Header("Save/Load")]
        [SerializeField] private bool saveGame;
        [SerializeField] private bool loadGame;

        [Header("World Scene Index")]
        [SerializeField] private int worldSceneIndex = 1;

        [Header("Save Data Writer")]
        private SaveFileDataWriter saveFileDataWriter;

        [Header("Current Character Slot")]
        public characterSlot currentCharacterSlotBeingUsed;
        public CharacterSaveData currentCharacterData;
        private string saveFileName;

        [Header("Character Slots")]
        public CharacterSaveData characterSlot01;
        // public CharacterSaveData characterSlot02;
        // public CharacterSaveData characterSlot03;
        // public CharacterSaveData characterSlot04;
        // public CharacterSaveData characterSlot05;
        // public CharacterSaveData characterSlot06;
        // public CharacterSaveData characterSlot07;
        // public CharacterSaveData characterSlot08;
        // public CharacterSaveData characterSlot09;
        // public CharacterSaveData characterSlot10;

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

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }


        private void Update()
        {
            if(saveGame)
            {
                saveGame = false;
                SaveGame();
            }

            if(loadGame)
            {
                loadGame = false;
                LoadGame();
            }
        }

        private void DecideCharacterFileNameBasedOnCharacterSlotBeingUsed()
        {
            switch (currentCharacterSlotBeingUsed)
            {
                case characterSlot.characterSlot_01:
                    saveFileName = "characterSlot_01";
                    break;
                case characterSlot.characterSlot_02:
                    saveFileName = "characterSlot_02";
                    break;
                case characterSlot.characterSlot_03:
                    saveFileName = "characterSlot_03";
                    break;
                case characterSlot.characterSlot_04:
                    saveFileName = "characterSlot_04";
                    break;
                case characterSlot.characterSlot_05:
                    saveFileName = "characterSlot_05";
                    break;
                case characterSlot.characterSlot_06:
                    saveFileName = "characterSlot_06";
                    break;
                case characterSlot.characterSlot_07:
                    saveFileName = "characterSlot_07";
                    break;
                case characterSlot.characterSlot_08:
                    saveFileName = "characterSlot_08";
                    break;
                case characterSlot.characterSlot_09:
                    saveFileName = "characterSlot_09";
                    break;
                case characterSlot.characterSlot_10:
                    saveFileName = "characterSlot_10";
                    break;
                default:
                    break;
            }

        }

        public void CreateNewGame()
        {
            // Create a new File, With a name depending on which slot we are using
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

            currentCharacterData = new CharacterSaveData();
        }

        public void LoadGame()
        {
            // Load a previous file, with a File name edpending on the character slot we are using
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

            saveFileDataWriter = new SaveFileDataWriter();

            // Generally works on multiple machine types (Application.persistentDataPath)
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = saveFileName;
            currentCharacterData = saveFileDataWriter.LoadSaveFile(); // 이상함

            StartCoroutine(LoadWorldGame());
        }

        public void SaveGame()
        {
            // save the current file under a file have depending on which slot we are using 
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

            saveFileDataWriter = new SaveFileDataWriter();
            // generally works on multiple machine types (Application.persistentDataPath)
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = saveFileName;

            // pass the players info, from game, to their save file
            player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);

            // write thet info onto a json file saved to this machine
            saveFileDataWriter.CreateNewCharacterFile(currentCharacterData);
        }

        // 새로운 게임을 로드하는 코루틴 함수
        public IEnumerator LoadWorldGame()
        {
            // 비동기적으로 월드 씬을 로드
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
            yield return null;
        }

        // 월드 씬의 인덱스를 반환하는 함수
        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }

    }
}