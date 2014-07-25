using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Main
{
    public class GameObjectReferences
    {
        public static GameObject getScriptsGameObject()
        {
            return GameObject.Find("_Scripts");
        }
        public static GameObject getGlobalScriptsGameObject()
        {
            return GameObject.Find("_GlobalScripts");
        }
    }
}
