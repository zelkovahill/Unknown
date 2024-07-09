using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG
{
    public class CharaterManager : MonoBehaviour
    {
        
        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        protected virtual void Update()
        {
            
        }
    }


}
