using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Runtime.InteropServices;

namespace SG
{
public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;

    [Header("NETWORK JOIN")]
    [SerializeField] private bool startGameAsClient;

    private void Awake()
    {
        if(instance == null)
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
        if(startGameAsClient)
        {
            startGameAsClient = false;

            // WE MUST FIRST 
            NetworkManager.Singleton.Shutdown();

            NetworkManager.Singleton.StartClient();
        }
    }

}
}