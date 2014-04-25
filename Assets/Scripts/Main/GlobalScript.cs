﻿using UnityEngine;

namespace Assets.Scripts.Main
{
    /// <summary>
    /// This class is used to either instantiate and set the globalscript or destroy it. So there are never more than 1 instance of the globalscript prefab.
    /// </summary>
    public class GlobalScript : MonoBehaviour
    {

        private static GameObject globalScripts;

        private void Awake()
        {
            if (globalScripts)
            {
                Destroy(gameObject);
            }
            else
            {
                globalScripts = this.gameObject;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}
