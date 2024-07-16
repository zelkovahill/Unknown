using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    // 게임 내 사운드 효과를 관리하는 역할을 하는 클래스
    public class WorldSoundFXManager : MonoBehaviour
    {

        public static WorldSoundFXManager instance;

        [Header("Action Sounds")]
        public AudioClip rollSFX;

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
    }
}