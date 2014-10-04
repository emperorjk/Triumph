using UnityEngine;

namespace Assets.Scripts.Main
{
    public class GameObjectReferences
    {
        public static GameObject GetScriptsGameObject()
        {
            return GameObject.Find("_Scripts");
        }
        public static GameObject GetGlobalScriptsGameObject()
        {
            return GameObject.Find("_GlobalScripts");
        }
    }
}
