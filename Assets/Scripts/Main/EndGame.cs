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
                GameObject.Find("_Scripts").GetComponent<GameManager>().LevelManager.LoadLevel(LevelsEnum.Menu);
            }
        }
    }
}