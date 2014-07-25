using Assets.Scripts.Levels;
using Assets.Scripts.Main;
using UnityEngine;

namespace Assets.Scripts.MenuBar
{
    public class GameBar : MonoBehaviour
    {
        // These values are set via unity editor.
        public GUISkin customSkin;
        public Texture2D gamebarTexture;
        public Texture2D settingsTexture;
        public Texture2D speakerOnTexture;
        public Texture2D speakerOffTexture;

        private Rect gamebarSize;
        public float verticalGameBarSize = 50f;
        public float minSizeBar = 175f;
        public float maxSizeBar = 240;

        private LevelManager lm;

        private void Start()
        {
            lm = GameObjectReferences.getGlobalScriptsGameObject().GetComponent<LevelManager>();
        }

        private void OnGUI()
        {
            GUI.skin = customSkin;
            Rect player = new Rect(40, 10, 300, 40);
            GUI.Label(player, "Player: " + lm.CurrentLevel.CurrentPlayer.Name);

            Rect gold = new Rect(40, 40, 300, 40);
            GUI.Label(gold, "Current Gold: " + lm.CurrentLevel.CurrentPlayer.Gold);
        }
    }
}