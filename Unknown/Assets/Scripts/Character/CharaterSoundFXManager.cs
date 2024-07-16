using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    // 캐릭터의 사운드 효과를 관리하는 클래스
    public class CharaterSoundFXManager : MonoBehaviour
    {
        private AudioSource audioSource;

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        // 사운드 효과를 재생하는 함수
        public void PlayRollSoundFX()
        {
            
            audioSource.PlayOneShot(WorldSoundFXManager.instance.rollSFX);
        }
    }
}
