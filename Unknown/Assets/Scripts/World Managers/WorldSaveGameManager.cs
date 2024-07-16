using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SG
{
    // 게임의 월드 씬 로드 및 관리 기능을 제공하는 클래스
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager instance;

        // 로드할 월드 씬의 인덱스를 저장하는 변수
        [SerializeField] private int worldSceneIndex = 1;

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

        // 새로운 게임을 로드하는 코루틴 함수
        public IEnumerator LoadNewGame()
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