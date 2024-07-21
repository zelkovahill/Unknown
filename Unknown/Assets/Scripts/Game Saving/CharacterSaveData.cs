using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    [System.Serializable]
    public class CharacterSaveData
    {
        [Header("Scene Index")]
        public int sceneIndex = 1;

        [Header("Character Name")]
        public string characterName = "Character";

        [Header("Time Played")]
        public float secondsPlayed;

        // 왜 Vector3를 쓰지 않는가?
        // Vector3는 Unity Engine에 종속적이기 때문에 사용하지 않는다.
        [Header("World Coordinates")]
        public float xPosition;
        public float yPosition;
        public float zPosition;

    }
}