using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SG
{
    public class UI_Character_Save_Slot : MonoBehaviour
    {
        private SaveFileDataWriter saveFileWriter;

        [Header("Game Slot")]
        public CharacterSlot characterSlot;

        [Header("Character Info")]
        public TextMeshProUGUI characterName;
        public TextMeshProUGUI timedPlayerd;

        private void OnEnable()
        {
            LoadSaveSlots();
        }

        private void LoadSaveSlots()
        {
            saveFileWriter = new SaveFileDataWriter();
            saveFileWriter.saveDataDirectoryPath = Application.persistentDataPath;

            // Save Slot 01

            switch (characterSlot)
            {
                case CharacterSlot.characterSlot_01:
                    saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    // if the file exists, get information from it
                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot01.characterName;
                    }
                    // if it does not, disable this gameobject
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;

                case CharacterSlot.characterSlot_02:
                    saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    // if the file exists, get information from it
                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot02.characterName;
                    }
                    // if it does not, disable this gameobject
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;

                case CharacterSlot.characterSlot_03:
                    saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    // if the file exists, get information from it
                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot03.characterName;
                    }
                    // if it does not, disable this gameobject
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;

                case CharacterSlot.characterSlot_04:
                    saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    // if the file exists, get information from it
                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot04.characterName;
                    }
                    // if it does not, disable this gameobject
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;

                case CharacterSlot.characterSlot_05:
                    saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    // if the file exists, get information from it
                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot05.characterName;
                    }
                    // if it does not, disable this gameobject
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;

                case CharacterSlot.characterSlot_06:
                    saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    // if the file exists, get information from it
                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot06.characterName;
                    }
                    // if it does not, disable this gameobject
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;

                case CharacterSlot.characterSlot_07:
                    saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    // if the file exists, get information from it
                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot07.characterName;
                    }
                    // if it does not, disable this gameobject
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;

                case CharacterSlot.characterSlot_08:
                    saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    // if the file exists, get information from it
                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot08.characterName;
                    }
                    // if it does not, disable this gameobject
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;

                case CharacterSlot.characterSlot_09:
                    saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    // if the file exists, get information from it
                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot09.characterName;
                    }
                    // if it does not, disable this gameobject
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;

                case CharacterSlot.characterSlot_10:
                    saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);

                    // if the file exists, get information from it
                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot10.characterName;
                    }
                    // if it does not, disable this gameobject
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;


            }
        }
    }
}


