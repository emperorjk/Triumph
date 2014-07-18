using Assets.Scripts.Levels;
using UnityEngine;

namespace Assets.Scripts.Main
{
    public class EndGame : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                LevelManager.LoadLevel(LevelsEnum.Menu);
            }
        }
    }
}