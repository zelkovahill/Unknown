using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SG
{
    // 게임의 월드 씬 로드 및 관리 기능을 제공하는 클래스
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager instance;

        public PlayerManager player;

        [Header("Save/Load")]
        [SerializeField] private bool saveGame;
        [SerializeField] private bool loadGame;

        [Header("World Scene Index")]
        [SerializeField] private int worldSceneIndex = 1;

        [Header("Save Data Writer")]
        private SaveFileDataWriter saveFileDataWriter;

        [Header("Current Character Slot")]
        public CharacterSlot currentCharacterSlotBeingUsed;
        public CharacterSaveData currentCharacterData;
        private string saveFileName;

        [Header("Character Slots")]
        public CharacterSaveData characterSlot01;
        public CharacterSaveData characterSlot02;
        public CharacterSaveData characterSlot03;
        public CharacterSaveData characterSlot04;
        public CharacterSaveData characterSlot05;
        public CharacterSaveData characterSlot06;
        public CharacterSaveData characterSlot07;
        public CharacterSaveData characterSlot08;
        public CharacterSaveData characterSlot09;
        public CharacterSaveData characterSlot10;

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
            LoadAllCharacterProfiles();
        }


        private void Update()
        {
            if (saveGame)
            {
                saveGame = false;
                SaveGame();
            }

            if (loadGame)
            {
                loadGame = false;
                LoadGame();
            }
        }

        public string DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot characterSlot)
        {
            string fileName = "";

            switch (characterSlot)
            {
                case CharacterSlot.characterSlot_01:
                    fileName = "characterSlot_01";
                    break;
                case CharacterSlot.characterSlot_02:
                    fileName = "characterSlot_02";
                    break;
                case CharacterSlot.characterSlot_03:
                    fileName = "characterSlot_03";
                    break;
                case CharacterSlot.characterSlot_04:
                    fileName = "characterSlot_04";
                    break;
                case CharacterSlot.characterSlot_05:
                    fileName = "characterSlot_05";
                    break;
                case CharacterSlot.characterSlot_06:
                    fileName = "characterSlot_06";
                    break;
                case CharacterSlot.characterSlot_07:
                    fileName = "characterSlot_07";
                    break;
                case CharacterSlot.characterSlot_08:
                    fileName = "characterSlot_08";
                    break;
                case CharacterSlot.characterSlot_09:
                    fileName = "characterSlot_09";
                    break;
                case CharacterSlot.characterSlot_10:
                    fileName = "characterSlot_10";
                    break;
                default:
                    break;
            }
            return fileName;
        }

        public void AttemptToCreateNewGame()
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.characterSlot_01);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                // if this profile slot is not taken, make a new one suiong this slot
                currentCharacterSlotBeingUsed = CharacterSlot.characterSlot_01;
                currentCharacterData = new CharacterSaveData();
                StartCoroutine(LoadWorldGame());
                return;
            }

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.characterSlot_02);

            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                // if this profile slot is not taken, make a new one suiong this slot
                currentCharacterSlotBeingUsed = CharacterSlot.characterSlot_02;
                currentCharacterData = new CharacterSaveData();
                StartCoroutine(LoadWorldGame());
                return;
            }

            TitleScreenManager.instance.DisplayNoFreeCharacterSlotsPopUp();

        }

        public void LoadGame()
        {
            // Load a previous file, with a File name edpending on the character slot we are using
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

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
            saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

            saveFileDataWriter = new SaveFileDataWriter();
            // generally works on multiple machine types (Application.persistentDataPath)
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = saveFileName;

            // pass the players info, from game, to their save file
            player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);

            // write thet info onto a json file saved to this machine
            saveFileDataWriter.CreateNewCharacterFile(currentCharacterData);
        }

        // load all charavter profiles on device when starting game
        private void LoadAllCharacterProfiles()
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.characterSlot_01);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.characterSlot_02);
            characterSlot02 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.characterSlot_03);
            characterSlot03 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.characterSlot_04);
            characterSlot04 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.characterSlot_05);
            characterSlot05 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.characterSlot_06);
            characterSlot06 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.characterSlot_07);
            characterSlot07 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.characterSlot_08);
            characterSlot08 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.characterSlot_09);
            characterSlot09 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.characterSlot_10);
            characterSlot10 = saveFileDataWriter.LoadSaveFile();
        }

        // 새로운 게임을 로드하는 코루틴 함수
        public IEnumerator LoadWorldGame()
        {
            // 비동기적으로 월드 씬을 로드
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

            player.LoadGameDataFromCurrentCharacterData(ref currentCharacterData);
            yield return null;
        }

        // 월드 씬의 인덱스를 반환하는 함수
        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }

    }
}